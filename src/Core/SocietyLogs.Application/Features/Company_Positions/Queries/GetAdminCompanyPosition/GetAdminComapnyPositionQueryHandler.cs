using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Queries.GetAdminCompanyPosition
{
    public class GetAdminComapnyPositionQueryHandler : IRequestHandler<GetAdminComapnyPositionQuery, List<GetAdminCompanyPositionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAdminComapnyPositionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAdminCompanyPositionResponse>> Handle(GetAdminComapnyPositionQuery request, CancellationToken cancellationToken)
        {
            var companiyPositions = await _unitOfWork.Repository<CompanyPosition>().GetAllAsync(tracking: false);

            return companiyPositions.Adapt<List<GetAdminCompanyPositionResponse>>();
        }
    }
}
