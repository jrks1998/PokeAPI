using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pokeApi.Migrations
{
    /// <inheritdoc />
    public partial class Nova : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "pokemons",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pokemons",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nome",
                table: "pokemons",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "pokemons",
                newName: "Id");
        }
    }
}
