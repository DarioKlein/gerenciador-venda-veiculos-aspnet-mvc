using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorVendaVeiculos.Models;

[Table("Cidade")]
public class Cidade
{
    [Key]
    [Display(Name = "ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Descrição")]
    public string Descricao { get; private set; }

    [Required]
    [MaxLength(2)]
    [Column(TypeName = "char(2)")]
    [Display(Name = "Sigla")]
    public string Sigla { get; private set; }

    private Cidade()
    {
    }

    public Cidade(string descricao, string sigla)
    {
        SetDescricao(descricao);
        SetSigla(sigla);
    }

    public void SetSigla(string sigla)
    {
        if (string.IsNullOrWhiteSpace(sigla))
        {
            throw new ArgumentNullException(nameof(sigla), "A sigla não pode ser nula ou vazia");
        }

        if (sigla.Length != 2)
        {
            throw new ArgumentException("A sigla deve conter 2 caracteres");
        }

        Sigla = sigla;
    }

    public void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentNullException(nameof(descricao), "A descrição não pode ser nula ou vazia");
        }

        if (descricao.Length > 100)
        {
            throw new ArgumentException("A descrição deve conter no máximo 100 caracteres");
        }

        Descricao = descricao;
    }
}