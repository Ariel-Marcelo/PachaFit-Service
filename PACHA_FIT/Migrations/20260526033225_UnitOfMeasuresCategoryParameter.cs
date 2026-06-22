using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PACHA_FIT.Migrations
{
    /// <inheritdoc />
    public partial class UnitOfMeasuresCategoryParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "UnitOfMeasures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "UnitOfMeasures");
        }
    }
}
