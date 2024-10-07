using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class removeContractSerialNumberProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractSerialNumberProduct");

            migrationBuilder.RenameColumn(
                name: "TotalRentPrice",
                table: "Contract",
                newName: "RentPrice");

            migrationBuilder.RenameColumn(
                name: "TotalDepositPrice",
                table: "Contract",
                newName: "DepositPrice");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumberProductSerialNumber",
                table: "Contract",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_SerialNumberProductSerialNumber",
                table: "Contract",
                column: "SerialNumberProductSerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_SerialNumberProduct_SerialNumberProductSerialNumber",
                table: "Contract",
                column: "SerialNumberProductSerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_SerialNumberProduct_SerialNumberProductSerialNumber",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_SerialNumberProductSerialNumber",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "SerialNumberProductSerialNumber",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "RentPrice",
                table: "Contract",
                newName: "TotalRentPrice");

            migrationBuilder.RenameColumn(
                name: "DepositPrice",
                table: "Contract",
                newName: "TotalDepositPrice");

            migrationBuilder.CreateTable(
                name: "ContractSerialNumberProduct",
                columns: table => new
                {
                    ContractSerialNumberProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DepositPrice = table.Column<double>(type: "float", nullable: true),
                    RentPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSerialNumberProduct", x => x.ContractSerialNumberProductId);
                    table.ForeignKey(
                        name: "FK_SerialMechanicalMachinery_ContractID",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK_SerialMechanicalMachinery_SerialNumber",
                        column: x => x.SerialNumber,
                        principalTable: "SerialNumberProduct",
                        principalColumn: "SerialNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractSerialNumberProduct_ContractId",
                table: "ContractSerialNumberProduct",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSerialNumberProduct_SerialNumber",
                table: "ContractSerialNumberProduct",
                column: "SerialNumber");
        }
    }
}
