using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorVendaVeiculos.Models;

[Table("Cidade")]
[Index(nameof(Descricao), IsUnique = true)]
[Index(nameof(Sigla), IsUnique = true)]
public class Cidade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Descricao { get; private set; }

    [Required]
    [MaxLength(3)]
    [Column(TypeName = "varchar(3)")]
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

        if (sigla.Length is < 2 or > 3)
        {
            throw new ArgumentException("A sigla deve conter de 2 a 3 caracteres");
        }

        if (!sigla.All(char.IsLetter))
        {
            throw new ArgumentException("A sigla deve conter apenas letras");
        }

        Sigla = sigla.ToUpper().Trim();
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

        if (!descricao.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new ArgumentException(
                "A descrição deve conter apenas letras e espaços"
            );
        }

        Descricao = descricao.Trim();
    }
}