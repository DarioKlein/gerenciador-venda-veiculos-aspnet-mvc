using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class MarcaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O nome tem um tamanho máximo de 50 caracteres")]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "A sigla é obrigatória.")]
    [MaxLength(10, ErrorMessage = "A sigla tem um tamanho máximo de 10 caracteres")]
    [Display(Name = "Sigla")]
    public string Sigla { get; set; }
}