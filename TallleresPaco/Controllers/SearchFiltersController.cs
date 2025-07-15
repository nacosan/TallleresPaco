using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallleresPaco.Models;

namespace TallleresPaco.Controllers
{
    public class SearchFiltersController : Controller
    {
        private readonly Contexto _context;

        public SearchFiltersController(Contexto context)
        {
            _context = context;
        }
        // GET: SearchFiltersController
        public async Task<ActionResult> Index(SearchFilters filters)
        {
            IQueryable<Vehiculos> consulta = _context.Vehiculos;

            if (!string.IsNullOrEmpty(filters.Marca))
                consulta = consulta.Where(v => v.Marca.Contains(filters.Marca));

            if (!string.IsNullOrEmpty(filters.Modelo))
                consulta = consulta.Where(v => v.Modelo.Contains(filters.Modelo));

            if (filters.AnioFab.HasValue)
                consulta = consulta.Where(v => v.AnioFab == filters.AnioFab.Value);

            if (!string.IsNullOrEmpty(filters.Tipo))
                consulta = consulta.Where(v => v.Tipo.Contains(filters.Tipo));

            if (!string.IsNullOrEmpty(filters.Categoria))
                consulta = consulta.Where(v => v.Categoria.Contains(filters.Categoria));

            if (filters.PrecioMin.HasValue)
                consulta = consulta.Where(v => v.Precio >= filters.PrecioMin.Value);

            if (filters.PrecioMax.HasValue)
                consulta = consulta.Where(v => v.Precio <= filters.PrecioMax.Value);

            ViewBag.Marcas = await _context.Vehiculos.Select(v => v.Marca).Distinct().ToListAsync();
            ViewBag.Modelos = await _context.Vehiculos.Select(v => v.Modelo).Distinct().ToListAsync();
            ViewBag.Tipos = await _context.Vehiculos.Select(v => v.Tipo).Distinct().ToListAsync();
            ViewBag.Categorias = await _context.Vehiculos.Select(v => v.Categoria).Distinct().ToListAsync();

            var vehiculosFiltrados = await consulta.ToListAsync();
            return View(vehiculosFiltrados);
        }



        // GET: SearchFiltersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SearchFiltersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchFiltersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchFiltersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SearchFiltersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchFiltersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SearchFiltersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
