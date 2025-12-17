using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class UserEducation : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsContinuing { get; set; }
    }
}
