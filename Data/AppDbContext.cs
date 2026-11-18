using Microsoft.EntityFrameworkCore;
using PontosApoioApi.Models;

namespace PontosApoioApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PontoApoio> PontosApoio { get; set; }
    public DbSet<RecursoEnergia> RecursosEnergia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TBL_ no nome da tabela, NM_ para texto, NR_ para número, ST_ para situação (bool) e ID_ para chave estrangeira.
        modelBuilder.Entity<PontoApoio>().ToTable("TBL_PONTOS_APOIO");
        modelBuilder.Entity<PontoApoio>().HasKey(p => p.Id);
        modelBuilder.Entity<PontoApoio>().Property(p => p.Nome).HasColumnName("NM_PONTO_APOIO");
        modelBuilder.Entity<PontoApoio>().Property(p => p.Endereco).HasColumnName("NM_ENDERECO");
        modelBuilder.Entity<PontoApoio>().Property(p => p.Bairro).HasColumnName("NM_BAIRRO");
        modelBuilder.Entity<PontoApoio>().Property(p => p.Telefone).HasColumnName("NM_TELEFONE");
        modelBuilder.Entity<PontoApoio>().Property(p => p.Capacidade).HasColumnName("NR_CAPACIDADE");
        modelBuilder.Entity<PontoApoio>().Property(p => p.Ativo).HasColumnName("ST_ATIVO");

        modelBuilder.Entity<RecursoEnergia>().ToTable("TBL_RECURSOS_ENERGIA");
        modelBuilder.Entity<RecursoEnergia>().HasKey(r => r.Id);
        modelBuilder.Entity<RecursoEnergia>().Property(r => r.Tipo).HasColumnName("NM_TIPO");
        modelBuilder.Entity<RecursoEnergia>().Property(r => r.Quantidade).HasColumnName("NR_QUANTIDADE");
        modelBuilder.Entity<RecursoEnergia>().Property(r => r.AutonomiaHoras).HasColumnName("NR_AUTONOMIA_HORAS");
        modelBuilder.Entity<RecursoEnergia>().Property(r => r.PontoApoioId).HasColumnName("ID_PONTO_APOIO");

        // A FK fica no recurso (o lado "muitos"), sem propriedade de navegação nos dois sentidos.
        // Ela é obrigatória, então o EF Core põe ON DELETE CASCADE sozinho: apagar o ponto apaga os recursos dele.
        modelBuilder.Entity<RecursoEnergia>()
            .HasOne<PontoApoio>()
            .WithMany()
            .HasForeignKey(r => r.PontoApoioId);

        base.OnModelCreating(modelBuilder);
    }
}
