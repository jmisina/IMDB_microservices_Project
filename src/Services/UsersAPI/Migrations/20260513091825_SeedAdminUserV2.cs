using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserAuthorisations",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "0409A6767571E8669947844002660700CD9E47A9A060C3817169A1A4E90B0929-37A092A2877665E00C6752044813A2AA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserAuthorisations",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "0000000000000000000000000000000000000000000000000000000000000000-00000000000000000000000000000000");
        }
    }
}
