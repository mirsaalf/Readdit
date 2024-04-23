using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readdit.Migrations
{
    /// <inheritdoc />
    public partial class AddSarahJMaasBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "book_name", "author_name", "book_genre", "date_added" },
                values: new object[,]
                {
                    { "Throne of Glass", "Sarah J. Maas", "Fantasy", DateTime.Now },
                    { "Crown of Midnight", "Sarah J. Maas", "Fantasy", DateTime.Now },
                    { "Heir of Fire", "Sarah J. Maas", "Fantasy", DateTime.Now },
                    { "Queen of Shadows", "Sarah J. Maas", "Fantasy", DateTime.Now },
                    { "Empire of Storms", "Sarah J. Maas", "Fantasy", DateTime.Now },
                    { "Tower of Dawn", "Sarah J. Maas", "Fantasy", DateTime.Now },
                    { "Kingdom of Ash", "Sarah J. Maas", "Fantasy", DateTime.Now }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "book_name",
                keyValues: new object[] { "Throne of Glass", "Crown of Midnight", "Heir of Fire", "Queen of Shadows", "Empire of Storms", "Tower of Dawn", "Kingdom of Ash" });
        }
    
    }
}
