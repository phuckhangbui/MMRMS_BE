using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateContractAndContractPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                table: "ContractPayment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "ContractPayment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "ContractPayment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentPeriod",
                table: "Contract",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "ContractPayment");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "ContractPayment");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "ContractPayment");

            migrationBuilder.DropColumn(
                name: "RentPeriod",
                table: "Contract");
        }
    }
}
