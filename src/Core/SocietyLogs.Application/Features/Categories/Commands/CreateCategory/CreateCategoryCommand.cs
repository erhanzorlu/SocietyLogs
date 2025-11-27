using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.CreateCategory
{
    // Geriye Guid (oluşan ID) döneceğiz.
    public record CreateCategoryCommand(
        string CategoryName,
        string Description,
        string Slug
    ) : IRequest<Guid>;
}
