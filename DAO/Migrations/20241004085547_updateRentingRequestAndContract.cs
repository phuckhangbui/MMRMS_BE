using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateRentingRequestAndContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "ServiceRentingRequest");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "ServiceRentingRequest");

            migrationBuilder.DropColumn(
                name: "PayType",
                table: "RentingService");

            migrationBuilder.RenameColumn(
                name: "TotalRentPricePerMonth",
                table: "RentingRequest",
                newName: "TotalRentPrice");

            migrationBuilder.RenameColumn(
                name: "DiscountPrice",
                table: "ContractSerialNumberProduct",
                newName: "RentPrice");

            migrationBuilder.RenameColumn(
                name: "TotalRentPricePerMonth",
                table: "Contract",
                newName: "TotalRentPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalRentPrice",
                table: "RentingRequest",
                newName: "TotalRentPricePerMonth");

            migrationBuilder.RenameColumn(
                name: "RentPrice",
                table: "ContractSerialNumberProduct",
                newName: "DiscountPrice");

            migrationBuilder.RenameColumn(
                name: "TotalRentPrice",
                table: "Contract",
                newName: "TotalRentPricePerMonth");

            migrationBuilder.AddColumn<double>(
                name: "DiscountPrice",
                table: "ServiceRentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FinalPrice",
                table: "ServiceRentingRequest",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayType",
                table: "RentingService",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
