using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PACHA_FIT.Migrations
{
    /// <inheritdoc />
    public partial class NewParamsToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "StockMovements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxRateId",
                table: "SaleItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsWeightBased",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IvaPercentage",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_TaxRateId",
                table: "SaleItems",
                column: "TaxRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_TaxRates_TaxRateId",
                table: "SaleItems",
                column: "TaxRateId",
                principalTable: "TaxRates",
                principalColumn: "TaxRateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_TaxRates_TaxRateId",
                table: "SaleItems");

            migrationBuilder.DropIndex(
                name: "IX_SaleItems_TaxRateId",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "TaxRateId",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "IsWeightBased",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IvaPercentage",
                table: "Products");
        }
    }
}
