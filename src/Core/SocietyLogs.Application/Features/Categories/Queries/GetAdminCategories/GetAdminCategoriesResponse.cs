using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Queries.GetAdminCategories
{
    public record GetAdminCategoriesResponse(
            Guid Id,
            string CategoryName,
            string Slug,
            bool IsDeleted,          // Silinmiş mi? (Kırmızı işaretlemek için)
            DateTime CreatedDate,    // Ne zaman oluşturuldu?
            DateTime? UpdatedDate,   // Ne zaman güncellendi?
            DateTime? DeletedDate    // Ne zaman silindi?
        );
}
