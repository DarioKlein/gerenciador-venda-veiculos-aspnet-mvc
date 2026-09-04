using GerenciadorVendaVeiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorVendaVeiculos.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cidade> Cidades { get; set; }
    public DbSet<Marca> Marcas { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Venda> Vendas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venda>()
            .Property(v => v.DataVenda)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<Veiculo>().HasOne(v => v.Marca).WithMany().HasForeignKey(v => v.MarcaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>().HasOne(c => c.Cidade).WithMany().HasForeignKey(c => c.CidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Venda>().HasOne(v => v.Cliente).WithMany().HasForeignKey(v => v.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Venda>().HasOne(v => v.Veiculo).WithMany().HasForeignKey(v => v.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}