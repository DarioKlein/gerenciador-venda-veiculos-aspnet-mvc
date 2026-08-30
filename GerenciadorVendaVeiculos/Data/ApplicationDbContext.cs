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
}