using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LiveChat.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrincipalId = table.Column<string>(nullable: true),
                    SecondaryId = table.Column<string>(nullable: true),
                    String = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Datetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    PrincipalId = table.Column<string>(nullable: false),
                    SecondaryId = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => new { x.PrincipalId, x.SecondaryId });
                    table.ForeignKey(
                        name: "FK_Contact_User_PrincipalId",
                        column: x => x.PrincipalId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contact_User_SecondaryId",
                        column: x => x.SecondaryId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Image", "Name", "Password", "Token" },
                values: new object[] { "89a32b87-e6da-4997-bf33-bafec658e446", "https://www.lifebonder.com/imagelink", "Alan Visnovezky", "0Fa1UhslO5lsMPL862LDVhcylDuBTrMFeIFGvt/HezI=", null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Image", "Name", "Password", "Token" },
                values: new object[] { "5bdc1f62-5129-401f-8725-01b323a3ab45", "https://www.lifebonder.com/imagelink", "Jesper Simonsen", "Ib89eldClKfWRyplrngFQsMbvTU97neXAnqwFqH58a8=", null });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "PrincipalId", "SecondaryId", "Message" },
                values: new object[] { "89a32b87-e6da-4997-bf33-bafec658e446", "5bdc1f62-5129-401f-8725-01b323a3ab45", null });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_SecondaryId",
                table: "Contact",
                column: "SecondaryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
