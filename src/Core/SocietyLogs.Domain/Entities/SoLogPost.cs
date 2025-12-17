using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class SoLogPost : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public string? Title { get; set; }

        public Guid AuthorUserId { get; set; }
        public AppUser AuthorUser { get; set; }

        public Guid? CommunityId { get; set; }
        public Community? Community { get; set; }

        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int ViewCount { get; set; } = 0;

        public Guid? ParentPostId { get; set; }
        public SoLogPost? ParentPost { get; set; }
        public bool IsQuote { get; set; }

        public ICollection<PostMedia> Medias { get; set; }
        public ICollection<SoLogComment> Comments { get; set; }
        public ICollection<PostHashtag> PostHashtags { get; set; }
    }

}
