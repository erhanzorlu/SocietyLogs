using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class CommunityMember : BaseEntity
    {
        public Guid CommunityId { get; set; }
        public Community Community { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public CommunityRoleEnum Role { get; set; } = CommunityRoleEnum.Member;
    }

}
