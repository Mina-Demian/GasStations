using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GasStations_GasAPI.Migrations
{
    /// <inheritdoc />
    public partial class StartingGasStationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GasStations",
                columns: new[] { "Id", "Address", "CreatedDate", "Name", "Number_of_Pumps", "Price", "Purity", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "600 Dundas St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shell", 8, 147.30000000000001, 87, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "1525 Burnhamthorpe Rd", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Petro Canada", 12, 146.80000000000001, 87, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GasStations",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
