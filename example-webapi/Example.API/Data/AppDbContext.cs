using Example.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User>      Users      { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id)           .HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(u => u.Username)     .HasColumnName("username").HasMaxLength(100).IsRequired();
            e.Property(u => u.PasswordHash) .HasColumnName("password_hash").IsRequired();
            e.Property(u => u.CreatedAt)    .HasColumnName("created_at");
            e.HasIndex(u => u.Username)     .IsUnique();
        });

        modelBuilder.Entity<UserToken>(e =>
        {
            e.ToTable("user_tokens");
            e.HasKey(t => t.Id);
            e.Property(t => t.Id)           .HasColumnName("id").ValueGeneratedOnAdd();
            e.Property(t => t.UserId)       .HasColumnName("user_id");
            e.Property(t => t.RefreshToken) .HasColumnName("refresh_token").IsRequired();
            e.Property(t => t.ExpiresAt)    .HasColumnName("expires_at");
            e.Property(t => t.IsRevoked)    .HasColumnName("is_revoked").HasDefaultValue(false);
            e.Property(t => t.CreatedAt)    .HasColumnName("created_at");
            e.HasIndex(t => t.RefreshToken) .IsUnique();

            e.HasOne(t => t.User)
             .WithMany()
             .HasForeignKey(t => t.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
