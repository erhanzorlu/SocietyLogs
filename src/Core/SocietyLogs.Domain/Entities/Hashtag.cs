using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class Hashtag : BaseEntity
    {
        public string Tag { get; set; } = string.Empty;
        public int UsageCount { get; set; }
    }
}
