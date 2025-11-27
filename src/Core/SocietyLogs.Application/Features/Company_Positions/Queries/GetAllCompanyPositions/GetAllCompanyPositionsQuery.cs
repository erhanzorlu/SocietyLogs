using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Queries.GetAllCompanyPositions
{
    public class GetAllCompanyPositionsQuery : IRequest<List<GetCompanyPositionResponse>>
    {
    }
}
