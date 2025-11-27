using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Queries.GetAllCategories
{
    // Geriye "Liste" halinde Response döneceğiz.
    public record GetAllCategoriesQuery : IRequest<List<GetCategoryResponse>>;
}
