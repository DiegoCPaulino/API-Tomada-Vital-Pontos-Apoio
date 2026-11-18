namespace PontosApoioApi.Models;

public class PontoApoio
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Endereco { get; private set; } = string.Empty;
    public string Bairro { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public int Capacidade { get; private set; }
    public bool Ativo { get; private set; }

    // O EF Core precisa de um construtor vazio para montar o ponto quando lê do banco.
    protected PontoApoio() { }

    // O Oracle é quem gera o Id (Identity), por isso ele não entra aqui.
    public PontoApoio(string nome, string endereco, string bairro, string telefone, int capacidade, bool ativo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do ponto de apoio é obrigatório.");
        if (string.IsNullOrWhiteSpace(endereco))
            throw new ArgumentException("O endereço é obrigatório.");
        if (string.IsNullOrWhiteSpace(bairro))
            throw new ArgumentException("O bairro é obrigatório.");
        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("O telefone é obrigatório.");
        ValidarCapacidade(capacidade);

        Nome = nome;
        Endereco = endereco;
        Bairro = bairro;
        Telefone = telefone;
        Capacidade = capacidade;
        Ativo = ativo;
    }

    public void AtualizarCapacidade(int novaCapacidade)
    {
        ValidarCapacidade(novaCapacidade);
        Capacidade = novaCapacidade;
    }

    // Um lugar só para a regra da capacidade, usado pelo construtor e pelo AtualizarCapacidade.
    private static void ValidarCapacidade(int capacidade)
    {
        if (capacidade <= 0)
            throw new ArgumentException("A capacidade deve ser maior que zero.");
    }
}
