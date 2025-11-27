using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.UpdateCompanyPosition
{
    public record UpdateCompanyPositionCommand(
        Guid Id,
        string PositionName,
        string? Requirements,
        Guid CompanyId
        ): IRequest<bool>;
}
