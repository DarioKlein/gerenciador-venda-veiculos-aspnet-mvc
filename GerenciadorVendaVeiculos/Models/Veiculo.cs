using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorVendaVeiculos.Models;

[Table("Veiculo")]
public class Veiculo
{
    [Key]
    [Display(Name = "ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    [Required]
    [MaxLength(60)]
    [Display(Name = "Modelo")]
    public string Modelo { get; private set; }

    [Required] public int MarcaId { get; private set; }

    public Marca Marca { get; private set; }

    [Required] [Display(Name = "Ano")] public int Ano { get; private set; }

    [Required]
    [MaxLength(30)]
    [Display(Name = "Cor")]
    public string Cor { get; private set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    [Display(Name = "Valor")]
    public double Valor { get; private set; }

    [Required] public SituacaoVeiculo Situacao { get; private set; }

    private Veiculo()
    {
    }

    public Veiculo(string modelo, Marca marca, int ano, string cor, double valor)
    {
        SetModelo(modelo);
        SetMarca(marca);
        SetAno(ano);
        SetCor(cor);
        SetValor(valor);
        Situacao = SituacaoVeiculo.Disponivel;
    }

    public void SetModelo(string modelo)
    {
        if (string.IsNullOrEmpty(modelo))
        {
            throw new ArgumentNullException(nameof(modelo), "O modelo não pode ser nulo ou vazio");
        }

        if (modelo.Length > 60)
        {
            throw new ArgumentException("O modelo deve conter no máximo 60 caracteres");
        }

        Modelo = modelo;
    }

    public void SetMarca(Marca marca)
    {
        Marca = marca ?? throw new ArgumentNullException(nameof(marca), "A marca não pode ser nula");
        MarcaId = Marca.Id;
    }

    public void SetAno(int ano)
    {
        int anoMaximo = DateTime.Now.Year + 1;
        if (ano < 1950 || ano > anoMaximo)
        {
            throw new ArgumentException($"O ano deve estar entre 1950 e {anoMaximo}");
        }

        Ano = ano;
    }

    public void SetCor(string cor)
    {
        if (string.IsNullOrEmpty(cor))
        {
            throw new ArgumentNullException(nameof(cor), "A cor não pode ser nula ou vazia");
        }

        if (cor.Length > 30)
        {
            throw new ArgumentException("A cor deve conter no máximo 30 caracteres");
        }

        Cor = cor;
    }

    public void SetValor(double valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor deve ser maior que zero");
        }

        Valor = valor;
    }

    public void SetSituacao(SituacaoVeiculo situacao)
    {
        Situacao = situacao;
    }
}