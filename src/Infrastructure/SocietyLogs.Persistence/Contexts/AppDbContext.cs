using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Entities;
using SocietyLogs.Domain.Entities.Identity; // AppUser ve AppRole için
using System.Linq.Expressions;
using System.Reflection;

namespace SocietyLogs.Persistence.Contexts
{
    // IdentityDbContext: Kullanıcı, Rol, Claim, Token tablolarını otomatik yönetir.
    // <AppUser, AppRole, Guid>: Primary Key tipinin GUID olduğunu belirtiyoruz.
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Interceptor artık ServiceRegistration'dan geliyor, burada eklemeye gerek yok.
        }

        // --- Senin Varlıkların (Entities) ---
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyPosition> CompanyPositions { get; set; }
        public DbSet<Category> Categories { get; set; }

        // =================================================================
        // 1. KULLANICI DETAYLARI (Identity Alt Tabloları)
        // =================================================================
        public DbSet<UserSocialMedia> UserSocialMedias { get; set; }
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<UserExperience> UserExperiences { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; } // Takipleşme

        // =================================================================
        // 2. TOPLULUK (COMMUNITY) MODÜLÜ
        // =================================================================
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityMember> CommunityMembers { get; set; }
        public DbSet<CommunitySocialMedia> CommunitySocialMedias { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        // =================================================================
        // 3. SOLOG (SOSYAL AKIŞ) MODÜLÜ
        // =================================================================
        public DbSet<SoLogPost> SoLogPosts { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<SoLogComment> SoLogComments { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<PostHashtag> PostHashtags { get; set; }

        // =================================================================
        // 4. SONEWS & PROGRAMLAR (Eğitim/Etkinlik)
        // =================================================================
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProgramDetail> ProgramDetails { get; set; }
        public DbSet<ProgramEnrollment> ProgramEnrollments { get; set; }
        public DbSet<ProgramSponsor> ProgramSponsors { get; set; }
        public DbSet<Badge> Badges { get; set; } // Rozet Tanımları

        // =================================================================
        // 5. LMS (MÜFREDAT & SERTİFİKA) MODÜLÜ
        // =================================================================
        public DbSet<CurriculumModule> CurriculumModules { get; set; }
        public DbSet<CurriculumContent> CurriculumContents { get; set; }
        public DbSet<UserContentProgress> UserContentProgresses { get; set; }
        public DbSet<ProgramTask> ProgramTasks { get; set; }
        public DbSet<TaskSubmission> TaskSubmissions { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; } // Kazanılan Rozetler

        // =================================================================
        // 6. BİLDİRİM SİSTEMİ
        // =================================================================
        public DbSet<Notification> Notifications { get; set; }

        // =================================================================
        // 7. OYUNLAŞTIRMA (GAMIFICATION) SİSTEMİ
        // =================================================================
        public DbSet<PointDefinition> PointDefinitions { get; set; }
        public DbSet<UserPointHistory> UserPointHistories { get; set; }
        public DbSet<CommunityPointHistory> CommunityPointHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. ÖNCE BUNU ÇAĞIR: Identity tablolarının (AspNetUsers vs.) ayarlarını yükler.
            base.OnModelCreating(modelBuilder);

            // 2. CONFIGURATION YÜKLEME: Yazdığın özel Configuration dosyalarını (Fluent API) yükler.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. GLOBAL QUERY FILTER (Otomatik Soft Delete Filtresi)
            // Vizyon Hamlesi: Tek tek her tabloya filtre yazmak yerine, generic olarak hallediyoruz.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Eğer entity ISoftDeletable interface'ini implemente ediyorsa
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    // p => p.IsDeleted == false ifadesini Expression Tree ile oluşturuyoruz
                    var parameter = Expression.Parameter(entityType.ClrType, "p");
                    var deletedCheck = Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, "IsDeleted"),
                            Expression.Constant(false)),
                        parameter
                    );

                    // EF Core'a kuralı ekle: "Select * From Table Where IsDeleted = 0"
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
                }
            }

            // 4. MANUEL İLİŞKİ AYARLARI (Configurations dosyası olmayanlar için Güvenlik Ağı)

            // UserFollow için özel ayar (Multiple Cascade Path hatasını önlemek için)
            modelBuilder.Entity<UserFollow>(b =>
            {
                b.HasIndex(f => new { f.FollowerId, f.FollowingId }).IsUnique();

                b.HasOne(f => f.Follower)
                 .WithMany()
                 .HasForeignKey(f => f.FollowerId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(f => f.Following)
                 .WithMany()
                 .HasForeignKey(f => f.FollowingId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // SoLogPost - Community İlişkisi (Topluluk silinirse postlar da gitsin mi?)
            modelBuilder.Entity<SoLogPost>()
                .HasOne(p => p.Community)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);

            // SoLogPost - User İlişkisi (Kullanıcı silinirse postlar kalsın - Soft Delete var zaten)
            modelBuilder.Entity<SoLogPost>()
                .HasOne(p => p.AuthorUser)
                .WithMany()
                .HasForeignKey(p => p.AuthorUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
