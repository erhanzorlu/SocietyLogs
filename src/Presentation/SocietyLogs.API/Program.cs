using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SocietyLogs.API.Services;
using SocietyLogs.Application;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Infrastructure.Services;
using SocietyLogs.Persistence;
using SocietyLogs.Persistence.Contexts;
using System.Text;

// 1. ADIM: Bootstrap Logger
// Uygulama daha ayağa kalkmadan (Builder oluşmadan) loglamayı başlatıyoruz.
// Böylece başlangıç aşamasındaki hataları (örn: appsettings hatası) bile yakalayabiliriz.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("SocietyLogs Uygulaması Başlatılıyor...");

    var builder = WebApplication.CreateBuilder(args);

    // 2. ADIM: Host Entegrasyonu
    // .NET'in varsayılan loglama mekanizmasını eziyoruz, patron artık Serilog.
    // Ayarları appsettings.json dosyasından okumasını söylüyoruz.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // --- KATMAN SERVİSLERİ (Onion Architecture) ---
    // Application Katmanı (MediatR, Validator, Behaviors)
    builder.Services.AddApplicationServices();

    // Persistence Katmanı (Veritabanı, Repo, UoW)
    builder.Services.AddPersistenceServices(builder.Configuration);

    // 1. Token Servisini Kaydet
    builder.Services.AddScoped<ITokenService, TokenService>();

    // 2. Identity Ayarları (Şifre kuralları vs.)
    builder.Services.AddIdentityCore<AppUser>(opt =>
    {
        opt.Password.RequireDigit = false;          // Rakam zorunlu mu?
        opt.Password.RequireLowercase = false;      // Küçük harf?
        opt.Password.RequireUppercase = false;      // Büyük harf?
        opt.Password.RequireNonAlphanumeric = false;// !?* gibi işaretler?
        opt.Password.RequiredLength = 3;            // Min uzunluk (Test için kısa tuttum)
        opt.User.RequireUniqueEmail = true;         // Email benzersiz olmalı
    })
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>();

    // 3. Authentication (Kimlik Doğrulama) Ayarları - JWT Buraya Bağlanıyor
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!)),
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                ValidateLifetime = true, // Süresi dolmuş tokenı reddet
                ClockSkew = TimeSpan.Zero // Sunucu saat farkını yok say
            };
        });

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    // --- API SERVİSLERİ ---
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "SocietyLogs.WebAPI", Version = "v1" });

        // Kilit (Authorize) butonunu tanýmlýyoruz
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        // Bu güvenliði tüm endpoint'lere zorunlu kýlýyoruz (Kilit ikonlarý çýkar)
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    });

    var app = builder.Build();

    // 3. ADIM: HTTP Trafik İzleme (Request Logging)
    // Gelen her isteği (GET, POST) ve süresini ölçüp loglar.
    // Middleware zincirinin en tepesine yakın olmalı.
    app.UseSerilogRequestLogging();

    // --- UYGULAMA AKIŞI (Pipeline) ---

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection(); // Geliştirme ortamında kapalı kalabilir
    app.UseAuthentication(); // <-- EKLENDİ: Kimlik sor (Kimsin?)
    app.UseAuthorization();
    app.UseSerilogRequestLogging();
    app.MapControllers();
    app.UseMiddleware<SocietyLogs.API.Middlewares.GlobalExceptionHandlerMiddleware>();
    app.Run();
}
catch (Exception ex)
{
    // Uygulama çökerse (Crash) buraya düşer ve sebebini loglarız.
    Log.Fatal(ex, "Uygulama beklenmedik bir hata yüzünden durdu!");
}
finally
{
    // Uygulama kapanırken tamponda kalan son logları diske yazar ve kapatır.
    Log.CloseAndFlush();
}