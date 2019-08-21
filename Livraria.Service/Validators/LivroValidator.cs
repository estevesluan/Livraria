using FluentValidation;
using Livraria.Domain.Entities;
using System;

namespace Livraria.Service.Validators
{
    public class LivroValidator : AbstractValidator<Livro>
    {
        public LivroValidator()
        {
            RuleFor(c => c)
                    .NotNull()
                    .OnAnyFailure(x =>
                    {
                        throw new ArgumentNullException("Objeto não definido.");
                    });

            RuleFor(c => c.Titulo)
                .NotEmpty().WithMessage("Não informado o Título.")
                .NotNull().WithMessage("Não informado o Título.");

            RuleFor(c => c.Resumo)
                .NotEmpty().WithMessage("Não informado o Resumo.")
                .NotNull().WithMessage("Não informado o Resumo.");

            RuleFor(c => c.Autor)
                .NotEmpty().WithMessage("Não informado o Autor.")
                .NotNull().WithMessage("Não informado o Autor.");
        }
    }
}
