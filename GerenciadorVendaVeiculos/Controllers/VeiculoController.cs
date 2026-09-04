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
    public class VeiculoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Veiculo
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Veiculos.Include(v => v.Marca);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Veiculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .Include(v => v.Marca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Veiculo/Create
        public IActionResult Create()
        {
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nome");

            ViewData["Situacoes"] = new List<SelectListItem>
            {
                new()
                {
                    Value = SituacaoVeiculo.Disponivel.ToString(),
                    Text = "Disponível"
                },
                new()
                {
                    Value = SituacaoVeiculo.EmManutencao.ToString(),
                    Text = "Em manutenção"
                }
            };
            return View(new VeiculoViewModel());
        }

        // POST: Veiculo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VeiculoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var marca = await _context.Marcas.FindAsync(viewModel.MarcaId);
                    if (marca == null)
                    {
                        ModelState.AddModelError("", "Marca inválida");
                        ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nome", viewModel.MarcaId);
                        return View(viewModel);
                    }

                    var veiculo = new Veiculo(viewModel.Modelo, marca, viewModel.Ano, viewModel.Cor, viewModel.Valor);
                    veiculo.SetSituacao(viewModel.Situacao);

                    _context.Add(veiculo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nome", viewModel.MarcaId);
            return View(viewModel);
        }

        // GET: Veiculo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            var viewModel = new VeiculoViewModel
            {
                Id = veiculo.Id,
                Modelo = veiculo.Modelo,
                MarcaId = veiculo.MarcaId,
                Ano = veiculo.Ano,
                Cor = veiculo.Cor,
                Valor = veiculo.Valor,
                Situacao = veiculo.Situacao
            };

            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nome", veiculo.MarcaId);
            return View(viewModel);
        }

        // POST: Veiculo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VeiculoViewModel viewModel)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var marca = await _context.Marcas.FindAsync(viewModel.MarcaId);
                    if (marca == null)
                    {
                        ModelState.AddModelError("", "Marca inválida");
                        ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nome", viewModel.MarcaId);
                        return View(viewModel);
                    }

                    veiculo.SetModelo(viewModel.Modelo);
                    veiculo.SetMarca(marca);
                    veiculo.SetAno(viewModel.Ano);
                    veiculo.SetCor(viewModel.Cor);
                    veiculo.SetValor(viewModel.Valor);
                    veiculo.SetSituacao(viewModel.Situacao);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(id))
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

            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nome", viewModel.MarcaId);
            return View(viewModel);
        }

        // GET: Veiculo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .Include(v => v.Marca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound();
            }

            var possuiVendas = await _context.Vendas.AnyAsync(v => v.VeiculoId == id);

            if (possuiVendas)
            {
                TempData["Erro"] = "Não foi possível excluir o veículo porque ele possui vendas relacionadas.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Veiculos.Remove(veiculo);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Veículo excluído com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Erro"] = "Não foi possível excluir o veículo porque ele está relacionado a outro registro.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}