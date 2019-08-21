using System.Linq;
using Livraria.Domain.Entities;
using Livraria.Domain.Interfaces;
using Livraria.Infra.Data.Context;

namespace Livraria.Infra.Data.Repository
{
    public class LivroRepository : IRepository<Livro>
    {
        private readonly LivrariaContext _context;

        public LivroRepository(LivrariaContext context)
        {
            _context = context;
        }

        public void Insert(Livro obj)
        {
            _context.Set<Livro>().Add(obj);
            _context.SaveChanges();
        }

        public void Update(Livro obj)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Set<Livro>().Remove(Select(id));
            _context.SaveChanges();
        }

        public IQueryable<Livro> SelectAll()
        {
            return _context.Set<Livro>();
        }

        public Livro Select(int id)
        {
            return _context.Set<Livro>().Find(id);
        }
    }
}
