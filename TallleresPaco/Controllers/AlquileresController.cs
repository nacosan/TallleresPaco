using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallleresPaco.Models;

namespace TallleresPaco.Controllers
{
    public class AlquileresController : Controller
    {
        private readonly Contexto _context;

        public AlquileresController(Contexto context)
        {
            _context = context;
        }

        // GET: Alquileres
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Alquileres.Include(a => a.Usuario).Include(a => a.Vehiculo);
            return View(await contexto.ToListAsync());
        }

        // GET: Alquileres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquileres = await _context.Alquileres
                .Include(a => a.Usuario)
                .Include(a => a.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alquileres == null)
            {
                return NotFound();
            }

            return View(alquileres);
        }

        // GET: Alquileres/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Matricula");

            return View();
        }

        // POST: Alquileres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,VehiculoId,FechaInicio,FechaFin,Precio,PrecioFinal,Estado")] Alquileres alquileres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alquileres);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", alquileres.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Id", alquileres.VehiculoId);
            return View(alquileres);
        }

        // GET: Alquileres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquileres = await _context.Alquileres.FindAsync(id);
            if (alquileres == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", alquileres.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Matricula", alquileres.VehiculoId);
            return View(alquileres);
        }

        // POST: Alquileres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,VehiculoId,FechaInicio,FechaFin,Precio,PrecioFinal")] Alquileres alquileres)
        {
            if (id != alquileres.Id)
            {
                return NotFound();
            }

            
                try
                {
                    // Obtener el alquiler original desde la base de datos
                    var alquilerExistente = await _context.Alquileres.FindAsync(id);
                    if (alquilerExistente == null)
                        return NotFound();

                    // Actualizar manualmente los campos permitidos
                    alquilerExistente.UsuarioId = alquileres.UsuarioId;
                    alquilerExistente.VehiculoId = alquileres.VehiculoId;
                    alquilerExistente.FechaInicio = alquileres.FechaInicio;
                    alquilerExistente.FechaFin = alquileres.FechaFin;
                    alquilerExistente.Precio = alquileres.Precio;
                    alquilerExistente.PrecioFinal = alquileres.PrecioFinal;
                    // NO actualizar el campo Estado

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlquileresExists(alquileres.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", alquileres.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Matricula", alquileres.VehiculoId);
            return View(alquileres);
        }

        // GET: Alquileres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquileres = await _context.Alquileres
                .Include(a => a.Usuario)
                .Include(a => a.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alquileres == null)
            {
                return NotFound();
            }

            return View(alquileres);
        }

        // POST: Alquileres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alquiler = await _context.Alquileres.FindAsync(id);
            if (alquiler != null)
            {
                alquiler.Estado = "Cancelado";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AlquileresExists(int id)
        {
            return _context.Alquileres.Any(e => e.Id == id);
        }
    }
}
