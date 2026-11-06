using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicAr.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class cargandoCatalogoDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Artista_ArtistaId",
                table: "Canciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Genero_GeneroId",
                table: "Canciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genero",
                table: "Genero");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artista",
                table: "Artista");

            migrationBuilder.RenameTable(
                name: "Genero",
                newName: "Generos");

            migrationBuilder.RenameTable(
                name: "Artista",
                newName: "Artistas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Generos",
                table: "Generos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artistas",
                table: "Artistas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Artistas_ArtistaId",
                table: "Canciones",
                column: "ArtistaId",
                principalTable: "Artistas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Generos_GeneroId",
                table: "Canciones",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Artistas_ArtistaId",
                table: "Canciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Generos_GeneroId",
                table: "Canciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Generos",
                table: "Generos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artistas",
                table: "Artistas");

            migrationBuilder.RenameTable(
                name: "Generos",
                newName: "Genero");

            migrationBuilder.RenameTable(
                name: "Artistas",
                newName: "Artista");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genero",
                table: "Genero",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artista",
                table: "Artista",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Artista_ArtistaId",
                table: "Canciones",
                column: "ArtistaId",
                principalTable: "Artista",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Genero_GeneroId",
                table: "Canciones",
                column: "GeneroId",
                principalTable: "Genero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
