using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationshipBetweenContractPaymentAndInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_ContractPayment",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ContractPaymentId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ContractPaymentId",
                table: "Invoices");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "ContractPayment",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractPayment_InvoiceId",
                table: "ContractPayment",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_ContractPayment",
                table: "ContractPayment",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_ContractPayment",
                table: "ContractPayment");

            migrationBuilder.DropIndex(
                name: "IX_ContractPayment_InvoiceId",
                table: "ContractPayment");

            migrationBuilder.AddColumn<int>(
                name: "ContractPaymentId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "ContractPayment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ContractPaymentId",
                table: "Invoices",
                column: "ContractPaymentId",
                unique: true,
                filter: "[ContractPaymentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_ContractPayment",
                table: "Invoices",
                column: "ContractPaymentId",
                principalTable: "ContractPayment",
                principalColumn: "ContractPaymentId");
        }
    }
}
