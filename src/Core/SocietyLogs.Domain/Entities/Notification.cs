using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; } // Bildirimi alan kişi
        public AppUser User { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string TargetUrl { get; set; } = string.Empty; // Örn: /post/123

        public bool IsRead { get; set; }
        public NotificationTypeEnum Type { get; set; }
    }

}
