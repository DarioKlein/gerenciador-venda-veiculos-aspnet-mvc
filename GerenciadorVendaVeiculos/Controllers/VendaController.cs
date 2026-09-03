using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorVendaVeiculos.Data;
using GerenciadorVendaVeiculos.Models;
using GerenciadorVendaVeiculos.Models.ViewModels;

namespace GerenciadorVendaVeiculos.Controllers
{
    public class VendaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Venda
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vendas.Include(v => v.Cliente).Include(v => v.Veiculo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Venda/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Venda/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["VeiculoId"] =
                new SelectList(_context.Veiculos.Where(v => v.Situacao == SituacaoVeiculo.Disponivel), "Id", "Modelo");
            return View(new VendaViewModel { DataVenda = DateTime.Now });
        }

        // POST: Venda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VendaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _context.Clientes.FindAsync(viewModel.ClienteId);
                    var veiculo = await _context.Veiculos.FindAsync(viewModel.VeiculoId);

                    if (cliente == null || veiculo == null)
                    {
                        ModelState.AddModelError("", "Cliente ou veículo inválido");
                        ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", viewModel.ClienteId);
                        ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", viewModel.VeiculoId);
                        return View(viewModel);
                    }

                    if (veiculo.Situacao != SituacaoVeiculo.Disponivel)
                    {
                        ModelState.AddModelError("", "Veículo com status não disponível");
                        ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", viewModel.ClienteId);
                        ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", viewModel.VeiculoId);
                        return View(viewModel);
                    }

                    var venda = new Venda(cliente, veiculo, viewModel.DataVenda, viewModel.ValorVenda,
                        viewModel.ValorCausa, viewModel.Vendedor);


                    veiculo.SetSituacao(SituacaoVeiculo.Vendido);
                    _context.Add(venda);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", viewModel.ClienteId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", viewModel.VeiculoId);
            return View(viewModel);
        }

        // GET: Venda/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            var viewModel = new VendaViewModel
            {
                Id = venda.Id,
                ClienteId = venda.ClienteId,
                VeiculoId = venda.VeiculoId,
                DataVenda = venda.DataVenda,
                ValorVenda = venda.ValorVenda,
                ValorCausa = venda.ValorCausa,
                Vendedor = venda.Vendedor
            };

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", venda.ClienteId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", venda.VeiculoId);
            return View(viewModel);
        }

        // POST: Venda/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VendaViewModel viewModel)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _context.Clientes.FindAsync(viewModel.ClienteId);
                    var veiculo = await _context.Veiculos.FindAsync(viewModel.VeiculoId);

                    if (cliente == null || veiculo == null)
                    {
                        ModelState.AddModelError("", "Cliente ou veículo inválido");
                        ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", viewModel.ClienteId);
                        ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", viewModel.VeiculoId);
                        return View(viewModel);
                    }

                    bool trocouVeiculo = veiculo.Id != venda.VeiculoId;

                    if (trocouVeiculo)
                    {
                        if (veiculo.Situacao != SituacaoVeiculo.Disponivel)
                        {
                            ModelState.AddModelError("", "Veículo com status não disponível");
                            ViewData["ClienteId"] =
                                new SelectList(_context.Clientes, "Id", "Nome", viewModel.ClienteId);
                            ViewData["VeiculoId"] =
                                new SelectList(_context.Veiculos, "Id", "Modelo", viewModel.VeiculoId);
                            return View(viewModel);
                        }

                        var veiculoAntigo = await _context.Veiculos.FindAsync(venda.VeiculoId);
                        veiculoAntigo?.SetSituacao(SituacaoVeiculo.Disponivel);

                        veiculo.SetSituacao(SituacaoVeiculo.Vendido);
                    }

                    venda.SetCliente(cliente);
                    venda.SetVeiculo(veiculo);
                    venda.SetDataVenda(viewModel.DataVenda);
                    venda.SetValorVenda(viewModel.ValorVenda);
                    venda.SetValorCausa(viewModel.ValorCausa);
                    venda.SetVendedor(viewModel.Vendedor);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nome", viewModel.ClienteId);
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Modelo", viewModel.VeiculoId);
            return View(viewModel);
        }

        // GET: Venda/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Venda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.Include(v => v.Veiculo).FirstOrDefaultAsync(v => v.Id == id);
            if (venda != null)
            {
                venda.Veiculo.SetSituacao(SituacaoVeiculo.Disponivel);
                _context.Vendas.Remove(venda);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}