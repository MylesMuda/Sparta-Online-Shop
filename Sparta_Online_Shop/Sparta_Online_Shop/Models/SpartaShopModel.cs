namespace Sparta_Online_Shop
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Diagnostics;

    public partial class SpartaShopModel : DbContext
    {

#if DEBUG
        private static string SpUser = Environment.GetEnvironmentVariable("SpartaShopUsername");
        private static string SpPass = Environment.GetEnvironmentVariable("SpartaShopPassword");
#else
        private static string SpUser = Environment.GetEnvironmentVariable("APPSETTING_SpartaShopUsername");
        private static string SpPass = Environment.GetEnvironmentVariable("APPSETTING_SpartaShopPassword");
#endif

        private static string connectionString = $"data source=spartaonlineshop.database.windows.net;initial catalog = SpartaShop; user id = {SpUser}; password={SpPass};MultipleActiveResultSets=True;App=EntityFramework";
        
        public SpartaShopModel()
            : base(connectionString)
        {
        }

        public virtual DbSet<BasketItem> BasketItems { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<Creator> Creators { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderStatu> OrderStatus { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<PayPalTransaction> PayPalTransactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>()
                .HasMany(e => e.BasketItems)
                .WithRequired(e => e.Basket)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Creator>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ProductPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalCost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderStatu>()
                .Property(e => e.StatusDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.BasketItems)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Review>()
                .Property(e => e.ReviewText)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserType>()
                .Property(e => e.TypeDescription)
                .IsUnicode(false);

            modelBuilder.Entity<PayPalTransaction>()
                .Property(e => e.Amount)
                .HasPrecision(10, 2);

        }
    }
}
