using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class HomeToProjectUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plots_Projects_HomeId",
                table: "Plots");

            migrationBuilder.RenameColumn(
                name: "HomeId",
                table: "Plots",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Plots_HomeId",
                table: "Plots",
                newName: "IX_Plots_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plots_Projects_ProjectId",
                table: "Plots",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plots_Projects_ProjectId",
                table: "Plots");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Plots",
                newName: "HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Plots_ProjectId",
                table: "Plots",
                newName: "IX_Plots_HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plots_Projects_HomeId",
                table: "Plots",
                column: "HomeId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
