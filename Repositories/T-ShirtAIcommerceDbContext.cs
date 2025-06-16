using BusinessObjects.Analytics;
using BusinessObjects.Cart;
using BusinessObjects.CustomDesigns;
using BusinessObjects.Entities.AI;
using BusinessObjects.Entities.Orders;
using BusinessObjects.Entities.Payments;
using BusinessObjects.Identity;
using BusinessObjects.Orders;
using BusinessObjects.Products;
using BusinessObjects.Reviews;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Comparisons;

namespace Repositories
{
    public class T_ShirtAIcommerceContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public T_ShirtAIcommerceContext(DbContextOptions<T_ShirtAIcommerceContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomDesign> CustomDesigns { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductComparison> ProductComparisons { get; set; }
        public DbSet<AiRecommendation> AiRecommendations { get; set; }
        public DbSet<DailyStat> DailyStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CustomDesign relationships - FIX cho lỗi multiple foreign key
            modelBuilder.Entity<CustomDesign>(entity =>
            {
                // User relationship (người tạo design)
                entity.HasOne(cd => cd.User)
                      .WithMany(u => u.CustomDesigns)
                      .HasForeignKey(cd => cd.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Staff relationship (nhân viên xử lý)
                entity.HasOne(cd => cd.Staff)
                      .WithMany(u => u.StaffDesigns)
                      .HasForeignKey(cd => cd.StaffId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Order relationships - FIX cho lỗi multiple foreign key
            modelBuilder.Entity<Order>(entity =>
            {
                // User relationship (người đặt hàng)
                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // AssignedStaff relationship (nhân viên được giao)
                entity.HasOne(o => o.AssignedStaff)
                      .WithMany(u => u.AssignedOrders)
                      .HasForeignKey(o => o.AssignedStaffId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Chỉ config những cái THỰC SỰ CẦN THIẾT

            // Decimal precision
            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Product>()
                .Property(e => e.SalePrice)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<CustomDesign>()
                .Property(e => e.TotalPrice)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalAmount)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(e => e.UnitPrice)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(e => e.TotalPrice)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<CartItem>()
                .Property(e => e.UnitPrice)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Payment>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<DailyStat>()
                .Property(e => e.TotalRevenue)
                .HasColumnType("decimal(12,2)");

            modelBuilder.Entity<Order>()
                .HasIndex(e => e.OrderNumber)
                .IsUnique();

            // Soft delete cho BaseEntity
            modelBuilder.Entity<Category>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CustomDesign>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Review>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Auto audit cho BaseEntity
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var currentUserId = Guid.NewGuid(); // TODO: Get actual current user ID from HttpContext

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = currentUserId;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = currentUserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = currentUserId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        entry.Entity.DeletedBy = currentUserId;
                        break;
                }
            }
        }
    }
}