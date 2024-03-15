using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readdit.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatedDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AspNetUsers",
                newName: "UserCreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserCreatedDate",
                table: "AspNetUsers",
                newName: "CreatedDate");
        }
    }
}
