using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class UserContentProgress : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public Guid ContentId { get; set; }
        public CurriculumContent Content { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime CompletedAt { get; set; }
    }

}
