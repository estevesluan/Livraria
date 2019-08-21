using System;
using System.Threading.Tasks;
using Livraria.Domain.Entities;
using Livraria.WebApp.HttpClients;
using Livraria.WebApp.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Livraria.WebApp.Controllers
{
    public class LivroController : Controller
    {
        private readonly IHubContext<LivroHub> _hubLivro;
        private readonly LivroApiClient _api;

        public LivroController(LivroApiClient api, IHubContext<LivroHub> hubLivro)
        {
            _api = api;
            _hubLivro = hubLivro;
        }

        [HttpGet]
        public IActionResult Cadastro(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dados(int id)
        {
            var model = await _api.GetLivroAsync(id);
            return Json(model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Cadastro(LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PostOrPutLivroAsync(model);
                    ModelState.Clear();
                    await _hubLivro.Clients.All.SendAsync("AtualizarLista");
                    return View();
                }
                catch (Exception e)
                {
                    return Json(new { erro = e.Message });
                }

            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Detalhes(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Remover(int id)
        {
            var model = await _api.GetLivroAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await _api.DeleteLivroAsync(id);
                await _hubLivro.Clients.All.SendAsync("AtualizarLista");
            }
            catch (Exception e)
            {
                return Json(new { erro = e.Message });
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Lista(string pesquisa)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaLivros(string pesquisa)
        {
            var lista = await _api.GetLivroAsync(pesquisa);

            return Json(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ListaLivrosTotalPaginas(string pesquisa)
        {
            var lista = await _api.GetLivroAsync(pesquisa);

            return Json(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ListaLivrosPagina(string pesquisa, int numeroPagina, int numeroItensPorPagina)
        {
            LivroPaginacao livroPaginacao = await _api.GetLivroPaginaAsync(pesquisa, numeroPagina, numeroItensPorPagina);

            return Json(livroPaginacao);
        }

        [HttpGet]
        public async Task<IActionResult> ImagemCapa(int id)
        {
            byte[] img = await _api.GetCapaLivroAsync(id);
            if (img != null)
            {
                return File(img, "image/png");
            }
            return null;
        }
    }
}
