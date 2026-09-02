using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class VendaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Selecione um cliente.")]
    [Display(Name = "Cliente")]
    public int? ClienteId { get; set; }

    [Required(ErrorMessage = "Selecione um veículo.")]
    [Display(Name = "Veículo")]
    public int? VeiculoId { get; set; }

    [Required(ErrorMessage = "A data da venda é obrigatória.")]
    [Display(Name = "Data da Venda")]
    [DataType(DataType.Date)]
    public DateTime DataVenda { get; set; }

    [Required(ErrorMessage = "O valor da venda é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor mínimo da venda é 0.01")]
    [Display(Name = "Valor da Venda")]
    public double ValorVenda { get; set; }

    [Required(ErrorMessage = "O valor da causa é obrigatório.")]
    [Range(0, double.MaxValue, ErrorMessage = "O valor da causa não pode ser negativo")]
    [Display(Name = "Valor da Causa")]
    public double ValorCausa { get; set; }

    [Required(ErrorMessage = "O vendedor é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O vendedor deve conter no máximo 100 caracteres")]
    [Display(Name = "Vendedor")]
    public string Vendedor { get; set; }
}