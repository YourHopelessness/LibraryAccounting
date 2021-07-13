using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryAccounting.DAL.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                table: "reservations",
                name: "FK_reservations_books_book_id",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
            migrationBuilder.AddForeignKey(
                table: "reservations",
                name: "FK_reservations_employees_reader_id",
                column: "reader_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                 table: "reservations",
                 name: "FK_reservations_books_book_id"
            );
            migrationBuilder.DropForeignKey(
                 table: "reservations",
                 name: "FK_reservations_employees_reader_id"
            );
        }
    }
}
