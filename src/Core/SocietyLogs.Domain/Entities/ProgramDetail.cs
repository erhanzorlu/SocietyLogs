using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class ProgramDetail : BaseEntity
    {
        public Guid ProgramId { get; set; }
        public Program Program { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public TargetAudienceEnum TargetAudience { get; set; }
        public string? Level { get; set; }
    }

}
