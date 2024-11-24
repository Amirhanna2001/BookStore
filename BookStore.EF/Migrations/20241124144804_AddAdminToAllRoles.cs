using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminToAllRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] (UserId, RoleId) SELECT 'e114a923-a267-4942-a84e-903acbabd90f', Id FROM [dbo].[AspNetRoles]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId = 'e114a923-a267-4942-a84e-903acbabd90f'");
        }
    }
}
