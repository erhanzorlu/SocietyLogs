using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.RestoreCompanyPosition
{
    public record RestoreCompanyPositionCommand(Guid Id) : IRequest<bool>;
    
}
