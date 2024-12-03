using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class updateAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBusinessId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "LogId",
                table: "Account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountBusinessId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LogId",
                table: "Account",
                type: "int",
                nullable: true);
        }
    }
}
