using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class VendaViewModel
{
    public int Id { get; set; }

    [Required] [Display(Name = "Cliente")] public int ClienteId { get; set; }

    [Required] [Display(Name = "Veículo")] public int VeiculoId { get; set; }

    [Required]
    [Display(Name = "Data da Venda")]
    [DataType(DataType.Date)]
    public DateTime DataVenda { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    [Display(Name = "Valor da Venda")]
    public double ValorVenda { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Valor da Causa")]
    public double ValorCausa { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Vendedor")]
    public string Vendedor { get; set; }
}