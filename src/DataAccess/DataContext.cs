using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<AuthorEntity> Authors { get; set; } = null!;
    public DbSet<BookEntity> Books { get; set; } = null!;
    public DbSet<BookItemEntity> BookItems { get; set; } = null!;
    public DbSet<PublisherEntity> Publishers { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<AdministratorEntity> Administrators { get; set; } = null!;
    public DbSet<CustomerEntity> Customers { get; set; } = null!;
    public DbSet<LibrarianEntity> Librarians { get; set; } = null!;
    public DbSet<AddressEntity> Addresses { get; set; } = null!;
    public DbSet<RoleEntity> Roles { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookEntity>(builder =>
        {
            builder
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity<BookAuthor>(
                ba => ba
                    .HasOne(ba => ba.AuthorEntity)
                    .WithMany()
                    .HasForeignKey(a => a.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict),
                ba => ba
                    .HasOne(ba => ba.BookEntity)
                    .WithMany()
                    .HasForeignKey(b => b.BookId))
                .ToTable("BookAuthors")
                .HasKey(ba => new { ba.AuthorId, ba.BookId });

            builder
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasMany(b => b.BookItems)
                .WithOne(b => b.Book)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasIndex(b => b.Isbn).IsUnique();
        });

        modelBuilder.Entity<BookItemEntity>(builder =>
        {
            builder
                .HasOne(bi => bi.Book)
                .WithMany(b => b.BookItems)
                .HasForeignKey(bi => bi.BookId);
            builder
                .HasIndex(bi => bi.Barcode).IsUnique();
        });

        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder
                .HasMany(u => u.Librarians)
                .WithOne(l => l.User);

            builder
                .HasMany(u => u.Customers)
                .WithOne(l => l.User);

            builder
                .HasMany(u => u.Administrators)
                .WithOne(l => l.User);

            builder
                .HasIndex(u => u.EmailAddress).IsUnique();
        });

        modelBuilder.Entity<CustomerEntity>(builder =>
        {
            builder
                .HasOne(c => c.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey<CustomerEntity>(c => c.AddressId);
        });

        modelBuilder.Entity<RoleEntity>(builder =>
        {
            builder
                .HasMany(r => r.Users)
                .WithOne(u => u.Role);
        });

        modelBuilder.Entity<AuthorEntity>()
            .HasIndex(a => a.Name).IsUnique();

        modelBuilder.Entity<PublisherEntity>()
            .HasIndex(p => p.Name).IsUnique();


    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        Save();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public void Save()
    {
       var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableEntity && 
            (e.State == EntityState.Added 
            || e.State == EntityState.Modified) 
            );

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
               ((AuditableEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
            }
            else
            {
                Entry((AuditableEntity)entry.Entity).Property(p => p.CreatedAt).IsModified = false;
            }
            ((AuditableEntity)entry.Entity).ModifiedAt = DateTime.UtcNow;
        }
    }
}