using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class CompanyPosition : BaseEntity
    {
        public string PositionName { get; set; } // Örn: Backend Developer
        public string? Requirements { get; set; } // Örn: .NET, Docker bilen...

        // Foreign Key (İlişki)
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
