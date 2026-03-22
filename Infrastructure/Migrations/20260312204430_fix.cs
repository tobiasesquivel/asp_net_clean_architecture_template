using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publication_AppUsers_UserId",
                table: "Publication");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicationStats_Publication_PublicationId",
                table: "PublicationStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publication",
                table: "Publication");

            migrationBuilder.RenameTable(
                name: "Publication",
                newName: "Publications");

            migrationBuilder.RenameIndex(
                name: "IX_Publication_UserId",
                table: "Publications",
                newName: "IX_Publications_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publications",
                table: "Publications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AppUsers_UserId",
                table: "Publications",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "UserAuthId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicationStats_Publications_PublicationId",
                table: "PublicationStats",
                column: "PublicationId",
                principalTable: "Publications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AppUsers_UserId",
                table: "Publications");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicationStats_Publications_PublicationId",
                table: "PublicationStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publications",
                table: "Publications");

            migrationBuilder.RenameTable(
                name: "Publications",
                newName: "Publication");

            migrationBuilder.RenameIndex(
                name: "IX_Publications_UserId",
                table: "Publication",
                newName: "IX_Publication_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publication",
                table: "Publication",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_AppUsers_UserId",
                table: "Publication",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "UserAuthId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicationStats_Publication_PublicationId",
                table: "PublicationStats",
                column: "PublicationId",
                principalTable: "Publication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
