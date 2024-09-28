using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalRentPrice",
                table: "Contract",
                newName: "TotalRentPricePerMonth");

            migrationBuilder.AddColumn<double>(
                name: "DiscountPrice",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountShip",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ShippingPrice",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalDepositPrice",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalRentPricePerMonth",
                table: "RentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountCreateId",
                table: "Contract",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountShip",
                table: "Contract",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RentingService",
                columns: table => new
                {
                    RentingServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    PayType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOptional = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentingService", x => x.RentingServiceId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceContract",
                columns: table => new
                {
                    ServiceContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingServiceId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServicePrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    FinalPrice = table.Column<double>(type: "float", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ServiceRentingRequest",
                columns: table => new
                {
                    ServiceRentingRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentingServiceId = table.Column<int>(type: "int", nullable: true),
                    RentingRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServicePrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    FinalPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRentingRequest", x => x.ServiceRentingRequestId);
                    table.ForeignKey(
                        name: "FK_rentingservice_servicerequest",
                        column: x => x.RentingServiceId,
                        principalTable: "RentingService",
                        principalColumn: "RentingServiceId");
                    table.ForeignKey(
                        name: "FK_servicerequest_rentingrequest",
                        column: x => x.RentingRequestId,
                        principalTable: "RentingRequest",
                        principalColumn: "RentingRequestId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_AccountCreateId",
                table: "Contract",
                column: "AccountCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContract_ContractId",
                table: "ServiceContract",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContract_RentingServiceId",
                table: "ServiceContract",
                column: "RentingServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRentingRequest_RentingRequestId",
                table: "ServiceRentingRequest",
                column: "RentingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRentingRequest_RentingServiceId",
                table: "ServiceRentingRequest",
                column: "RentingServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Account_Create",
                table: "Contract",
                column: "AccountCreateId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Account_Create",
                table: "Contract");

            migrationBuilder.DropTable(
                name: "ServiceContract");

            migrationBuilder.DropTable(
                name: "ServiceRentingRequest");

            migrationBuilder.DropTable(
                name: "RentingService");

            migrationBuilder.DropIndex(
                name: "IX_Contract_AccountCreateId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "DiscountShip",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "ShippingPrice",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "TotalDepositPrice",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "TotalRentPricePerMonth",
                table: "RentingRequest");

            migrationBuilder.DropColumn(
                name: "AccountCreateId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DiscountShip",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "TotalRentPricePerMonth",
                table: "Contract",
                newName: "TotalRentPrice");
        }
    }
}
