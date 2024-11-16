using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class CatalogueRoomPlotTypeMappingUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogueRoomMappings");

            migrationBuilder.CreateTable(
                name: "catalogueRoomPlotTypeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogueId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    PlotTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogueRoomPlotTypeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_catalogueRoomPlotTypeMappings_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_catalogueRoomPlotTypeMappings_PlotTypes_PlotTypeId",
                        column: x => x.PlotTypeId,
                        principalTable: "PlotTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_catalogueRoomPlotTypeMappings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_catalogueRoomPlotTypeMappings_CatalogueId",
                table: "catalogueRoomPlotTypeMappings",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_catalogueRoomPlotTypeMappings_PlotTypeId",
                table: "catalogueRoomPlotTypeMappings",
                column: "PlotTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_catalogueRoomPlotTypeMappings_RoomId",
                table: "catalogueRoomPlotTypeMappings",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalogueRoomPlotTypeMappings");

            migrationBuilder.CreateTable(
                name: "CatalogueRoomMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogueId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogueRoomMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogueRoomMappings_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogueRoomMappings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueRoomMappings_CatalogueId",
                table: "CatalogueRoomMappings",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogueRoomMappings_RoomId",
                table: "CatalogueRoomMappings",
                column: "RoomId");
        }
    }
}
