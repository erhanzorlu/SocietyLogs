using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Queries.GetCompanyPositionById
{
    public class GetCompanyPositionByIdQueryHandler : IRequestHandler<GetCompanyPositionByIdQuery, GetCompanyPositionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyPositionByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCompanyPositionResponse> Handle(GetCompanyPositionByIdQuery request, CancellationToken cancellationToken)
        {
            var companyPosition = await _unitOfWork.Repository<CompanyPosition>().GetByIdAsync(request.Id);

            if(companyPosition == null)
            {
                return null;
            }

            return companyPosition.Adapt<GetCompanyPositionResponse>();
        }
    }
}
