using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class PointDefinition : BaseEntity
    {
        public string Key { get; set; } = string.Empty; // "LOGIN_DAILY"
        public string Name { get; set; } = string.Empty; // "Günlük Giriş"
        public string? Description { get; set; }
        public int ScoreAmount { get; set; } // Varsayılan puan
        public bool IsActive { get; set; } = true;
        public int MaxOccurrencePerDay { get; set; } = 0;
    }

}
