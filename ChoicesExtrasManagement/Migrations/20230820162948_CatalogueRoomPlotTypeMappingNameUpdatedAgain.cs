using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class CatalogueRoomPlotTypeMappingNameUpdatedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                table: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_Rooms_RoomId",
                table: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_SubCatalogues_SubCatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCatalogueRoomPlotTypeMappings",
                table: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.RenameTable(
                name: "SubCatalogueRoomPlotTypeMappings",
                newName: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.RenameIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                newName: "IX_CatalogueRoomPlotTypeMappings_SubCatalogueId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_RoomId",
                table: "CatalogueRoomPlotTypeMappings",
                newName: "IX_CatalogueRoomPlotTypeMappings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_PlotTypeId",
                table: "CatalogueRoomPlotTypeMappings",
                newName: "IX_CatalogueRoomPlotTypeMappings_PlotTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCatalogueRoomPlotTypeMappings_CatalogueId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_SubCatalogues_SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings",
                column: "SubCatalogueId",
                principalTable: "SubCatalogues",
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

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogueRoomPlotTypeMappings_SubCatalogues_SubCatalogueId",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogueRoomPlotTypeMappings",
                table: "CatalogueRoomPlotTypeMappings");

            migrationBuilder.RenameTable(
                name: "CatalogueRoomPlotTypeMappings",
                newName: "SubCatalogueRoomPlotTypeMappings");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_SubCatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                newName: "IX_SubCatalogueRoomPlotTypeMappings_SubCatalogueId");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_RoomId",
                table: "SubCatalogueRoomPlotTypeMappings",
                newName: "IX_SubCatalogueRoomPlotTypeMappings_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_PlotTypeId",
                table: "SubCatalogueRoomPlotTypeMappings",
                newName: "IX_SubCatalogueRoomPlotTypeMappings_PlotTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogueRoomPlotTypeMappings_CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                newName: "IX_SubCatalogueRoomPlotTypeMappings_CatalogueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCatalogueRoomPlotTypeMappings",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "CatalogueId",
                principalTable: "Catalogues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "PlotTypeId",
                principalTable: "PlotTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_Rooms_RoomId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCatalogueRoomPlotTypeMappings_SubCatalogues_SubCatalogueId",
                table: "SubCatalogueRoomPlotTypeMappings",
                column: "SubCatalogueId",
                principalTable: "SubCatalogues",
                principalColumn: "Id");
        }
    }
}
