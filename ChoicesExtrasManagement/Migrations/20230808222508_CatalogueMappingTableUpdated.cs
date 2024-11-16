using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class CatalogueMappingTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubCatalogueRoomMappings");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogueRoomMappings");

            migrationBuilder.CreateTable(
                name: "SubCatalogueRoomMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    SubCatalogueId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCatalogueRoomMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCatalogueRoomMappings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubCatalogueRoomMappings_SubCatalogues_SubCatalogueId",
                        column: x => x.SubCatalogueId,
                        principalTable: "SubCatalogues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomMappings_RoomId",
                table: "SubCatalogueRoomMappings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCatalogueRoomMappings_SubCatalogueId",
                table: "SubCatalogueRoomMappings",
                column: "SubCatalogueId");
        }
    }
}
