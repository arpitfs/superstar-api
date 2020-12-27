using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiWorld.Data.Migrations
{
    public partial class TableCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuperStars",
                columns: table => new
                {
                    SuperstarId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Height = table.Column<string>(nullable: true),
                    Weight = table.Column<string>(nullable: true),
                    Smack = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperStars", x => x.SuperstarId);
                    table.ForeignKey(
                        name: "FK_SuperStars_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuperStars_UserId",
                table: "SuperStars",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuperStars");
        }
    }
}
