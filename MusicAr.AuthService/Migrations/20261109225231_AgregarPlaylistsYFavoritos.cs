using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAr.AuthService.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPlaylistsYFavoritos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "PlaylistCanciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Orden",
                table: "PlaylistCanciones");
        }
    }
}
