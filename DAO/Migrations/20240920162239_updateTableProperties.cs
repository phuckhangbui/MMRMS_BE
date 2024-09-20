using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateTableProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintainingTicket_SerialNumberProduct_SerialNumberProductSerialNumber",
                table: "MaintainingTicket");

            migrationBuilder.DropIndex(
                name: "IX_MaintainingTicket_SerialNumberProductSerialNumber",
                table: "MaintainingTicket");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "MaintainingTicket");

            migrationBuilder.DropColumn(
                name: "SerialNumberProductSerialNumber",
                table: "MaintainingTicket");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Product",
                newName: "RentPrice");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Contract",
                newName: "TotalRentPrice");

            migrationBuilder.RenameColumn(
                name: "FinalPrice",
                table: "Contract",
                newName: "TotalDepositPrice");

            migrationBuilder.AddColumn<int>(
                name: "RentTimeCounter",
                table: "SerialNumberProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductPrice",
                table: "Product",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductSerialNumber",
                table: "MaintainingTicket",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "HiringRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnetimePayment",
                table: "HiringRequest",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "HiringRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMonth",
                table: "HiringRequest",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DepositPrice",
                table: "ContractSerialNumberProduct",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountPrice",
                table: "ContractSerialNumberProduct",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Contract",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FinalAmount",
                table: "Contract",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingTicket_ProductSerialNumber",
                table: "MaintainingTicket",
                column: "ProductSerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintainingTicket_SerialNumberProduct",
                table: "MaintainingTicket",
                column: "ProductSerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintainingTicket_SerialNumberProduct",
                table: "MaintainingTicket");

            migrationBuilder.DropIndex(
                name: "IX_MaintainingTicket_ProductSerialNumber",
                table: "MaintainingTicket");

            migrationBuilder.DropColumn(
                name: "RentTimeCounter",
                table: "SerialNumberProduct");

            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "HiringRequest");

            migrationBuilder.DropColumn(
                name: "IsOnetimePayment",
                table: "HiringRequest");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "HiringRequest");

            migrationBuilder.DropColumn(
                name: "NumberOfMonth",
                table: "HiringRequest");

            migrationBuilder.DropColumn(
                name: "DepositPrice",
                table: "ContractSerialNumberProduct");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "ContractSerialNumberProduct");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "RentPrice",
                table: "Product",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "TotalRentPrice",
                table: "Contract",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "TotalDepositPrice",
                table: "Contract",
                newName: "FinalPrice");

            migrationBuilder.AlterColumn<string>(
                name: "ProductSerialNumber",
                table: "MaintainingTicket",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "MaintainingTicket",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumberProductSerialNumber",
                table: "MaintainingTicket",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Method",
                table: "Contract",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintainingTicket_SerialNumberProductSerialNumber",
                table: "MaintainingTicket",
                column: "SerialNumberProductSerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintainingTicket_SerialNumberProduct_SerialNumberProductSerialNumber",
                table: "MaintainingTicket",
                column: "SerialNumberProductSerialNumber",
                principalTable: "SerialNumberProduct",
                principalColumn: "SerialNumber");
        }
    }
}
