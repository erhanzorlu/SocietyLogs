using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions
{
    public record GetCompanyPositionResponse(
        Guid Id,
        string PositionName,
        string? Requirements,
        Guid CompanyId
        );
}
