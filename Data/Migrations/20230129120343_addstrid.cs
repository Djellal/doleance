using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doleance.Data.Migrations
{
    public partial class addstrid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "StructId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "StructId",
                table: "AspNetUsers");

          
        }
    }
}
