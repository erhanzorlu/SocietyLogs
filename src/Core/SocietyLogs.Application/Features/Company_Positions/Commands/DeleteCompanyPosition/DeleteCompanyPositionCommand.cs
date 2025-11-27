using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.DeleteCompanyPosition
{
    public record DeleteCompanyPositionCommand(Guid Id) : IRequest<bool>;

}
