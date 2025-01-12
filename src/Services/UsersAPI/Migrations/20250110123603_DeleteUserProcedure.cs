using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeleteUserProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var CreateProcedure = "CREATE OR REPLACE PROCEDURE DeleteUser(\r\n" +
                                  " p_user_id INT\r\n" +
                                  ")\r\n" +
                                  " LANGUAGE plpgsql\r\n" +
                                  " AS $$\r\n" +
                                  "BEGIN\r\n" +
                                  "   BEGIN\r\n" +
                                  "       DELETE FROM \"UserAuthorisations\"\r\n" +
                                  "       WHERE \"UserId\" = p_user_id;\r\n" +
                                  "\r\n" +
                                  "       DELETE FROM \"UserProfiles\"\r\n" +
                                  "       WHERE \"UserId\" = p_user_id;\r\n" +
                                  "\r\n" +
                                  "       DELETE FROM \"Addresses\"\r\n" +
                                  "       WHERE \"UserId\" = p_user_id;\r\n" +
                                  "\r\n" +
                                  "       DELETE FROM \"Users\"\r\n" +
                                  "       WHERE \"Id\" = p_user_id;\r\n" +
                                  "\r\n" +
                                  "   EXCEPTION WHEN OTHERS THEN\r\n" +
                                  "       RAISE EXCEPTION 'Error deleting data: %', SQLERRM;\r\n" +
                                  "   END;\r\n" +
                                  "END;\r\n" +
                                  "$$;";

            migrationBuilder.Sql(CreateProcedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
