using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class UserSocialMedia : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public SocialPlatformEnum Platform { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
