using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("\rINSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'e114a923-a267-4942-a84e-903acbabd90f', N'Super', N'Admin', N'SuperAdmin', N'SUPERADMIN', N'SuperAdmin@BookStore.com', N'SUPERADMIN@BOOKSTORE.COM', 0, N'AQAAAAIAAYagAAAAEN0FXsF/z8r9+52COOxuQDc1GUMgt+TdPhwDgsndS8v9MwppczICJH8WXpje3MH+Rg==', N'24CDQSS6BFQ3GHIOBETLFMIVS3NMHE4Q', N'ca50e4a8-1f50-455f-9ab1-576efdc0fe71', NULL, 0, 0, NULL, 1, 0)\r");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM  [dbo].[AspNetUsers] WHERE [Email] = 'SuperAdmin@BookStore.com'");
        }
    }
}
