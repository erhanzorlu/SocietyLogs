using SocietyLogs.Domain.Common;

namespace SocietyLogs.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } 
        public string Description { get; set; } 

        // URL dostu isim (örn: "yazilim-teknolojileri")
        public string Slug { get; set; } = string.Empty;
    }
}
