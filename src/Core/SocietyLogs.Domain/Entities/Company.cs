using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }
        public string? Description { get; set; } // Opsiyonel (?)
        public string? LogoUrl { get; set; }

        // İlişki: Bir firmanın birden çok pozisyonu olabilir.
        public ICollection<CompanyPosition> Positions { get; set; }

        // Constructor: Liste null hatası vermesin diye başlatıyoruz.
        public Company()
        {
            Positions = new HashSet<CompanyPosition>();
        }
    }
}
