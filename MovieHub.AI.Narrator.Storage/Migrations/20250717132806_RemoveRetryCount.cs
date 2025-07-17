using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.AI.Narrator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRetryCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetryCount",
                schema: "quartz",
                table: "failed_narrator_jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                schema: "quartz",
                table: "failed_narrator_jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
