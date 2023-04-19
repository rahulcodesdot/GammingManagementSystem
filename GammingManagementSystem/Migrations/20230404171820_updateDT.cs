using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GammingManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class updateDT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ReferralUsers",
                newName: "RefUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefUserId",
                table: "ReferralUsers",
                newName: "UserId");
        }
    }
}
