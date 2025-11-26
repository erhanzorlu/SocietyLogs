🚀 SocietyLogs - Backend APISocietyLogs; öğrencileri, toplulukları ve firmaları bir araya getiren, yeni nesil profesyonel sosyal ağ ve kariyer platformudur.Bu proje .NET 8, Onion Architecture ve CQRS desenleri kullanılarak geliştirilmiştir.🛠️ Teknoloji YığınıCore: .NET 8, C# 12Database: MSSQL Server 2022 (Docker)Cache: Redis (Docker)Storage: MinIO (Docker - S3 Compatible)ORM: Entity Framework Core 8 (Code First)Architecture: Onion Architecture, Modular MonolithCommunication: MediatR, CQRSValidation: FluentValidation⚙️ Kurulum Rehberi (Getting Started)Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları sırasıyla uygulayın.1. Gereksinimler (Prerequisites)Bilgisayarınızda şunların kurulu olması gerekir:.NET 8 SDKDocker Desktop2. Projeyi KlonlayınTerminali açın ve projeyi indirin:git clone [https://github.com/KULLANICI_ADINIZ/SocietyLogs.git](https://github.com/KULLANICI_ADINIZ/SocietyLogs.git)
cd SocietyLogs
3. Altyapıyı Ayağa Kaldırın (Docker)Veritabanı, Cache ve Dosya sunucusunu kurmak için Docker Compose'u çalıştırın. Bu işlem MSSQL, Redis ve MinIO'yu kuracaktır.docker-compose up -d
Not: İlk çalıştırmada SQL Server'ın tamamen açılması 15-20 saniye sürebilir.4. Veritabanını Güncelleyin (Migration)Docker'daki veritabanı başlangıçta boştur. Tabloları oluşturmak için Migration'ı uygulayın:dotnet ef database update --project src/Infrastructure/SocietyLogs.Persistence --startup-project src/Presentation/SocietyLogs.API
5. Projeyi BaşlatınArtık API'yi çalıştırabilirsiniz:dotnet run --project src/Presentation/SocietyLogs.API
Veya Visual Studio içerisinden SocietyLogs.API projesini seçip Start tuşuna basabilirsiniz.🧪 Test Etme (Swagger)Proje çalıştığında tarayıcınızda otomatik olarak açılacaktır. Açılmazsa aşağıdaki adrese gidin:👉 http://localhost:5294/swaggerÖrnek Test Senaryosu (Faz 0)POST /api/Companies endpoint'ini açın.Try it out butonuna tıklayın.Aşağıdaki JSON'u yapıştırın ve Execute deyin:{
  "companyName": "Test Firması",
  "description": "Docker üzerinden oluşturuldu."
}
200 OK yanıtını görüyorsanız sistem sorunsuz çalışıyor demektir!🔑 Varsayılan Şifreler (Development)Geliştirme ortamı için Docker servis bilgileri şöyledir:ServisAdresKullanıcıŞifreMSSQLlocalhost,1433saGucluBirSifre123!Redislocalhost:6379--MinIOlocalhost:9001minioadminminioadmin📂 Proje Yapısı (Onion Architecture)src/
├── Core/
│   ├── Domain/        (Varlıklar ve Temel Kurallar)
│   └── Application/   (İş Mantığı, CQRS, Interfaces)
├── Infrastructure/
│   ├── Persistence/   (Veritabanı, EF Core, Migrations)
│   └── Infrastructure/(Dış Servisler: Redis, Mail, Storage)
└── Presentation/
    └── API/           (Controllerlar ve Endpointler)
