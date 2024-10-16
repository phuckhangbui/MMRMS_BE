using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateInvoiceAndTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceTimeStamp",
                table: "Invoices",
                newName: "PayOsOrderId");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "DigitalTransactions",
                newName: "PayOsOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayOsOrderId",
                table: "Invoices",
                newName: "InvoiceTimeStamp");

            migrationBuilder.RenameColumn(
                name: "PayOsOrderId",
                table: "DigitalTransactions",
                newName: "Reference");
        }
    }
}
