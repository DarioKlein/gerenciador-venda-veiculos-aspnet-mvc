using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class VeiculoViewModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    [Display(Name = "Modelo")]
    public string Modelo { get; set; }

    [Required] [Display(Name = "Marca")] public int MarcaId { get; set; }

    [Required]
    [Range(1950, 2100)]
    [Display(Name = "Ano")]
    public int Ano { get; set; }

    [Required]
    [MaxLength(30)]
    [Display(Name = "Cor")]
    public string Cor { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    [Display(Name = "Valor")]
    public double Valor { get; set; }

    [Required]
    [Display(Name = "Situação")]
    public SituacaoVeiculo Situacao { get; set; }
}