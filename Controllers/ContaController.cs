using BancoChu.API.Models;
using BancoChu.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BancoChu.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly BancoChuContext _context;

        public ContaController(BancoChuContext context)
        {
            _context = context;
        }

        // Endpoint para listar todas as contas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conta>>> Get()
        {
            try
            {
                var contas = await _context.Contas.ToListAsync();
                return Ok(contas);
            }
            catch (System.Exception ex)
            {
                
                return StatusCode(500, new Error { Message = "Erro ao buscar as contas.", Detail = ex.Message });
            }
        }

        // Endpoint para criar uma nova conta
        [HttpPost]
        public async Task<ActionResult<Conta>> Post([FromBody] Conta conta)
        {
            if (conta == null)
            {
                
                return BadRequest(new Error { Message = "Dados da conta não fornecidos." });
            }

            // Verifica se o número da conta já existe
            var contaExistente = await _context.Contas
                .FirstOrDefaultAsync(c => c.NumeroConta == conta.NumeroConta);

            if (contaExistente != null)
            {
                
                return BadRequest(new Error { Message = "O número da conta já está em uso." });
            }

            try
            {
                // Adicionando a nova conta no banco
                _context.Contas.Add(conta);
                await _context.SaveChangesAsync(); // Salva as alterações no banco

                
                return CreatedAtAction(nameof(Get), new { id = conta.Id }, conta);
            }
            catch (System.Exception ex)
            {
                
                return StatusCode(500, new Error { Message = "Erro ao criar a conta.", Detail = ex.Message });
            }
        }
    }

  
}
