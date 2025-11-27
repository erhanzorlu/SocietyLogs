using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Queries.GetCompanyPositionById
{
    public class GetCompanyPositionByIdQuery : IRequest<GetCompanyPositionResponse>
    {
        public Guid Id { get; set; }

        public GetCompanyPositionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
