using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocietyLogs.Domain.Entities;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Persistence.Seeds
{
    public static class ContextSeed
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // 1. ROLLERİ EKLE
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new AppRole { Name = "Admin", Id = Guid.NewGuid() });
                await roleManager.CreateAsync(new AppRole { Name = "Moderator", Id = Guid.NewGuid() });
                await roleManager.CreateAsync(new AppRole { Name = "Member", Id = Guid.NewGuid() });
            }

            // 2. ADMİN KULLANCISINI EKLE
            var adminUser = await userManager.FindByEmailAsync("admin@societylogs.com");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin",
                    Email = "admin@societylogs.com",
                    Name = "Sistem",
                    Surname = "Admin",
                    EmailConfirmed = true,
                    IsVerified = true,
                    CurrentLevel = 99,
                    CreatedDate = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString() // Token hatası almamak için önemli
                };

                // Şifre: Admin123!
                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. PUAN KURALLARINI EKLE
            if (!await context.PointDefinitions.AnyAsync())
            {
                var rules = new List<PointDefinition>
                {
                    new PointDefinition { Key = "LOGIN_DAILY", Name = "Günlük Giriş", ScoreAmount = 10, MaxOccurrencePerDay = 1 },
                    new PointDefinition { Key = "POST_SHARE", Name = "İçerik Paylaşımı", ScoreAmount = 5, MaxOccurrencePerDay = 10 },
                    new PointDefinition { Key = "EDUCATION_COMPLETE", Name = "Eğitim Tamamlama", ScoreAmount = 50, MaxOccurrencePerDay = 0 }
                };

                await context.PointDefinitions.AddRangeAsync(rules);
                await context.SaveChangesAsync();
            }
        }
    }
}
