using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorVendaVeiculos.Models;

[Table("Cliente")]
public class Cliente
{
    [Key]
    [Display(Name = "ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }


    [Required]
    [MaxLength(100)]
    [Display(Name = "Nome")]
    public string Nome { get; private set; }

    [Required] [Display(Name = "Área")] public TipoArea Area { get; private set; }


    [Required]
    [Range(0, 150)]
    [Display(Name = "Idade")]
    public int Idade { get; private set; }


    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Valor Hora")]
    public double ValorHora { get; private set; }

    private Cliente()
    {
    }

    public Cliente(string nome, TipoArea area, int idade, double valorHora)
    {
        SetNome(nome);
        SetArea(area);
        SetIdade(idade);
        SetValorHora(valorHora);
    }

    public void SetNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
        {
            throw new ArgumentNullException(nameof(nome), "O nome não pode ser nulo ou vazio");
        }

        if (nome.Length > 100)
        {
            throw new ArgumentException("O nome deve conter no máximo 100 caracteres");
        }

        Nome = nome;
    }

    public void SetArea(TipoArea area)
    {
        Area = area;
    }

    public void SetIdade(int idade)
    {
        if (idade < 0 || idade > 150)
        {
            throw new ArgumentException("A idade deve estar entre 0 e 150 anos");
        }

        Idade = idade;
    }

    public void SetValorHora(double valorHora)
    {
        if (valorHora < 0)
        {
            throw new ArgumentException("O valor hora não pode ser negativo");
        }

        ValorHora = valorHora;
    }
}