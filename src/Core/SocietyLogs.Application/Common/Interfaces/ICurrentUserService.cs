using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        // İlerde "IsAdmin" vs. de ekleyebilirsin
    }
}
