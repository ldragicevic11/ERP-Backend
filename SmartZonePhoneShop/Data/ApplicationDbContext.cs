using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Data
{
    public class ApplicationDbContext : DbContext  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            //modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            //modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();   

            modelBuilder.Entity<OrderItem>()
                .ToTable(tb => tb.HasTrigger("TR_DecreaseOnStock"));

            modelBuilder.Entity<Order>()
                .ToTable(tb => tb.HasTrigger("trg_NewOrder"));
        }
    }
}
