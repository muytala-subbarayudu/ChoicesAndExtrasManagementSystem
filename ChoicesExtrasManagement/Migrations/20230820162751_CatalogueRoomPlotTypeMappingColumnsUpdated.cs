using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class CatalogueRoomPlotTypeMappingColumnsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "CatalogueId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "CatalogueId",
                principalTable: "Catalogues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.DropIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.DropColumn(
                name: "CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings");
        }
    }
}
