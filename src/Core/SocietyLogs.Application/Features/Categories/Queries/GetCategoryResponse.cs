using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Queries
{
    // Kullanıcıya döneceğimiz JSON formatı bu olacak.
    public record GetCategoryResponse(
        Guid Id,
        string CategoryName,
        string Description,
        string Slug
    );
}
