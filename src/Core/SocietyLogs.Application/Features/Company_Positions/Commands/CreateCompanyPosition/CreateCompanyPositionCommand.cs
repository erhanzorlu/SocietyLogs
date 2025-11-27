using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.CreateCompanyPosition
{
    public record CreateCompanyPositionCommand
    (
        string PositionName,
        string? Requirements,
        Guid CompanyId
    ) : IRequest<Guid>;
}
