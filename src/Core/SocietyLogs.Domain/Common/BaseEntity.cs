using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Common
{
    // Hem IEntity (Kimlik) hem de ISoftDeletable (Silinme Yeteneği) sözleşmelerini uygular.
    public abstract class BaseEntity : IEntity, ISoftDeletable
    {
        // 1. KİMLİK: Guid
        // Yeni nesne oluşturulur oluşturulmaz kimliği (Id) oluşur.
        public Guid Id { get; set; } = Guid.NewGuid();

        // 2. AUDITING (İzlenebilirlik)
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        // 3. SOFT DELETE (Yumuşak Silme)
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        // ISoftDeletable Metotları: İş mantığı Entity'nin içine taşındı.
        public void Delete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                DeletedDate = DateTime.UtcNow;
            }
        }

        public void UndoDelete()
        {
            IsDeleted = false;
            DeletedDate = null;
        }
    }
}
