using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class UserFollow : BaseEntity
    {
        public Guid FollowerId { get; set; }
        public AppUser Follower { get; set; }
        public Guid FollowingId { get; set; }
        public AppUser Following { get; set; }
    }
}
