using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Cerveja Bohemia garrafa 600ml",               "Bohemia 600ml" },
                    { new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Cerveja Corona long neck 330ml",              "Corona 330ml" },
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Refrigerante Guaraná Antarctica garrafa 2L",  "Guaraná Antarctica 2L" },
                    { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Bebida com gás sabor limão 500ml",            "H2OH! Limão 500ml" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Cerveja Skol lata 350ml",                     "Skol 350ml" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Cerveja Brahma lata 350ml",                   "Brahma 350ml" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Cerveja Antarctica garrafa 600ml",            "Antarctica 600ml" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Cerveja Stella Artois garrafa 550ml",         "Stella Artois 550ml" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Cerveja Budweiser lata 350ml",                "Budweiser 350ml" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Cerveja Original garrafa 600ml",              "Original 600ml" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));
            migrationBuilder.DeleteData(table: "Product", keyColumn: "Id", keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

        }
    }
}
