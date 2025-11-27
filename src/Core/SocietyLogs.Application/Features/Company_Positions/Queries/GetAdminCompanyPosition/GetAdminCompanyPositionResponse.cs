using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Queries.GetAdminCompanyPosition
{
    public record GetAdminCompanyPositionResponse
    (
        Guid Id,
        string PositionName,
        string? Requirements,
        Guid CompanyId,
         bool IsDeleted,          // Silinmiş mi? (Kırmızı işaretlemek için)
            DateTime CreatedDate,    // Ne zaman oluşturuldu?
            DateTime? UpdatedDate,   // Ne zaman güncellendi?
            DateTime? DeletedDate    // Ne zaman silindi?
        );
}
