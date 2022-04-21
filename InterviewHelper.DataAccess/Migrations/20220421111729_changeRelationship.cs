using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewHelper.DataAccess.Migrations
{
    public partial class changeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionTag");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Tag",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_QuestionId",
                table: "Tag",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Questions_QuestionId",
                table: "Tag",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Questions_QuestionId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_QuestionId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Tag");

            migrationBuilder.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    QuestionsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTag", x => new { x.QuestionsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_QuestionTag_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_TagsId",
                table: "QuestionTag",
                column: "TagsId");
        }
    }
}
