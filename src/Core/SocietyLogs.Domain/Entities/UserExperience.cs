using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class UserExperience : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
    }

}
