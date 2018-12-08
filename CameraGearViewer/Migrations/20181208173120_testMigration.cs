using Microsoft.EntityFrameworkCore.Migrations;

namespace CameraGearViewer.Migrations
{
    public partial class testMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GearComponents",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ForumLink = table.Column<string>(nullable: false),
                    Date = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GearComponents", x => x.ForumLink);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GearComponents");
        }
    }
}
