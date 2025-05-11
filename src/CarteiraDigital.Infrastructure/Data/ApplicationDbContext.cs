using CarteiraDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace CarteiraDigital.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.CreatedOn).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();

                // Adicionando a propriedade FullName como campo calculado
                entity.Property(u => u.FullName)
                    .HasComputedColumnSql("\"FirstName\" || ' ' || \"LastName\"", stored: true);

                entity.HasOne(u => u.Wallet)
                    .WithOne(w => w.User)
                    .HasForeignKey<Wallet>(w => w.UserId);
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Balance).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(w => w.CreatedOn).IsRequired();

                entity.HasMany(w => w.Transactions)
                    .WithOne(t => t.Wallet)
                    .HasForeignKey(t => t.WalletId);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(t => t.Type).IsRequired();
                entity.Property(t => t.Status).IsRequired();
                entity.Property(t => t.CreatedOn).IsRequired();

                entity.HasOne(t => t.RelatedWallet)
                    .WithMany()
                    .HasForeignKey(t => t.RelatedWalletId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}