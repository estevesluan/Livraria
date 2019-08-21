using Livraria.Domain.Entities;
using Livraria.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Livraria.WebApp.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;

        public LivroApiClient(HttpClient client, IHttpContextAccessor accessor)
        {
            _httpClient = client;
        }

        public async Task<byte[]> GetCapaLivroAsync(int id)
        {
            var resposta = await _httpClient.GetAsync($"livros/{id}/capa");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsByteArrayAsync();
        }

        public async Task<IEnumerable<LivroApi>> GetLivroAsync(string pesquisa)
        {
            var resposta = await _httpClient.GetAsync($"livros?pesquisa={pesquisa}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<IEnumerable<LivroApi>>();
        }

        public async Task<IEnumerable<LivroApi>> GetLivroTotalPaginaAsync(string pesquisa)
        {
            var resposta = await _httpClient.GetAsync($"livros?pesquisa={pesquisa}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<IEnumerable<LivroApi>>();
        }

        public async Task<LivroPaginacao> GetLivroPaginaAsync(string pesquisa, int numeroPagina, int numeroItensPorPagina)
        {
            var resposta = await _httpClient.GetAsync($"livros?pesquisa={pesquisa}&numeroPagina={numeroPagina}&numeroItensPorPagina={numeroItensPorPagina}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<LivroPaginacao>();
        }

        public async Task<LivroApi> GetLivroAsync(int id)
        {
            var resposta = await _httpClient.GetAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<LivroApi>();
        }

        public async Task DeleteLivroAsync(int id)
        {
            var resposta = await _httpClient.DeleteAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();
            if (resposta.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new InvalidOperationException(resposta.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task PostOrPutLivroAsync(LivroUpload livro)
        {
            HttpContent content = CreateMultipartContent(livro.ToLivro());
            var resposta = await (livro.Id == 0 ? _httpClient.PostAsync("livros", content) : _httpClient.PutAsync("livros", content));
            resposta.EnsureSuccessStatusCode();
            if (resposta.StatusCode != System.Net.HttpStatusCode.Created && resposta.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException(resposta.Content.ReadAsStringAsync().Result);
            }
        }

        private string EnvolveComAspasDuplas(string valor)
        {
            return $"\u0022{valor}\u0022";
        }

        private HttpContent CreateMultipartContent(Livro livro)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(livro.Titulo), EnvolveComAspasDuplas("titulo"));
            content.Add(new StringContent(livro.Resumo), EnvolveComAspasDuplas("resumo"));
            content.Add(new StringContent(livro.Autor), EnvolveComAspasDuplas("autor"));

            if (livro.Id > 0)
            {
                content.Add(new StringContent(Convert.ToString(livro.Id)), EnvolveComAspasDuplas("id"));
            }

            if (!string.IsNullOrEmpty(livro.Subtitulo))
            {
                content.Add(new StringContent(livro.Subtitulo), EnvolveComAspasDuplas("subtitulo"));
            }

            if (livro.ImagemCapa != null)
            {
                var imageContent = new ByteArrayContent(livro.ImagemCapa);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
                content.Add(imageContent, EnvolveComAspasDuplas("capa"), EnvolveComAspasDuplas("capa.png"));
            }

            return content;
        }
    }
}
