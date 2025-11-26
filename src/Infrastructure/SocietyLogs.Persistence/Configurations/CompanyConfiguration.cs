using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocietyLogs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Tablo Adı (İsteğe bağlı, vermezsek sınıf adını alır)
            builder.ToTable("Companies");

            // ID Tanımı
            builder.HasKey(c => c.Id);

            // Özellikler
            builder.Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            // İlişkiler (Firma -> Pozisyonlar)
            builder.HasMany(c => c.Positions)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict); // Firma silinirse pozisyonlar silinmesin (Soft Delete var ama yine de güvenlik)
        }
    }
}
