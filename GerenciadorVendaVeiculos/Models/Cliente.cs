using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorVendaVeiculos.Models;

[Table("Cliente")]
public class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }


    [Required] [MaxLength(100)] public string Nome { get; private set; }

    [Required] public TipoArea Area { get; private set; }


    [Required] [Range(18, 150)] public int Idade { get; private set; }


    [Required] [Range(0, double.MaxValue)] public double ValorHora { get; private set; }

    [Required] public int CidadeId { get; private set; }

    public Cidade Cidade { get; private set; }

    private Cliente()
    {
    }

    public Cliente(string nome, TipoArea area, int idade, double valorHora, Cidade cidade)
    {
        SetNome(nome);
        SetArea(area);
        SetIdade(idade);
        SetValorHora(valorHora);
        SetCidade(cidade);
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

        if (!nome.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new ArgumentException(
                "O nomes deve conter apenas letras e espaços"
            );
        }

        Nome = nome;
    }

    public void SetArea(TipoArea area)
    {
        Area = area;
    }

    public void SetIdade(int idade)
    {
        if (idade < 18 || idade > 150)
        {
            throw new ArgumentException("A idade deve estar entre 18 e 150 anos");
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

    public void SetCidade(Cidade cidade)
    {
        Cidade = cidade ?? throw new ArgumentNullException(nameof(cidade), "A cidade não pode ser nula");
        CidadeId = Cidade.Id;
    }
}