﻿// <auto-generated />
using System;
using LibraryAccounting.DAL.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LibraryAccounting.DAL.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20210712014323_NewMigration")]
    partial class NewMigration
    {
        /// <summary>
        /// правила примененеия миграции
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Books", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("author");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isbn");

                    b.Property<string>("PublishedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("published_by");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("published_date");

                    b.Property<int>("StatusId")
                        .HasColumnType("integer")
                        .HasColumnName("status_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasAlternateKey("ISBN");

                    b.HasIndex("StatusId");

                    b.ToTable("books");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.BooksStatuses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status_name");

                    b.HasKey("Id");

                    b.ToTable("books_statuses");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Changes", b =>
                {
                    b.Property<Guid>("ChangemakerId")
                        .HasColumnType("uuid")
                        .HasColumnName("changemaker_id");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid")
                        .HasColumnName("book_id");

                    b.Property<DateTime?>("ChangeDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("change_date");

                    b.Property<string>("Comment")
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.HasKey("ChangemakerId", "BookId", "ChangeDate");

                    b.HasIndex("BookId");

                    b.ToTable("changes");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.DbLogin", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("employee_id");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("salt");

                    b.HasKey("UserName", "EmployeeId")
                        .HasName("pk_auth");

                    b.HasIndex("EmployeeId");

                    b.ToTable("auth");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Employees", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("PersonalEmail")
                        .HasColumnType("text")
                        .HasColumnName("personal_email");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("position");

                    b.Property<string>("WorkEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("work_email");

                    b.HasKey("Id");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Reservations", b =>
                {
                    b.Property<Guid>("ReaderId")
                        .HasColumnType("uuid")
                        .HasColumnName("reader_id");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid")
                        .HasColumnName("book_id");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("reservation_date");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("return_date");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<bool?>("ReturningFlag")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("returning_flag");

                    b.HasKey("ReaderId", "BookId", "ReservationDate", "ReturnDate");

                    b.HasIndex("BookId");

                    b.ToTable("reservations");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.UserRoles", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("employee_id");

                    b.Property<int?>("RoleId")
                        .IsRequired()
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.HasKey("EmployeeId");

                    b.HasIndex("RoleId");

                    b.ToTable("user_roles");
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Books", b =>
                {
                    b.HasOne("LibraryAccounting.DAL.Entities.BooksStatuses", null)
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Changes", b =>
                {
                    b.HasOne("LibraryAccounting.DAL.Entities.Books", null)
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryAccounting.DAL.Entities.Employees", null)
                        .WithMany()
                        .HasForeignKey("ChangemakerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.DbLogin", b =>
                {
                    b.HasOne("LibraryAccounting.DAL.Entities.Employees", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.Reservations", b =>
                {
                    b.HasOne("LibraryAccounting.DAL.Entities.Books", null)
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryAccounting.DAL.Entities.Employees", null)
                        .WithMany()
                        .HasForeignKey("ReaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LibraryAccounting.DAL.Entities.UserRoles", b =>
                {
                    b.HasOne("LibraryAccounting.DAL.Entities.Employees", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryAccounting.DAL.Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
