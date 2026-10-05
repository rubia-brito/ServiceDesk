using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.DTOs;
using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChamadosController : ControllerBase
{
    private readonly ServiceDeskContext _context;

    public ChamadosController(ServiceDeskContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarChamadoRequest request)
    {
        var chamado = new Chamado
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Solicitante = request.Solicitante,
            Categoria = request.Categoria,
            Prioridade = request.Prioridade,
            Status = "Aberto",
            DataAbertura = DateTime.Now
        };

        _context.Chamados.Add(chamado);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = chamado.Id },
            chamado);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos(
        string? status,
        string? prioridade,
        string? categoria)
    {
        var query = _context.Chamados.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(c => c.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(prioridade))
        {
            query = query.Where(c => c.Prioridade == prioridade);
        }

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            query = query.Where(c => c.Categoria == categoria);
        }

        var chamados = await query.ToListAsync();

        return Ok(chamados);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var chamado = await _context.Chamados.FindAsync(id);

        if (chamado == null)
        {
            return NotFound();
        }

        return Ok(chamado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(
        int id,
        [FromBody] AtualizarChamadoRequest request)
    {
        var chamado = await _context.Chamados.FindAsync(id);

        if (chamado == null)
        {
            return NotFound();
        }

        chamado.Titulo = request.Titulo;
        chamado.Descricao = request.Descricao;
        chamado.Solicitante = request.Solicitante;
        chamado.Categoria = request.Categoria;
        chamado.Prioridade = request.Prioridade;
        chamado.Status = request.Status;

        if (request.Status.Equals("Fechado", StringComparison.OrdinalIgnoreCase))
        {
            chamado.DataFechamento ??= DateTime.Now;
        }
        else
        {
            chamado.DataFechamento = null;
        }

        await _context.SaveChangesAsync();

        return Ok(chamado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var chamado = await _context.Chamados.FindAsync(id);

        if (chamado == null)
        {
            return NotFound();
        }

        _context.Chamados.Remove(chamado);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}