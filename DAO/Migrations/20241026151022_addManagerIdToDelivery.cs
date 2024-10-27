using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addManagerIdToDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "DeliveryTask",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTask_ManagerId",
                table: "DeliveryTask",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTask_ManagerID",
                table: "DeliveryTask",
                column: "ManagerId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTask_ManagerID",
                table: "DeliveryTask");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryTask_ManagerId",
                table: "DeliveryTask");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "DeliveryTask");
        }
    }
}
