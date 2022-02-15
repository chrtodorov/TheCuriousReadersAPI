using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base (options)
    { }

    public DbSet<AuthorEntity> Authors { get; set; } = null!;
    public DbSet<BookEntity> Books { get; set; } = null!;
    public DbSet<BookItemEntity> BookItems { get; set; } = null!;
    public DbSet<PublisherEntity> Publishers { get; set; } = null!;
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
                .WithMany(a => a.Books);
            builder
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId);
            builder
                .HasMany(b => b.BookItems)
                .WithOne(b => b.Book);
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

        modelBuilder.Entity<CustomerEntity>(builder =>
        {
            builder
                .HasOne(c => c.Address)
                .WithOne(a => a.Customer)
                .HasForeignKey<CustomerEntity>(c => c.AddressId);
            builder
                .HasOne(c => c.Role)
                .WithMany(r => r.Customers);
        });

        modelBuilder.Entity<LibrarianEntity>(builder =>
        {
            builder
                .HasOne(c => c.Role)
                .WithMany(r => r.Librarians);
        });

        modelBuilder.Entity<AuthorEntity>()
            .HasIndex(a => a.Name).IsUnique();

        modelBuilder.Entity<PublisherEntity>()
            .HasIndex(p => p.Name).IsUnique();

    }
}