using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<GetCategoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCategoriesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetCategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            // 1. Veriyi Çek (Performance Mode: ON)
            // tracking: false diyerek EF Core'a "bunu bellekte tutma, sadece oku geç" diyoruz.
            // Not: Eğer Global Query Filter yapmadıysan silinenler de gelir. 
            // Şimdilik silinenler gelmesin diye manuel filtre de ekleyebiliriz ama 
            // doğrusu DbContext'te global filtre yapmaktır.

            // Eğer Global Filter yoksa repository'deki GetWhereAsync'i kullanabilirsin:
            // var categories = await _unitOfWork.Repository<Category>()
            //    .GetWhereAsync(x => !x.IsDeleted, tracking: false, cancellationToken);

            var categories = await _unitOfWork.Repository<Category>()
                                              .GetAllAsync(tracking: false);

            // 2. Mapping (Entity List -> DTO List)
            // Mapster listeyi otomatik çevirir. Döngü kurmana gerek yok.
            return categories.Adapt<List<GetCategoryResponse>>();
        }
    }
}
