using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            // 1. Önce veritabanından mevcut kaydı bul
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id, tracking: true);

            // Eğer kayıt yoksa? (Burada Exception fırlatmak daha doğrudur ama şimdilik bool dönelim)
            if (category == null) return false; // veya throw new NotFoundException();

            // 2. MAPPING (Mevcut nesnenin üzerine yazma)
            // Mapster'ın bu özelliği harikadır: request içindeki verileri category nesnesine aktarır.
            request.Adapt(category);

            // 3. Update İşlemi
            _unitOfWork.Repository<Category>().Update(category);

            // 4. Kaydet
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
