using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TallleresPaco.Models;

namespace TallleresPaco.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;

        public UsuariosController(Contexto context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios
                         .Where(u => u.Estado != "Baja")
                         .ToListAsync());

        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,FechaNacimiento,Dni,Estado")] UsuarioRegistroViewModel ur)
        {
            if (ModelState.IsValid)
            {
                Usuarios usuarios = new Usuarios() { 
                    Nombre = ur.Nombre,
                    Apellido = ur.Apellido,
                    FechaNacimiento = ur.FechaNacimiento,
                    Dni = ur.Dni,
                    Estado = ur.Estado,
                };
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ur);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var vm = new UsuarioRegistroViewModel
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                FechaNacimiento = usuario.FechaNacimiento,
                Dni = usuario.Dni,
                Estado = usuario.Estado
            };

            return View(vm); 
        }
        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Edit(int id, UsuarioRegistroViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Actualizar los campos
            usuario.Nombre = vm.Nombre;
            usuario.Apellido = vm.Apellido;
            usuario.FechaNacimiento = vm.FechaNacimiento;
            usuario.Dni = vm.Dni;
            usuario.Estado = vm.Estado;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Estado = "Baja";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Manage()
        {
            return View("Manage/Index");

        }

        // 1. Obtener usuarios registrados últimos 2 meses
        public async Task<IActionResult> UsuariosRecientes()
        {
            var dosMesesAtras = DateTime.Now.AddMonths(-2);

            var usuarios = await _context.Usuarios
                .Where(u => u.FechaNacimiento >= dosMesesAtras && u.Estado == "Activo")
                .Select(u => new UsuarioReporte
                {
                    Id = u.Id,
                    NombreCompleto = u.Nombre + " " + u.Apellido,
                    FechaRegistro = u.FechaNacimiento,
                    Email = u.Email
                })
                .ToListAsync();

            var vm = new ReportesViewModel { UsuariosRecientes = usuarios };
            return View("Manage/UsuariosRecientes", vm);
        }

        // 2. Histórico de alquileres por usuario
        public async Task<IActionResult> HistoricoAlquileres()
        {
            var historico = await _context.Alquileres
                .Include(a => a.Usuario) // incluye usuario
                .Include(a => a.Vehiculo) // incluye vehículo
                .Select(a => new AlquilerReporte
                {
                    IdAlquiler = a.Id,
                    UsuarioNombre = a.Usuario.Nombre + " " + a.Usuario.Apellido,
                    VehiculoMatricula = a.Vehiculo.Matricula,
                    FechaInicio = a.FechaInicio,
                    FechaFin = a.FechaFin,
                    Precio = a.Precio,
                    Estado = a.Estado
                }).ToListAsync();

            var vm = new ReportesViewModel { HistoricoAlquileres = historico };
            return View("Manage/HistoricoAlquileres", vm);
        }

        // 3. Descargar PDF con todos los vehículos (solo paso datos, el PDF se genera en la vista o servicio)
        public async Task<IActionResult> DescargarVehiculosPdf()
        {
            var vehiculos = await _context.Vehiculos
                .Select(v => new VehiculoReporte
                {
                    IdVehiculo = v.Id,
                    Matricula = v.Matricula,
                    Modelo = v.Modelo,
                    Marca = v.Marca,
                    Color = v.Color,
                    AnioFab = v.AnioFab,
                    Precio = v.Precio,
                    Categoria = v.Categoria
                }).ToListAsync();

            var vm = new ReportesViewModel { Vehiculos = vehiculos };
            return View(vm);
        }

        // 4. Reporte escudería: nombre + total alquileres
        public async Task<IActionResult> ReporteEscuderia()
        {
            var query = await (from a in _context.Alquileres
                               join v in _context.Vehiculos on a.VehiculoId equals v.Id
                               group a by v.Marca into g
                               select new EscuderiaReporte
                               {
                                   NombreEscuderia = g.Key,
                                   TotalAlquileres = g.Count()
                               }).ToListAsync();

            var vm = new ReportesViewModel { Escuderias = query };
            return View("Manage/ReporteEscuderia", vm);
        }

        // 5. Consultar reservas usuario (supongo que id usuario es el logueado, lo puedes ajustar)
        public async Task<IActionResult> ConsultarReservas()
        {
            // Ejemplo con usuario logueado (ajustar con autenticación)
            var userId = GetLoggedUserId();

            var reservas = await _context.Alquileres
                .Where(a => a.Id == userId)
                .Include(a => a.Vehiculo)
                .Select(a => new AlquilerReporte
                {
                    IdAlquiler = a.Id,
                    UsuarioNombre = "", // no es necesario porque es el usuario actual
                    VehiculoMatricula = a.Vehiculo.Matricula,
                    FechaInicio = a.FechaInicio,
                    FechaFin = a.FechaFin,
                    Precio = a.Precio,
                    Estado = a.Estado
                }).ToListAsync();

            var vm = new ReportesViewModel { ReservasUsuario = reservas };
            return View("Manage/ConsultarReservas", vm);
        }

        // 6. Consultar reservas últimos 2 meses (para el usuario logueado)
        public async Task<IActionResult> ReservasRecientes()
        {
            var userId = GetLoggedUserId();
            var dosMesesAtras = DateTime.Now.AddMonths(-2);

            var reservas = await _context.Alquileres
                .Where(a => a.Id == userId && a.FechaInicio >= dosMesesAtras)
                .Include(a => a.Vehiculo)
                .Select(a => new AlquilerReporte
                {
                    IdAlquiler = a.Id,
                    UsuarioNombre = "", // usuario actual
                    VehiculoMatricula = a.Vehiculo.Matricula,
                    FechaInicio = a.FechaInicio,
                    FechaFin = a.FechaFin,
                    Precio = a.Precio,
                    Estado = a.Estado
                }).ToListAsync();

            var vm = new ReportesViewModel { ReservasUltimosDosMeses = reservas };
            return View("Manage/ReservasRecientes", vm);
        }

        // Método auxiliar ficticio para obtener el id de usuario actual (ajustar según auth real)
        private int GetLoggedUserId()
        {
            string mail = User.Identity.Name;
            int usuId = 0;
            if (mail != null)
            {
                usuId = _context.Usuarios
                          .Where(u => u.Email == mail)
                          .Select(u => u.Id )
                          .FirstOrDefault();
            }
            return usuId; 
        }
    }
}
