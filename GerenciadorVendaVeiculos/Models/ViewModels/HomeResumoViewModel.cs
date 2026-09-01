namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class HomeResumoViewModel
{
    public int TotalVeiculos { get; set; }
    public int VeiculosDisponiveis { get; set; }
    public int TotalClientes { get; set; }
    public int TotalVendas { get; set; }
    public decimal ValorTotalVendido { get; set; }
}   