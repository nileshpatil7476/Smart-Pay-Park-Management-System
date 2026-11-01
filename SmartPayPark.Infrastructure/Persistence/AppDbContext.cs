using Microsoft.EntityFrameworkCore;
using SmartPayPark.Domain.Entities;
using SmartPayPark.Domain.Entities.Enums;
using SmartPayPark.Domain.Entities.User;
using System.Collections.Generic;

namespace SmartPayPark.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Operator> Operators => Set<Operator>();

    public DbSet<OperatorUser> OperatorUsers => Set<OperatorUser>();

    public DbSet<OperatorDocument> OperatorDocuments
        => Set<OperatorDocument>();


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


        // Operator configuration
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.ToTable("Operators");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.BusinessName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.ContactName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.ContactEmail)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(x => x.ContactPhone)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.BusinessAddress)
                .HasMaxLength(500);

            entity.Property(x => x.VerificationStatus)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.RejectionReason)
                .HasMaxLength(500);

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });

        // OperatorUser configuration
        modelBuilder.Entity<OperatorUser>(entity =>
        {
            entity.ToTable("OperatorUsers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.OperatorRole)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired();

            entity.HasIndex(x => new { x.OperatorId, x.UserId })
                .IsUnique();

            entity.HasOne(x => x.Operator)
                .WithMany(x => x.OperatorUsers)
                .HasForeignKey(x => x.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.User)
                .WithMany(x => x.OperatorMemberships)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.InvitedByUser)
                .WithMany(x => x.InvitationsSent)
                .HasForeignKey(x => x.InvitedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OperatorDocument configuration
        modelBuilder.Entity<OperatorDocument>(entity =>
        {
            entity.ToTable("OperatorDocuments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.DocumentType)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.StorageKey)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.VerificationStatus)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.RejectionReason)
                .HasMaxLength(500)
                .IsRequired(false);

            entity.HasOne(x => x.Operator)
                .WithMany(x => x.OperatorDocuments)
                .HasForeignKey(x => x.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.OperatorId);
        });
    }
}
