using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PontosApoioApi.Data;
using PontosApoioApi.Models;

namespace PontosApoioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecursoEnergiaController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public RecursoEnergiaController(AppDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RecursoEnergia>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarRecursosEnergia()
    {
        var recursos = await dbContext.RecursosEnergia.ToListAsync();

        if (recursos.Count == 0)
            return NotFound(new ErroResposta("Nenhum recurso de energia encontrado."));

        return Ok(recursos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RecursoEnergia), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarRecursoEnergiaPorId(int id)
    {
        var recurso = await dbContext.RecursosEnergia.FindAsync(id);

        if (recurso == null)
            return NotFound(new ErroResposta("Recurso de energia não encontrado."));

        return Ok(recurso);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RecursoEnergia), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CadastrarRecursoEnergia([FromBody] NovoRecursoEnergia dados)
    {
        // Confiro o ponto antes de criar o recurso: sem isso o Oracle recusaria a FK e a resposta seria 500.
        var ponto = await dbContext.PontosApoio.FindAsync(dados.PontoApoioId);

        if (ponto == null)
            return NotFound(new ErroResposta("Ponto de apoio não encontrado."));

        RecursoEnergia novoRecurso;
        try
        {
            novoRecurso = new RecursoEnergia(dados.Tipo, dados.Quantidade, dados.AutonomiaHoras, dados.PontoApoioId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErroResposta(ex.Message));
        }

        dbContext.RecursosEnergia.Add(novoRecurso);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarRecursoEnergiaPorId), new { id = novoRecurso.Id }, novoRecurso);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarQuantidadeDoRecurso(int id, [FromBody] AtualizarQuantidade dados)
    {
        var recurso = await dbContext.RecursosEnergia.FindAsync(id);

        if (recurso == null)
            return NotFound(new ErroResposta("Recurso de energia não encontrado."));

        try
        {
            recurso.AtualizarQuantidade(dados.NovaQuantidade);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErroResposta(ex.Message));
        }

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverRecursoEnergia(int id)
    {
        var recurso = await dbContext.RecursosEnergia.FindAsync(id);

        if (recurso == null)
            return NotFound(new ErroResposta("Recurso de energia não encontrado."));

        dbContext.RecursosEnergia.Remove(recurso);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}

public record NovoRecursoEnergia(string Tipo, int Quantidade, int AutonomiaHoras, int PontoApoioId);

public record AtualizarQuantidade(int NovaQuantidade);
