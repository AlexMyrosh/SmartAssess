using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnIsAssedByAi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAssessedByAi",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAssessedByAi",
                table: "Exams");
        }
    }
}
