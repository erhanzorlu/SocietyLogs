using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class UserBadge : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public Guid BadgeId { get; set; }
        public Badge Badge { get; set; }

        public DateTime EarnedAt { get; set; }
        public bool IsPinned { get; set; }
    }
}
