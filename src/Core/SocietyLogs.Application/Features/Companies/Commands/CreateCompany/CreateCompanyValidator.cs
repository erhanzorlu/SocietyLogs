using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Commands.Create
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Firma adı boş olamaz.")
                .MaximumLength(100).WithMessage("Firma adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama çok uzun.");
        }
    }
}
