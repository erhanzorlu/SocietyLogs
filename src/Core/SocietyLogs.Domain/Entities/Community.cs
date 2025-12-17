using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class Community : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DescriptionShort { get; set; } = string.Empty;
        public string DescriptionLong { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = "default_community_avatar.png";
        public string BannerUrl { get; set; } = "default_community_banner.png";

        public int TotalPoints { get; set; } = 0;
        public bool IsOfficial { get; set; }
        public bool IsPrivate { get; set; }

        public Guid OwnerId { get; set; }

        public ICollection<CommunityMember> Members { get; set; }
        public ICollection<CommunitySocialMedia> SocialMedias { get; set; }
        public ICollection<SoLogPost> Posts { get; set; }
    }

}
