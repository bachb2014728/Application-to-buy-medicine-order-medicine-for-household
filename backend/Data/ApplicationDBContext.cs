using System.Globalization;
using backend.Model;
using CsvHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext(DbContextOptions dbContextOptions) : IdentityDbContext<AppUser>(dbContextOptions)
    {
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contraindication> Contraindications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Receiver> Receivers { get; set; }
        public DbSet<DosageForm> DosageForms { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Use> Uses { get; set; }
        
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<CustomerVouchers> CustomerVouchers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            LoadDataUse(builder);
            LoadDataContraindication(builder);
            LoadDataCategories(builder);
            LoadDataManufacturer(builder);
            LoadDataDosageForms(builder);
            
            List<IdentityRole> roles =
            [
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            ];
            builder.Entity<IdentityRole>().HasData(roles);
            var hasher = new PasswordHasher<AppUser>();
            var adminUser = new AppUser
            {
                UserName = "admin123",
                Email = "admin@gmail.com",
                NormalizedUserName = "ADMIN123",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin123");
            builder.Entity<AppUser>().HasData(adminUser);
            
            var adminUserRole = new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = roles[0].Id };
            builder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);
            var customerOfAdmin = new Customer
            {
                Id = 1,
                FirstName = "Lê",
                LastName = "Admin",
                Address = "3/2, Xuân Khánh, Ninh Kiều, Cần Thơ",
                UserId = adminUser.Id
            };
            builder.Entity<Customer>().HasData(customerOfAdmin);
        }
        
        private static void LoadDataUse(ModelBuilder builder)
        {
            using var reader = new StreamReader("../backend/CSV/Uses.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Use>();
            foreach (var r in records)
            {
                var newItem = new Use()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Info = r.Info
                };
                builder.Entity<Use>().HasData(newItem);
            }
        }
        private static void LoadDataContraindication(ModelBuilder builder)
        {
            using var reader = new StreamReader("../backend/CSV/Contraindications.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Contraindication>();
            foreach (var r in records)
            {
                var newItem = new Contraindication()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Info = r.Info
                };
                builder.Entity<Contraindication>().HasData(newItem);
            }
        }
        private static void LoadDataCategories(ModelBuilder builder)
        {
            using var reader = new StreamReader("../backend/CSV/Categories.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Category>();
            foreach (var r in records)
            {
                var newItem = new Category()
                {
                    Id = r.Id,
                    Name = r.Name,
                    URL = r.URL,
                    CategoryParent = r.CategoryParent,
                    IsEnable = r.IsEnable
                };
                builder.Entity<Category>().HasData(newItem);
            }
        }
        private static void LoadDataDosageForms(ModelBuilder builder)
        {
            using var reader = new StreamReader("../backend/CSV/DosageForms.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<DosageForm>();
            foreach (var r in records)
            {
                var newItem = new DosageForm()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Info = r.Info
                };
                builder.Entity<DosageForm>().HasData(newItem);
            }
        }
        private static void LoadDataManufacturer(ModelBuilder builder)
        {
            using var reader = new StreamReader("../backend/CSV/Manufacturers.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Manufacturer>();
            foreach (var r in records)
            {
                var newItem = new Manufacturer()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Info = r.Info
                };
                builder.Entity<Manufacturer>().HasData(newItem);
            }
        }
    }
}