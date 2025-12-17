using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class UserPointHistory : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public Guid? PointDefinitionId { get; set; }
        public PointDefinition? PointDefinition { get; set; }

        public int PointsAmount { get; set; } // O an kazanılan puan (History consistency)
        public int NewTotalScore { get; set; } // İşlem sonrası bakiye (Performance)

        public string Description { get; set; } = string.Empty;
        public Guid? RelatedEntityId { get; set; } // Traceability
    }

}
