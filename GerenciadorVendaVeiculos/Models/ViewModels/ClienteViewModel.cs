using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class ClienteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome tem um tamanho máximo de 100 caracteres")]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "A área é obrigatória.")]
    [Display(Name = "Área")]
    public TipoArea Area { get; set; }

    [Required(ErrorMessage = "A idade é obrigatória.")]
    [Range(0, 150, ErrorMessage = "A idade deve ser maior que 0 e menor que 150")]
    [Display(Name = "Idade")]
    public int Idade { get; set; }

    [Required(ErrorMessage = "O valor da hora é obrigatório.")]
    [Range(0, double.MaxValue, ErrorMessage = "O valor não pode ser negativo")]
    [Display(Name = "Valor Hora")]
    public double ValorHora { get; set; }

    [Required(ErrorMessage = "Selecione uma cidade.")]
    [Display(Name = "Cidade")]
    public int? CidadeId { get; set; }
}