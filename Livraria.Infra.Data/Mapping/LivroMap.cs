using Livraria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Infra.Data.Mapping
{
    public class LivroMap : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.ToTable("Livro");

            builder.HasKey(c => c.Id);

            builder
                .Property(l => l.Titulo)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder
                .Property(l => l.Subtitulo)
                .HasColumnType("nvarchar(150)");

            builder
                .Property(l => l.Resumo)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder
                .Property(l => l.Autor)
                .HasColumnType("nvarchar(100)")
                .IsRequired(); ;

            builder
                .Property(l => l.ImagemCapa);
        }
    }
}
