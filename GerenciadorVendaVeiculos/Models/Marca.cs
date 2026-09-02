using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorVendaVeiculos.Models;

[Table("Marca")]
public class Marca
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    [Required] [MaxLength(50)] public string Nome { get; private set; }

    [Required] [MaxLength(10)] public string Sigla { get; private set; }

    private Marca()
    {
    }

    public Marca(string nome, string sigla)
    {
        SetNome(nome);
        SetSigla(sigla);
    }

    public void SetNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
        {
            throw new ArgumentNullException(nameof(nome), "O nome não pode ser nulo ou vazio");
        }

        if (nome.Length > 50)
        {
            throw new ArgumentException("O nome deve conter no máximo 50 caracteres ");
        }

        Nome = nome;
    }

    public void SetSigla(string sigla)
    {
        if (string.IsNullOrEmpty(sigla))
        {
            throw new ArgumentNullException(nameof(sigla), "A sigla não pode ser nula ou vazia");
        }

        if (sigla.Length > 10)
        {
            throw new ArgumentException("A sigla deve conter no máximo 10 caracteres");
        }

        Sigla = sigla;
    }
}