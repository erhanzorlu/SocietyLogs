using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog; // <-- Serilog kütüphanesini ekledik
using SocietyLogs.Application;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities.Identity;
using SocietyLogs.Infrastructure.Services;
using SocietyLogs.Persistence;
using SocietyLogs.Persistence.Contexts;
using System.Text;

// 1. ADIM: Bootstrap Logger
// Uygulama daha ayaða kalkmadan (Builder oluþmadan) loglamayý baþlatýyoruz.
// Böylece baþlangýç aþamasýndaki hatalarý (örn: appsettings hatasý) bile yakalayabiliriz.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("SocietyLogs Uygulamasý Baþlatýlýyor...");

    var builder = WebApplication.CreateBuilder(args);

    // 2. ADIM: Host Entegrasyonu
    // .NET'in varsayýlan loglama mekanizmasýný eziyoruz, patron artýk Serilog.
    // Ayarlarý appsettings.json dosyasýndan okumasýný söylüyoruz.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // --- KATMAN SERVÝSLERÝ (Onion Architecture) ---
    // Application Katmaný (MediatR, Validator, Behaviors)
    builder.Services.AddApplicationServices();

    // Persistence Katmaný (Veritabaný, Repo, UoW)
    builder.Services.AddPersistenceServices(builder.Configuration);

    // 1. Token Servisini Kaydet
    builder.Services.AddScoped<ITokenService, TokenService>();

    // 2. Identity Ayarlarý (Þifre kurallarý vs.)
    builder.Services.AddIdentityCore<AppUser>(opt =>
    {
        opt.Password.RequireDigit = false;          // Rakam zorunlu mu?
        opt.Password.RequireLowercase = false;      // Küçük harf?
        opt.Password.RequireUppercase = false;      // Büyük harf?
        opt.Password.RequireNonAlphanumeric = false;// !?* gibi iþaretler?
        opt.Password.RequiredLength = 3;            // Min uzunluk (Test için kýsa tuttum)
        opt.User.RequireUniqueEmail = true;         // Email benzersiz olmalý
    })
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>();

    // 3. Authentication (Kimlik Doðrulama) Ayarlarý - JWT Buraya Baðlanýyor
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
                ValidateLifetime = true, // Süresi dolmuþ tokený reddet
                ClockSkew = TimeSpan.Zero // Sunucu saat farkýný yok say
            };
        });


    // --- API SERVÝSLERÝ ---
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // 3. ADIM: HTTP Trafik Ýzleme (Request Logging)
    // Gelen her isteði (GET, POST) ve süresini ölçüp loglar.
    // Middleware zincirinin en tepesine yakýn olmalý.
    app.UseSerilogRequestLogging();

    // --- UYGULAMA AKIÞI (Pipeline) ---

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection(); // Geliþtirme ortamýnda kapalý kalabilir
    app.UseAuthentication(); // <-- EKLENDÝ: Kimlik sor (Kimsin?)
    app.UseAuthorization();
    app.UseSerilogRequestLogging();
    app.MapControllers();
    app.UseMiddleware<SocietyLogs.API.Middlewares.GlobalExceptionHandlerMiddleware>();
    app.Run();
}
catch (Exception ex)
{
    // Uygulama çökerse (Crash) buraya düþer ve sebebini loglarýz.
    Log.Fatal(ex, "Uygulama beklenmedik bir hata yüzünden durdu!");
}
finally
{
    // Uygulama kapanýrken tamponda kalan son loglarý diske yazar ve kapatýr.
    Log.CloseAndFlush();
}