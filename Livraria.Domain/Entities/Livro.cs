using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Livraria.Domain.Entities
{
    public class Livro : BaseEntity
    {
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Resumo { get; set; }
        public byte[] ImagemCapa { get; set; }
        public string Autor { get; set; }
    }

    [XmlType("Livro")]
    public class LivroApi : BaseEntity
    {
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Resumo { get; set; }
        public string ImagemCapa { get; set; }
        public string Autor { get; set; }
    }

    public class LivroUpload: BaseEntity
    {
        [Required]
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        [Required]
        public string Autor { get; set; }
        [Required]
        public string Resumo { get; set; }
        public IFormFile Capa { get; set; }
    }

    public class LivroPaginacao
    {
        public int Total { get; set; }
        public int NumeroItensPorPagina { get; set; }
        public int NumeroPagina { get; set; }
        public IEnumerable<Livro> Livros { get; set; }
    }
}
