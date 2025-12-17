using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class Program : BaseEntity
    {
        public Guid CreatorCommunityId { get; set; }
        public Community CreatorCommunity { get; set; }

        public string Title { get; set; } = string.Empty;
        public string ImagePath { get; set; } = "default_program.png";

        public ProgramCategoryEnum Category { get; set; }
        public ProgramStatusEnum Status { get; set; } = ProgramStatusEnum.Draft;

        public string Location { get; set; } = "Online";
        public DateTime ApplicationDeadline { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool HasCertificate { get; set; }
        public int PointsToEarn { get; set; } = 0;

        public Guid? RewardBadgeId { get; set; }
        public Badge? RewardBadge { get; set; }

        public bool IsFeatured { get; set; }

        public ProgramDetail? Detail { get; set; }
        public ICollection<ProgramSponsor> Sponsors { get; set; }
        public ICollection<ProgramEnrollment> Enrollments { get; set; }
        public ICollection<CurriculumModule> CurriculumModules { get; set; }
        public ICollection<ProgramTask> Tasks { get; set; }
    }

}
