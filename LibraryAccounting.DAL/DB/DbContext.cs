using Microsoft.EntityFrameworkCore;
using System;
using LibraryAccounting.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.DB
{
    /// <summary>
    /// класс, хранящий все взаимосвязи в базе
    /// </summary>
    public class BaseLibraryContext : DbContext
    {
        /// <summary> Конструктор создания контекста с помощью внедрения зависимости</summary>
        /// <param name="options">Параметр для внедрения зависимости</param>
        public BaseLibraryContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        /// <summary>Таблица явок-паролей</summary>
        public DbSet<DbLogin> Logins { get; set; }

        /// <summary>Справочниик сотрудников </summary>
        public DbSet<Employees> Employees { get; set; }

        /// <summary>Таблица бронирования</summary>
        public DbSet<Reservations> Reservations { get; set; }

        /// <summary>Таблица изменений</summary>
        public DbSet<Changes> Changes { get; set; }

        /// <summary> Справочник статусов книг </summary>
        public DbSet<BooksStatuses> BooksStatuses { get; set; }

        /// <summary>Справочник ролей</summary>
        public DbSet<Roles> Roles { get; set; }

        /// <summary>Таблица ролей сотрудников </summary>
        public DbSet<UserRoles> UserRoles { get; set; }

        /// <summary>Таблица книг</summary>
        public DbSet<Books> Books { get; set; }

        /// <inheritdoc></inheritdoc>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbLogin>(login =>
            {
                login.HasKey(key => new { key.UserName, key.EmployeeId })
                     .HasName("pk_auth");
                login.Property(e => e.EmployeeId)
                     .HasColumnName("employee_id");
                login.HasOne<Employees>()
                     .WithMany()
                     .HasForeignKey(e => e.EmployeeId);
                login.Property(name => name.UserName)
                     .HasColumnName("username");
                login.Property(pass => pass.Password)
                     .HasColumnName("password");
                login.Property(e => e.Salt)
                     .HasColumnName("salt");
            });

            modelBuilder.Entity<Employees>(employee =>
            {
                employee.HasKey(key => key.Id);
                employee.Property(key => key.Id)
                        .HasColumnName("id");
                employee.Property(e => e.FirstName)
                        .HasColumnName("first_name");
                employee.Property(e => e.LastName)
                        .HasColumnName("last_name");
                employee.Property(e => e.Position)
                        .HasColumnName("position");
                employee.Property(e => e.WorkEmail)
                        .HasColumnName("work_email");
                employee.Property(e => e.PersonalEmail)
                        .HasColumnName("personal_email");
                employee.Property(e => e.PhoneNumber)
                        .HasColumnName("phone_number");
            });

            modelBuilder.Entity<Roles>(role =>
            {
                role.HasKey(key => key.Id);
                role.Property(key => key.Id)
                    .HasColumnName("id");
                role.Property(r => r.Name)
                    .HasColumnName("name");
                role.Property(r => r.Description)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<UserRoles>(role =>
            {
                role.HasKey(key => key.EmployeeId);
                role.Property(ur => ur.RoleId)
                    .HasColumnName("role_id");
                role.Property(ur => ur.EmployeeId)
                    .HasColumnName("employee_id");
                role.HasOne<Employees>()
                    .WithMany()
                    .HasForeignKey(key => key.EmployeeId);
                role.HasOne<Roles>()
                    .WithMany()
                    .HasForeignKey(key => key.RoleId);
            });

            modelBuilder.Entity<Books>(book =>
            {
                book.HasKey(key => key.Id);
                book.Property(key => key.Id)
                    .HasColumnName("id");
                book.HasAlternateKey(b => b.ISBN);
                book.Property(key => key.ISBN)
                    .HasColumnName("isbn");
                book.Property(r => r.Title)
                    .HasColumnName("title");
                book.Property(r => r.Author)
                    .HasColumnName("author");
                book.Property(r => r.PublishedBy)
                    .HasColumnName("published_by");
                book.Property(r => r.PublishedDate)
                    .HasColumnName("published_date");
                book.Property(r => r.StatusId)
                    .HasColumnName("status_id");
                book.HasOne<BooksStatuses>()
                    .WithMany()
                    .HasForeignKey(key => key.StatusId);
            });

            modelBuilder.Entity<BooksStatuses>(status =>
            {
                status.HasKey(key => key.Id);
                status.Property(key => key.Id)
                      .HasColumnName("id");
                status.Property(s => s.Status)
                      .HasColumnName("status_name");
            });

            modelBuilder.Entity<Reservations>(reservation =>
            {
                reservation.HasKey(key => new {
                    key.ReaderId,
                    key.BookId,
                    key.ReservationDate,
                    key.ReturnDate
                });
                reservation.Property(r => r.ReaderId)
                           .HasColumnName("reader_id");
                reservation.Property(r => r.BookId)
                           .HasColumnName("book_id");
                reservation.Property(r => r.ReservationDate)
                           .HasColumnName("reservation_date");
                reservation.Property(r => r.ReturnDate)
                           .HasColumnName("return_date");
                reservation.Property(r => r.ReturningFlag)
                           .HasColumnName("returning_flag")
                           .HasDefaultValue(false);
                reservation.HasOne<Books>()
                           .WithMany()
                           .HasForeignKey(r => r.BookId);
                reservation.HasOne<Employees>()
                           .WithMany()
                           .HasForeignKey(e => e.ReaderId);
            });

            modelBuilder.Entity<Changes>(change =>
            {
                change.HasKey(key => new {
                    key.ChangemakerId,
                    key.BookId,
                    key.ChangeDate
                });
                change.Property(r => r.ChangemakerId)
                           .HasColumnName("changemaker_id");
                change.Property(r => r.BookId)
                           .HasColumnName("book_id");
                change.Property(r => r.ChangeDate)
                           .HasColumnName("change_date");
                change.Property(r => r.Comment)
                           .HasColumnName("comment");
                change.HasOne<Employees>()
                      .WithMany()
                      .HasForeignKey(key => key.ChangemakerId);
                change.HasOne<Books>()
                      .WithMany()
                      .HasForeignKey(key => key.BookId);
            });
        }
    }

    /// <summary> Главный контекст базы данных</summary>
    public class LibraryDbContext : BaseLibraryContext
    {
        /// <inheritdoc></inheritdoc>
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        /// <inheritdoc></inheritdoc>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <inheritdoc></inheritdoc>>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(DbConfig.GetConnectionString());
        }
    }
}
