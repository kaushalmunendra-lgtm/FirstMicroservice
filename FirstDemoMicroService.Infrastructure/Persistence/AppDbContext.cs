using FirstDemoMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirstDemoMicroService.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();

        user.ToTable("users");
        user.HasKey(entity => entity.Id);
        user.Property(entity => entity.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        user.Property(entity => entity.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(150)
            .IsRequired();
        user.Property(entity => entity.Email)
            .HasColumnName("email")
            .HasMaxLength(254)
            .IsRequired();
        user.HasIndex(entity => entity.Email)
            .IsUnique()
            .HasDatabaseName("ux_users_email");
        user.Property(entity => entity.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}
