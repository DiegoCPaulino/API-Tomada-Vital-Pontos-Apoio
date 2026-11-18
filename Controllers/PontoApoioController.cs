using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PontosApoioApi.Data;
using PontosApoioApi.Models;

namespace PontosApoioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PontoApoioController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public PontoApoioController(AppDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PontoApoio>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarPontosApoio()
    {
        var pontos = await dbContext.PontosApoio.ToListAsync();

        if (pontos.Count == 0)
            return NotFound(new ErroResposta("Nenhum ponto de apoio encontrado."));

        return Ok(pontos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PontoApoio), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPontoApoioPorId(int id)
    {
        var ponto = await dbContext.PontosApoio.FindAsync(id);

        if (ponto == null)
            return NotFound(new ErroResposta("Ponto de apoio não encontrado."));

        return Ok(ponto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PontoApoio), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarPontoApoio([FromBody] NovoPontoApoio dados)
    {
        // Recebo um record, não a entidade: se o leitor de JSON chamasse o construtor direto,
        // a ArgumentException escaparia antes do controller e viraria 500. Aqui o construtor
        // continua sendo quem cria o objeto salvo, e o Id nunca vem de fora.
        PontoApoio novoPonto;
        try
        {
            novoPonto = new PontoApoio(dados.Nome, dados.Endereco, dados.Bairro, dados.Telefone, dados.Capacidade, dados.Ativo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErroResposta(ex.Message));
        }

        dbContext.PontosApoio.Add(novoPonto);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPontoApoioPorId), new { id = novoPonto.Id }, novoPonto);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResposta), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarCapacidadeDoPonto(int id, [FromBody] AtualizarCapacidade dados)
    {
        var ponto = await dbContext.PontosApoio.FindAsync(id);

        if (ponto == null)
            return NotFound(new ErroResposta("Ponto de apoio não encontrado."));

        try
        {
            ponto.AtualizarCapacidade(dados.NovaCapacidade);
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
    public async Task<IActionResult> RemoverPontoApoio(int id)
    {
        var ponto = await dbContext.PontosApoio.FindAsync(id);

        if (ponto == null)
            return NotFound(new ErroResposta("Ponto de apoio não encontrado."));

        dbContext.PontosApoio.Remove(ponto);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}

public record NovoPontoApoio(string Nome, string Endereco, string Bairro, string Telefone, int Capacidade, bool Ativo);

public record AtualizarCapacidade(int NovaCapacidade);
