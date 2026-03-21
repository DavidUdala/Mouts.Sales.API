using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIsCancelledToSaleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Product");

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "SaleItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "SaleItem");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Product",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Cerveja Bohemia garrafa 600ml", "Bohemia 600ml", 0m },
                    { new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Cerveja Corona garrafa long neck 330ml", "Corona 330ml", 0m },
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Refrigerante Guaraná Antarctica garrafa 2L", "Guaraná Antarctica 2L", 0m },
                    { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Bebida com gás sabor limão 500ml", "H2OH! Limão 500ml", 0m },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Cerveja Skol lata 350ml", "Skol 350ml", 0m },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Cerveja Brahma lata 350ml", "Brahma 350ml", 0m },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Cerveja Antarctica garrafa 600ml", "Antarctica 600ml", 0m },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Cerveja Stella Artois garrafa 550ml", "Stella Artois 550ml", 0m },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Cerveja Budweiser lata 350ml", "Budweiser 350ml", 0m },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Cerveja Original garrafa 600ml", "Original 600ml", 0m }
                });
        }
    }
}
