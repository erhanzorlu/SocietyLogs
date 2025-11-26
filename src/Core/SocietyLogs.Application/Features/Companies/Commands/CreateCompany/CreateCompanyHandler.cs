using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Companies.Commands.Create
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, Guid>
    {
        private readonly IGenericRepository<Company> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCompanyHandler(IGenericRepository<Company> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            // 1. Entity'yi oluştur
            var company = new Company
            {
                CompanyName = request.CompanyName,
                Description = request.Description
            };

            // 2. Repository'e (Hafızaya) ekle
            await _repository.AddAsync(company);

            // 3. Veritabanına kesin olarak kaydet (Commit)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return company.Id;
        }
    }
}
