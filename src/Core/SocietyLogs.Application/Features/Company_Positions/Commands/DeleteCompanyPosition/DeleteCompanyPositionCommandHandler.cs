using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.DeleteCompanyPosition
{
    public class DeleteCompanyPositionCommandHandler : IRequestHandler<DeleteCompanyPositionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyPositionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCompanyPositionCommand request, CancellationToken cancellationToken)
        {
            var companyPosition = await _unitOfWork.Repository<CompanyPosition>().GetByIdAsync(request.Id);
            if(companyPosition == null)
            {
                return false; 
            }
            await _unitOfWork.Repository<CompanyPosition>().DeleteAsync(companyPosition);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
