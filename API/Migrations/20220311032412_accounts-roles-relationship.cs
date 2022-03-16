using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class accountsrolesrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_accounts_roles",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_accounts_roles", x => new { x.NIK, x.Role_Id });
                    table.ForeignKey(
                        name: "FK_tb_tr_accounts_roles_tb_m_role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "tb_m_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_tr_accounts_roles_tb_tr_account_NIK",
                        column: x => x.NIK,
                        principalTable: "tb_tr_account",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_accounts_roles_Role_Id",
                table: "tb_tr_accounts_roles",
                column: "Role_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_tr_accounts_roles");

            migrationBuilder.DropTable(
                name: "tb_m_role");
        }
    }
}
