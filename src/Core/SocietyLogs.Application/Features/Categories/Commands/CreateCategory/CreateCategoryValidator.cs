using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MaximumLength(100).WithMessage("Kategori adı 100 karakteri geçemez.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug alanı zorunludur.")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir.");

            RuleFor(x => x.Description)
                 .MaximumLength(500).WithMessage("Açıklama çok uzun.");
        }
    }
}
