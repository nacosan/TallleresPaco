using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallleresPaco.Models;

namespace TallleresPaco.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _contexto;

        public HomeController(ILogger<HomeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        public async Task<IActionResult> Index()
        {
            string email = User.Identity?.Name;

            if (!string.IsNullOrEmpty(email))
            {
                var usuario = await _contexto.Usuarios
                    .Where(u => u.Email == email)
                    .Select(u => new { u.Estado, u.Id })
                    .FirstOrDefaultAsync();

                if (usuario != null && usuario.Estado == "Pendiente")
                {
                    // Redirigir a la acción Edit para completar el perfil
                    return RedirectToAction("Edit", "Usuarios", new { id = usuario.Id });
                }
            }

            var vehiculos = await _contexto.Vehiculos.ToListAsync();

            return View(vehiculos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
