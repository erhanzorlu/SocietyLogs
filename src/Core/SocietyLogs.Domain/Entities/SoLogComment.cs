using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class SoLogComment : BaseEntity
    {
        public Guid PostId { get; set; }
        public SoLogPost Post { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsPinned { get; set; }

        public Guid? ParentCommentId { get; set; }
        public SoLogComment? ParentComment { get; set; }
        public ICollection<SoLogComment> Replies { get; set; }
    }

}
