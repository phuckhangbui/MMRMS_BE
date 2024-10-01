using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class addDeliveryLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "TaskLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCompleted",
                table: "EmployeeTask",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "EmployeeTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCompleted",
                table: "Delivery",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Delivery",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryLog",
                columns: table => new
                {
                    DeliveryLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryLog", x => x.DeliveryLogId);
                    table.ForeignKey(
                        name: "FK_DeliveryLog_AccountID",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_DeliveryLog_DeliveryID",
                        column: x => x.DeliveryId,
                        principalTable: "Delivery",
                        principalColumn: "DeliveryId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLog_AccountId",
                table: "TaskLog",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLog_AccountId",
                table: "DeliveryLog",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLog_DeliveryId",
                table: "DeliveryLog",
                column: "DeliveryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLog_AccountID",
                table: "TaskLog",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLog_AccountID",
                table: "TaskLog");

            migrationBuilder.DropTable(
                name: "DeliveryLog");

            migrationBuilder.DropIndex(
                name: "IX_TaskLog_AccountId",
                table: "TaskLog");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "TaskLog");

            migrationBuilder.DropColumn(
                name: "DateCompleted",
                table: "EmployeeTask");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "EmployeeTask");

            migrationBuilder.DropColumn(
                name: "DateCompleted",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Delivery");
        }
    }
}
