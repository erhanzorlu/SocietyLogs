using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.UpdateCompanyPosition
{
    public class UpdateCompanyPositionCommandHandler : IRequestHandler<UpdateCompanyPositionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyPositionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCompanyPositionCommand request, CancellationToken cancellationToken)
        {
            var companyPosition = await _unitOfWork.Repository<CompanyPosition>().GetByIdAsync(request.Id);

            if (companyPosition == null)
            {
                return false;
            }

            request.Adapt(companyPosition);

            _unitOfWork.Repository<CompanyPosition>().Update(companyPosition);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
