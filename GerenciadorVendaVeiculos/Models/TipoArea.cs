using System.ComponentModel.DataAnnotations;

namespace GerenciadorVendaVeiculos.Models;

public enum TipoArea
{
    [Display(Name = "CLT")] Clt,

    [Display(Name = "Servidor Público")] ServidorPublico,

    [Display(Name = "Autônomo")] Autonomo,

    [Display(Name = "Empresário")] Empresario,

    [Display(Name = "Aposentado")] Aposentado,

    [Display(Name = "Estudante")] Estudante,

    [Display(Name = "Outro")] Outro
}