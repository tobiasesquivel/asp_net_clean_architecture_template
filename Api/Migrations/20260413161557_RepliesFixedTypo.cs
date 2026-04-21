using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RepliesFixedTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentCommendId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ParentCommendId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ParentCommendId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentcommentId",
                table: "Comment",
                column: "ParentcommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentcommentId",
                table: "Comment",
                column: "ParentcommentId",
                principalTable: "Comment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentcommentId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ParentcommentId",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "ParentCommendId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentCommendId",
                table: "Comment",
                column: "ParentCommendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentCommendId",
                table: "Comment",
                column: "ParentCommendId",
                principalTable: "Comment",
                principalColumn: "Id");
        }
    }
}
