using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class TaskSubmission : BaseEntity
    {
        public Guid TaskId { get; set; }
        public ProgramTask Task { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public string SubmissionUrl { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public SubmissionStatusEnum Status { get; set; } = SubmissionStatusEnum.Pending;
    }
}
