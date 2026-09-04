using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorVendaVeiculos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorVendaVeiculos.Models;
using GerenciadorVendaVeiculos.Models.ViewModels;

namespace GerenciadorVendaVeiculos.Controllers
{
    public class CidadeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CidadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cidade
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cidades.ToListAsync());
        }

        // GET: Cidade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidade = await _context.Cidades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cidade == null)
            {
                return NotFound();
            }

            return View(cidade);
        }

        // GET: Cidade/Create
        public IActionResult Create()
        {
            return View(new CidadeViewModel());
        }

        // POST: Cidade/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CidadeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var descricaoExiste = await _context.Cidades
                    .AnyAsync(c => c.Descricao == viewModel.Descricao);

                if (descricaoExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Descricao),
                        "Já existe uma cidade com essa descrição."
                    );
                }

                var siglaExiste = await _context.Cidades
                    .AnyAsync(c => c.Sigla == viewModel.Sigla);

                if (siglaExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Sigla),
                        "Já existe uma cidade com essa sigla."
                    );
                }

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                try
                {
                    var cidade = new Cidade(viewModel.Descricao, viewModel.Sigla);
                    _context.Add(cidade);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(viewModel);
        }

        // GET: Cidade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidade = await _context.Cidades.FindAsync(id);
            if (cidade == null)
            {
                return NotFound();
            }

            var viewModel = new CidadeViewModel
            {
                Id = cidade.Id,
                Descricao = cidade.Descricao,
                Sigla = cidade.Sigla
            };

            return View(viewModel);
        }

        // POST: Cidade/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CidadeViewModel viewModel)
        {
            var cidade = await _context.Cidades.FindAsync(id);
            if (cidade == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var descricaoExiste = await _context.Cidades
                    .AnyAsync(c => c.Descricao == viewModel.Descricao
                                   && c.Id != viewModel.Id);

                if (descricaoExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Descricao),
                        "Já existe uma cidade com essa descrição."
                    );
                }

                var siglaExiste = await _context.Cidades
                    .AnyAsync(c => c.Sigla == viewModel.Sigla
                                   && c.Id != viewModel.Id);

                if (siglaExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Sigla),
                        "Já existe uma cidade com essa sigla."
                    );
                }

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                try
                {
                    cidade.SetDescricao(viewModel.Descricao);
                    cidade.SetSigla(viewModel.Sigla);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CidadeExists(id))
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

            return View(viewModel);
        }

        // GET: Cidade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cidade = await _context.Cidades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cidade == null)
            {
                return NotFound();
            }

            return View(cidade);
        }

        // POST: Cidade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cidade = await _context.Cidades.FindAsync(id);

            if (cidade == null)
            {
                return NotFound();
            }

            var possuiClientes = await _context.Clientes.AnyAsync(c => c.CidadeId == id);

            if (possuiClientes)
            {
                TempData["Erro"] = "Não foi possível excluir a cidade porque ela possui clientes relacionados.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Cidades.Remove(cidade);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Cidade excluída com sucesso.";
            }
            catch (DbUpdateException)
            {
                TempData["Erro"] = "Não foi possível excluir a cidade porque ela está relacionada a outro registro.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CidadeExists(int id)
        {
            return _context.Cidades.Any(e => e.Id == id);
        }
    }
}
