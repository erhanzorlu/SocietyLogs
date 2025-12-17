using SocietyLogs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities
{
    public class ProgramSponsor : BaseEntity
    {
        public Guid ProgramId { get; set; }
        public Program Program { get; set; }
        public string SponsorName { get; set; } = string.Empty;
        public string SponsorLogo { get; set; } = string.Empty;
        public string Role { get; set; } = "Main Sponsor";
    }

}
