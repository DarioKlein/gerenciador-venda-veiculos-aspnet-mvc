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
    public class MarcaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarcaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Marca
        public async Task<IActionResult> Index()
        {
            return View(await _context.Marcas.ToListAsync());
        }

        // GET: Marca/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: Marca/Create
        public IActionResult Create()
        {
            return View(new MarcaViewModel());
        }

        // POST: Marca/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarcaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var nomeExiste = await _context.Marcas
                    .AnyAsync(c => c.Nome == viewModel.Nome);

                if (nomeExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Nome),
                        "Já existe uma marca com esse nome."
                    );
                }

                var siglaExiste = await _context.Marcas
                    .AnyAsync(c => c.Sigla == viewModel.Sigla);

                if (siglaExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Sigla),
                        "Já existe uma marca com essa sigla."
                    );
                }

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                
                try
                {
                    var marca = new Marca(viewModel.Nome, viewModel.Sigla);
                    _context.Add(marca);
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

        // GET: Marca/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }

            var viewModel = new MarcaViewModel
            {
                Id = marca.Id,
                Nome = marca.Nome,
                Sigla = marca.Sigla
            };

            return View(viewModel);
        }

        // POST: Marca/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MarcaViewModel viewModel)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var nomeExiste = await _context.Marcas
                    .AnyAsync(c => c.Nome == viewModel.Nome
                                   && c.Id != viewModel.Id);

                if (nomeExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Nome),
                        "Já existe uma marca com esse nome."
                    );
                }

                var siglaExiste = await _context.Marcas
                    .AnyAsync(c => c.Sigla == viewModel.Sigla
                                   && c.Id != viewModel.Id);

                if (siglaExiste)
                {
                    ModelState.AddModelError(
                        nameof(viewModel.Sigla),
                        "Já existe uma marca com essa sigla."
                    );
                }

                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                try
                {
                    marca.SetNome(viewModel.Nome);
                    marca.SetSigla(viewModel.Sigla);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(id))
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

        // GET: Marca/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // POST: Marca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca != null)
            {
                _context.Marcas.Remove(marca);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(int id)
        {
            return _context.Marcas.Any(e => e.Id == id);
        }
    }
}