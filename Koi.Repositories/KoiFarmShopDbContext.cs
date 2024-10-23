using Koi.BusinessObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Koi.Repositories
{
    public class KoiFarmShopDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<KoiFish> KoiFishs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<KoiBreed> KoiBreeds { get; set; }

        //public DbSet<KoiFishKoiBreed> KoiFishKoiBreeds { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<OrderDetailFeedback> OrderDetailFeedbacks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ConsignmentForSale> ConsignmentForSales { get; set; }
        public DbSet<ConsignmentForNurture> ConsignmentForNurtures { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<KoiCertificate> KoiCertificates { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }  // Add DbSet for WalletTransaction
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<KoiFishImage> KoiFishImages { get; set; }
        public DbSet<KoiDiary> KoiDiaries { get; set; }
        public DbSet<Diet> Diets { get; set; }
        public DbSet<RequestForSale> RequestForSales { get; set; }
        public DbSet<WithdrawnRequest> WithdrawnRequests { get; set; }

        public KoiFarmShopDbContext(DbContextOptions<KoiFarmShopDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Configure many-to-many relationship
            //modelBuilder.Entity<KoiFishKoiBreed>()
            //    .HasKey(kfkb => new { kfkb.KoiFishId, kfkb.KoiBreedId });

            //modelBuilder.Entity<KoiFishKoiBreed>()
            //    .HasOne(kfkb => kfkb.KoiFish)
            //    .WithMany(kf => kf.KoiFishKoiBreeds)
            //    .HasForeignKey(kfkb => kfkb.KoiFishId);

            //modelBuilder.Entity<KoiFishKoiBreed>()
            //    .HasOne(kfkb => kfkb.KoiBreed)
            //    .WithMany(kb => kb.KoiFishKoiBreeds)
            //    .HasForeignKey(kfkb => kfkb.KoiBreedId);

            modelBuilder.Entity<OrderDetail>()
        .HasOne(od => od.Order)
        .WithMany(o => o.OrderDetails)
        .HasForeignKey(od => od.OrderId)
        .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Order and Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Transactions)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between ConsignmentForNurture and Staff (User)
            modelBuilder.Entity<ConsignmentForNurture>()
                .HasOne(c => c.Staff)
                .WithMany()
                .HasForeignKey(c => c.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConsignmentForNurture>()
          .HasOne(c => c.Customer)
          .WithMany()
          .HasForeignKey(c => c.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between ConsignmentForSale and Staff (User)
            modelBuilder.Entity<ConsignmentForSale>()
                .HasOne(c => c.Staff)
                .WithMany()
                .HasForeignKey(c => c.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between ConsignmentForSale and KoiFish
            modelBuilder.Entity<ConsignmentForSale>()
                .HasOne(c => c.KoiFish)
                .WithMany()
                .HasForeignKey(c => c.KoiFishId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure 1-1 relationship between User and Wallet
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId);
        }
    }
}