using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class VeiculoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [MaxLength(60, ErrorMessage = "O modelo tem um tamanho máximo de 100 caracteres")]
    [Display(Name = "Modelo")]
    public string Modelo { get; set; }

    [Required(ErrorMessage = "Selecione uma marca.")]
    [Display(Name = "Marca")]
    public int? MarcaId { get; set; }

    [Required(ErrorMessage = "O ano é obrigatório.")]
    [Display(Name = "Ano")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "A cor é obrigatória.")]
    [MaxLength(30, ErrorMessage = "A cor tem um tamanho máximo de 30 caracteres")]
    [Display(Name = "Cor")]
    public string Cor { get; set; }

    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O campo deve ter o valor de no mínimo de 0.01")]
    [Display(Name = "Valor")]
    public double Valor { get; set; }

    [Required(ErrorMessage = "Selecione uma situacao.")]
    [Display(Name = "Situação")]
    public SituacaoVeiculo Situacao { get; set; }
}