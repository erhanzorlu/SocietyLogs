using MediatR;

namespace SocietyLogs.Application.Features.Categories.Commands.RestoreCategory
{
    // Geriye işlemin başarılı olup olmadığını (bool) dönüyoruz.
    public record RestoreCategoryCommand(Guid Id) : IRequest<bool>;
}