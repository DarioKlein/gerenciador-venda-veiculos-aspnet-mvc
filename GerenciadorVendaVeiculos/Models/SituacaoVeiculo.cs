using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models;

public enum SituacaoVeiculo
{
    [Display(Name = "Disponível")] Disponivel,
    Reservado,
    Vendido,
    [Display(Name = "Em manutenção")] EmManutencao
}