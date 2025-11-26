
# 🚀 SocietyLogs - Backend API

**SocietyLogs**; öğrencileri, üniversite topluluklarını ve firmaları tek bir dijital çatı altında buluşturan, yeni nesil bir **profesyonel sosyal ağ ve kariyer platformudur.**

Bu proje, yüksek ölçeklenebilirlik ve sürdürülebilirlik hedeflenerek **.NET 8**, **Onion Architecture** ve **CQRS** desenleri üzerine inşa edilmiştir.

---

## 🏗️ Mimari ve Teknoloji Yığını

Proje, bağımlılıkların dıştan içe doğru olduğu **Domain-Centric Onion Architecture** (Modular Monolith) yapısına sahiptir.

| Katman | Teknoloji / Desen | Amaç |
| :--- | :--- | :--- |
| **Core** | .NET 8, C# 12 | Ana Geliştirme Platformu |
| **Database** | MSSQL Server 2022 (Docker) | İlişkisel Veri Yönetimi |
| **Cache** | Redis (Docker) | Performans ve Gündem Algoritması |
| **Storage** | MinIO (S3 Compatible) | Dosya ve Medya Depolama |
| **Data Access** | Entity Framework Core 8 | Code First ORM |
| **Communication** | MediatR (CQRS) | Gevşek Bağlılık (Loose Coupling) |
| **Validation** | FluentValidation | İş Kuralları Doğrulaması |

---

## ⚡ Temel Özellikler

- **Gelişmiş Profil Sistemi:** Öğrenciler için dijital CV, yetenek vitrini ve sertifika cüzdanı.
- **Topluluk Yönetimi:** Üniversite kulüpleri için üye yönetimi, etkinlik oluşturma ve duyuru sistemi.
- **Colog (Sosyal Akış):** Hashtag bazlı gündem takibi, içerik paylaşımı ve etkileşim (Sözlük/Twitter hibrit yapı).
- **CoNews (Kariyer & Etkinlik):** Staj ilanları, hackathonlar ve onaylı etkinliklerin listelendiği kurumsal akış.
- **Gamification:** Rozet sistemi, puanlama ve liderlik tabloları.
- **Güvenli Altyapı:** ASP.NET Core Identity, JWT Token ve Refresh Token mekanizması.

---

## ⚙️ Kurulum (Nasıl Çalıştırılır?)

Projeyi yerel ortamınızda ayağa kaldırmak için aşağıdaki adımları izleyin.

### 1. Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### 2. İndirme ve Altyapı
Terminali açın ve projeyi klonlayın:

```bash
git clone [https://github.com/KULLANICI_ADINIZ/SocietyLogs.git](https://github.com/KULLANICI_ADINIZ/SocietyLogs.git)
cd SocietyLogs
