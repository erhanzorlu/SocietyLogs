using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.RestoreCompanyPosition
{
    public class RestoreCompanyPositionCommandHandler : IRequestHandler<RestoreCompanyPositionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RestoreCompanyPositionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RestoreCompanyPositionCommand request, CancellationToken cancellationToken)
        {
            var companyPosition = await _unitOfWork.Repository<CompanyPosition>().GetByIdAsync(request.Id);

            if (companyPosition == null || !companyPosition.IsDeleted)
            {
                return false; // Not found or not deleted
            }

            companyPosition.UndoDelete();
            _unitOfWork.Repository<CompanyPosition>().Update(companyPosition);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
