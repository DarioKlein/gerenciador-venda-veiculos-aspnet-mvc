using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class ClienteViewModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required] [Display(Name = "Área")] public TipoArea Area { get; set; }

    [Required]
    [Range(0, 150)]
    [Display(Name = "Idade")]
    public int Idade { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Valor Hora")]
    public double ValorHora { get; set; }

    [Required] [Display(Name = "Cidade")] public int CidadeId { get; set; }
}