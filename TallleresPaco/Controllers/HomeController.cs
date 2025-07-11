using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SQLitePCL;
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

        public IActionResult Index()
        {
            string email = User.Identity.Name;
            if (email != null)
            {
                var usu = _contexto.Usuarios
                          .Where(u => u.Email == email)
                          .Select(u => new { u.Estado, u.Id })
                          .FirstOrDefault();
                Console.WriteLine(usu.Id);
                if (usu.Estado == "Pendiente")
                {
                    return RedirectToAction("Edit", "Usuarios", new { id = usu.Id });    // Usuarios/Edit/5
                }
            }
            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
