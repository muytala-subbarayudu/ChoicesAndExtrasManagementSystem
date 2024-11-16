using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoicesExtrasManagement.Migrations
{
    /// <inheritdoc />
    public partial class SavedExtraAndTransactionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedExtras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlotId = table.Column<int>(type: "int", nullable: false),
                    PlotRoomId = table.Column<int>(type: "int", nullable: false),
                    CatalogueId = table.Column<int>(type: "int", nullable: false),
                    SubCatalogueId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    isSubmitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedExtras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedExtras_Catalogues_CatalogueId",
                        column: x => x.CatalogueId,
                        principalTable: "Catalogues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedExtras_PaymentTransactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "PaymentTransactions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SavedExtras_PlotTypeRoomMappings_PlotRoomId",
                        column: x => x.PlotRoomId,
                        principalTable: "PlotTypeRoomMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedExtras_Plots_PlotId",
                        column: x => x.PlotId,
                        principalTable: "Plots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedExtras_SubCatalogues_SubCatalogueId",
                        column: x => x.SubCatalogueId,
                        principalTable: "SubCatalogues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedExtras_CatalogueId",
                table: "SavedExtras",
                column: "CatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedExtras_PlotId",
                table: "SavedExtras",
                column: "PlotId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedExtras_PlotRoomId",
                table: "SavedExtras",
                column: "PlotRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedExtras_SubCatalogueId",
                table: "SavedExtras",
                column: "SubCatalogueId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedExtras_TransactionId",
                table: "SavedExtras",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedExtras");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");
        }
    }
}
