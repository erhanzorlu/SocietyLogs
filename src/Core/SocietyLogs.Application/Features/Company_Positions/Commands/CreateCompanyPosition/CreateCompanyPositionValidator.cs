using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.CreateCompanyPosition
{
    public class CreateCompanyPositionValidator : AbstractValidator<CreateCompanyPositionCommand>
    {
        public CreateCompanyPositionValidator()
        {
            // 1. Pozisyon Adı Kontrolleri
            RuleFor(x => x.PositionName)
                .NotEmpty().WithMessage("Pozisyon adı boş bırakılamaz.") // Boş string "" veya null kontrolü
                .NotNull().WithMessage("Pozisyon adı gereklidir.")
                .MinimumLength(2).WithMessage("Pozisyon adı en az 2 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Pozisyon adı 100 karakterden uzun olamaz.");

            // 2. Şirket ID Kontrolü (Guid Empty olmamalı)
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Bir şirket seçilmelidir (Şirket ID boş olamaz).")
                .NotNull();

            // 3. Gereksinimler (Requirements) Kontrolü
            // Bu alan nullable (?) olduğu için boş gelebilir, ama dolu gelirse sınır koymalıyız.
            RuleFor(x => x.Requirements)
                .MaximumLength(500).WithMessage("Gereksinimler alanı en fazla 500 karakter olabilir.");
        }
    }
}
