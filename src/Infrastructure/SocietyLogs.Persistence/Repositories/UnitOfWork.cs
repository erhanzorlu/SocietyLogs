using Microsoft.EntityFrameworkCore.Storage;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Common;
using SocietyLogs.Persistence.Contexts;
using System.Collections;

namespace SocietyLogs.Persistence.Repositories
{
    /// <summary>
    /// UNIT OF WORK (UoW) IMPLEMENTASYONU
    /// -----------------------------------------------------------------------
    /// Amaç: Veritabanı işlemlerini (CRUD) tek bir merkezden yönetmek ve 
    /// yapılan tüm değişikliklerin (Transaction) bütünlüğünü garanti altına almaktır.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        // Veritabanı bağlantımızı sağlayan ana EF Core nesnesi.
        private readonly AppDbContext _context;

        // PERFORMANS OPTİMİZASYONU (Caching):
        // Her repository istendiğinde 'new' anahtar kelimesi ile yeniden oluşturmak 
        // belleği yorar. Bu Hashtable (Sözlük), oluşturulan repository'leri hafızada tutar.
        // Aynı istek içinde ikinci kez istenirse, direkt buradan verilir.
        private Hashtable _repositories;

        // MİMARİ KORUMA (Encapsulation):
        // IDbContextTransaction, EF Core'a ait bir nesnedir.
        // Clean Architecture gereği, Core katmanı EF Core'u bilmemelidir.
        // Bu yüzden Transaction nesnesini burada 'private' tutuyoruz, dışarı sızdırmıyoruz.
        private IDbContextTransaction? _currentTransaction;

        /// <summary>
        /// Constructor: Bağımlılıkları (Dependency Injection) içeri alır.
        /// </summary>
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// AKILLI REPOSITORY ÇÖZÜCÜSÜ (The Magic Method)
        /// -------------------------------------------------------------------
        /// Geliştiriciyi her tablo için ayrı property tanımlama hamallığından kurtarır.
        /// Örn: _uow.Repository<Company>() dendiğinde arka planda her şeyi halleder.
        /// </summary>
        public IGenericRepository<T> Repository<T>() where T : class, IEntity
        {
            // 1. Adım: Depo kutusu (Hashtable) henüz oluşmamışsa oluştur.
            if (_repositories == null)
                _repositories = new Hashtable();

            // İstenen Entity'nin tipini al (Örn: Company, Product, User)
            var type = typeof(T);

            // 2. Adım: Bu repository daha önce oluşturulmuş mu?
            // Eğer kutuda varsa, yenisini oluşturma maliyetine girme, kutudakini ver.
            if (_repositories.ContainsKey(type))
            {
                return (IGenericRepository<T>)_repositories[type]!;
            }

            // 3. Adım: Eğer yoksa, Reflection kullanarak dinamik bir şekilde oluştur.
            // Bu satır: "new GenericRepository<Company>(_context)" kodunun dinamik halidir.
            var repositoryType = typeof(GenericRepository<>);

            // Generic tipi T (Company) ile birleştirip nesneyi üretiyoruz (Instantiate).
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

            // 4. Adım: Üretilen nesneyi kutuya (Cache) at ki bir dahaki sefere hızlı olsun.
            _repositories.Add(type, repositoryInstance);

            return (IGenericRepository<T>)repositoryInstance!;
        }

        /// <summary>
        /// COMMIT İŞLEMİ (Kaydetme)
        /// -------------------------------------------------------------------
        /// Generic Repository'lerde SaveChanges metodunu bilerek kullanmadık.
        /// Tüm ekleme, silme, güncelleme işlemleri bellekte birikir,
        /// sadece bu metod çağrıldığında veritabanına tek seferde gidilir.
        /// </summary>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // ====================================================================
        // TRANSACTION YÖNETİMİ (İşlem Bütünlüğü)
        // ====================================================================

        /// <summary>
        /// İşlemler zincirini başlatır. Hata olursa hepsi geri alınabilsin diye nokta koyar.
        /// </summary>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            // Eğer zaten açık bir transaction varsa, ikincisini açma.
            if (_currentTransaction != null) return;

            // EF Core üzerinden transaction başlat.
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// İşlemleri onayla ve veritabanına kalıcı olarak yaz.
        /// </summary>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Transaction başarılı bir şekilde başlatılmışsa onayla (Commit).
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                // Eğer Commit sırasında bir hata olursa (DB koptu, Constraint hatası vs.),
                // yapılan her şeyi geri al (Rollback) ki veri bozulmasın.
                await RollbackTransactionAsync(cancellationToken);
                throw; // Hatayı yutma, yukarı fırlat ki API haberdar olsun.
            }
            finally
            {
                // İşlem bitince transaction nesnesini bellekten temizle.
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// Acil Durum Butonu: İşlemleri geri alır.
        /// </summary>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <summary>
        /// TEMİZLİK (Garbage Collection)
        /// -------------------------------------------------------------------
        /// İstek (Request) bittiğinde bu sınıf yok edilirken açık kalan bağlantıları kapatır.
        /// Memory Leak (Bellek sızıntısı) olmaması için kritiktir.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            // Önce Transaction'ı kapat.
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }

            // Sonra veritabanı bağlantısını (DbContext) kapat.
            await _context.DisposeAsync();
        }
    }
}