using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessProject.Infrastracture.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChessPlayerId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ChessPlayerId",
                table: "Games",
                column: "ChessPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_ChessPlayerId",
                table: "Games",
                column: "ChessPlayerId",
                principalTable: "Players",
                principalColumn: "ChessPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_ChessPlayerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ChessPlayerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ChessPlayerId",
                table: "Games");
        }
    }
}
