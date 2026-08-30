using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models.ViewModels;

public class CidadeViewModel
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; }

    [Required]
    [MaxLength(2)]
    [Display(Name = "Sigla")]
    public string Sigla { get; set; }
}