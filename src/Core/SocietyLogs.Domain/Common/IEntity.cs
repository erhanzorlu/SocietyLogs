using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Common
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
