using MediatR;
using Microsoft.AspNetCore.Http;
using SocietyLogs.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SocietyLogs.Application.Features.Users.Commands.UploadProfileImage
{
    // Geriye resmin URL'ini (string) döneceğiz.
    public class UploadProfileImageCommand : IRequest<ServiceResponse<string>>
    {
        public IFormFile ProfileImage { get; set; }

        // Güvenlik: Kullanıcı başkasının ID'sini gönderemesin diye gizliyoruz.
        // Handler içinde Token'dan dolduracağız.
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
