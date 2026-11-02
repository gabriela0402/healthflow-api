using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProfessionalNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProfessionalId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ProfessionalId",
                table: "Notifications",
                column: "ProfessionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Professionals_ProfessionalId",
                table: "Notifications",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Professionals_ProfessionalId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ProfessionalId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ProfessionalId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
