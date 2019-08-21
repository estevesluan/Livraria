using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Livraria.WebApp.Models;

namespace Livraria.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return RedirectToAction("Cadastro", "Livro");
        }

        public IActionResult Lista()
        {
            return RedirectToAction("Lista", "Livro");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
