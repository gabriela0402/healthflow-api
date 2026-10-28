using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessionalPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Professionals");
        }
    }
}
