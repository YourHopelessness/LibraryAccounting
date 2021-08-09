using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LibraryAccounting.DAL.Migrations
{
    /// <summary>
    /// миграция
    /// </summary>
    public partial class DbMigration : Migration
    {
        /// <summary>
        /// примененеие миграции
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<string>(type: "text", nullable: false),
                    work_email = table.Column<string>(type: "text", nullable: false),
                    personal_email = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    reader_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reservation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    return_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    returning_flag = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => new { x.reader_id, x.book_id, x.reservation_date, x.return_date });
                    table.ForeignKey(
                        name: "FK_reservations_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_employees_reader_id",
                        column: x => x.reader_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    isbn = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    author = table.Column<string>(type: "text", nullable: false),
                    published_by = table.Column<string>(type: "text", nullable: false),
                    published_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                    table.UniqueConstraint("AK_books_isbn", x => x.isbn);
                    table.ForeignKey(
                        name: "FK_books_books_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "books_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auth",
                columns: table => new
                {
                    username = table.Column<string>(type: "text", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    salt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth", x => new { x.username, x.password, x.employee_id });
                    table.ForeignKey(
                        name: "FK_auth_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_user_roles_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "changes",
                columns: table => new
                {
                    changemaker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    change_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_changes", x => new { x.changemaker_id, x.book_id, x.change_date });
                    table.ForeignKey(
                        name: "FK_changes_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_changes_employees_changemaker_id",
                        column: x => x.changemaker_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_auth_employee_id",
                table: "auth",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_books_status_id",
                table: "books",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_changes_book_id",
                table: "changes",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");
        }
        /// <summary>
        /// откат миграции
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth");

            migrationBuilder.DropTable(
                name: "changes");

            migrationBuilder.DropTable(
                name: "reservations");
            
            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "books_statuses");
        }
    }
}
