using MediatR;
using Microsoft.AspNetCore.Http;
using SocietyLogs.Application.Common.Wrappers;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SocietyLogs.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateUserProfileCommand : IRequest<ServiceResponse<bool>>
    {
        // Kullanıcı ID'sini dışarıdan almayacağız, Handler içeride Token'dan dolduracak.
        [JsonIgnore]
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderEnum Gender { get; set; }

        // Frontend buraya dosya yükler
        public IFormFile? ProfileFile { get; set; }
        public IFormFile? BannerFile { get; set; }
    }
}
