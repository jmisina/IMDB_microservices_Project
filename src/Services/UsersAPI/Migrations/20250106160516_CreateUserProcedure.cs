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
                                    " p_email VARCHAR(50),\r\n  " +
                                    " p_name VARCHAR(50),\r\n" +
                                    " p_lastname VARCHAR(50)\r\n" +
                                    ")\r\n" +
                                    " LANGUAGE plpgsql\r\n" +
                                    " AS $$\r\n" +
                                    "DECLARE\r\n" +
                                    " new_user_id INT;\r\n" +
                                    "BEGIN\r\n" +
                                    "   BEGIN\r\n" +
                                    "       INSERT INTO \"Users\" (\"Username\", \"CreatedAt\")\r\n" +
                                    "       VALUES (p_username, NOW())\r\n" +
                                    "       RETURNING \"Id\" INTO new_user_id;\r\n" +
                                    "\r\n" +
                                    "       INSERT INTO \"UserAuthorisations\" (\"UserId\", \"PasswordHash\", \"Email\", \"Role\")\r\n" +
                                    "       VALUES (new_user_id, p_password_hash, p_email, 'USER');\r\n" +
                                    "\r\n" +
                                    "       INSERT INTO \"UserProfiles\" (\"UserId\", \"FirstName\", \"LastName\")\r\n" +
                                    "       VALUES (new_user_id, p_name, p_lastname);\r\n" +
                                    "\r\n" +
                                    "       INSERT INTO \"Addresses\" (\"UserId\")\r\n" +
                                    "       VALUES (new_user_id);\r\n" +
                                    "\r\n" +
                                    "   EXCEPTION WHEN OTHERS THEN\r\n" +
                                    "       RAISE EXCEPTION 'Error inserting data: %', SQLERRM;\r\n" +
                                    "   END;\r\n" +
                                    "END;\r\n" +
                                    "$$;";

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
