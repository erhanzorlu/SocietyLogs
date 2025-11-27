using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Queries.GetAdminCompanyPosition
{
    public class GetAdminComapnyPositionQuery : IRequest<List<GetAdminCompanyPositionResponse>>
    {
    }
}
