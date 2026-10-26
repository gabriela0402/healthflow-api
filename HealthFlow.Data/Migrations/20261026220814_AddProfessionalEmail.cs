using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessionalEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Professionals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_Email",
                table: "Professionals",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Professionals_Email",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Professionals");
        }
    }
}
