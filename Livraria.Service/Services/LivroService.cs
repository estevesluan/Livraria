using FluentValidation;
using Livraria.Domain.Entities;
using Livraria.Domain.Interfaces;
using System;
using System.Linq;

namespace Livraria.Service.Services
{
    public class LivroService : IService<Livro>
    {
        private IRepository<Livro> _repositoryLivro;

        public LivroService(IRepository<Livro> repositoryLivro)
        {
            _repositoryLivro = repositoryLivro;
        }

        public Livro Post<V>(Livro obj) where V : AbstractValidator<Livro>
        {
            Validate(obj, Activator.CreateInstance<V>());

            _repositoryLivro.Insert(obj);
            return obj;
        }

        public Livro Put<V>(Livro obj) where V : AbstractValidator<Livro>
        {
            Validate(obj, Activator.CreateInstance<V>());

            _repositoryLivro.Update(obj);
            return obj;
        }

        public void Delete(int id)
        {
            _repositoryLivro.Delete(id);
        }

        public IQueryable<Livro> Get() {
            return _repositoryLivro.SelectAll(); ;
        } 

        public Livro Get(int id)
        {
            return _repositoryLivro.Select(id);
        }

        private void Validate<V>(Livro obj, V validator) where V : AbstractValidator<Livro>
        {
            if (obj == null)
                throw new Exception("Registros não detectados!");

            validator.ValidateAndThrow(obj);
        }
    }
}
