using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Common
{
    // Soft Delete yapabilme yeteneği.
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedDate { get; set; }
        void Delete();
        void UndoDelete();
    }
}
