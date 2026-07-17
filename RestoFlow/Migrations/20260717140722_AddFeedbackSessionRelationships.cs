using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestoFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackSessionRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackResponses_FeedbackQuestions_QuestionId",
                table: "FeedbackResponses");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackResponses_FeedbackSessionId",
                table: "FeedbackResponses");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackResponses_FeedbackSessionId_QuestionId",
                table: "FeedbackResponses",
                columns: new[] { "FeedbackSessionId", "QuestionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackResponses_FeedbackQuestions_QuestionId",
                table: "FeedbackResponses",
                column: "QuestionId",
                principalTable: "FeedbackQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackResponses_FeedbackQuestions_QuestionId",
                table: "FeedbackResponses");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackResponses_FeedbackSessionId_QuestionId",
                table: "FeedbackResponses");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackResponses_FeedbackSessionId",
                table: "FeedbackResponses",
                column: "FeedbackSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackResponses_FeedbackQuestions_QuestionId",
                table: "FeedbackResponses",
                column: "QuestionId",
                principalTable: "FeedbackQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
