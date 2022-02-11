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
    [Migration("20220211092204_AddIndexesToAuthorAndPublishers")]
    partial class AddIndexesToAuthorAndPublishers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AuthorEntityBookEntity", b =>
                {
                    b.Property<Guid>("AuthorsAuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BooksBookId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AuthorsAuthorId", "BooksBookId");

                    b.HasIndex("BooksBookId");

                    b.ToTable("AuthorEntityBookEntity");
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

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("AuthorId");

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique();

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DataAccess.Entities.BookEntity", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CoverUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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

                    b.Property<Guid>("PublisherId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BookId");

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

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BookStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BookItemId");

                    b.HasIndex("Barcode")
                        .IsUnique();

                    b.HasIndex("BookId");

                    b.ToTable("BookItems");
                });

            modelBuilder.Entity("DataAccess.Entities.PublisherEntity", b =>
                {
                    b.Property<Guid>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PublisherId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("AuthorEntityBookEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.AuthorEntity", null)
                        .WithMany()
                        .HasForeignKey("AuthorsAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.BookEntity", null)
                        .WithMany()
                        .HasForeignKey("BooksBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccess.Entities.BookEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.PublisherEntity", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("DataAccess.Entities.BookItemEntity", b =>
                {
                    b.HasOne("DataAccess.Entities.BookEntity", "Book")
                        .WithMany("BookItems")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("DataAccess.Entities.BookEntity", b =>
                {
                    b.Navigation("BookItems");
                });

            modelBuilder.Entity("DataAccess.Entities.PublisherEntity", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
