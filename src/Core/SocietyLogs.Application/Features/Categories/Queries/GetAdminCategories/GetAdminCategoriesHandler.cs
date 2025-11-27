using Mapster;
using MediatR;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Categories.Queries.GetAdminCategories
{
    public class GetAdminCategoriesHandler : IRequestHandler<GetAdminCategoriesQuery, List<GetAdminCategoriesResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAdminCategoriesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAdminCategoriesResponse>> Handle(GetAdminCategoriesQuery request, CancellationToken cancellationToken)
        {
            // BURASI ÖNEMLİ: ignoreQueryFilters = true
            // Artık silinenler dahil her şey geliyor.
            var categories = await _unitOfWork.Repository<Category>()
                                              .GetAllAsync(tracking: false, ignoreQueryFilters: true, cancellationToken);

            // Mapster entity'deki CreatedDate, IsDeleted alanlarını DTO'ya otomatik eşler.
            return categories.Adapt<List<GetAdminCategoriesResponse>>();
        }
    }
}
