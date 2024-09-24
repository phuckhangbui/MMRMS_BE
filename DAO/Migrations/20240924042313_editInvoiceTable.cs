using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class editInvoiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Invoices",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Method",
                table: "Invoices",
                newName: "AccountPaidId");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AccountPaidId",
                table: "Invoices",
                column: "AccountPaidId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Account",
                table: "Invoices",
                column: "AccountPaidId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Account",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_AccountPaidId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Invoices",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "AccountPaidId",
                table: "Invoices",
                newName: "Method");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Invoices",
                type: "bit",
                nullable: true);
        }
    }
}
