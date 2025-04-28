using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using proj_tt.Authorization.Roles;
using proj_tt.Authorization.Users;
using proj_tt.MultiTenancy;
using Abp.EntityFrameworkCore;
using proj_tt.Persons;
using proj_tt.Tasks;
using proj_tt.Products;
using proj_tt.Categories;
using proj_tt.Banner;
using proj_tt.Cart;
using proj_tt.Order;


namespace proj_tt.EntityFrameworkCore
{
    public class proj_ttDbContext : AbpZeroDbContext<Tenant, Role, User, proj_ttDbContext>
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Banners> Banners { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public DbSet<Orders> Orders { get; set; }     
        public DbSet<OrderItem> OrderItems { get; set; }

        public proj_ttDbContext(DbContextOptions<proj_ttDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Carts>(b =>
            {
                b.ToTable("Carts");
                b.Property(x => x.UserId).IsRequired();
            });

            modelBuilder.Entity<CartItem>(b =>
            {
                b.ToTable("CartItems");
                b.Property(x => x.CartId).IsRequired();
                b.Property(x => x.ProductId).IsRequired();
                b.Property(x => x.Quantity).IsRequired();
                b.Property(x => x.Price).IsRequired();
            });

            modelBuilder.Entity<Orders>(b =>
            {
                b.ToTable("Orders");
                b.Property(x => x.UserId).IsRequired();
                b.Property(x => x.UserName).IsRequired().HasMaxLength(256);
                b.Property(x => x.UserEmail).IsRequired().HasMaxLength(256);
                b.Property(x => x.TotalAmount).IsRequired();
                b.Property(x => x.Status).IsRequired();
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.ToTable("OrderItems");
                b.Property(x => x.OrderId).IsRequired();
                b.Property(x => x.ProductId).IsRequired();
                b.Property(x => x.ProductName).IsRequired().HasMaxLength(256);
                b.Property(x => x.Quantity).IsRequired();
                b.Property(x => x.Price).IsRequired();
            });

            modelBuilder.Entity<Orders>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Orders)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
        }


