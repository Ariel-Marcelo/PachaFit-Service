using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PACHA_FIT.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelToCurrentState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Accountin__Close__0E391C95",
                table: "AccountingPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK__Accountin__LastR__0F2D40CE",
                table: "AccountingPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK__AuditLogs__UserI__72910220",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK__CreditNot__UserI__6AEFE058",
                table: "CreditNotes");

            migrationBuilder.DropForeignKey(
                name: "FK__Sales__UserId__70DDC3D8",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    IdentificationType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "05"),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4C54F69319", x => x.UserId);
                    table.ForeignKey(
                        name: "FK__Users__RoleId__4AB81AF0",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105345028B452",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Accountin__Close__0E391C95",
                table: "AccountingPeriods",
                column: "ClosedBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__Accountin__LastR__0F2D40CE",
                table: "AccountingPeriods",
                column: "LastReopenedBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__AuditLogs__UserI__72910220",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__CreditNot__UserI__6AEFE058",
                table: "CreditNotes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__Sales__UserId__70DDC3D8",
                table: "Sales",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Accountin__Close__0E391C95",
                table: "AccountingPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK__Accountin__LastR__0F2D40CE",
                table: "AccountingPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK__AuditLogs__UserI__72910220",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK__CreditNot__UserI__6AEFE058",
                table: "CreditNotes");

            migrationBuilder.DropForeignKey(
                name: "FK__Sales__UserId__70DDC3D8",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IdentificationType = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "05"),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4C54F69319", x => x.UserId);
                    table.ForeignKey(
                        name: "FK__Users__RoleId__4AB81AF0",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105345028B452",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Accountin__Close__0E391C95",
                table: "AccountingPeriods",
                column: "ClosedBy",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__Accountin__LastR__0F2D40CE",
                table: "AccountingPeriods",
                column: "LastReopenedBy",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__AuditLogs__UserI__72910220",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__CreditNot__UserI__6AEFE058",
                table: "CreditNotes",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__Sales__UserId__70DDC3D8",
                table: "Sales",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
