using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addContractDeliveryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTask_ContractID",
                table: "DeliveryTask");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryTask_ContractId",
                table: "DeliveryTask");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "DeliveryTask");

            migrationBuilder.CreateTable(
                name: "ContractDelivery",
                columns: table => new
                {
                    ContractDeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeliveryTaskId = table.Column<int>(type: "int", nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDelivery", x => x.ContractDeliveryId);
                    table.ForeignKey(
                        name: "FK_Contract_ContractDelivery",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_DeliveryTask_ContractDelivery",
                        column: x => x.DeliveryTaskId,
                        principalTable: "DeliveryTask",
                        principalColumn: "DeliveryTaskId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractDelivery_ContractId",
                table: "ContractDelivery",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDelivery_DeliveryTaskId",
                table: "ContractDelivery",
                column: "DeliveryTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractDelivery");

            migrationBuilder.AddColumn<string>(
                name: "ContractId",
                table: "DeliveryTask",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTask_ContractId",
                table: "DeliveryTask",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTask_ContractID",
                table: "DeliveryTask",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "ContractId");
        }
    }
}
