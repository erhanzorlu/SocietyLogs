using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Commands.DeleteCompany
{
    // Request
    public record DeleteCompanyCommand(Guid Id) : IRequest<bool>;

    // Handler
    public class DeleteCompanyHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly IGenericRepository<Company> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyHandler(IGenericRepository<Company> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            // 1. Veriyi bul
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null) return false;

            // 2. Sil (Soft Delete devreye girecek)
            _repository.DeleteAsync(entity);

            // 3. Kaydet
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
