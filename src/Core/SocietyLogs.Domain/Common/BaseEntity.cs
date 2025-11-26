using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Common
{
    public abstract class BaseEntity
    {
        // 10 Yıllık Vizyon: Sıralı GUID (TSID) için altyapı.
        public Guid Id { get; set; } = Guid.NewGuid();

        // Audit (İzlenebilirlik)
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // Soft Delete (Veri Güvenliği)
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
