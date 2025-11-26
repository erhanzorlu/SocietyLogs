using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Commands.UpdateCompany
{
    // Request (ID'yi de alıyoruz ki hangisini güncelleyeceğimizi bilelim)
    public record UpdateCompanyCommand(Guid Id, string CompanyName, string? Description) : IRequest<bool>;

    // Handler
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, bool>
    {
        private readonly IGenericRepository<Company> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyHandler(IGenericRepository<Company> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            // 1. Önce veriyi bul
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null) return false; // Veya Exception fırlatabilirsin

            // 2. Yeni değerleri ata
            entity.CompanyName = request.CompanyName;
            entity.Description = request.Description;

            // 3. Update işaretle
            _repository.Update(entity);

            // 4. Kaydet
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
