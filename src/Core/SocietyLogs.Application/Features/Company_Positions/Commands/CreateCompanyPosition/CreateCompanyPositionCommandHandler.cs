using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Company_Positions.Commands.CreateCompanyPosition
{
    public class CreateCompanyPositionCommandHandler : IRequestHandler<CreateCompanyPositionCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompanyPositionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCompanyPositionCommand request, CancellationToken cancellationToken)
        {
            var companyPosition = request.Adapt<CompanyPosition>();
            await _unitOfWork.Repository<CompanyPosition>().AddAsync(companyPosition, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return companyPosition.Id;
        }
    }
}
