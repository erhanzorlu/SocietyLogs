using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class PostMedia : BaseEntity
    {
        public Guid PostId { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public string MediaType { get; set; } = "image";
        public int OrderIndex { get; set; }
    }

}
