using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class MarcaViewModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required]
    [MaxLength(10)]
    [Display(Name = "Sigla")]
    public string Sigla { get; set; }
}