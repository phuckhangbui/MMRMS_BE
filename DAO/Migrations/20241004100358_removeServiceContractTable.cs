using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class removeServiceContractTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceContract");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceContract",
                columns: table => new
                {
                    ServiceContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RentingServiceId = table.Column<int>(type: "int", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    FinalPrice = table.Column<double>(type: "float", nullable: true),
                    ServicePrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceContract", x => x.ServiceContractId);
                    table.ForeignKey(
                        name: "FK_rentingservice_servicecontract",
                        column: x => x.RentingServiceId,
                        principalTable: "RentingService",
                        principalColumn: "RentingServiceId");
                    table.ForeignKey(
                        name: "FK_servicecontract_contract",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContract_ContractId",
                table: "ServiceContract",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContract_RentingServiceId",
                table: "ServiceContract",
                column: "RentingServiceId");
        }
    }
}
