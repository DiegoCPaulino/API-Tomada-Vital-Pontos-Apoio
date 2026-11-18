namespace PontosApoioApi.Models;

public class RecursoEnergia
{
    public int Id { get; private set; }
    public string Tipo { get; private set; } = string.Empty;
    public int Quantidade { get; private set; }
    public int AutonomiaHoras { get; private set; }
    public int PontoApoioId { get; private set; }

    // O EF Core precisa de um construtor vazio para montar o recurso quando lê do banco.
    protected RecursoEnergia() { }

    // O Oracle é quem gera o Id (Identity), por isso ele não entra aqui.
    // Esta entidade não confere se o ponto existe: ela não tem acesso ao banco. Quem confere é o controller.
    public RecursoEnergia(string tipo, int quantidade, int autonomiaHoras, int pontoApoioId)
    {
        ValidarTipo(tipo);
        ValidarQuantidade(quantidade);
        if (autonomiaHoras <= 0)
            throw new ArgumentException("A autonomia deve ser maior que zero.");

        Tipo = tipo;
        Quantidade = quantidade;
        AutonomiaHoras = autonomiaHoras;
        PontoApoioId = pontoApoioId;
    }

    public void AtualizarQuantidade(int novaQuantidade)
    {
        ValidarQuantidade(novaQuantidade);
        Quantidade = novaQuantidade;
    }

    // Um lugar só para a regra da quantidade, usado pelo construtor e pelo AtualizarQuantidade.
    private static void ValidarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");
    }

    private static void ValidarTipo(string tipo)
    {
        if (tipo != "GERADOR" && tipo != "NOBREAK" && tipo != "CILINDRO_O2_RESERVA")
            throw new ArgumentException("Tipo inválido. Use GERADOR, NOBREAK ou CILINDRO_O2_RESERVA.");
    }
}
