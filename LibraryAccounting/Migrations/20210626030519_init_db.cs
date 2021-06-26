using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryAccounting.Migrations
{
    public partial class init_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emloyees");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Auth",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Auth",
                newName: "password");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "Auth",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Auth",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "employeeId",
                table: "Auth",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Auth",
                table: "Auth",
                columns: new[] { "username", "password", "employeeId" });

            migrationBuilder.CreateTable(
                name: "BooksStatuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    statusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksStatuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstName = table.Column<string>(type: "text", nullable: false),
                    lastName = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<string>(type: "text", nullable: false),
                    workEmail = table.Column<string>(type: "text", nullable: false),
                    personalEmail = table.Column<string>(type: "text", nullable: true),
                    phoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    readerId = table.Column<Guid>(type: "uuid", nullable: false),
                    bookId = table.Column<Guid>(type: "uuid", nullable: false),
                    reservationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    returnDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    returningFlag = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => new { x.readerId, x.bookId, x.reservationDate, x.returnDate });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    descriprion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ISBN = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    author = table.Column<string>(type: "text", nullable: false),
                    publishedBy = table.Column<string>(type: "text", nullable: false),
                    publishedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    statusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.id);
                    table.UniqueConstraint("isbn", x => x.ISBN);
                    table.ForeignKey(
                        name: "FK_Books_BooksStatuses_statusId",
                        column: x => x.statusId,
                        principalTable: "BooksStatuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    employeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    roleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.employeeId, x.roleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Changes",
                columns: table => new
                {
                    changemakerId = table.Column<Guid>(type: "uuid", nullable: false),
                    bookId = table.Column<Guid>(type: "uuid", nullable: false),
                    changeDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Changes", x => new { x.changemakerId, x.bookId, x.changeDate });
                    table.ForeignKey(
                        name: "FK_Changes_Books_bookId",
                        column: x => x.bookId,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Changes_Employees_changemakerId",
                        column: x => x.changemakerId,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auth_employeeId",
                table: "Auth",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_statusId",
                table: "Books",
                column: "statusId");

            migrationBuilder.CreateIndex(
                name: "IX_Changes_bookId",
                table: "Changes",
                column: "bookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_roleId",
                table: "UserRoles",
                column: "roleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_Employees_employeeId",
                table: "Auth",
                column: "employeeId",
                principalTable: "Employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auth_Employees_employeeId",
                table: "Auth");

            migrationBuilder.DropTable(
                name: "Changes");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "BooksStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Auth",
                table: "Auth");

            migrationBuilder.DropIndex(
                name: "IX_Auth_employeeId",
                table: "Auth");

            migrationBuilder.DropColumn(
                name: "employeeId",
                table: "Auth");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Auth",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Auth",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Auth",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Auth",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Emloyees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    PersonalEmail = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true),
                    WorkEmail = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emloyees", x => x.Id);
                });
        }
    }
}
