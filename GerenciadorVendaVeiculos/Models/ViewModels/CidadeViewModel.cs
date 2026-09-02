using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class CidadeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(100, ErrorMessage = "A descrição tem um tamanho máximo de 100 caracteres")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "A sigla é obrigatória.")]
    [MaxLength(3, ErrorMessage = "A sigla tem um tamanho máximo de 3 caracteres")]
    [Display(Name = "Sigla")]
    public string Sigla { get; set; }
}