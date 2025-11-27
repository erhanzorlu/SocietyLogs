using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // 1. Kaydı bul
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id);

            if (category == null) return false;

            // 2. SOFT DELETE
            // Generic Repository'ye eklediğimiz o özel metodu çağırıyoruz.
            // Bu metod IsDeleted = true yapacak.
            await _unitOfWork.Repository<Category>().DeleteAsync(category);

            // 3. Kaydet
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
