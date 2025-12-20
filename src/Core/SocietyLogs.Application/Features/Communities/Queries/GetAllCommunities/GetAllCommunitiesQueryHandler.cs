using MediatR;
using Microsoft.EntityFrameworkCore;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Application.Common.Wrappers;
using SocietyLogs.Domain.Entities;

namespace SocietyLogs.Application.Features.Communities.Queries.GetAllCommunities
{
    public class GetAllCommunitiesQueryHandler : IRequestHandler<GetAllCommunitiesQuery, ServiceResponse<List<Community>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCommunitiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<List<Community>>> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
        {
            // 1. SORGULANABİLİR VERİYİ AL (Henüz DB'ye gitmedi)
            var query = _unitOfWork.Repository<Community>().GetAllAsQueryable(tracking: false);

            // 2. FİLTRELER (Örn: Silinmemişleri getir)
            // Senin BaseEntity'de IsDeleted var mıydı? Varsa ekle:
            query = query.Where(x => x.IsDeleted == false);

            // 3. SAYFALAMA MATEMATİĞİ (Pagination) 🧮
            // SQL'e "OFFSET x ROWS FETCH NEXT y ROWS ONLY" komutu gider.
            var pagedList = await query
                .OrderByDescending(x => x.CreatedDate)      // En yeniler en üstte olsun
                .Skip((request.PageNumber - 1) * request.PageSize) // Atla
                .Take(request.PageSize)                            // Al
                .ToListAsync(cancellationToken);                   // ŞİMDİ DB'ye GİT 🚀

            // 4. CEVAP DÖN
            return new ServiceResponse<List<Community>>(pagedList, "Topluluklar listelendi.");
        }
    }
}
