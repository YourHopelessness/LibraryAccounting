using Microsoft.EntityFrameworkCore;
using System;
using LibraryAccounting.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DB
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        public DbSet<DbLogin> Logins { get; set; }
        public DbSet<Emloyees> Emloyees { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Changes> Changes { get; set; }
        public DbSet<BooksStatuses> BooksStatuses { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Books> Books {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbLogin>(login =>
            {
                login.HasKey(key => new { key.UserName, key.Password, key.EmployeeId });
                login.Property(e => e.EmployeeId)
                     .HasColumnName("employee_id");
                login.HasOne<Emloyees>()
                     .WithMany()
                     .HasForeignKey(e => e.EmployeeId);
                login.Property(name => name.UserName)
                     .HasColumnName("username");
                login.Property(pass => pass.Password)
                     .HasColumnName("password");
            });

            modelBuilder.Entity<Emloyees>(employee =>
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
                role.HasOne<Emloyees>()
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
                reservation.HasKey(key => new { key.ReaderId,
                                                key.BookId,
                                                key.ReservationDate,
                                                key.ReturnDate });
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
                change.HasOne<Emloyees>()
                      .WithMany()
                      .HasForeignKey(key => key.ChangemakerId);
                change.HasOne<Books>()
                      .WithMany()
                      .HasForeignKey(key => key.BookId);
            });
        }

    }
}
