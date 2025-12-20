using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class AppSetting : BaseEntity
    {
        // Ayarın Anahtarı (Örn: "Community_Creation_Limit_Hours")
        public string Key { get; set; }

        // Ayarın Değeri (Örn: "48") - String tutuyoruz, çekerken int'e çeviririz.
        public string Value { get; set; }

        // Açıklama (Admin panelinde ne işe yaradığı görünsün diye)
        public string Description { get; set; }
    }
}
