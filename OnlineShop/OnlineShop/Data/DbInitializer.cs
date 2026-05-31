using System.Security.Cryptography;
using System.Text;
using OnlineShop.Models;

namespace OnlineShop.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Categories.Any()) return;

        var categories = new Category[]
        {
            new() { Name = "Ноутбуки" },
            new() { Name = "Смартфони" },
            new() { Name = "Навушники" },
            new() { Name = "Аксесуари" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        var products = new Product[]
        {
            new()
            {
                Name = "MacBook Air M2",
                ShortDescription = "Легкий та потужний ноутбук Apple",
                FullDescription = "Apple MacBook Air з чіпом M2 забезпечує неймовірну продуктивність у надтонкому корпусі. 13.6-дюймовий дисплей Liquid Retina, до 18 годин роботи від батареї, 8 ГБ оперативної пам'яті та 256 ГБ SSD.",
                Price = 42999.00m,
                ImagePath = "https://raw.githubusercontent.com/Sherenok12/photo_for_project/main/image.jpg",

                CategoryId = categories[0].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Lenovo ThinkPad X1 Carbon",
                ShortDescription = "Бізнес-ноутбук преміум класу",
                FullDescription = "Lenovo ThinkPad X1 Carbon Gen 11 — ультралегкий бізнес-ноутбук з 14-дюймовим дисплеєм 2.8K OLED, процесором Intel Core i7-1365U, 16 ГБ RAM та 512 ГБ SSD. Ідеальний для професіоналів.",
                Price = 55999.00m,
                ImagePath = "https://picsum.photos/seed/thinkpad/400/300",
                CategoryId = categories[0].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "ASUS ROG Strix G16",
                ShortDescription = "Ігровий ноутбук з RTX 4060",
                FullDescription = "ASUS ROG Strix G16 — потужний ігровий ноутбук з процесором Intel Core i7-13650HX, відеокартою NVIDIA RTX 4060, 16 ГБ DDR5 RAM, 512 ГБ SSD та 16-дюймовим дисплеєм 165 Гц.",
                Price = 47500.00m,
                ImagePath = "https://picsum.photos/seed/rog/400/300",
                CategoryId = categories[0].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "iPhone 15 Pro",
                ShortDescription = "Флагманський смартфон Apple",
                FullDescription = "iPhone 15 Pro з титановим корпусом, чіпом A17 Pro, камерою 48 Мп, USB-C та Action Button. 6.1-дюймовий Super Retina XDR дисплей з ProMotion 120 Гц.",
                Price = 49999.00m,
                ImagePath = "https://picsum.photos/seed/iphone15/400/300",
                CategoryId = categories[1].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Samsung Galaxy S24 Ultra",
                ShortDescription = "Флагман Samsung зі стилусом S Pen",
                FullDescription = "Samsung Galaxy S24 Ultra з процесором Snapdragon 8 Gen 3, камерою 200 Мп, 6.8-дюймовим QHD+ дисплеєм, 12 ГБ RAM, 256 ГБ пам'яті та вбудованим стилусом S Pen.",
                Price = 47999.00m,
                ImagePath = "https://picsum.photos/seed/galaxy24/400/300",
                CategoryId = categories[1].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Google Pixel 8 Pro",
                ShortDescription = "Смартфон Google з найкращою камерою",
                FullDescription = "Google Pixel 8 Pro з чіпом Tensor G3, потрійною камерою 50 Мп, 6.7-дюймовим LTPO OLED дисплеєм 120 Гц, 12 ГБ RAM та 128 ГБ пам'яті. 7 років оновлень Android.",
                Price = 34999.00m,
                ImagePath = "https://picsum.photos/seed/pixel8/400/300",
                CategoryId = categories[1].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Sony WH-1000XM5",
                ShortDescription = "Бездротові навушники з шумозаглушенням",
                FullDescription = "Sony WH-1000XM5 — преміальні бездротові навушники з найкращим шумозаглушенням, 30 годин роботи, підтримкою LDAC та 360 Reality Audio. Легкі та зручні для тривалого носіння.",
                Price = 12999.00m,
                ImagePath = "https://picsum.photos/seed/sonywh/400/300",
                CategoryId = categories[2].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "AirPods Pro 2",
                ShortDescription = "Бездротові навушники Apple з USB-C",
                FullDescription = "AirPods Pro 2 з чіпом H2, адаптивним шумозаглушенням, персоналізованим просторовим аудіо та до 6 годин прослуховування. USB-C кейс з точним пошуком.",
                Price = 9999.00m,
                ImagePath = "https://picsum.photos/seed/airpods/400/300",
                CategoryId = categories[2].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Чохол для iPhone 15 Pro",
                ShortDescription = "Силіконовий чохол MagSafe",
                FullDescription = "Оригінальний силіконовий чохол Apple для iPhone 15 Pro з підтримкою MagSafe. Приємний на дотик матеріал, захист від подряпин та падінь.",
                Price = 1999.00m,
                ImagePath = "https://picsum.photos/seed/case/400/300",
                CategoryId = categories[3].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Зарядка MagSafe 15W",
                ShortDescription = "Бездротова зарядка Apple MagSafe",
                FullDescription = "Бездротова зарядка Apple MagSafe потужністю 15 Вт. Магнітне кріплення для ідеального позиціонування. Сумісна з iPhone 12 та новіше.",
                Price = 1799.00m,
                ImagePath = "https://picsum.photos/seed/magsafe/400/300",
                CategoryId = categories[3].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "USB-C хаб 7-в-1",
                ShortDescription = "Багатопортовий адаптер USB-C",
                FullDescription = "USB-C хаб 7-в-1: HDMI 4K, 2x USB 3.0, USB-C PD 100W, SD/microSD картрідер, Ethernet. Алюмінієвий корпус, компактний дизайн.",
                Price = 1299.00m,
                ImagePath = "https://picsum.photos/seed/usbhub/400/300",
                CategoryId = categories[3].Id,
                CreatedAt = DateTime.UtcNow
            }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        // Create admin user (password: Admin123)
        var adminHash = HashPassword("Admin123");
        var admin = new User
        {
            UserName = "admin",
            Email = "admin@shop.com",
            PasswordHash = adminHash,
            IsAdmin = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(admin);
        context.SaveChanges();
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
