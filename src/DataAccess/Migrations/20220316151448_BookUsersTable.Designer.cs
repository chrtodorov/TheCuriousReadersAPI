﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220316151448_BookUsersTable")]
    partial class BookUsersTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BookEntityUserEntity", b =>
                {
                    b.Property<Guid>("BooksBookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BooksBookId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("BookEntityUserEntity");
                });

            modelBuilder.Entity("DataAccess.Entities.AddressEntity", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(1028)
                        .HasColumnType("nvarchar(1028)");

                    b.Property<string>("ApartmentNumber")
                        .HasMaxLength(65)
                        .HasColumnType("nvarchar(65)");

                    b.Property<string>("BuildingNumber")
                        .HasMaxLength(65)
                        .HasColumnType("nvarchar(65)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DataAccess.Entities.AdministratorEntity", b =>
                {
                    b.Property<Guid>("AdministartorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdministartorId");

                    b.HasIndex("UserId");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("DataAccess.Entities.AuthorEntity", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("AuthorId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DataAccess.Entities.BlobMetadata", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BlobName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BlobsMetadata");
                });

            modelBuilder.Entity("DataAccess.Entities.BookAuthor", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AuthorId", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("BookAuthors", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.BookEntity", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BlobMetadataId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CoverUrl")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasMaxLength(17)
                        .HasColumnType("nvarchar(17)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PublisherId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BookId");

                    b.HasIndex("BlobMetadataId")
                        .IsUnique()
                        .HasFilter("[BlobMetadataId] IS NOT NULL");

                    b.HasIndex("Isbn")
                        .IsUnique();

                    b.HasIndex("PublisherId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("DataAccess.Entities.BookItemEntity", b =>
                {
                    b.Property<Guid>("BookItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid?>("BookId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookLoanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BookStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BookItemId");

                    b.HasIndex("Barcode")
                        .IsUnique();

                    b.HasIndex("BookId");

                    b.HasIndex("BookLoanId")
                        .IsUnique()
                        .HasFilter("[BookLoanId] IS NOT NULL");

                    b.ToTable("BookItems");
                });

            modelBuilder.Entity("DataAccess.Entities.BookLoanEntity", b =>
                {
                    b.Property<Guid>("BookLoanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LoanedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LoanedTo")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TimesExtended")
                        .HasColumnType("int");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("BookLoanId");

                    b.HasIndex("LoanedBy");

                    b.HasIndex("LoanedTo");

                    b.ToTable("BookLoans");
                });

            modelBuilder.Entity("DataAccess.Entities.BookRequestEntity", b =>
                {
                    b.Property<Guid>("BookRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AuditedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RequestedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookRequestId");

                    b.HasIndex("AuditedBy");

                    b.HasIndex("BookItemId");

                    b.HasIndex("RequestedBy");

                    b.ToTable("BookRequests");
                });

            modelBuilder.Entity("DataAccess.Entities.CommentEntity", b =>
                {
                    b.Property<Guid>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CommentId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DataAccess.Entities.CustomerEntity", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CustomerId");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DataAccess.Entities.LibrarianEntity", b =>
                {
                    b.Property<Guid>("LibrarianId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LibrarianId");

                    b.HasIndex("UserId");

                    b.ToTable("Librarians");
                });

            modelBuilder.Entity("DataAccess.Entities.PublisherEntity", b =>
                {
                    b.Property<Guid>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PublisherId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("DataAccess.Entities.RoleEntity", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(65)
                        .HasColumnType("nvarchar(65)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BookEntityUserEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.BookEntity", null)
                        .WithMany()
                        .HasForeignKey("BooksBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccess.Entities.AdministratorEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.UserEntity", "User")
                        .WithMany("Administrators")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.BookAuthor", b =>
                {
                    b.HasOne("DataAccess.Entities.AuthorEntity", "AuthorEntity")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.BookEntity", "BookEntity")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuthorEntity");

                    b.Navigation("BookEntity");
                });

            modelBuilder.Entity("DataAccess.Entities.BookEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.BlobMetadata", "BlobMetadata")
                        .WithOne("BookEntity")
                        .HasForeignKey("DataAccess.Entities.BookEntity", "BlobMetadataId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DataAccess.Entities.PublisherEntity", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BlobMetadata");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("DataAccess.Entities.BookItemEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.BookEntity", "Book")
                        .WithMany("BookItems")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.BookLoanEntity", "BookLoan")
                        .WithOne("BookItem")
                        .HasForeignKey("DataAccess.Entities.BookItemEntity", "BookLoanId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Book");

                    b.Navigation("BookLoan");
                });

            modelBuilder.Entity("DataAccess.Entities.BookLoanEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.LibrarianEntity", "Librarian")
                        .WithMany("BookLoans")
                        .HasForeignKey("LoanedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataAccess.Entities.CustomerEntity", "Customer")
                        .WithMany("BookLoans")
                        .HasForeignKey("LoanedTo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Librarian");
                });

            modelBuilder.Entity("DataAccess.Entities.BookRequestEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.LibrarianEntity", "Librarian")
                        .WithMany("BookRequests")
                        .HasForeignKey("AuditedBy");

                    b.HasOne("DataAccess.Entities.BookItemEntity", "BookItem")
                        .WithMany("BookRequests")
                        .HasForeignKey("BookItemId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.CustomerEntity", "Customer")
                        .WithMany("BookRequests")
                        .HasForeignKey("RequestedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookItem");

                    b.Navigation("Customer");

                    b.Navigation("Librarian");
                });

            modelBuilder.Entity("DataAccess.Entities.CommentEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.BookEntity", "Book")
                        .WithMany("Comments")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.UserEntity", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.CustomerEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.AddressEntity", "Address")
                        .WithOne("Customer")
                        .HasForeignKey("DataAccess.Entities.CustomerEntity", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.UserEntity", "User")
                        .WithMany("Customers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.LibrarianEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.UserEntity", "User")
                        .WithMany("Librarians")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.UserEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.RoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DataAccess.Entities.AddressEntity", b =>
                {
                    b.Navigation("Customer")
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccess.Entities.BlobMetadata", b =>
                {
                    b.Navigation("BookEntity");
                });

            modelBuilder.Entity("DataAccess.Entities.BookEntity", b =>
                {
                    b.Navigation("BookItems");

                    b.Navigation("Comments");
                });

            modelBuilder.Entity("DataAccess.Entities.BookItemEntity", b =>
                {
                    b.Navigation("BookRequests");
                });

            modelBuilder.Entity("DataAccess.Entities.BookLoanEntity", b =>
                {
                    b.Navigation("BookItem")
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccess.Entities.CustomerEntity", b =>
                {
                    b.Navigation("BookLoans");

                    b.Navigation("BookRequests");
                });

            modelBuilder.Entity("DataAccess.Entities.LibrarianEntity", b =>
                {
                    b.Navigation("BookLoans");

                    b.Navigation("BookRequests");
                });

            modelBuilder.Entity("DataAccess.Entities.PublisherEntity", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("DataAccess.Entities.RoleEntity", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("DataAccess.Entities.UserEntity", b =>
                {
                    b.Navigation("Administrators");

                    b.Navigation("Comments");

                    b.Navigation("Customers");

                    b.Navigation("Librarians");
                });
#pragma warning restore 612, 618
        }
    }
}
