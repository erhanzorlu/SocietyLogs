using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;
}
