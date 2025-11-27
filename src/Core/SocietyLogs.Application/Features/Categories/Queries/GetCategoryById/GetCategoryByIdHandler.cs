using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Kaydı Bul
            var category = await _unitOfWork.Repository<Category>()
                                            .GetByIdAsync(request.Id, tracking: false);

            // 2. Kontrol Et
            if (category == null)
            {
                // Burası sana kalmış, null dönüp Controller'da 404 verebilirsin 
                // veya direkt burada Custom Exception fırlatabilirsin.
                return null;
            }

            // 3. Map ve Dön
            return category.Adapt<GetCategoryResponse>();
        }
    }
}
