namespace PontosApoioApi.Controllers;

// NotFound("texto") e BadRequest("texto") do professor saem como texto puro; o enunciado pede a mensagem em JSON.
public record ErroResposta(string Mensagem);
