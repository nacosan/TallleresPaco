
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallleresPaco.Models;
/*using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;*/
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

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
        [Authorize(Roles = "User")]
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
        [Authorize]
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
            var userId = GetLoggedUserId();
            var historico = await _context.Alquileres
                .Where(a => a.Id == userId)
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


            byte[] pdfBytes = GenerarPDFVehiculos(vehiculos);

            return File(pdfBytes, "application/pdf", "Listado_Vehiculos.pdf");
            /*// Generar PDF con QuestPDF --> Como peta la funcion tambien peta aqui
            var pdfBytes = GenerateVehiculosPdf(vehiculos);

            return File(pdfBytes, "application/pdf", "Vehiculos.pdf");*/
        }
        private byte[] GenerarPDFVehiculos(List<VehiculoReporte> vehiculos)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Título del documento
                var titulo = new Paragraph("Listado de Vehículos")
                    .SetFontSize(20)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginBottom(20);
                document.Add(titulo);

                // Crear tabla
                iText.Layout.Element.Table tabla = new iText.Layout.Element.Table(8).UseAllAvailableWidth();

                // Encabezados
                string[] encabezados = new[] {
            "ID", "Matrícula", "Modelo", "Marca", "Color", "Año Fab.", "Precio", "Categoría"
        };

                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                foreach (var header in encabezados)
                {
                    tabla.AddHeaderCell(new Cell()
                        .Add(new Paragraph(header).SetFont(fontBold))
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                }

                // Filas de datos
                foreach (var v in vehiculos)
                {
                    tabla.AddCell(new Paragraph(v.IdVehiculo.ToString()));
                    tabla.AddCell(new Paragraph(v.Matricula));
                    tabla.AddCell(new Paragraph(v.Modelo));
                    tabla.AddCell(new Paragraph(v.Marca));
                    tabla.AddCell(new Paragraph(v.Color));
                    tabla.AddCell(new Paragraph(v.AnioFab.Year.ToString()));
                    tabla.AddCell(new Paragraph(v.Precio.ToString("C")));
                    tabla.AddCell(new Paragraph(v.Categoria));
                }

                document.Add(tabla);

                document.Close();
                return memoryStream.ToArray();
            }
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
                    UsuarioNombre = "", 
                    VehiculoMatricula = a.Vehiculo.Matricula,
                    FechaInicio = a.FechaInicio,
                    FechaFin = a.FechaFin,
                    Precio = a.Precio,
                    Estado = a.Estado
                }).ToListAsync();

            var vm = new ReportesViewModel { ReservasUltimosDosMeses = reservas };
            return View("Manage/ReservasRecientes", vm);
        }

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
