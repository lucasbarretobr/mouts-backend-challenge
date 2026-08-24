using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    [Migration("20260822170000_EmptyMigration")]
    public partial class EmptyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
    INSERT INTO "Users" ("Id", "Username", "Password", "Phone", "Email", "Status", "Role")
    SELECT 'b8f3a7e1-0c5d-4f29-9a64-2d7e8c1b5f30', 'admin', '$2a$11$tiTfOrztylb0sALkrdR9guJNffRbexf.cPwBts6RQTw53d.xvdoVa', '(11) 99999-9999', 'admin@ambev.com', 'Active', 'Admin'
    WHERE NOT EXISTS (
        SELECT 1 FROM "Users" WHERE "Email" = 'admin@ambev.com'
    );
    """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Users\" WHERE \"Id\" = 'b8f3a7e1-0c5d-4f29-9a64-2d7e8c1b5f30';");

        }
    }
}
