using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GammingManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileNo",
                table: "AspNetUsers",
                newName: "UpdatedById");

            migrationBuilder.RenameColumn(
                name: "EmailId",
                table: "AspNetUsers",
                newName: "CreatedById");

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampaignImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinTarget = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "AspNetUsers",
                newName: "MobileNo");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "AspNetUsers",
                newName: "EmailId");
        }
    }
}
