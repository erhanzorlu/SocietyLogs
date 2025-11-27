using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CategoryName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Slug).NotEmpty();
        }
    }
}
