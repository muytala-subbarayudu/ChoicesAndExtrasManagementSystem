using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class PlotUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Plots",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plots_AppUserId",
                table: "Plots",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plots_AspNetUsers_AppUserId",
                table: "Plots",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plots_AspNetUsers_AppUserId",
                table: "Plots");

            migrationBuilder.DropIndex(
                name: "IX_Plots_AppUserId",
                table: "Plots");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Plots");
        }
    }
}
