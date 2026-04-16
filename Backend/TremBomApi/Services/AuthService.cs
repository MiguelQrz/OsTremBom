using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TremBomApi.Data;
using TremBomApi.Models;

namespace TremBomApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Preferencias)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return ApiResponse<LoginResponse>.Fail("Email ou senha inválidos");

            if (!VerificarSenha(request.Senha, usuario.SenhaHash))
                return ApiResponse<LoginResponse>.Fail("Email ou senha inválidos");

            // Atualiza último login
            usuario.UltimoLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            var token = GerarToken(usuario, request.Lembrar);

            var sessao = new Sessao
            {
                UsuarioId = usuario.Id,
                Token = token,
                TokenLembrar = request.Lembrar ? GerarTokenLembrar() : null,
                DataExpiracao = request.Lembrar ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(2)
            };

            _context.Sessoes.Add(sessao);
            await _context.SaveChangesAsync();

            var response = new LoginResponse
            {
                Token = token,
                Usuario = MapearUsuarioResponse(usuario)
            };

            return ApiResponse<LoginResponse>.Ok(response, "Login realizado com sucesso");
        }

        public async Task<ApiResponse<LoginResponse>> RegistroAsync(RegistroRequest request)
        {
            if (await EmailExisteAsync(request.Email))
                return ApiResponse<LoginResponse>.Fail("Este email já está cadastrado");

            var preferenciasInvalidas = request.Preferencias
                .Where(p => !PreferenciasDisponiveis.Valores.Contains(p))
                .ToList();

            if (preferenciasInvalidas.Any())
                return ApiResponse<LoginResponse>.Fail($"Preferências inválidas: {string.Join(", ", preferenciasInvalidas)}");

            string? fotoUrl = null;
            if (request.FotoPerfil != null)
                fotoUrl = await SalvarFotoPerfilAsync(request.FotoPerfil);

            var usuario = new Usuario
            {
                NomeCompleto = request.NomeCompleto,
                Email = request.Email,
                Telefone = LimparTelefone(request.Telefone),
                SenhaHash = HashSenha(request.Senha),
                FotoPerfilUrl = fotoUrl,
                TermosAceitosEm = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            foreach (var pref in request.Preferencias)
            {
                _context.UsuariosPreferencias.Add(new UsuarioPreferencia
                {
                    UsuarioId = usuario.Id,
                    Preferencia = pref
                });
            }

            await _context.SaveChangesAsync();

            var token = GerarToken(usuario, false);

            var sessao = new Sessao
            {
                UsuarioId = usuario.Id,
                Token = token,
                DataExpiracao = DateTime.Now.AddHours(2)
            };

            _context.Sessoes.Add(sessao);
            await _context.SaveChangesAsync();

            usuario.Preferencias = await _context.UsuariosPreferencias
                .Where(p => p.UsuarioId == usuario.Id)
                .ToListAsync();

            var response = new LoginResponse
            {
                Token = token,
                Usuario = MapearUsuarioResponse(usuario)
            };

            return ApiResponse<LoginResponse>.Ok(response, "Conta criada com sucesso");
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> LogoutAsync(string token)
        {
            var sessao = await _context.Sessoes.FirstOrDefaultAsync(s => s.Token == token);
            if (sessao != null)
            {
                _context.Sessoes.Remove(sessao);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<UsuarioResponse?> GetUsuarioByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? 
                    "chave_super_secreta_trem_bom_2026_minimo_32_caracteres!!");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"] ?? "TremBom",
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"] ?? "TremBomClient",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var usuarioId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(usuarioId))
                    return null;

                var sessao = await _context.Sessoes
                    .FirstOrDefaultAsync(s => s.Token == token && s.DataExpiracao > DateTime.Now);

                if (sessao == null)
                    return null;

                var usuario = await _context.Usuarios
                    .Include(u => u.Preferencias)
                    .FirstOrDefaultAsync(u => u.Id == int.Parse(usuarioId));

                return usuario != null ? MapearUsuarioResponse(usuario) : null;
            }
            catch
            {
                return null;
            }
        }

        #region Métodos Privados

        private string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return HashSenha(senha) == hash;
        }

        private string GerarToken(Usuario usuario, bool lembrar)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? "chave_super_secreta_trem_bom_2026_minimo_32_caracteres!!"));

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, usuario.Email),
                new("nome", usuario.NomeCompleto),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "TremBom",
                audience: _configuration["Jwt:Audience"] ?? "TremBomClient",
                claims: claims,
                expires: lembrar ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GerarTokenLembrar()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private async Task<string> SalvarFotoPerfilAsync(IFormFile foto)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads", "perfil");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(foto.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await foto.CopyToAsync(stream);

            return $"/uploads/perfil/{fileName}";
        }

        private string LimparTelefone(string telefone)
        {
            return new string(telefone.Where(char.IsDigit).ToArray());
        }

        private UsuarioResponse MapearUsuarioResponse(Usuario usuario)
        {
            return new UsuarioResponse
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                FotoPerfilUrl = usuario.FotoPerfilUrl,
                Preferencias = usuario.Preferencias?.Select(p => p.Preferencia).ToList() ?? new List<string>(),
                DataCadastro = usuario.DataCadastro,
                UltimoLogin = usuario.UltimoLogin
            };
        }

        #endregion
    }
}