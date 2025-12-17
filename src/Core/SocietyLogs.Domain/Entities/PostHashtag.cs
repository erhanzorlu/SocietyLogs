using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class PostHashtag : BaseEntity
    {
        public Guid PostId { get; set; }
        public SoLogPost Post { get; set; }
        public Guid HashtagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
