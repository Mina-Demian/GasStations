using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GasStations_GasAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingCityandCountryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "GasStations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryofOriginId",
                table: "GasStations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries_of_Origin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryofOrigin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries_of_Origin", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName" },
                values: new object[,]
                {
                    { 1, "Mississauga" },
                    { 2, "Oakville" }
                });

            migrationBuilder.InsertData(
                table: "Countries_of_Origin",
                columns: new[] { "Id", "CountryofOrigin" },
                values: new object[,]
                {
                    { 1, "France" },
                    { 2, "Canada" }
                });

            migrationBuilder.UpdateData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CityId", "CountryofOriginId", "CreatedDate" },
                values: new object[] { 2, 1, new DateTime(2023, 6, 29, 21, 30, 30, 519, DateTimeKind.Local).AddTicks(47) });

            migrationBuilder.UpdateData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CityId", "CountryofOriginId", "CreatedDate" },
                values: new object[] { 1, 2, new DateTime(2023, 6, 29, 21, 30, 30, 519, DateTimeKind.Local).AddTicks(100) });

            migrationBuilder.CreateIndex(
                name: "IX_GasStations_CityId",
                table: "GasStations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStations_CountryofOriginId",
                table: "GasStations",
                column: "CountryofOriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_GasStations_Cities_CityId",
                table: "GasStations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GasStations_Countries_of_Origin_CountryofOriginId",
                table: "GasStations",
                column: "CountryofOriginId",
                principalTable: "Countries_of_Origin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GasStations_Cities_CityId",
                table: "GasStations");

            migrationBuilder.DropForeignKey(
                name: "FK_GasStations_Countries_of_Origin_CountryofOriginId",
                table: "GasStations");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries_of_Origin");

            migrationBuilder.DropIndex(
                name: "IX_GasStations_CityId",
                table: "GasStations");

            migrationBuilder.DropIndex(
                name: "IX_GasStations_CountryofOriginId",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "CountryofOriginId",
                table: "GasStations");

            migrationBuilder.UpdateData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 21, 21, 19, 21, 182, DateTimeKind.Local).AddTicks(6680));

            migrationBuilder.UpdateData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 21, 21, 19, 21, 182, DateTimeKind.Local).AddTicks(6764));
        }
    }
}
