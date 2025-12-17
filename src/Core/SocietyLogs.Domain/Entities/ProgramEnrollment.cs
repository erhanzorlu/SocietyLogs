using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class ProgramEnrollment : BaseEntity
    {
        public Guid ProgramId { get; set; }
        public Program Program { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public EnrollmentStatusEnum Status { get; set; } = EnrollmentStatusEnum.Pending;
        public int ProgressPercentage { get; set; } = 0;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }

}
