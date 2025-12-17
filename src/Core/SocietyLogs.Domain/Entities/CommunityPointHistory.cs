using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class CommunityPointHistory : BaseEntity
    {
        public Guid CommunityId { get; set; }
        public Community Community { get; set; }

        public Guid? PointDefinitionId { get; set; }
        public PointDefinition? PointDefinition { get; set; }

        public int PointsAmount { get; set; }
        public int NewTotalScore { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid? RelatedEntityId { get; set; }
    }

}
