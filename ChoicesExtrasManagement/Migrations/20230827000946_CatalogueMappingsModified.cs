using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class CatalogueMappingsModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_SubCatalogues_SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.DropIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.DropColumn(
                name: "SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "SubCatalogueId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_SubCatalogues_SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "SubCatalogueId",
                principalTable: "SubCatalogues",
                principalColumn: "Id");
        }
    }
}
