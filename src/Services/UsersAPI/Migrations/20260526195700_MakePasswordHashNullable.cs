using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakePasswordHashNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserAuthorisations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.UpdateData(
                table: "UserAuthorisations",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "DB05358E2A1DFB67BD4070BC6452869DE69A92B55BB2CFE05CFC0759D3E86F56-B7DDA628FB99D19ECA90463D1F128A30");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserAuthorisations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "UserAuthorisations",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "0409A6767571E8669947844002660700CD9E47A9A060C3817169A1A4E90B0929-37A092A2877665E00C6752044813A2AA");
        }
    }
}
