using Mapster;
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
        private readonly IUnitOfWork _unitOfWork;

        // Constructor Injection: Sadece UnitOfWork istiyoruz, Repository değil!
        public CreateCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            // 1. MAPPING (Mapster):
            // Gelen Command nesnesini Company Entity'sine çevir.
            // Mapster'ın güzelliği: Ayrı bir profil dosyası yazmana gerek yok, isimlendirme aynıysa otomatik eşler.
            // .Adapt<HedefTip>() metodu Mapster'dan gelir.
            var companyEntity = request.Adapt<Company>();

            // 2. LOGIC & DATABASE:
            // "Akıllı" UnitOfWork sayesinde Repository<Company>() anlık oluşturulur (veya cache'den gelir).
            await _unitOfWork.Repository<Company>().AddAsync(companyEntity);

            // 3. COMMIT:
            // İşlemi veritabanına kaydet.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4. RETURN:
            // Oluşan ID'yi geri dön.
            return companyEntity.Id;
        }
    }
}
