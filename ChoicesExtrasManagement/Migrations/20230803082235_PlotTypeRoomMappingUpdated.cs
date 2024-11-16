using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class PlotTypeRoomMappingUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotTypeRoomMappings_Plots_PlotId",
                table: "PlotTypeRoomMappings");

            migrationBuilder.RenameColumn(
                name: "PlotId",
                table: "PlotTypeRoomMappings",
                newName: "PlotTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotTypeRoomMappings_PlotId",
                table: "PlotTypeRoomMappings",
                newName: "IX_PlotTypeRoomMappings_PlotTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTypeRoomMappings_PlotTypes_PlotTypeId",
                table: "PlotTypeRoomMappings",
                column: "PlotTypeId",
                principalTable: "PlotTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotTypeRoomMappings_PlotTypes_PlotTypeId",
                table: "PlotTypeRoomMappings");

            migrationBuilder.RenameColumn(
                name: "PlotTypeId",
                table: "PlotTypeRoomMappings",
                newName: "PlotId");

            migrationBuilder.RenameIndex(
                name: "IX_PlotTypeRoomMappings_PlotTypeId",
                table: "PlotTypeRoomMappings",
                newName: "IX_PlotTypeRoomMappings_PlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTypeRoomMappings_Plots_PlotId",
                table: "PlotTypeRoomMappings",
                column: "PlotId",
                principalTable: "Plots",
                principalColumn: "Id");
        }
    }
}
