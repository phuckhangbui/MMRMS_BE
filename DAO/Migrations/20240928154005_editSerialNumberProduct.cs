using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class editSerialNumberProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComponentStatus_SerialNumberProduct",
                table: "ProductComponentStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComponentStatus_SerialNumberProduct",
                table: "ProductComponentStatus",
                column: "SerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComponentStatus_SerialNumberProduct",
                table: "ProductComponentStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComponentStatus_SerialNumberProduct",
                table: "ProductComponentStatus",
                column: "SerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber");
        }
    }
}
