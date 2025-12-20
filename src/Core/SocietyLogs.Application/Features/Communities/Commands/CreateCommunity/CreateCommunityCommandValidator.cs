using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Communities.Commands.CreateCommunity
{
    public class CreateCommunityCommandValidator : AbstractValidator<CreateCommunityCommand>
    {
        public CreateCommunityCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Topluluk adı boş olamaz.")
                .MaximumLength(100).WithMessage("Topluluk adı 100 karakteri geçemez.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama alanı zorunludur.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır.");

            // İstersen dosya boyutu kontrolü de ekleyebilirsin ama şimdilik basit kalsın.
        }
    }
}
