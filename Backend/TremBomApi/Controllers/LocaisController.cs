using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TremBomApi.Data;
using TremBomApi.Models;

namespace TremBomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocaisController : ControllerBase
    {
        private readonly AppDbContext _context;
        // Injeta o Banco de dados
        public LocaisController(AppDbContext context) { _context = context;}

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var locais = await _context.Locais.ToListAsync();
            return Ok(locais);
        }
    }
}