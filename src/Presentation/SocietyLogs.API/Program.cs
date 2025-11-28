using SocietyLogs.Application;
using SocietyLogs.Persistence;
using Serilog; // <-- Serilog kütüphanesini ekledik

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