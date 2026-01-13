using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChineseRaffleApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDonorDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_Title",
                table: "Gifts",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_Title",
                table: "Gifts");
        }
    }
}
