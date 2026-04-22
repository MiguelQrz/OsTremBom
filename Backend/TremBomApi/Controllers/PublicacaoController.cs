/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TremBomApi.Models;

namespace TremBomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PublicacaoController : ControllerBase
    {
        private readonly IPublicacaoService _publicacaoService;

        public PublicacaoController(IPublicacaoService publicacaoService)
        {
            _publicacaoService = publicacaoService;
        }

        private int GetUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? User.FindFirst("sub")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        /// <summary>
        /// Lista todas as publicações
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicacoes()
        {
            int? usuarioId = User.Identity?.IsAuthenticated == true ? GetUsuarioId() : null;
            var resultado = await _publicacaoService.GetPublicacoesAsync(usuarioId);
            return Ok(resultado);
        }

        /// <summary>
        /// Obtém uma publicação por ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicacao(int id)
        {
            int? usuarioId = User.Identity?.IsAuthenticated == true ? GetUsuarioId() : null;
            var resultado = await _publicacaoService.GetPublicacaoByIdAsync(id, usuarioId);
            
            if (!resultado.Sucesso)
                return NotFound(resultado);
            
            return Ok(resultado);
        }

        /// <summary>
        /// Cria uma nova publicação
        /// IDs: publicacaoLocalId, publicacaoLocal, publicacaoFotos, 
        /// publicacaoFeedback, publicacaoRatingSelecionado
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CriarPublicacao([FromForm] PublicacaoRequest request)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _publicacaoService.CriarPublicacaoAsync(usuarioId, request);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            
            return Ok(resultado);
        }

        /// <summary>
        /// Curtir uma publicação
        /// </summary>
        [HttpPost("{id}/like")]
        public async Task<IActionResult> CurtirPublicacao(int id)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _publicacaoService.CurtirPublicacaoAsync(id, usuarioId);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            
            return Ok(resultado);
        }

        /// <summary>
        /// Descurtir uma publicação
        /// </summary>
        [HttpDelete("{id}/like")]
        public async Task<IActionResult> DescurtirPublicacao(int id)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _publicacaoService.DescurtirPublicacaoAsync(id, usuarioId);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            
            return Ok(resultado);
        }

        /// <summary>
        /// Excluir uma publicação
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPublicacao(int id)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _publicacaoService.ExcluirPublicacaoAsync(id, usuarioId);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            
            return Ok(resultado);
        }

        /// <summary>
        /// Adicionar comentário em uma publicação
        /// </summary>
        [HttpPost("{id}/comentarios")]
        public async Task<IActionResult> AdicionarComentario(int id, [FromBody] ComentarioRequest request)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _publicacaoService.AdicionarComentarioAsync(id, usuarioId, request);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            
            return Ok(resultado);
        }

        /// <summary>
        /// Listar comentários de uma publicação
        /// </summary>
        [HttpGet("{id}/comentarios")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComentarios(int id)
        {
            var resultado = await _publicacaoService.GetComentariosAsync(id);
            return Ok(resultado);
        }

        /// <summary>
        /// Excluir um comentário
        /// </summary>
        [HttpDelete("comentarios/{comentarioId}")]
        public async Task<IActionResult> ExcluirComentario(int comentarioId)
        {
            var usuarioId = GetUsuarioId();
            var resultado = await _publicacaoService.ExcluirComentarioAsync(comentarioId, usuarioId);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            
            return Ok(resultado);
        }
    }
}*/