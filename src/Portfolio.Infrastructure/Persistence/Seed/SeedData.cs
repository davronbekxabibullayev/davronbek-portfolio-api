using Portfolio.Domain.Common;
using Portfolio.Domain.Entities;

namespace Portfolio.Infrastructure.Persistence.Seed;

/// <summary>
/// First-run content. After the admin panel exists, edit content there instead of here.
/// Items marked TODO are placeholders to replace with real facts.
/// </summary>
internal static class SeedData
{
    public static Profile Profile() => new()
    {
        Email = "davronbekxabibullayev03.06.88@gmail.com",
        LinkedInUrl = "https://www.linkedin.com/in/davronbek-xabibullayev-197422235/",
        GitHubUrl = null,   // TODO
        TelegramUrl = null, // TODO
        PhotoUrl = null,    // TODO
        CareerStartDate = new DateOnly(2023, 6, 1),
        TeamSize = 3,
        OpenToWork = true,
        Translations =
        [
            new ProfileTranslation
            {
                LanguageCode = Languages.En,
                FullName = "Davronbek Xabibullayev",
                Headline = ".NET Team Lead · Backend Engineer",
                Tagline = "I design and build high-load backend systems with .NET and PostgreSQL — from architecture to production.",
                Location = "Tashkent / Andijon, Uzbekistan",
                About =
                    "I'm a .NET developer and Team Lead at BePro-DevHub, where I lead a team of three backend developers. " +
                    "I build B2B SaaS products for industries like construction, retail, emergency services and education.\n\n" +
                    "My day-to-day covers architecture design, project planning and mentoring. I'm strongest in high-load backend " +
                    "systems and PostgreSQL performance tuning. I care about clean boundaries, predictable deployments, and code " +
                    "the next developer can read.",
            },
            new ProfileTranslation
            {
                LanguageCode = Languages.Uz,
                FullName = "Davronbek Xabibullayev",
                Headline = ".NET Team Lead · Backend muhandis",
                Tagline = ".NET va PostgreSQL asosida yuqori yuklamali backend tizimlarni loyihalayman va quraman — arxitekturadan productiongacha.",
                Location = "Toshkent / Andijon, O'zbekiston",
                About =
                    "Men BePro-DevHub kompaniyasida .NET dasturchi va Team Leadman, uch nafar backend dasturchidan iborat jamoani boshqaraman. " +
                    "Qurilish, savdo, favqulodda xizmatlar va ta'lim sohalari uchun B2B SaaS mahsulotlar yarataman.\n\n" +
                    "Kundalik ishim — arxitektura loyihalash, rejalashtirish va mentorlik. Eng kuchli tomonim — yuqori yuklamali backend " +
                    "tizimlar va PostgreSQL unumdorligini optimallashtirish. Toza chegaralar, barqaror deploy va keyingi dasturchi " +
                    "oson o'qiy oladigan kod men uchun muhim.",
            },
            new ProfileTranslation
            {
                LanguageCode = Languages.Ru,
                FullName = "Давронбек Хабибуллаев",
                Headline = ".NET Team Lead · Backend-инженер",
                Tagline = "Проектирую и создаю высоконагруженные backend-системы на .NET и PostgreSQL — от архитектуры до продакшена.",
                Location = "Ташкент / Андижан, Узбекистан",
                About =
                    "Я .NET-разработчик и тимлид в BePro-DevHub, руковожу командой из трёх backend-разработчиков. " +
                    "Создаю B2B SaaS-продукты для строительства, ритейла, экстренных служб и образования.\n\n" +
                    "В мои задачи входят проектирование архитектуры, планирование и менторство. Сильнее всего я в высоконагруженных " +
                    "backend-системах и оптимизации производительности PostgreSQL. Ценю чёткие границы модулей, предсказуемые деплои " +
                    "и код, который легко читать следующему разработчику.",
            },
        ],
    };

    public static IEnumerable<SkillCategory> SkillCategories()
    {
        var order = 0;

        SkillCategory Category(string en, string uz, string ru, params string[] skills) => new()
        {
            SortOrder = order++,
            Translations =
            [
                new SkillCategoryTranslation { LanguageCode = Languages.En, Name = en },
                new SkillCategoryTranslation { LanguageCode = Languages.Uz, Name = uz },
                new SkillCategoryTranslation { LanguageCode = Languages.Ru, Name = ru },
            ],
            Skills = skills.Select((s, i) => new Skill { Name = s, SortOrder = i }).ToList(),
        };

        return
        [
            Category("Backend", "Backend", "Backend",
                "C#", ".NET 8", "ASP.NET Core", "EF Core", "Dapper", "REST API", "SignalR"),
            Category("Architecture", "Arxitektura", "Архитектура",
                "Clean Architecture", "CQRS", "Microservices", "Multi-tenancy", "Ocelot"),
            Category("Data", "Ma'lumotlar", "Данные",
                "PostgreSQL", "Query tuning", "GIN / partial indexes", "Redis", "OpenSearch", "MinIO"),
            Category("Messaging & Security", "Xabar almashish va xavfsizlik", "Очереди и безопасность",
                "RabbitMQ", "MassTransit", "JWT", "OAuth2", "RBAC"),
            Category("DevOps", "DevOps", "DevOps",
                "Docker", "Nginx", "GitLab CI/CD", "Hetzner", "Cloudflare", "Ubuntu"),
            Category("Integrations & Frontend", "Integratsiyalar va frontend", "Интеграции и frontend",
                "Telegram Bot API", "Firebase", "Eskiz.uz SMS", "Angular"),
        ];
    }

    public static IEnumerable<Experience> Experiences() =>
    [
        new Experience
        {
            Company = "BePro-DevHub LLC",
            StartDate = new DateOnly(2024, 9, 1),
            EndDate = null,
            SortOrder = 0,
            Translations =
            [
                new ExperienceTranslation
                {
                    LanguageCode = Languages.En,
                    Role = "Team Lead, .NET",
                    Highlights =
                    [
                        "Lead a team of 3 .NET developers; own architecture decisions, planning and code review",
                        "Mentor developers on Clean Architecture, CQRS and PostgreSQL performance",
                    ],
                },
                new ExperienceTranslation
                {
                    LanguageCode = Languages.Uz,
                    Role = "Team Lead, .NET",
                    Highlights =
                    [
                        "3 nafar .NET dasturchidan iborat jamoani boshqaraman; arxitektura qarorlari, rejalashtirish va code review menda",
                        "Dasturchilarga Clean Architecture, CQRS va PostgreSQL unumdorligi bo'yicha mentorlik qilaman",
                    ],
                },
                new ExperienceTranslation
                {
                    LanguageCode = Languages.Ru,
                    Role = "Team Lead, .NET",
                    Highlights =
                    [
                        "Руковожу командой из 3 .NET-разработчиков; отвечаю за архитектурные решения, планирование и code review",
                        "Менторю разработчиков по Clean Architecture, CQRS и производительности PostgreSQL",
                    ],
                },
            ],
        },
        new Experience
        {
            Company = "BePro-DevHub LLC",
            StartDate = new DateOnly(2023, 6, 1),
            EndDate = new DateOnly(2024, 8, 31),
            SortOrder = 1,
            Translations =
            [
                new ExperienceTranslation
                {
                    LanguageCode = Languages.En,
                    Role = "C# / .NET Developer",
                    Highlights = ["Promoted to Team Lead"], // TODO: main responsibilities
                },
                new ExperienceTranslation
                {
                    LanguageCode = Languages.Uz,
                    Role = "C# / .NET dasturchi",
                    Highlights = ["Team Lead lavozimiga ko'tarildim"],
                },
                new ExperienceTranslation
                {
                    LanguageCode = Languages.Ru,
                    Role = "C# / .NET-разработчик",
                    Highlights = ["Повышен до тимлида"],
                },
            ],
        },
    ];

    public static IEnumerable<Project> Projects() =>
    [
        new Project
        {
            Slug = "uds-emergency-dispatch",
            Stack = [".NET", "SignalR", "MassTransit", "RabbitMQ", "PostgreSQL", "EF Core"],
            IsFeatured = true,
            IsPublished = true,
            SortOrder = 0,
            Translations =
            [
                new ProjectTranslation
                {
                    LanguageCode = Languages.En,
                    Title = "UDS — Emergency Dispatch System",
                    Category = "Real-time · Public safety",
                    Summary = "Real-time ambulance tracking over SignalR, event-driven workflows with MassTransit, and integrations with external services.",
                    Problem = "Emergency services need to coordinate calls and ambulances in real time across several organisations.",
                    Solution =
                        "- Live ambulance tracking pushed to dispatchers over SignalR/WebSockets\n" +
                        "- Event-driven communication between services with MassTransit\n" +
                        "- Integrations with external systems, including EMaterial and police control cards\n" +
                        "- Fixed data-integrity issues with PostgreSQL partial unique indexes and duplicate cleanup embedded in the migration",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Uz,
                    Title = "UDS — Favqulodda xizmatlar dispetcherlik tizimi",
                    Category = "Real vaqt · Jamoat xavfsizligi",
                    Summary = "SignalR orqali tez yordam mashinalarini real vaqtda kuzatish, MassTransit bilan event-driven jarayonlar va tashqi tizimlar bilan integratsiya.",
                    Problem = "Favqulodda xizmatlar bir nechta tashkilot o'rtasida chaqiruvlar va tez yordam mashinalarini real vaqtda muvofiqlashtirishi kerak.",
                    Solution =
                        "- Tez yordam mashinalari joylashuvi SignalR/WebSocket orqali dispetcherlarga jonli uzatiladi\n" +
                        "- Servislar o'rtasida MassTransit bilan event-driven aloqa\n" +
                        "- Tashqi tizimlar bilan integratsiya, jumladan EMaterial va politsiya nazorat kartalari\n" +
                        "- PostgreSQL partial unique index va migratsiya ichidagi dublikatlarni tozalash orqali ma'lumotlar yaxlitligi muammolari hal qilindi",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Ru,
                    Title = "UDS — Система диспетчеризации экстренных служб",
                    Category = "Real-time · Общественная безопасность",
                    Summary = "Отслеживание машин скорой помощи в реальном времени через SignalR, event-driven процессы на MassTransit и интеграции с внешними системами.",
                    Problem = "Экстренным службам нужно координировать вызовы и бригады скорой помощи в реальном времени между несколькими организациями.",
                    Solution =
                        "- Местоположение машин скорой помощи в реальном времени передаётся диспетчерам через SignalR/WebSockets\n" +
                        "- Событийное взаимодействие сервисов на MassTransit\n" +
                        "- Интеграции с внешними системами, включая EMaterial и карточки контроля полиции\n" +
                        "- Проблемы целостности данных решены частичными уникальными индексами PostgreSQL и очисткой дублей внутри миграции",
                },
            ],
        },
        new Project
        {
            Slug = "storeos-retail-saas",
            Stack = [".NET", "Microservices", "Ocelot", "CQRS", "PostgreSQL"],
            IsFeatured = false,
            IsPublished = true,
            SortOrder = 1,
            Translations =
            [
                new ProjectTranslation
                {
                    LanguageCode = Languages.En,
                    Title = "StoreOS — Multi-branch Retail SaaS",
                    Category = "SaaS · Microservices",
                    Summary = "Multi-tenant SaaS behind an Ocelot gateway: POS, inventory, catalog, CRM, loyalty and analytics, built with Clean Architecture and CQRS.",
                    Problem = "Retail chains need one system for every branch: sales, stock and customers.",
                    Solution =
                        "- Microservices behind an Ocelot API gateway\n" +
                        "- Clean Architecture + CQRS in every service, shared BuildingBlocks libraries\n" +
                        "- Multi-tenant by design, built as a monetizable SaaS\n" +
                        "- Modules: POS, inventory, catalog, CRM, loyalty, analytics",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Uz,
                    Title = "StoreOS — Ko'p filialli savdo SaaS",
                    Category = "SaaS · Mikroservislar",
                    Summary = "Ocelot gateway ortidagi multi-tenant SaaS: POS, ombor, katalog, CRM, sodiqlik dasturi va analitika — Clean Architecture va CQRS asosida.",
                    Problem = "Savdo tarmoqlariga barcha filiallar uchun yagona tizim kerak: savdo, qoldiq va mijozlar.",
                    Solution =
                        "- Ocelot API gateway ortidagi mikroservislar\n" +
                        "- Har bir servisda Clean Architecture + CQRS, umumiy BuildingBlocks kutubxonalari\n" +
                        "- Boshidanoq multi-tenant, daromad keltiradigan SaaS sifatida qurilgan\n" +
                        "- Modullar: POS, ombor, katalog, CRM, sodiqlik, analitika",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Ru,
                    Title = "StoreOS — SaaS для сетевого ритейла",
                    Category = "SaaS · Микросервисы",
                    Summary = "Мультитенантный SaaS за шлюзом Ocelot: POS, склад, каталог, CRM, лояльность и аналитика на Clean Architecture и CQRS.",
                    Problem = "Торговым сетям нужна единая система для всех филиалов: продажи, остатки и клиенты.",
                    Solution =
                        "- Микросервисы за API-шлюзом Ocelot\n" +
                        "- Clean Architecture + CQRS в каждом сервисе, общие библиотеки BuildingBlocks\n" +
                        "- Мультитенантность с самого начала, продукт для монетизации\n" +
                        "- Модули: POS, склад, каталог, CRM, лояльность, аналитика",
                },
            ],
        },
        new Project
        {
            Slug = "ucms-construction-management",
            Stack = [".NET 8", "PostgreSQL", "JWT", "Docker", "Nginx", "Hetzner"],
            IsPublished = true,
            SortOrder = 2,
            Translations =
            [
                new ProjectTranslation
                {
                    LanguageCode = Languages.En,
                    Title = "UCMS — Construction Management",
                    Category = "Construction · Production",
                    Summary = ".NET 8 Clean Architecture with JWT and role-based access, taken to production on Hetzner with Docker, Nginx and SSL.",
                    Solution =
                        "- .NET 8, Clean Architecture, JWT and role-based access\n" +
                        "- Production deployment on a Hetzner VPS: Docker, Nginx, PostgreSQL and SSL",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Uz,
                    Title = "UCMS — Qurilishni boshqarish tizimi",
                    Category = "Qurilish · Production",
                    Summary = ".NET 8 Clean Architecture, JWT va rollarga asoslangan kirish; Hetzner'da Docker, Nginx va SSL bilan productionga chiqarilgan.",
                    Solution =
                        "- .NET 8, Clean Architecture, JWT va rollar bo'yicha kirish huquqlari\n" +
                        "- Hetzner VPS'ga production deploy: Docker, Nginx, PostgreSQL va SSL",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Ru,
                    Title = "UCMS — Управление строительством",
                    Category = "Строительство · Продакшен",
                    Summary = ".NET 8 на Clean Architecture с JWT и ролевым доступом; выведен в продакшен на Hetzner с Docker, Nginx и SSL.",
                    Solution =
                        "- .NET 8, Clean Architecture, JWT и ролевой доступ\n" +
                        "- Продакшен-деплой на Hetzner VPS: Docker, Nginx, PostgreSQL и SSL",
                },
            ],
        },
        new Project
        {
            Slug = "buildcost-uz",
            Stack = ["Hetzner", "Cloudflare"],
            LiveUrl = "https://buildcost.uz",
            IsPublished = true,
            SortOrder = 3,
            Translations =
            [
                new ProjectTranslation
                {
                    LanguageCode = Languages.En,
                    Title = "buildcost.uz",
                    Category = "Construction · Live",
                    Summary = "Construction cost platform hosted on Hetzner behind Cloudflare.", // TODO: what it solves
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Uz,
                    Title = "buildcost.uz",
                    Category = "Qurilish · Ishlab turibdi",
                    Summary = "Hetzner'da, Cloudflare ortida joylashgan qurilish narxlari platformasi.",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Ru,
                    Title = "buildcost.uz",
                    Category = "Строительство · Работает",
                    Summary = "Платформа стоимости строительства на Hetzner за Cloudflare.",
                },
            ],
        },
        new Project
        {
            Slug = "kindergarten-management",
            Stack = [".NET", "SignalR", "PostgreSQL", "Telegram Bot API", "Eskiz.uz"],
            IsPublished = true,
            SortOrder = 4,
            Translations =
            [
                new ProjectTranslation
                {
                    LanguageCode = Languages.En,
                    Title = "Kindergarten Management",
                    Category = "Education · Notifications",
                    Summary = "Attendance and payment tracking with Telegram and SMS notifications to parents and a live SignalR dashboard.",
                    Problem = "Kindergartens track attendance and payments by hand.",
                    Solution =
                        "- Attendance and payment tracking\n" +
                        "- Telegram and SMS (Eskiz.uz) notifications to parents\n" +
                        "- Real-time dashboard on SignalR",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Uz,
                    Title = "Bog'cha boshqaruv tizimi",
                    Category = "Ta'lim · Bildirishnomalar",
                    Summary = "Davomat va to'lovlarni hisobga olish, ota-onalarga Telegram va SMS orqali xabarnomalar hamda SignalR asosidagi jonli dashboard.",
                    Problem = "Bog'chalar davomat va to'lovlarni qo'lda yuritadi.",
                    Solution =
                        "- Davomat va to'lovlarni hisobga olish\n" +
                        "- Ota-onalarga Telegram va SMS (Eskiz.uz) xabarnomalari\n" +
                        "- SignalR asosidagi real vaqt dashboardi",
                },
                new ProjectTranslation
                {
                    LanguageCode = Languages.Ru,
                    Title = "Управление детским садом",
                    Category = "Образование · Уведомления",
                    Summary = "Учёт посещаемости и оплат, уведомления родителям в Telegram и по SMS и живой дашборд на SignalR.",
                    Problem = "Детские сады ведут учёт посещаемости и оплат вручную.",
                    Solution =
                        "- Учёт посещаемости и оплат\n" +
                        "- Уведомления родителям в Telegram и по SMS (Eskiz.uz)\n" +
                        "- Дашборд в реальном времени на SignalR",
                },
            ],
        },
    ];
}
