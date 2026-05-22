using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jornadas_Metalurgia_2026.Migrations
{
    /// <inheritdoc />
    public partial class Tablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentDNI",
                table: "Inscription",
                newName: "StudentDni");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentDni",
                table: "Inscription",
                newName: "StudentDNI");
        }
    }
}
