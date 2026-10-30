using Microsoft.EntityFrameworkCore;
using SmartPayPark.Api.Models;
using SmartPayPark.Api.Models.Enums;

namespace SmartPayPark.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Operator> Operators => Set<Operator>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.FullName)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(u => u.PhoneNumber)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(u => u.UserName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .HasMaxLength(500)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");

                entity.HasKey(u => u.Id);

                entity.Property(r => r.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(r => r.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");

                entity.HasKey(x => new { x.UserId, x.RoleId });

                entity.HasOne(x => x.User)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.UserId);

                entity.HasOne(x => x.Role)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleId);
            });


            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Customer" },
                new Role { Id = 2, Name = "OperatorOwner" },
                new Role { Id = 3, Name = "OperatorManager" },
                new Role { Id = 4, Name = "ParkingAttendant" },
                new Role { Id = 5, Name = "PlatformAdmin" }
            );


            modelBuilder.Entity<Operator>(entity =>
            {
                entity.ToTable("Operators");

                //public Guid Id { get; set; } = Guid.NewGuid();
                //public Guid UserId { get; set; }
                //public string BusinessName { get; set; } = string.Empty;
                //public string BusinessEmail { get; set; } = string.Empty;
                //public string BusinessPhone { get; set; } = string.Empty;
                //public string? GSTIN { get; set; }
                //public string Address { get; set; } = string.Empty;
                //public OperatorStatus Status { get; set; } = OperatorStatus.Pending;
                //public DateTime ReviewedAtUtc { get; set; } = DateTime.UtcNow;
                //public string? RejectionReason { get; set; }
                //public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
                //public DateTime UpdatedAtUtc { get; set; }
                //public User User { get; set; } = null!;

                entity.HasKey(o => o.Id);

                entity.Property(o => o.BusinessName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(o => o.BusinessEmail)
                    .HasMaxLength(254)
                    .IsRequired();

                entity.Property(o => o.BusinessPhone)
                    .HasMaxLength(10);

                entity.Property(o => o.BusinessPhone)
                    .HasMaxLength(10);

                entity.Property(o => o.GSTIN)
                    .HasMaxLength(15);

                entity.Property(o => o.Address)
                    .HasMaxLength(500);

                entity.Property(o => o.Status)
                    .HasConversion<int>();

                entity.HasIndex(o => o.UserId)
                    .IsUnique();

                entity.HasOne(o => o.User)
                    .WithOne()
                    .HasForeignKey<Operator>(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
