using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addLogToSerialNumberProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductComponentStatusLog");

            migrationBuilder.CreateTable(
                name: "SerialNumberProductLog",
                columns: table => new
                {
                    SerialNumberProductLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountTriggerId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumberProductLog", x => x.SerialNumberProductLogId);
                    table.ForeignKey(
                        name: "FK_SerialNumberProductLog_AccountID",
                        column: x => x.AccountTriggerId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_SerialNumberProduct_Log",
                        column: x => x.SerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumberProductLog_AccountTriggerId",
                table: "SerialNumberProductLog",
                column: "AccountTriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumberProductLog_SerialNumber",
                table: "SerialNumberProductLog",
                column: "SerialNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerialNumberProductLog");

            migrationBuilder.CreateTable(
                name: "ProductComponentStatusLog",
                columns: table => new
                {
                    ProductComponentStatusLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductComponentStatusId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComponentStatusLog", x => x.ProductComponentStatusLogId);
                    table.ForeignKey(
                        name: "FK_ProductComponentStatus_Log",
                        column: x => x.ProductComponentStatusId,
                        principalTable: "ProductComponentStatus",
                        principalColumn: "ProductComponentStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponentStatusLog_ProductComponentStatusId",
                table: "ProductComponentStatusLog",
                column: "ProductComponentStatusId");
        }
    }
}
