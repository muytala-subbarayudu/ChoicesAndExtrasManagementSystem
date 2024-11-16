using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class CatalogueRoomPlotTypeMappingNameUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_catalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "catalogueRoomPlotTypeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_catalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                table: "catalogueRoomPlotTypeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_catalogueRoomPlotTypeMappings_Rooms_RoomId",
                table: "catalogueRoomPlotTypeMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_catalogueRoomPlotTypeMappings",
                table: "catalogueRoomPlotTypeMappings");

            migrationBuilder.RenameTable(
                name: "catalogueRoomPlotTypeMappings",
                newName: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.RenameIndex(
                name: "IX_catalogueRoomPlotTypeMappings_RoomId",
                table: "CatalogueRoomPlotTypeMappings",
                newName: "IX_CatalogueRoomPlotTypeMappings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_catalogueRoomPlotTypeMappings_PlotTypeId",
                table: "CatalogueRoomPlotTypeMappings",
                newName: "IX_CatalogueRoomPlotTypeMappings_PlotTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_catalogueRoomPlotTypeMappings_CatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                newName: "IX_CatalogueRoomPlotTypeMappings_CatalogueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogueRoomPlotTypeMappings",
                table: "CatalogueRoomPlotTypeMappings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "CatalogueId",
                principalTable: "Catalogues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "PlotTypeId",
                principalTable: "PlotTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_Rooms_RoomId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_Rooms_RoomId",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogueRoomPlotTypeMappings",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.RenameTable(
                name: "CatalogueRoomPlotTypeMappings",
                newName: "catalogueRoomPlotTypeMappings");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_RoomId",
                table: "catalogueRoomPlotTypeMappings",
                newName: "IX_catalogueRoomPlotTypeMappings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_PlotTypeId",
                table: "catalogueRoomPlotTypeMappings",
                newName: "IX_catalogueRoomPlotTypeMappings_PlotTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_CatalogueId",
                table: "catalogueRoomPlotTypeMappings",
                newName: "IX_catalogueRoomPlotTypeMappings_CatalogueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_catalogueRoomPlotTypeMappings",
                table: "catalogueRoomPlotTypeMappings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_catalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "catalogueRoomPlotTypeMappings",
                column: "CatalogueId",
                principalTable: "Catalogues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_catalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                table: "catalogueRoomPlotTypeMappings",
                column: "PlotTypeId",
                principalTable: "PlotTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_catalogueRoomPlotTypeMappings_Rooms_RoomId",
                table: "catalogueRoomPlotTypeMappings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
