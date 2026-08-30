using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorVendaVeiculos.Models;

[Table("Venda")]
public class Venda
{
    [Key]
    [Display(Name = "ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    [Required] public int ClienteId { get; private set; }

    [Display(Name = "Cliente")] public Cliente Cliente { get; private set; }

    [Required] public int VeiculoId { get; private set; }

    [Display(Name = "Veículo")] public Veiculo Veiculo { get; private set; }

    [Required]
    [Display(Name = "Data da Venda")]
    public DateTime DataVenda { get; private set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    [Display(Name = "Valor da Venda")]
    public double ValorVenda { get; private set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Valor da Causa")]
    public double ValorCausa { get; private set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Vendedor")]
    public string Vendedor { get; private set; }

    private Venda()
    {
    }

    public Venda(Cliente cliente, Veiculo veiculo, DateTime dataVenda, double valorVenda, double valorCausa,
        string vendedor)
    {
        SetCliente(cliente);
        SetVeiculo(veiculo);
        SetDataVenda(dataVenda);
        SetValorVenda(valorVenda);
        SetValorCausa(valorCausa);
        SetVendedor(vendedor);
    }

    public void SetCliente(Cliente cliente)
    {
        Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente), "O cliente não pode ser nulo");
        ClienteId = Cliente.Id;
    }

    public void SetVeiculo(Veiculo veiculo)
    {
        Veiculo = veiculo ?? throw new ArgumentNullException(nameof(veiculo), "O veículo não pode ser nulo");
        VeiculoId = Veiculo.Id;
    }

    public void SetDataVenda(DateTime dataVenda)
    {
        if (dataVenda > DateTime.Now)
        {
            throw new ArgumentException("A data da venda não pode ser futura");
        }

        DataVenda = dataVenda;
    }

    public void SetValorVenda(double valorVenda)
    {
        if (valorVenda <= 0)
        {
            throw new ArgumentException("O valor da venda deve ser maior que zero");
        }

        ValorVenda = valorVenda;
    }

    public void SetValorCausa(double valorCausa)
    {
        if (valorCausa < 0)
        {
            throw new ArgumentException("O valor da causa não pode ser negativo");
        }

        ValorCausa = valorCausa;
    }

    public void SetVendedor(string vendedor)
    {
        if (string.IsNullOrEmpty(vendedor))
        {
            throw new ArgumentNullException(nameof(vendedor), "O vendedor não pode ser nulo ou vazio");
        }

        if (vendedor.Length > 100)
        {
            throw new ArgumentException("O vendedor deve conter no máximo 100 caracteres");
        }

        Vendedor = vendedor;
    }
}