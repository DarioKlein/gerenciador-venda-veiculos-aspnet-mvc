using System.Diagnostics;
using System.Threading.Tasks;
using GerenciadorVendaVeiculos.Data;
using GerenciadorVendaVeiculos.Models;
using GerenciadorVendaVeiculos.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorVendaVeiculos.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var resumo = new HomeResumoViewModel
        {
            TotalVeiculos = await _context.Veiculos.CountAsync(),
            VeiculosDisponiveis = await _context.Veiculos.CountAsync(v => v.Situacao == SituacaoVeiculo.Disponivel),
            TotalClientes = await _context.Clientes.CountAsync(),
            TotalVendas = await _context.Vendas.CountAsync(),
            ValorTotalVendido = await _context.Vendas.SumAsync(v => (decimal?)v.ValorVenda) ?? 0,
        };

        return View(resumo);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}   