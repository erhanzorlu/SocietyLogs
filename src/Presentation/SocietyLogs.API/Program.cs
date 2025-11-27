using SocietyLogs.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- 1. KENDÝ SERVÝSLERÝMÝZ (Onion Architecture Baðlantýlarý) ---
// Infrastructure (Veritabaný, Repo, Interceptor)
builder.Services.AddInfrastructureServices(builder.Configuration);
// Application (MediatR, Validation, Mapping)
builder.Services.AddApplicationServices();

// --- 2. API STANDART SERVÝSLERÝ ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- 3. SWAGGER SERVÝSÝ (Arayüz Ýçin) ---
// AddOpenApi yerine AddSwaggerGen kullanýyoruz.
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    // Swagger JSON dökümanýný oluþtur
    app.UseSwagger();
    // Swagger UI (Arayüzü) aktif et - Tarayýcýda göreceðin ekran
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();//

app.UseAuthorization();

app.MapControllers();

app.Run();