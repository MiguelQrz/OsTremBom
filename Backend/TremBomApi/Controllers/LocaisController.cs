/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TremBomApi.Models;
using TremBomApi.Services;

namespace TremBomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocaisController : ControllerBase
    {
        private readonly IPublicacaoService _publicacaoService;

        public LocaisController(IPublicacaoService publicacaoService)
        {
            _publicacaoService = publicacaoService;
        }

        /// <summary>
        /// Lista todos os locais para o carrossel
        /// IDs: carrossel-locais, card-local-{id}, local-imagem-{id}, 
        /// local-nome-{id}, local-categoria-{id}, local-descricao-{id}, local-likes-{id}
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocais()
        {
            var resultado = await _publicacaoService.GetLocaisAsync();
            return Ok(resultado);
        }

        /// <summary>
        /// Obtém um local específico
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocal(int id)
        {
            var resultado = await _publicacaoService.GetLocalByIdAsync(id);
            
            if (!resultado.Sucesso)
                return NotFound(resultado);
            
            return Ok(resultado);
        }

        /// <summary>
        /// Cria um novo local (Admin)
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CriarLocal([FromBody] Local local)
        {
            var resultado = await _publicacaoService.CriarLocalAsync(local);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            
            return Ok(resultado);
        }
    }
}*/