using System;
using System.Linq;
using Livraria.Domain.Entities;
using Livraria.Domain.Extensions;
using Livraria.Domain.Interfaces;
using Livraria.Service.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Livraria.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private IService<Livro> _serviceLivro;

        public LivrosController(IService<Livro> serviceLivro)
        {
            _serviceLivro = serviceLivro;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult Post([FromForm] LivroUpload item)
        {
            try
            {
                _serviceLivro.Post<LivroValidator>(item.ToLivro());

                var uri = Url.Action("Get", new { id = item.Id });
                return Created(uri, item);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [DisableRequestSizeLimit]
        public IActionResult Put([FromForm] LivroUpload item)
        {
            try
            {
                Livro livro = item.ToLivro();

                if (item.Capa == null)
                {
                    livro.ImagemCapa = _serviceLivro.Get().Where(x => x.Id == item.Id).Select(s => s.ImagemCapa).FirstOrDefault();
                }

                _serviceLivro.Put<LivroValidator>(livro);

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _serviceLivro.Delete(id);

                return new NoContentResult();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}/capa")]
        public IActionResult ImagemCapa(int id)
        {
            byte[] img = _serviceLivro.Get(id)?.ImagemCapa;

            if (img != null)
            {
                return File(img, "image/png");
            }
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(_serviceLivro.Get(id));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult Get(string pesquisa, int numeroPagina, int numeroItensPorPagina)
        {
            try
            {
                IQueryable<Livro> lista = _serviceLivro.Get();

                if (!String.IsNullOrEmpty(pesquisa))
                {
                    lista = lista.Where(x => x.Titulo.ToUpper().Contains(pesquisa.ToUpper()));
                }

                LivroPaginacao livroPaginacao = new LivroPaginacao();
                livroPaginacao.NumeroPagina = numeroPagina;
                livroPaginacao.NumeroItensPorPagina = numeroItensPorPagina;
                livroPaginacao.Total = (int)Math.Ceiling((lista.Count() / (double) numeroItensPorPagina));

                lista = lista.Skip((numeroPagina - 1) * numeroItensPorPagina).Take(numeroItensPorPagina);

                livroPaginacao.Livros = lista.OrderBy(x => x.Titulo).ToList();

                return new ObjectResult(livroPaginacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}