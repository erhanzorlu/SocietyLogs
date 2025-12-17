using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class Certificate : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public Guid ProgramId { get; set; }
        public Program Program { get; set; }

        public string CertificateNumber { get; set; } = string.Empty;
        public string QrCodeData { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
    }

}
