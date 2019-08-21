using Livraria.Domain.Entities;
using Livraria.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Infra.Data.Context
{
    public class LivrariaContext : DbContext
    {
        public DbSet<Livro> Livro { get; set; }

        public LivrariaContext(DbContextOptions<LivrariaContext> options)
            : base(options)
        {
            //irá criar o banco e a estrutura de tabelas necessárias
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Livro>(new LivroMap().Configure);
        }
    }
}
