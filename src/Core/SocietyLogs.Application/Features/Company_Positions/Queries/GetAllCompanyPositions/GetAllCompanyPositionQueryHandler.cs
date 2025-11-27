using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Queries.GetAllCompanyPositions
{
    public class GetAllCompanyPositionQueryHandler : IRequestHandler<GetAllCompanyPositionsQuery, List<GetCompanyPositionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCompanyPositionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetCompanyPositionResponse>> Handle(GetAllCompanyPositionsQuery request, CancellationToken cancellationToken)
        {
            var companyPositions = await _unitOfWork.Repository<CompanyPosition>().GetAllActiveAsync(tracking: false);

            return companyPositions.Adapt<List<GetCompanyPositionResponse>>();
        }
    }
}
