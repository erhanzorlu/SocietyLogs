using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Queries.GetAdminCategories
{
    public record GetAdminCategoriesQuery : IRequest<List<GetAdminCategoriesResponse>>;
}
