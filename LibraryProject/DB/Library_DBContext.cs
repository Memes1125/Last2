using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LibraryProject.DB
{
    public partial class Library_DBContext : DbContext
    {
        public Library_DBContext()
        {
        }

        public Library_DBContext(DbContextOptions<Library_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookAuthorCross> BookAuthorCrosses { get; set; } = null!;
        public virtual DbSet<BookGenreCross> BookGenreCrosses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Publisher> Publishers { get; set; } = null!;
        public virtual DbSet<Shelf> Shelves { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=192.168.1.14\\SQLEXPRESS;Initial Catalog=lib; user=student; password=student") ;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Author");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(true)
                    .HasColumnName("FIrstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(true);

                entity.Property(e => e.Patronimyc)
                    .HasMaxLength(50)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BookName).IsUnicode(true);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.YearPublished).HasColumnType("datetime");

                entity.HasOne(d => d.Departament)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.DepartamentId)
                    .HasConstraintName("FK_Book_Department");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_Book_Publisher");

                entity.HasOne(d => d.Shelf)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.ShelfId)
                    .HasConstraintName("FK_Book_Shelf");
            });

            modelBuilder.Entity<BookAuthorCross>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.AuthorId })
                    .HasName("PK_BookAuthorCross_1");

                entity.ToTable("BookAuthorCross");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(true);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthorCrosses)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookAuthorCross_Author");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthorCrosses)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookAuthorCross_Book");
            });

            modelBuilder.Entity<BookGenreCross>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.GenreId });

                entity.ToTable("BookGenreCross");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookGenreCrosses)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookGenreCross_Book");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.BookGenreCrosses)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookGenreCross_Genre");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DepartmentName).IsUnicode(true);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GenreName).IsUnicode(true);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("Publisher");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PublisherLocation).IsUnicode(true);

                entity.Property(e => e.PublisherName).IsUnicode(true);
            });

            modelBuilder.Entity<Shelf>(entity =>
            {
                entity.ToTable("Shelf");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ShelfNumber).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
