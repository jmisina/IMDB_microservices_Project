using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var CreateProcedure = "CREATE OR REPLACE PROCEDURE CreateUser(\r\n" +
                " p_username VARCHAR(50),\r\n " +
                " p_password_hash VARCHAR(255),\r\n " +
                " p_email VARCHAR(255),\r\n  " +
                " p_role VARCHAR(20)\r\n" +
                " )\r\n" +
                " LANGUAGE plpgsql\r\n" +
                " AS $$" +
                "\r\nDECLARE\r\n" +
                " new_user_id INT;\r\nBEGIN\r\n" +
                " BEGIN\r\n" +
                " INSERT INTO \"Users\" (\"Username\", \"CreatedAt\")\r\n" +
                "   VALUES (p_username, NOW())\r\n     " +
                "   RETURNING \"Id\" INTO new_user_id;\r\n\r\n" +
                " INSERT INTO \"UserAuthorisations\" (\"UserId\", \"PasswordHash\", \"Email\", \"Role\")\r\n" +
                "   VALUES (new_user_id, p_password_hash, p_email, p_role);\r\n\r\n\r\n\r\n" +
                " EXCEPTION WHEN OTHERS THEN\r\n" +
                "   RAISE EXCEPTION 'Error inserting data: %'," +
                "   SQLERRM;\r\n" +
                "   ROLLBACK;\r\n" +
                " END;\r\n" +
                " END;\r\n " +
                " $$;";
            migrationBuilder.Sql(CreateProcedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var CreateProcedure = "DROP CreateUser;";
            migrationBuilder.Sql(CreateProcedure);

        }
    }
}
