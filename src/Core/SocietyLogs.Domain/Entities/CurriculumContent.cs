using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class CurriculumContent : BaseEntity
    {
        public Guid ModuleId { get; set; }
        public CurriculumModule Module { get; set; }

        public string Title { get; set; } = string.Empty;
        public ContentTypeEnum Type { get; set; }
        public string ContentUrl { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public int OrderIndex { get; set; }
    }
}
