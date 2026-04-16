using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TremBomApi.Data;
using TremBomApi.Models;

namespace TremBomApi.Services
{
    public class PublicacaoService : IPublicacaoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PublicacaoService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        #region Publicações

        public async Task<ApiResponse<PublicacaoResponse>> CriarPublicacaoAsync(int usuarioId, PublicacaoRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return ApiResponse<PublicacaoResponse>.Fail("Usuário não encontrado");

            var local = await _context.Locais.FindAsync(request.LocalId);
            if (local == null)
                return ApiResponse<PublicacaoResponse>.Fail("Local não encontrado");

            var publicacao = new Publicacao
            {
                UsuarioId = usuarioId,
                LocalId = request.LocalId,
                LocalNome = local.Nome,
                Feedback = request.Feedback,
                Rating = request.Rating,
                DataCriacao = DateTime.Now
            };

            _context.Publicacoes.Add(publicacao);
            await _context.SaveChangesAsync();

            // Salvar fotos
            if (request.Fotos != null && request.Fotos.Any())
            {
                var ordem = 0;
                foreach (var foto in request.Fotos)
                {
                    var fotoUrl = await SalvarFotoPublicacaoAsync(foto, publicacao.Id);
                    _context.PublicacoesFotos.Add(new PublicacaoFoto
                    {
                        PublicacaoId = publicacao.Id,
                        FotoUrl = fotoUrl,
                        Ordem = ordem++
                    });
                }
                await _context.SaveChangesAsync();
            }

            var response = await MapearPublicacaoResponse(publicacao, usuarioId);
            return ApiResponse<PublicacaoResponse>.Ok(response, "Publicação criada com sucesso");
        }

        public async Task<ApiResponse<List<PublicacaoResponse>>> GetPublicacoesAsync(int? usuarioId = null)
        {
            var query = _context.Publicacoes
                .Include(p => p.Usuario)
                .Include(p => p.Fotos)
                .Include(p => p.Comentarios.Where(c => c.Ativo))
                    .ThenInclude(c => c.Usuario)
                .Where(p => p.Ativo)
                .OrderByDescending(p => p.DataCriacao);

            var publicacoes = await query.ToListAsync();

            var responses = new List<PublicacaoResponse>();
            foreach (var pub in publicacoes)
            {
                responses.Add(await MapearPublicacaoResponse(pub, usuarioId));
            }

            return ApiResponse<List<PublicacaoResponse>>.Ok(responses);
        }

        public async Task<ApiResponse<PublicacaoResponse>> GetPublicacaoByIdAsync(int id, int? usuarioAtualId = null)
        {
            var publicacao = await _context.Publicacoes
                .Include(p => p.Usuario)
                .Include(p => p.Fotos)
                .Include(p => p.Comentarios.Where(c => c.Ativo))
                    .ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id && p.Ativo);

            if (publicacao == null)
                return ApiResponse<PublicacaoResponse>.Fail("Publicação não encontrada");

            var response = await MapearPublicacaoResponse(publicacao, usuarioAtualId);
            return ApiResponse<PublicacaoResponse>.Ok(response);
        }

        public async Task<ApiResponse<bool>> CurtirPublicacaoAsync(int publicacaoId, int usuarioId)
        {
            var publicacao = await _context.Publicacoes.FindAsync(publicacaoId);
            if (publicacao == null)
                return ApiResponse<bool>.Fail("Publicação não encontrada");

            var likeExistente = await _context.PublicacoesLikes
                .FirstOrDefaultAsync(l => l.PublicacaoId == publicacaoId && l.UsuarioId == usuarioId);

            if (likeExistente != null)
                return ApiResponse<bool>.Fail("Você já curtiu esta publicação");

            _context.PublicacoesLikes.Add(new PublicacaoLike
            {
                PublicacaoId = publicacaoId,
                UsuarioId = usuarioId,
                DataLike = DateTime.Now
            });

            publicacao.TotalLikes++;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Publicação curtida");
        }

        public async Task<ApiResponse<bool>> DescurtirPublicacaoAsync(int publicacaoId, int usuarioId)
        {
            var like = await _context.PublicacoesLikes
                .FirstOrDefaultAsync(l => l.PublicacaoId == publicacaoId && l.UsuarioId == usuarioId);

            if (like == null)
                return ApiResponse<bool>.Fail("Like não encontrado");

            _context.PublicacoesLikes.Remove(like);

            var publicacao = await _context.Publicacoes.FindAsync(publicacaoId);
            if (publicacao != null)
                publicacao.TotalLikes--;

            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Like removido");
        }

        public async Task<ApiResponse<bool>> ExcluirPublicacaoAsync(int publicacaoId, int usuarioId)
        {
            var publicacao = await _context.Publicacoes
                .FirstOrDefaultAsync(p => p.Id == publicacaoId && p.UsuarioId == usuarioId);

            if (publicacao == null)
                return ApiResponse<bool>.Fail("Publicação não encontrada ou você não tem permissão");

            publicacao.Ativo = false;
            publicacao.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Publicação excluída");
        }

        #endregion

        #region Comentários

        public async Task<ApiResponse<ComentarioResponse>> AdicionarComentarioAsync(int publicacaoId, int usuarioId, ComentarioRequest request)
        {
            var publicacao = await _context.Publicacoes.FindAsync(publicacaoId);
            if (publicacao == null)
                return ApiResponse<ComentarioResponse>.Fail("Publicação não encontrada");

            var comentario = new Comentario
            {
                PublicacaoId = publicacaoId,
                UsuarioId = usuarioId,
                Texto = request.Texto,
                DataCriacao = DateTime.Now
            };

            _context.Comentarios.Add(comentario);
            publicacao.TotalComentarios++;
            await _context.SaveChangesAsync();

            // Carrega dados do usuário
            await _context.Entry(comentario).Reference(c => c.Usuario).LoadAsync();

            var response = MapearComentarioResponse(comentario);
            return ApiResponse<ComentarioResponse>.Ok(response, "Comentário adicionado");
        }

        public async Task<ApiResponse<List<ComentarioResponse>>> GetComentariosAsync(int publicacaoId)
        {
            var comentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .Where(c => c.PublicacaoId == publicacaoId && c.Ativo)
                .OrderByDescending(c => c.DataCriacao)
                .ToListAsync();

            var responses = comentarios.Select(MapearComentarioResponse).ToList();
            return ApiResponse<List<ComentarioResponse>>.Ok(responses);
        }

        public async Task<ApiResponse<bool>> ExcluirComentarioAsync(int comentarioId, int usuarioId)
        {
            var comentario = await _context.Comentarios
                .FirstOrDefaultAsync(c => c.Id == comentarioId && c.UsuarioId == usuarioId);

            if (comentario == null)
                return ApiResponse<bool>.Fail("Comentário não encontrado ou você não tem permissão");

            comentario.Ativo = false;
            comentario.DataAtualizacao = DateTime.Now;

            var publicacao = await _context.Publicacoes.FindAsync(comentario.PublicacaoId);
            if (publicacao != null)
                publicacao.TotalComentarios--;

            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Comentário excluído");
        }

        #endregion

        #region Locais

        public async Task<ApiResponse<List<LocalResponse>>> GetLocaisAsync()
        {
            var locais = await _context.Locais
                .Where(l => l.Ativo)
                .OrderBy(l => l.Nome)
                .ToListAsync();

            var responses = locais.Select(MapearLocalResponse).ToList();
            return ApiResponse<List<LocalResponse>>.Ok(responses);
        }

        public async Task<ApiResponse<LocalResponse>> GetLocalByIdAsync(int id)
        {
            var local = await _context.Locais.FindAsync(id);
            if (local == null || !local.Ativo)
                return ApiResponse<LocalResponse>.Fail("Local não encontrado");

            return ApiResponse<LocalResponse>.Ok(MapearLocalResponse(local));
        }

        public async Task<ApiResponse<LocalResponse>> CriarLocalAsync(Local local)
        {
            _context.Locais.Add(local);
            await _context.SaveChangesAsync();

            return ApiResponse<LocalResponse>.Ok(MapearLocalResponse(local), "Local criado com sucesso");
        }

        #endregion

        #region Métodos Privados

        private async Task<string> SalvarFotoPublicacaoAsync(IFormFile foto, int publicacaoId)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads", "publicacoes", publicacaoId.ToString());

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(foto.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await foto.CopyToAsync(stream);

            return $"/uploads/publicacoes/{publicacaoId}/{fileName}";
        }

        private async Task<PublicacaoResponse> MapearPublicacaoResponse(Publicacao publicacao, int? usuarioAtualId = null)
        {
            bool usuarioCurtiu = false;
            if (usuarioAtualId.HasValue)
            {
                usuarioCurtiu = await _context.PublicacoesLikes
                    .AnyAsync(l => l.PublicacaoId == publicacao.Id && l.UsuarioId == usuarioAtualId);
            }

            return new PublicacaoResponse
            {
                Id = publicacao.Id,
                UsuarioId = publicacao.UsuarioId,
                UsuarioNome = publicacao.Usuario?.NomeCompleto ?? "Usuário",
                UsuarioUsername = publicacao.Usuario != null ? "@" + publicacao.Usuario.NomeCompleto.Replace(" ", "").ToLower() : "@usuario",
                UsuarioAvatar = publicacao.Usuario?.FotoPerfilUrl,
                LocalId = publicacao.LocalId,
                LocalNome = publicacao.LocalNome ?? "Local",
                Feedback = publicacao.Feedback,
                Rating = publicacao.Rating,
                TotalLikes = publicacao.TotalLikes,
                TotalComentarios = publicacao.TotalComentarios,
                TotalCompartilhamentos = publicacao.TotalCompartilhamentos,
                UsuarioCurtiu = usuarioCurtiu,
                Fotos = publicacao.Fotos?.OrderBy(f => f.Ordem).Select(f => f.FotoUrl).ToList() ?? new List<string>(),
                Comentarios = publicacao.Comentarios?
                    .Where(c => c.Ativo)
                    .OrderByDescending(c => c.DataCriacao)
                    .Take(3)
                    .Select(MapearComentarioResponse)
                    .ToList() ?? new List<ComentarioResponse>(),
                DataCriacao = publicacao.DataCriacao,
                DataFormatada = FormatarData(publicacao.DataCriacao)
            };
        }

        private ComentarioResponse MapearComentarioResponse(Comentario comentario)
        {
            return new ComentarioResponse
            {
                Id = comentario.Id,
                UsuarioId = comentario.UsuarioId,
                UsuarioNome = comentario.Usuario?.NomeCompleto ?? "Usuário",
                UsuarioUsername = comentario.Usuario != null ? "@" + comentario.Usuario.NomeCompleto.Replace(" ", "").ToLower() : "@usuario",
                UsuarioAvatar = comentario.Usuario?.FotoPerfilUrl,
                Texto = comentario.Texto,
                DataCriacao = comentario.DataCriacao,
                DataFormatada = FormatarData(comentario.DataCriacao)
            };
        }

        private LocalResponse MapearLocalResponse(Local local)
        {
            return new LocalResponse
            {
                Id = local.Id,
                Nome = local.Nome,
                Categoria = local.Categoria,
                Descricao = local.Descricao,
                ImagemUrl = local.ImagemUrl,
                TotalLikes = local.TotalLikes,
                TotalComentarios = local.TotalComentarios,
                LikesFormatado = FormatarNumero(local.TotalLikes),
                ComentariosFormatado = FormatarNumero(local.TotalComentarios)
            };
        }

        private string FormatarData(DateTime data)
        {
            var agora = DateTime.Now;
            var diff = agora - data;

            if (diff.TotalSeconds < 60) return "Agora mesmo";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} min";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} h";
            if (diff.TotalDays == 1) return "Ontem";
            if (diff.TotalDays < 7) return $"{(int)diff.TotalDays} dias";
            return data.ToString("dd/MM/yyyy");
        }

        private string FormatarNumero(int num)
        {
            if (num >= 1000000) return (num / 1000000.0).ToString("0.0") + "M";
            if (num >= 1000) return (num / 1000.0).ToString("0.0") + "k";
            return num.ToString();
        }

        #endregion
    }
}