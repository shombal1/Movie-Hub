using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexUSerAndMovie_MovieBasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovieBaskets_UserId",
                table: "MovieBaskets");

            migrationBuilder.CreateIndex(
                name: "idx_user_and_movie",
                table: "MovieBaskets",
                columns: new[] { "UserId", "MovieId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_user_and_movie",
                table: "MovieBaskets");

            migrationBuilder.CreateIndex(
                name: "IX_MovieBaskets_UserId",
                table: "MovieBaskets",
                column: "UserId");
        }
    }
}
