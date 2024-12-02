﻿using BancoChu.API.Models;
using BancoChu.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace BancoChu.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciaController : ControllerBase
    {
        private readonly BancoChuContext _context;

        
        public TransferenciaController(BancoChuContext context)
        {
            _context = context;
        }

        // busca os feriados do ano
        private const string FeriadosApiUrl = "https://brasilapi.com.br/api/feriados/v1/2023";

        private async Task<List<DateTime>> ObterFeriadosAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(FeriadosApiUrl);
                var feriados = JsonConvert.DeserializeObject<List<Feriado>>(response);

                // Converte as datas
                return feriados.Select(f => DateTime.Parse(f.Date)).ToList();
            }
        }

        // Endpoint para transferências
        [HttpPost]
        public async Task<ActionResult<Transferencia>> Post([FromBody] Transferencia transferencia)
        {
            
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return BadRequest(new Error { Message = "Erro de validação", Detail = "Transferências só podem ser feitas em dias úteis." });
            }

            
            var feriados = await ObterFeriadosAsync();
            if (feriados.Contains(DateTime.Now.Date))
            {
                return BadRequest(new Error { Message = "Erro de validação", Detail = "Transferências não podem ser realizadas em feriados." });
            }

            // Validação de contas 
            var contaOrigem = await _context.Contas.FirstOrDefaultAsync(c => c.Id == transferencia.ContaOrigemId);
            var contaDestino = await _context.Contas.FirstOrDefaultAsync(c => c.Id == transferencia.ContaDestinoId);

            if (contaOrigem == null || contaDestino == null)
            {
                return BadRequest(new Error { Message = "Erro de validação", Detail = "Conta de origem ou destino não encontrada." });
            }

            // Valida se o caboblo tem money suficiente
            if (contaOrigem.Saldo < transferencia.Valor)
            {
                return BadRequest(new Error { Message = "Erro de validação", Detail = "Saldo insuficiente na conta de origem." });
            }

          
            contaOrigem.Saldo -= transferencia.Valor;
            contaDestino.Saldo += transferencia.Valor;

           
            transferencia.Id = 0;  
            transferencia.DataTransferencia = DateTime.Now;
            _context.Transferencias.Add(transferencia);

            
            await _context.SaveChangesAsync();

           
            return CreatedAtAction(nameof(Get), new { id = transferencia.Id }, transferencia);
        }

        [HttpGet("extrato")]
        public async Task<ActionResult<IEnumerable<Transferencia>>> GerarExtratoPorPeriodo([FromQuery] string dataInicio, [FromQuery] string dataFim)
        {
            // precisei converter o formato da data p/ DateTime
            DateTime dataInicioParsed;
            DateTime dataFimParsed;

            if (!DateTime.TryParseExact(dataInicio, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataInicioParsed))
            {
                return BadRequest(new Error { Message = "Erro de validação", Detail = "Data de início no formato inválido. Utilize dd/MM/yyyy." });
            }

            if (!DateTime.TryParseExact(dataFim, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataFimParsed))
            {
                return BadRequest(new Error { Message = "Erro de validação", Detail = "Data de fim no formato inválido. Utilize dd/MM/yyyy." });
            }

            // Ajustado a data de início para MEIA NOITE 
            dataInicioParsed = dataInicioParsed.Date;

            // Ajustada a data final  para 23:59:59
            dataFimParsed = dataFimParsed.Date.AddDays(1).AddSeconds(-1);

            var extrato = await _context.Transferencias
                .Where(t => t.DataTransferencia >= dataInicioParsed && t.DataTransferencia <= dataFimParsed)
                .ToListAsync();

            if (extrato.Count == 0)
            {
                return NotFound(new Error { Message = "Erro de não encontrado", Detail = "Nenhuma transferência encontrada no período solicitado." });
            }

            return Ok(extrato);
        }

        // Endpoint para obter uma transferência por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Transferencia>> Get(int id)
        {
            var transferencia = await _context.Transferencias.FindAsync(id);
            if (transferencia == null)
            {
                return NotFound(new Error { Message = "Erro de não encontrado", Detail = "Transferência não encontrada." });
            }
            return Ok(transferencia);
        }
    }

    // Classe que representa o objeto de feriado
    public class Feriado
    {
        public string Date { get; set; }  // Formato: yyyy-MM-dd
        public string Name { get; set; }
        public string Type { get; set; }
    }
}