using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryAccounting.DAL.Migrations
{
    /// <summary>
    /// миграция
    /// </summary>
    public partial class NewMigration : Migration
    {
        /// <summary>
        /// применение миграции
        /// </summary>
        /// <param name="migrationBuilder"></param>
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
        /// <summary>
        /// откат миграции
        /// </summary>
        /// <param name="migrationBuilder"></param>
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
