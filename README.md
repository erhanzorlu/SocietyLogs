<!DOCTYPE html>
<html lang="tr">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>SocietyLogs Teknik Mimari Raporu</title>
    <!-- Tailwind CSS -->
    <script src="https://cdn.tailwindcss.com"></script>
    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    
    <!-- Chosen Palette: Slate & Emerald (Professional, Clean, Trustworthy) -->
    <!-- Warm neutral background with deep slate for structure and emerald for success/core elements. -->
    
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;600;700&display=swap');
        
        body { font-family: 'Inter', sans-serif; background-color: #f8fafc; color: #334155; }
        
        /* Custom scrollbar for clean look */
        ::-webkit-scrollbar { width: 8px; }
        ::-webkit-scrollbar-track { background: #f1f5f9; }
        ::-webkit-scrollbar-thumb { background: #cbd5e1; border-radius: 4px; }
        ::-webkit-scrollbar-thumb:hover { background: #94a3b8; }

        .glass-panel {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(10px);
            border: 1px solid rgba(226, 232, 240, 0.8);
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05), 0 2px 4px -1px rgba(0, 0, 0, 0.03);
        }

        .chart-container {
            position: relative;
            width: 100%;
            max-width: 100%;
            margin-left: auto;
            margin-right: auto;
            height: 300px;
        }

        @media (min-width: 768px) {
            .chart-container { height: 400px; }
        }

        .nav-item.active {
            background-color: #f1f5f9;
            border-left: 4px solid #10b981;
            color: #0f172a;
            font-weight: 600;
        }

        .transition-all-300 { transition: all 0.3s ease-in-out; }
        
        /* Interactive Tree View Styling */
        details > summary { list-style: none; cursor: pointer; }
        details > summary::-webkit-details-marker { display: none; }
        details[open] > summary ~ * { animation: sweep .5s ease-in-out; }
        
        @keyframes sweep {
            0%    {opacity: 0; transform: translateX(-10px)}
            100%  {opacity: 1; transform: translateX(0)}
        }
    </style>

    <!-- Application Structure Plan: 
         Structure: Dashboard layout with a sidebar navigation and a main content area.
         Rationale: The source report contains distinct, dense technical categories (Architecture, Tech Stack, Scenarios, Roadmap). 
         A dashboard approach allows the user to switch context easily without losing the "big picture".
         - Dashboard: High-level metrics and the "10-Year Vision".
         - Architecture (Soğan): Interactive Canvas visualization of the Onion Architecture layers.
         - Tech Stack: Chart.js bar chart categorizing tools, with filtering.
         - Scenarios (CQRS): Interactive step-by-step flow for key logic (Create Post, Feed).
         - Roadmap & Files: Visual timeline and tree structure for project organization.
    -->

    <!-- Visualization & Content Choices:
         1. Architecture: Custom HTML5 Canvas drawing (concentric circles) to visualize Onion Architecture. 
            Goal: Show dependencies (Outside -> In). Interaction: Hover to reveal layer details. 
            Justification: Best way to represent "Onion" without SVG.
         2. Tech Stack: Chart.js Bar Chart. Goal: Compare tool density per layer.
         3. Scenarios: Dynamic text/status steps using JS. Goal: Explain the data flow (CQRS).
         4. Roadmap: HTML/CSS Timeline. Goal: Show progression from Phase 0 to 4.
         Confirming NO SVG graphics used (using Canvas and Unicode). NO Mermaid JS used.
    -->

    <!-- CONFIRMATION: NO SVG graphics used. NO Mermaid JS used. -->
</head>
<body class="flex h-screen overflow-hidden">

    <!-- Sidebar Navigation -->
    <aside class="w-64 bg-white border-r border-slate-200 hidden md:flex flex-col z-20">
        <div class="p-6 border-b border-slate-100">
            <h1 class="text-2xl font-bold text-slate-800 tracking-tight">SocietyLogs</h1>
            <p class="text-xs text-slate-500 mt-1 uppercase tracking-widest">Teknik Rapor v2.0</p>
        </div>
        <nav class="flex-1 overflow-y-auto py-4">
            <ul class="space-y-1">
                <li><button onclick="navigate('dashboard')" id="nav-dashboard" class="nav-item w-full text-left px-6 py-3 text-slate-600 hover:bg-slate-50 transition-colors">📊 Genel Bakış</button></li>
                <li><button onclick="navigate('architecture')" id="nav-architecture" class="nav-item w-full text-left px-6 py-3 text-slate-600 hover:bg-slate-50 transition-colors">🧅 Soğan Mimarisi</button></li>
                <li><button onclick="navigate('techstack')" id="nav-techstack" class="nav-item w-full text-left px-6 py-3 text-slate-600 hover:bg-slate-50 transition-colors">🛠️ Teknoloji Yığını</button></li>
                <li><button onclick="navigate('scenarios')" id="nav-scenarios" class="nav-item w-full text-left px-6 py-3 text-slate-600 hover:bg-slate-50 transition-colors">⚡ Kritik Senaryolar</button></li>
                <li><button onclick="navigate('roadmap')" id="nav-roadmap" class="nav-item w-full text-left px-6 py-3 text-slate-600 hover:bg-slate-50 transition-colors">🗺️ Yol Haritası & Yapı</button></li>
            </ul>
        </nav>
        <div class="p-4 border-t border-slate-100 bg-slate-50">
            <div class="text-xs text-slate-400">Veritabanı: MSSQL (Docker)</div>
            <div class="text-xs text-slate-400 mt-1">Cache: Redis</div>
            <div class="text-xs text-emerald-600 font-bold mt-2">● Sistem Aktif (Faz 0)</div>
        </div>
    </aside>

    <!-- Mobile Header -->
    <div class="md:hidden fixed w-full bg-white border-b border-slate-200 z-50 p-4 flex justify-between items-center">
        <h1 class="font-bold text-lg">SocietyLogs</h1>
        <button onclick="document.getElementById('mobile-menu').classList.toggle('hidden')" class="text-slate-600 font-bold text-2xl">≡</button>
    </div>
    <div id="mobile-menu" class="hidden fixed inset-0 bg-white z-40 pt-20 px-6 space-y-4 md:hidden">
        <button onclick="navigate('dashboard'); toggleMobile()" class="block w-full text-left py-2 text-lg border-b">📊 Genel Bakış</button>
        <button onclick="navigate('architecture'); toggleMobile()" class="block w-full text-left py-2 text-lg border-b">🧅 Soğan Mimarisi</button>
        <button onclick="navigate('techstack'); toggleMobile()" class="block w-full text-left py-2 text-lg border-b">🛠️ Teknoloji Yığını</button>
        <button onclick="navigate('scenarios'); toggleMobile()" class="block w-full text-left py-2 text-lg border-b">⚡ Kritik Senaryolar</button>
        <button onclick="navigate('roadmap'); toggleMobile()" class="block w-full text-left py-2 text-lg border-b">🗺️ Yol Haritası</button>
    </div>

    <!-- Main Content Area -->
    <main class="flex-1 overflow-y-auto bg-slate-50 p-6 md:p-10 mt-16 md:mt-0 relative">
        
        <!-- SECTION: DASHBOARD -->
        <section id="dashboard" class="space-y-8 animate-fade-in">
            <div class="max-w-4xl">
                <h2 class="text-3xl font-bold text-slate-800 mb-4">Proje Özeti</h2>
                <p class="text-slate-600 leading-relaxed mb-6">
                    SocietyLogs; öğrencileri, toplulukları ve firmaları tek bir çatı altında toplayan, <strong>Modular Monolith</strong> yapısında, <strong>.NET 8</strong> tabanlı yeni nesil bir sosyal ağ ve kariyer platformudur. 
                    Mimarisi, <strong>Onion Architecture</strong> ve <strong>CQRS</strong> prensiplerine dayanarak, 10 yıllık sürdürülebilirlik vizyonuyla tasarlanmıştır.
                </p>
                
                <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
                    <div class="glass-panel p-6 rounded-xl border-l-4 border-blue-500">
                        <div class="text-sm text-slate-500 uppercase font-bold">Mimari</div>
                        <div class="text-2xl font-bold text-slate-800 mt-1">Onion</div>
                        <div class="text-xs text-blue-500 mt-2">Domain-Centric</div>
                    </div>
                    <div class="glass-panel p-6 rounded-xl border-l-4 border-emerald-500">
                        <div class="text-sm text-slate-500 uppercase font-bold">Veri Deseni</div>
                        <div class="text-2xl font-bold text-slate-800 mt-1">CQRS</div>
                        <div class="text-xs text-emerald-500 mt-2">MediatR ile</div>
                    </div>
                    <div class="glass-panel p-6 rounded-xl border-l-4 border-amber-500">
                        <div class="text-sm text-slate-500 uppercase font-bold">Altyapı</div>
                        <div class="text-2xl font-bold text-slate-800 mt-1">Docker</div>
                        <div class="text-xs text-amber-500 mt-2">MSSQL, Redis, MinIO</div>
                    </div>
                    <div class="glass-panel p-6 rounded-xl border-l-4 border-purple-500">
                        <div class="text-sm text-slate-500 uppercase font-bold">Durum</div>
                        <div class="text-2xl font-bold text-slate-800 mt-1">Faz 0</div>
                        <div class="text-xs text-purple-500 mt-2">Tamamlandı</div>
                    </div>
                </div>

                <div class="glass-panel p-8 rounded-xl">
                    <h3 class="text-xl font-bold text-slate-800 mb-4">🎯 10 Yıllık Sürdürülebilirlik Vizyonu</h3>
                    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div class="flex items-start space-x-3">
                            <div class="bg-blue-100 text-blue-600 p-2 rounded text-xl">♻️</div>
                            <div>
                                <h4 class="font-bold text-slate-700">Soft Delete</h4>
                                <p class="text-sm text-slate-500">Veri asla fiziksel silinmez. `IsDeleted` flag'i ile işaretlenir.</p>
                            </div>
                        </div>
                        <div class="flex items-start space-x-3">
                            <div class="bg-amber-100 text-amber-600 p-2 rounded text-xl">🛡️</div>
                            <div>
                                <h4 class="font-bold text-slate-700">Audit Logging</h4>
                                <p class="text-sm text-slate-500">Her değişikliğin (Kim, Ne zaman, Eski/Yeni değer) kaydı tutulur.</p>
                            </div>
                        </div>
                        <div class="flex items-start space-x-3">
                            <div class="bg-emerald-100 text-emerald-600 p-2 rounded text-xl">🆔</div>
                            <div>
                                <h4 class="font-bold text-slate-700">TSID / GUID</h4>
                                <p class="text-sm text-slate-500">Sıralı integer yerine dağıtık sistem uyumlu benzersiz ID'ler.</p>
                            </div>
                        </div>
                        <div class="flex items-start space-x-3">
                            <div class="bg-purple-100 text-purple-600 p-2 rounded text-xl">🌍</div>
                            <div>
                                <h4 class="font-bold text-slate-700">Localization</h4>
                                <p class="text-sm text-slate-500">Hata mesajları koddan ayrıştırıldı (.resx), globalleşmeye hazır.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- SECTION: ARCHITECTURE -->
        <section id="architecture" class="space-y-8 hidden animate-fade-in">
            <div class="max-w-5xl">
                <div class="mb-6">
                    <h2 class="text-3xl font-bold text-slate-800">Onion Architecture</h2>
                    <p class="text-slate-600 mt-2">
                        Proje, dıştan içe doğru bağımlılık kuralına göre tasarlanmıştır. 
                        <strong>Core</strong> katmanı hiçbir şeyi bilmez, dış katmanlar (API, Infra) Core'a bağımlıdır.
                        Çemberlerin üzerine gelerek detayları görebilirsiniz.
                    </p>
                </div>

                <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
                    <!-- Interactive Canvas Diagram -->
                    <div class="lg:col-span-2 glass-panel p-4 rounded-xl flex justify-center items-center bg-white">
                        <div class="chart-container" style="height: 450px;">
                            <canvas id="onionCanvas"></canvas>
                        </div>
                    </div>

                    <!-- Details Panel -->
                    <div class="glass-panel p-6 rounded-xl flex flex-col justify-center bg-slate-800 text-white">
                        <h3 id="layer-title" class="text-2xl font-bold mb-2 text-emerald-400">Katman Seçiniz</h3>
                        <p id="layer-desc" class="text-slate-300 mb-4">Soldaki diyagram üzerinde farenizi gezdirerek katmanların sorumluluklarını inceleyin.</p>
                        
                        <div id="layer-content" class="space-y-2 text-sm hidden">
                            <div class="bg-slate-700/50 p-2 rounded border-l-2 border-emerald-500">
                                <span class="font-bold block text-slate-200">İçerik:</span>
                                <span id="layer-items" class="text-slate-400">-</span>
                            </div>
                            <div class="bg-slate-700/50 p-2 rounded border-l-2 border-amber-500">
                                <span class="font-bold block text-slate-200">Kural:</span>
                                <span id="layer-rules" class="text-slate-400">-</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- SECTION: TECH STACK -->
        <section id="techstack" class="space-y-8 hidden animate-fade-in">
            <div class="max-w-5xl">
                <h2 class="text-3xl font-bold text-slate-800 mb-6">Teknoloji Yığını</h2>
                <p class="text-slate-600 mb-6">
                    Proje, kurumsal ölçeklenebilirlik için seçilmiş modern teknolojileri kullanır.
                    Grafikte teknolojilerin katmanlara göre dağılımını görebilirsiniz.
                </p>

                <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
                    <div class="glass-panel p-4 rounded-xl">
                        <div class="chart-container">
                            <canvas id="techChart"></canvas>
                        </div>
                    </div>

                    <div class="space-y-3">
                        <div class="p-3 bg-white rounded-lg shadow-sm border border-slate-100 hover:border-emerald-500 transition-colors cursor-default">
                            <div class="flex justify-between items-center">
                                <span class="font-bold text-slate-700">.NET 8 & C# 12</span>
                                <span class="text-xs bg-emerald-100 text-emerald-700 px-2 py-1 rounded">Core</span>
                            </div>
                            <p class="text-xs text-slate-500 mt-1">Ana geliştirme platformu. Yüksek performans.</p>
                        </div>
                        <div class="p-3 bg-white rounded-lg shadow-sm border border-slate-100 hover:border-amber-500 transition-colors cursor-default">
                            <div class="flex justify-between items-center">
                                <span class="font-bold text-slate-700">MSSQL Server 2022</span>
                                <span class="text-xs bg-amber-100 text-amber-700 px-2 py-1 rounded">Persistence</span>
                            </div>
                            <p class="text-xs text-slate-500 mt-1">Ana ilişkisel veritabanı. Docker üzerinde çalışır.</p>
                        </div>
                        <div class="p-3 bg-white rounded-lg shadow-sm border border-slate-100 hover:border-blue-500 transition-colors cursor-default">
                            <div class="flex justify-between items-center">
                                <span class="font-bold text-slate-700">MediatR</span>
                                <span class="text-xs bg-blue-100 text-blue-700 px-2 py-1 rounded">Application</span>
                            </div>
                            <p class="text-xs text-slate-500 mt-1">CQRS ve Loose Coupling için mesajlaşma kütüphanesi.</p>
                        </div>
                         <div class="p-3 bg-white rounded-lg shadow-sm border border-slate-100 hover:border-purple-500 transition-colors cursor-default">
                            <div class="flex justify-between items-center">
                                <span class="font-bold text-slate-700">Redis & MinIO</span>
                                <span class="text-xs bg-purple-100 text-purple-700 px-2 py-1 rounded">Infra</span>
                            </div>
                            <p class="text-xs text-slate-500 mt-1">Önbellekleme (Cache) ve S3 uyumlu dosya depolama.</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- SECTION: SCENARIOS -->
        <section id="scenarios" class="space-y-8 hidden animate-fade-in">
            <div class="max-w-4xl">
                <h2 class="text-3xl font-bold text-slate-800 mb-6">Kritik İş Akışları (CQRS)</h2>
                <p class="text-slate-600 mb-8">
                    Sistemde Command (Yazma) ve Query (Okuma) işlemleri ayrılmıştır. 
                    Aşağıda "Post Oluşturma" ve "Anasayfa Akışı" senaryolarının veri akışını simüle edebilirsiniz.
                </p>

                <!-- Scenario Selector -->
                <div class="flex space-x-4 mb-6">
                    <button onclick="loadScenario('createPost')" id="btn-createPost" class="px-4 py-2 bg-slate-800 text-white rounded-lg shadow hover:bg-slate-700 transition">📝 Post Oluştur (Command)</button>
                    <button onclick="loadScenario('homeFeed')" id="btn-homeFeed" class="px-4 py-2 bg-white text-slate-700 border border-slate-200 rounded-lg hover:bg-slate-50 transition">👀 Akış Getir (Query)</button>
                </div>

                <!-- Scenario Visualization Container -->
                <div class="glass-panel p-8 rounded-xl bg-white min-h-[300px] relative overflow-hidden">
                    <div id="scenario-canvas" class="space-y-6">
                        <!-- Steps will be injected here via JS -->
                    </div>
                </div>
            </div>
        </section>

        <!-- SECTION: ROADMAP & STRUCTURE -->
        <section id="roadmap" class="space-y-8 hidden animate-fade-in">
            <div class="max-w-5xl">
                <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
                    
                    <!-- Roadmap -->
                    <div>
                        <h2 class="text-2xl font-bold text-slate-800 mb-6">🗺️ Geliştirme Yol Haritası</h2>
                        <div class="space-y-0 pl-4 border-l-2 border-slate-200">
                            <div class="relative pb-8">
                                <div class="absolute -left-[21px] bg-emerald-500 h-4 w-4 rounded-full border-4 border-white shadow"></div>
                                <h3 class="font-bold text-emerald-600">Faz 0: Altyapı & Pilot</h3>
                                <p class="text-sm text-slate-500 mt-1">Docker kurulumu, Onion iskeleti, BaseEntity, GenericRepo, Swagger. (Tamamlandı)</p>
                            </div>
                            <div class="relative pb-8">
                                <div class="absolute -left-[21px] bg-slate-300 h-4 w-4 rounded-full border-4 border-white"></div>
                                <h3 class="font-bold text-slate-700">Faz 1: Kimlik (Identity)</h3>
                                <p class="text-sm text-slate-500 mt-1">User entity, Login/Register, JWT Token, Refresh Token.</p>
                            </div>
                            <div class="relative pb-8">
                                <div class="absolute -left-[21px] bg-slate-300 h-4 w-4 rounded-full border-4 border-white"></div>
                                <h3 class="font-bold text-slate-700">Faz 2: Çekirdek Modüller</h3>
                                <p class="text-sm text-slate-500 mt-1">Topluluk yönetimi, Colog (Post), Yorumlar, CQRS implementasyonu.</p>
                            </div>
                            <div class="relative pb-8">
                                <div class="absolute -left-[21px] bg-slate-300 h-4 w-4 rounded-full border-4 border-white"></div>
                                <h3 class="font-bold text-slate-700">Faz 3: Gamification & Jobs</h3>
                                <p class="text-sm text-slate-500 mt-1">Puan sistemi, Rozetler, Hangfire bülten gönderimi, Redis Gündem algoritması.</p>
                            </div>
                        </div>
                    </div>

                    <!-- Project Structure Tree -->
                    <div>
                        <h2 class="text-2xl font-bold text-slate-800 mb-6">📂 Klasör Yapısı</h2>
                        <div class="glass-panel p-6 rounded-xl bg-slate-900 text-slate-300 font-mono text-sm overflow-hidden">
                            <details open>
                                <summary class="hover:text-white">src</summary>
                                <div class="pl-4 border-l border-slate-700">
                                    <details open>
                                        <summary class="text-emerald-400 hover:text-emerald-300">Core</summary>
                                        <div class="pl-4 border-l border-slate-700">
                                            <details>
                                                <summary class="hover:text-white">SocietyLogs.Domain</summary>
                                                <div class="pl-4 text-slate-500">
                                                    <div>📄 BaseEntity.cs</div>
                                                    <div>📄 Company.cs</div>
                                                    <div>📄 User.cs</div>
                                                </div>
                                            </details>
                                            <details>
                                                <summary class="hover:text-white">SocietyLogs.Application</summary>
                                                <div class="pl-4 text-slate-500">
                                                    <div>📂 Features (CQRS)</div>
                                                    <div>📂 Interfaces</div>
                                                    <div>📂 Validators</div>
                                                </div>
                                            </details>
                                        </div>
                                    </details>
                                    <details open>
                                        <summary class="text-amber-400 hover:text-amber-300">Infrastructure</summary>
                                        <div class="pl-4 border-l border-slate-700">
                                            <details>
                                                <summary class="hover:text-white">SocietyLogs.Persistence</summary>
                                                <div class="pl-4 text-slate-500">
                                                    <div>📄 AppDbContext.cs</div>
                                                    <div>📄 GenericRepository.cs</div>
                                                    <div>📄 AuditInterceptor.cs</div>
                                                </div>
                                            </details>
                                            <details>
                                                <summary class="hover:text-white">SocietyLogs.Infrastructure</summary>
                                                <div class="pl-4 text-slate-500">
                                                    <div>📂 Services (Mail, File)</div>
                                                </div>
                                            </details>
                                        </div>
                                    </details>
                                    <details open>
                                        <summary class="text-blue-400 hover:text-blue-300">Presentation</summary>
                                        <div class="pl-4 border-l border-slate-700">
                                            <details>
                                                <summary class="hover:text-white">SocietyLogs.API</summary>
                                                <div class="pl-4 text-slate-500">
                                                    <div>📂 Controllers</div>
                                                    <div>📄 Program.cs</div>
                                                </div>
                                            </details>
                                        </div>
                                    </details>
                                </div>
                            </details>
                            <div class="mt-2 text-slate-500">📄 docker-compose.yml</div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

    </main>

    <script>
        // --- Navigation Logic ---
        function navigate(sectionId) {
            // Hide all sections
            document.querySelectorAll('main > section').forEach(el => {
                el.classList.add('hidden');
            });
            
            // Show selected section
            const target = document.getElementById(sectionId);
            target.classList.remove('hidden');
            
            // Update Sidebar Active State
            document.querySelectorAll('.nav-item').forEach(el => {
                el.classList.remove('active');
            });
            document.getElementById(`nav-${sectionId}`).classList.add('active');

            // Trigger animations or chart redraws if necessary
            if (sectionId === 'architecture') drawOnionArchitecture();
            if (sectionId === 'techstack') renderTechChart();
        }

        function toggleMobile() {
            document.getElementById('mobile-menu').classList.add('hidden');
        }

        // --- Onion Architecture Visualization (Canvas) ---
        function drawOnionArchitecture() {
            const canvas = document.getElementById('onionCanvas');
            const ctx = canvas.getContext('2d');
            
            // Set canvas resolution for crisp rendering
            const dpr = window.devicePixelRatio || 1;
            const rect = canvas.getBoundingClientRect();
            canvas.width = rect.width * dpr;
            canvas.height = rect.height * dpr;
            ctx.scale(dpr, dpr);

            const centerX = rect.width / 2;
            const centerY = rect.height / 2;
            const maxRadius = Math.min(centerX, centerY) - 20;

            // Layers: [Name, Color, Description, Content, Rules]
            const layers = [
                { r: maxRadius, color: '#3b82f6', name: 'Presentation (API)', desc: 'Dış Dünyaya Açılan Kapı', content: 'Controllers, Middlewares', rules: 'Sadece Core ve Infra kullanır.' },
                { r: maxRadius * 0.75, color: '#f59e0b', name: 'Infrastructure', desc: 'Dış Kaynaklar ve DB', content: 'EF Core, Redis Service, MailKit', rules: 'Domain ve Application implementasyonu.' },
                { r: maxRadius * 0.50, color: '#10b981', name: 'Application', desc: 'İş Mantığı ve Kurallar', content: 'MediatR Handlers, Interfaces, Validators', rules: 'Sadece Domain\'i bilir.' },
                { r: maxRadius * 0.25, color: '#ef4444', name: 'Domain', desc: 'Çekirdek Varlıklar', content: 'Entities, Enums, Exceptions', rules: 'Hiçbir bağımlılığı yoktur.' }
            ];

            // Draw Circles
            layers.forEach(layer => {
                ctx.beginPath();
                ctx.arc(centerX, centerY, layer.r, 0, 2 * Math.PI);
                ctx.fillStyle = layer.color + '20'; // Transparent fill
                ctx.fill();
                ctx.lineWidth = 2;
                ctx.strokeStyle = layer.color;
                ctx.stroke();
                
                // Draw Label
                ctx.fillStyle = layer.color;
                ctx.font = 'bold 12px Inter';
                ctx.textAlign = 'center';
                ctx.textBaseline = 'bottom';
                ctx.fillText(layer.name, centerX, centerY - layer.r + 15);
            });

            // Interaction
            canvas.onmousemove = function(e) {
                const rect = canvas.getBoundingClientRect();
                const x = e.clientX - rect.left - centerX;
                const y = e.clientY - rect.top - centerY;
                const dist = Math.sqrt(x*x + y*y);

                let activeLayer = null;
                // Check from smallest to largest to catch inner circles first
                for (let i = layers.length - 1; i >= 0; i--) {
                    if (dist < layers[i].r) {
                        activeLayer = layers[i];
                        break;
                    }
                }

                if (activeLayer) {
                    document.getElementById('layer-title').innerText = activeLayer.name;
                    document.getElementById('layer-title').style.color = activeLayer.color;
                    document.getElementById('layer-desc').innerText = activeLayer.desc;
                    document.getElementById('layer-content').classList.remove('hidden');
                    document.getElementById('layer-items').innerText = activeLayer.content;
                    document.getElementById('layer-rules').innerText = activeLayer.rules;
                    canvas.style.cursor = 'pointer';
                } else {
                    canvas.style.cursor = 'default';
                }
            };
        }

        // --- Tech Stack Chart (Chart.js) ---
        let techChartInstance = null;
        function renderTechChart() {
            if (techChartInstance) return;

            const ctx = document.getElementById('techChart').getContext('2d');
            techChartInstance = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['Core (Domain/App)', 'Persistence (DB)', 'Infrastructure (Ext)', 'Presentation (API)'],
                    datasets: [{
                        label: 'Teknoloji/Kütüphane Sayısı',
                        data: [6, 4, 5, 3], // Approximate count based on report
                        backgroundColor: [
                            'rgba(16, 185, 129, 0.6)', // Emerald
                            'rgba(245, 158, 11, 0.6)', // Amber
                            'rgba(139, 92, 246, 0.6)', // Purple
                            'rgba(59, 130, 246, 0.6)'  // Blue
                        ],
                        borderColor: [
                            'rgb(16, 185, 129)',
                            'rgb(245, 158, 11)',
                            'rgb(139, 92, 246)',
                            'rgb(59, 130, 246)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    responsive: true,
                    scales: {
                        y: { beginAtZero: true, ticks: { stepSize: 1 } }
                    },
                    plugins: {
                        legend: { display: false },
                        tooltip: {
                            callbacks: {
                                afterLabel: function(context) {
                                    const stacks = [
                                        ['.NET 8', 'MediatR', 'FluentValidation', 'Mapster', 'Entities', 'Interfaces'],
                                        ['MSSQL', 'EF Core', 'Migrations', 'Interceptors'],
                                        ['Redis', 'MinIO', 'MailKit', 'Hangfire', 'Serilog'],
                                        ['Controllers', 'Swagger', 'JWT']
                                    ];
                                    return 'Örnekler: ' + stacks[context.dataIndex].join(', ');
                                }
                            }
                        }
                    }
                }
            });
        }

        // --- Scenario Logic ---
        const scenarios = {
            createPost: {
                title: '📝 Post Oluşturma (Command Flow)',
                steps: [
                    { role: 'API', text: 'Frontend JSON gönderir: { title: "Merhaba" }', color: 'bg-blue-100 border-blue-500' },
                    { role: 'Validation', text: 'FluentValidation: Boş mu? Küfür var mı?', color: 'bg-yellow-100 border-yellow-500' },
                    { role: 'Handler', text: 'CreatePostHandler çalışır. Entity oluşturulur.', color: 'bg-emerald-100 border-emerald-500' },
                    { role: 'Repo/UoW', text: 'Repository hafızaya alır. UnitOfWork DB\'ye Commit eder.', color: 'bg-amber-100 border-amber-500' },
                    { role: 'Event', text: 'PostCreatedEvent fırlatılır -> Puan Sistemi tetiklenir.', color: 'bg-purple-100 border-purple-500' }
                ]
            },
            homeFeed: {
                title: '👀 Anasayfa Akışı (Query Flow)',
                steps: [
                    { role: 'API', text: 'Frontend istek atar: GetHomeFeedQuery', color: 'bg-blue-100 border-blue-500' },
                    { role: 'Cache Check', text: 'Handler önce Redis\'e sorar: "Feed_User_1 var mı?"', color: 'bg-purple-100 border-purple-500' },
                    { role: 'Cache Hit', text: 'Varsa: DB\'ye gitmeden anında veri döner. (Milisaniyeler)', color: 'bg-emerald-100 border-emerald-500' },
                    { role: 'Cache Miss', text: 'Yoksa: MSSQL\'den çek, Redis\'e yaz, dön.', color: 'bg-gray-100 border-gray-500' }
                ]
            }
        };

        function loadScenario(type) {
            const container = document.getElementById('scenario-canvas');
            const data = scenarios[type];
            
            // Highlight Buttons
            document.getElementById('btn-createPost').className = type === 'createPost' ? 'px-4 py-2 bg-slate-800 text-white rounded-lg shadow transition' : 'px-4 py-2 bg-white text-slate-700 border border-slate-200 rounded-lg hover:bg-slate-50 transition';
            document.getElementById('btn-homeFeed').className = type === 'homeFeed' ? 'px-4 py-2 bg-slate-800 text-white rounded-lg shadow transition' : 'px-4 py-2 bg-white text-slate-700 border border-slate-200 rounded-lg hover:bg-slate-50 transition';

            container.innerHTML = `<h3 class="text-lg font-bold mb-4">${data.title}</h3>`;
            
            data.steps.forEach((step, index) => {
                setTimeout(() => {
                    const el = document.createElement('div');
                    el.className = `p-4 rounded-lg border-l-4 ${step.color} transform transition-all duration-500 translate-x-10 opacity-0`;
                    el.innerHTML = `<span class="font-bold text-xs uppercase tracking-wide opacity-50 block mb-1">${step.role}</span><div class="text-slate-800">${step.text}</div>`;
                    container.appendChild(el);
                    
                    // Trigger animation
                    requestAnimationFrame(() => {
                        el.classList.remove('translate-x-10', 'opacity-0');
                    });
                    
                    // Add arrow if not last
                    if (index < data.steps.length - 1) {
                        const arrow = document.createElement('div');
                        arrow.className = 'flex justify-center text-slate-300 text-xl py-1 opacity-0 transition-opacity duration-500';
                        arrow.innerHTML = '↓';
                        container.appendChild(arrow);
                        requestAnimationFrame(() => arrow.classList.remove('opacity-0'));
                    }
                }, index * 600);
            });
        }

        // Initialize
        navigate('dashboard');
    </script>
</body>
</html>
