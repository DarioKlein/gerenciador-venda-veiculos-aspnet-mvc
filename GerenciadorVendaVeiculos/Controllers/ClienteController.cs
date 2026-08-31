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
    public class ClienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Clientes.Include(c => c.Cidade);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Cidade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            ViewData["CidadeId"] = new SelectList(_context.Cidades, "Id", "Descricao");
            return View(new ClienteViewModel());
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cidade = await _context.Cidades.FindAsync(viewModel.CidadeId);
                    if (cidade == null)
                    {
                        ModelState.AddModelError("", "Cidade inválida");
                        ViewData["CidadeId"] = new SelectList(_context.Cidades, "Id", "Descricao", viewModel.CidadeId);
                        return View(viewModel);
                    }

                    var cliente = new Cliente(viewModel.Nome, viewModel.Area, viewModel.Idade, viewModel.ValorHora,
                        cidade);

                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewData["CidadeId"] = new SelectList(_context.Cidades, "Id", "Descricao", viewModel.CidadeId);
            return View(viewModel);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            var viewModel = new ClienteViewModel
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Area = cliente.Area,
                Idade = cliente.Idade,
                ValorHora = cliente.ValorHora,
                CidadeId = cliente.CidadeId
            };

            ViewData["CidadeId"] = new SelectList(_context.Cidades, "Id", "Descricao", cliente.CidadeId);
            return View(viewModel);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel viewModel)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cidade = await _context.Cidades.FindAsync(viewModel.CidadeId);
                    if (cidade == null)
                    {
                        ModelState.AddModelError("", "Cidade inválida");
                        ViewData["CidadeId"] = new SelectList(_context.Cidades, "Id", "Descricao", viewModel.CidadeId);
                        return View(viewModel);
                    }

                    cliente.SetNome(viewModel.Nome);
                    cliente.SetArea(viewModel.Area);
                    cliente.SetIdade(viewModel.Idade);
                    cliente.SetValorHora(viewModel.ValorHora);
                    cliente.SetCidade(cidade);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(id))
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

            ViewData["CidadeId"] = new SelectList(_context.Cidades, "Id", "Descricao", viewModel.CidadeId);
            return View(viewModel);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Cidade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}