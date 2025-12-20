using Mapster;
using MediatR;
using Microsoft.IdentityModel.Logging;
using SocietyLogs.Application.Common.Helpers;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Application.Common.Wrappers;
using SocietyLogs.Domain.Entities;
using SocietyLogs.Domain.Enums;


namespace SocietyLogs.Application.Features.Communities.Commands.CreateCommunity
{
    public class CreateCommunityCommandHandler : IRequestHandler<CreateCommunityCommand, ServiceResponse<Guid>>
    {
        // DEĞİŞİKLİK 1: AppDbContext gitti, UnitOfWork geldi.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CreateCommunityCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

            public async Task<ServiceResponse<Guid>> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
            {
                // 1. Dosya Yükleme (Varsayılan değerleri Entity'deki gibi koruyoruz)
                // Eğer dosya yüklenmezse null kalacak, Entity içindeki "default_...png" devreye girecek mi? 
                // Hayır, null atarsak o default ezilebilir. O yüzden kontrol ediyoruz.

                string avatarPath = null;
                string bannerPath = null;

                if (request.AvatarFile != null)
                    avatarPath = await _fileService.UploadAsync(request.AvatarFile, "communities/avatars");

                if (request.BannerFile != null)
                    bannerPath = await _fileService.UploadAsync(request.BannerFile, "communities/banners");

                // 2. Slug Oluştur
                var slug = SlugHelper.GenerateSlug(request.Name);

                // 3. Entity Oluşturma (Mapping)
                // Senin Entity yapın karmaşık olduğu için Mapster yerine elle atamak daha güvenli ve okunaklı.

                var community = new Community
                {
                    // BaseEntity alanları (Otomatik doluyor ama ID lazım olabilir)
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,

                    // Community alanları
                    Name = request.Name,
                    Slug = slug,

                    // Açıklama Mantığı: Kullanıcı tek açıklama girer, biz kısasını da üretiriz.
                    DescriptionLong = request.Description,
                    DescriptionShort = request.Description.Length > 150
                                        ? request.Description.Substring(0, 147) + "..."
                                        : request.Description,

                    // Dosyalar: Eğer yüklenmediyse Entity'deki varsayılan değerleri (default_...png) korumak için atama yapmıyoruz
                    // Veya null değilse atıyoruz:
                    AvatarUrl = avatarPath ?? "default_community_avatar.png",
                    BannerUrl = bannerPath ?? "default_community_banner.png",

                    IsPrivate = request.IsPrivate,
                    IsOfficial = false,
                    TotalPoints = 0,
                    OwnerId = request.CreatorUserId
                };

                // 4. İlk Üye (Kurucu) Ekleme
                // Role mantığını senin Enum yapına uydurdum.
                var ownerMember = new CommunityMember
                {
                    Id = Guid.NewGuid(),
                    CommunityId = community.Id,
                    UserId = request.CreatorUserId,

                    // DİKKAT: Senin Enum'da Admin veya Owner neyse onu yaz!
                    Role = CommunityRoleEnum.Admin,

                    CreatedDate = DateTime.UtcNow
                };

                // İlişkiyi kuruyoruz
                community.Members = new List<CommunityMember> { ownerMember };

                // 5. Veritabanına Kayıt (UnitOfWork)
                await _unitOfWork.Repository<Community>().AddAsync(community);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new ServiceResponse<Guid>(community.Id, "Topluluk başarıyla oluşturuldu.");
            }
        }
    }

