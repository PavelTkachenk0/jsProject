using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace university_backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class T7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Messages",
                newName: "Login");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Login",
                table: "Messages",
                newName: "Name");
        }
    }
}
