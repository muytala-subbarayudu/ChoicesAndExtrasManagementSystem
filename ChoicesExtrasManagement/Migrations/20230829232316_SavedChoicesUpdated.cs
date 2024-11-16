using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class SavedChoicesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedChoices_PlotTypeRoomMappings_PlotTypeRoomMappingId",
                table: "SavedChoices");

            migrationBuilder.DropIndex(
                name: "IX_SavedChoices_PlotTypeRoomMappingId",
                table: "SavedChoices");

            migrationBuilder.DropColumn(
                name: "PlotTypeRoomMappingId",
                table: "SavedChoices");

            migrationBuilder.CreateIndex(
                name: "IX_SavedChoices_PlotRoomId",
                table: "SavedChoices",
                column: "PlotRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedChoices_PlotTypeRoomMappings_PlotRoomId",
                table: "SavedChoices",
                column: "PlotRoomId",
                principalTable: "PlotTypeRoomMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedChoices_PlotTypeRoomMappings_PlotRoomId",
                table: "SavedChoices");

            migrationBuilder.DropIndex(
                name: "IX_SavedChoices_PlotRoomId",
                table: "SavedChoices");

            migrationBuilder.AddColumn<int>(
                name: "PlotTypeRoomMappingId",
                table: "SavedChoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SavedChoices_PlotTypeRoomMappingId",
                table: "SavedChoices",
                column: "PlotTypeRoomMappingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedChoices_PlotTypeRoomMappings_PlotTypeRoomMappingId",
                table: "SavedChoices",
                column: "PlotTypeRoomMappingId",
                principalTable: "PlotTypeRoomMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
