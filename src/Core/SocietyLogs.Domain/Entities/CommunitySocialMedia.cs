using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class CommunitySocialMedia : BaseEntity
    {
        public Guid CommunityId { get; set; }
        public Community Community { get; set; }
        public SocialPlatformEnum Platform { get; set; }
        public string Url { get; set; } = string.Empty;
    }

}
