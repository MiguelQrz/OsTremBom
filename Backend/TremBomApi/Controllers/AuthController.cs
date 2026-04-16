using Microsoft.AspNetCore.Mvc;
using TremBomApi.Models;
using TremBomApi.Services;

namespace TremBomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Autentica um usuário
        /// IDs: loginEmail, loginSenha, lembrarLogin
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Fail("Dados inválidos",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var resultado = await _authService.LoginAsync(request);

            if (!resultado.Sucesso)
                return Unauthorized(resultado);

            return Ok(resultado);
        }

        /// <summary>
        /// Registra um novo usuário
        /// IDs: registroNome, registroEmail, registroTelefone, registroSenha, 
        /// registroFoto, preferencias, termosRegistro
        /// </summary>
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromForm] RegistroRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Fail("Dados inválidos",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var resultado = await _authService.RegistroAsync(request);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        /// <summary>
        /// Verifica se um email já está cadastrado
        /// </summary>
        [HttpGet("verificar-email")]
        public async Task<IActionResult> VerificarEmail([FromQuery] string email)
        {
            var existe = await _authService.EmailExisteAsync(email);
            return Ok(new { email, disponivel = !existe });
        }

        /// <summary>
        /// Logout - invalida o token
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var sucesso = await _authService.LogoutAsync(token);
            
            if (sucesso)
                return Ok(ApiResponse<object>.Ok(null, "Logout realizado com sucesso"));
            
            return BadRequest(ApiResponse<object>.Fail("Erro ao realizar logout"));
        }

        /// <summary>
        /// Obtém dados do usuário pelo token
        /// IDs: usuarioId, usuarioNome, usuarioEmail, usuarioTelefone, usuarioAvatar, usuarioPreferencias
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetUsuarioAtual()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var usuario = await _authService.GetUsuarioByTokenAsync(token);

            if (usuario == null)
                return Unauthorized(ApiResponse<object>.Fail("Usuário não autenticado"));

            return Ok(ApiResponse<UsuarioResponse>.Ok(usuario));
        }
    }
}