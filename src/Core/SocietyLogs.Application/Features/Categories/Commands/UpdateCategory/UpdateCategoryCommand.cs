using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.UpdateCategory
{
    // Güncelleme başarılıysa geriye true/false veya void dönebiliriz.
    // Ben genellikle güncellenen ID'yi veya Unit dönerim.
    public record UpdateCategoryCommand(
        Guid Id, // Hangi kategori?
        string CategoryName,
        string Description,
        string Slug
    ) : IRequest<bool>;
}
