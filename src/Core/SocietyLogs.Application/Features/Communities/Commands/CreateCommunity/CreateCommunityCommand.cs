using MediatR;
using Microsoft.AspNetCore.Http;
using SocietyLogs.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Communities.Commands.CreateCommunity
{
    // DİKKAT: IRequest<Guid> değil, IRequest<ServiceResponse<Guid>> yaptık.
    // Çünkü artık geriye "Zarf içinde Guid" döneceğiz.
    public class CreateCommunityCommand : IRequest<ServiceResponse<Guid>>
    {
        public string Name { get; set; }
        public string Description { get; set; } // Bunu hem Long hem Short için kullanacağız

        // Formdan dosya olarak gelecekler
        public IFormFile? AvatarFile { get; set; }  // Değişti: ImageFile -> AvatarFile
        public IFormFile? BannerFile { get; set; }

        public bool IsPrivate { get; set; } // Gizli topluluk mu?

        // Controller set edecek
        public Guid CreatorUserId { get; set; }
    }
}
