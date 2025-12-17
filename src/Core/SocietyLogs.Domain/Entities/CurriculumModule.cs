using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class CurriculumModule : BaseEntity
    {
        public Guid ProgramId { get; set; }
        public Program Program { get; set; }
        public string Title { get; set; } = string.Empty;
        public int OrderIndex { get; set; }

        public ICollection<CurriculumContent> Contents { get; set; }
    }

}
