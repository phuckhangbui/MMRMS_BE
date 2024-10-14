using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateContractPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ContractPayment",
                newName: "Amount");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "ContractPayment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ContractPayment",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "ContractPayment");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ContractPayment");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "ContractPayment",
                newName: "Price");
        }
    }
}
