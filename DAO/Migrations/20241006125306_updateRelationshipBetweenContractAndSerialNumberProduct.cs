using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationshipBetweenContractAndSerialNumberProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_SerialNumberProduct_ContractSerialNumberProductSerialNumber",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_ContractSerialNumberProductSerialNumber",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ContractSerialNumberProductSerialNumber",
                table: "Contract");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Contract",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_SerialNumber",
                table: "Contract",
                column: "SerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_SerialNumberProduct",
                table: "Contract",
                column: "SerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_SerialNumberProduct",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_SerialNumber",
                table: "Contract");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ContractSerialNumberProductSerialNumber",
                table: "Contract",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ContractSerialNumberProductSerialNumber",
                table: "Contract",
                column: "ContractSerialNumberProductSerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_SerialNumberProduct_ContractSerialNumberProductSerialNumber",
                table: "Contract",
                column: "ContractSerialNumberProductSerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber");
        }
    }
}
