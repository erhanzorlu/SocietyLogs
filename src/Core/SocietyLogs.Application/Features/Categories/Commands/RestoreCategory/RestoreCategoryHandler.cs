using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Commands.RestoreCategory
{
    public class RestoreCategoryHandler : IRequestHandler<RestoreCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RestoreCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
        {
            // 1. Silinen kaydı bulmak için yine IgnoreQueryFilters kullanmalıyız!
            // Çünkü normal GetByIdAsync silineni getirmez.

            // Bunun için GenericRepo'ya GetByIdAsync için de ignoreQueryFilters eklemelisin 
            // VEYA GetSingleAsync kullanabilirsin:
            var category = await _unitOfWork.Repository<Category>()
                .GetSingleAsync(x => x.Id == request.Id, tracking: true, ignoreQueryFilters: true, cancellationToken);

            if (category == null) return false;

            // 2. BaseEntity içindeki UndoDelete metodunu çağır (IsDeleted = false)
            category.UndoDelete();

            // 3. Güncelle
            _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
