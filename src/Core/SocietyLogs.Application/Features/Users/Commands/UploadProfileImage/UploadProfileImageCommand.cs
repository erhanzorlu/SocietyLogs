using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Users.Commands.UploadProfileImage
{
    // Geriye resmin URL'ini (string) döneceğiz.
    public class UploadProfileImageCommand : IRequest<string>
    {
        public IFormFile ProfileImage { get; set; }
        public Guid UserId { get; set; } // Hangi kullanıcının resmi?
    }
}
