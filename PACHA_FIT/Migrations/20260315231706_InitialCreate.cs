using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PACHA_FIT.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentAccountId = table.Column<int>(type: "int", nullable: true),
                    IsPostable = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Accounts__349DA5A6FE8E4FCF", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK__Accounts__Parent__756D6ECB",
                        column: x => x.ParentAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "AdjustmentReasons",
                columns: table => new
                {
                    ReasonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Adjustme__A4F8C0E7C12F4CE2", x => x.ReasonId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__19093A0BDE4C94FA", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK__Categorie__Paren__5070F446",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentM__DC31C1D323A7DDC2", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__8AFACE1AD6C9EA39", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Attributes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__4BE666B4EBC13550", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    TaxRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaxRates__B114CEC1F99DCA29", x => x.TaxRateId);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasures",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,8)", nullable: false, defaultValue: 1m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UnitOfMe__44F5ECB5BFBA4837", x => x.UnitId);
                });

            migrationBuilder.CreateTable(
                name: "User",
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

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,4)", nullable: true, defaultValue: 0m),
                    TaxRateId = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__6B0A6BBE3B4C16E6", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK__PurchaseO__Suppl__619B8048",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK__PurchaseO__TaxRa__6383C8BA",
                        column: x => x.TaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "TaxRateId");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ManageStockDirectly = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    PurchaseUnitId = table.Column<int>(type: "int", nullable: true),
                    SaleUnitId = table.Column<int>(type: "int", nullable: true),
                    StockUnitId = table.Column<int>(type: "int", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StockQty = table.Column<decimal>(type: "decimal(18,4)", nullable: true, defaultValue: 0m),
                    MinStockLevel = table.Column<decimal>(type: "decimal(18,4)", nullable: true, defaultValue: 5m),
                    MainImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Specs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__B40CC6CD396E8750", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK__Products__Catego__5629CD9C",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK__Products__Purcha__5812160E",
                        column: x => x.PurchaseUnitId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK__Products__SaleUn__59063A47",
                        column: x => x.SaleUnitId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK__Products__StockU__59FA5E80",
                        column: x => x.StockUnitId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "AccountingPeriods",
                columns: table => new
                {
                    PeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ClosedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ClosedBy = table.Column<int>(type: "int", nullable: true),
                    LastReopenedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastReopenedBy = table.Column<int>(type: "int", nullable: true),
                    ReopenReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Accounti__E521BB16795CFA57", x => x.PeriodId);
                    table.ForeignKey(
                        name: "FK__Accountin__Close__0E391C95",
                        column: x => x.ClosedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK__Accountin__LastR__0F2D40CE",
                        column: x => x.LastReopenedBy,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecordId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AuditLog__A17F23980B61CE03", x => x.AuditId);
                    table.ForeignKey(
                        name: "FK__AuditLogs__UserI__72910220",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "POS"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Completed"),
                    Subtotal = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,4)", nullable: true, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SaleDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    IsCredit = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sales__1EE3C3FFBED21984", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK__Sales__PaymentMe__71D1E811",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId");
                    table.ForeignKey(
                        name: "FK__Sales__UserId__70DDC3D8",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseExpenses",
                columns: table => new
                {
                    PurchaseExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__D5E71DC0FA258615", x => x.PurchaseExpenseId);
                    table.ForeignKey(
                        name: "FK__PurchaseE__Accou__65370702",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK__PurchaseE__Purch__6442E2C9",
                        column: x => x.PurchaseId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseId");
                });

            migrationBuilder.CreateTable(
                name: "ProductCompositions",
                columns: table => new
                {
                    CompositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentProductId = table.Column<int>(type: "int", nullable: true),
                    BaseProductId = table.Column<int>(type: "int", nullable: true),
                    QuantityUsed = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductC__B8E2331F8834F3C0", x => x.CompositionId);
                    table.ForeignKey(
                        name: "FK__ProductCo__BaseP__03F0984C",
                        column: x => x.BaseProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__ProductCo__Paren__02FC7413",
                        column: x => x.ParentProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__ProductCo__UnitI__04E4BC85",
                        column: x => x.UnitId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseItems",
                columns: table => new
                {
                    PurchaseItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitCostSnapshot = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TaxRateSnapshot = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__B48BB687CE5BA814", x => x.PurchaseItemId);
                    table.ForeignKey(
                        name: "FK__PurchaseI__Produ__68487DD7",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__PurchaseI__Purch__6754599E",
                        column: x => x.PurchaseId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseId");
                });

            migrationBuilder.CreateTable(
                name: "CreditNotes",
                columns: table => new
                {
                    CreditNoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditNoteUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    NoteNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    AdjustmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "DEVOLUCION"),
                    AffectsInventory = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CreditNo__AF360DC6FF05BBC4", x => x.CreditNoteId);
                    table.ForeignKey(
                        name: "FK__CreditNot__SaleI__69FBBC1F",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
                    table.ForeignKey(
                        name: "FK__CreditNot__UserI__6AEFE058",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPayments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PaymentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__9B556A382CD9967D", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK__CustomerP__Payme__7D0E9093",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId");
                    table.ForeignKey(
                        name: "FK__CustomerP__SaleI__7C1A6C5A",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    SaleItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPriceSnapshot = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CostPriceSnapshot = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TaxRateSnapshot = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SaleItem__C605940140441B44", x => x.SaleItemId);
                    table.ForeignKey(
                        name: "FK__SaleItems__Produ__797309D9",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__SaleItems__SaleI__787EE5A0",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
                });

            migrationBuilder.CreateTable(
                name: "AccountingEntries",
                columns: table => new
                {
                    EntryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    PurchaseId = table.Column<int>(type: "int", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,4)", nullable: true, defaultValue: 0m),
                    Credit = table.Column<decimal>(type: "decimal(18,4)", nullable: true, defaultValue: 0m),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EntryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    CreditNoteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Accounti__F57BD2F79E3E7104", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK__Accountin__Accou__07C12930",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK__Accountin__Credi__078C1F06",
                        column: x => x.CreditNoteId,
                        principalTable: "CreditNotes",
                        principalColumn: "CreditNoteId");
                    table.ForeignKey(
                        name: "FK__Accountin__Purch__09A971A2",
                        column: x => x.PurchaseId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseId");
                    table.ForeignKey(
                        name: "FK__Accountin__SaleI__08B54D69",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
                });

            migrationBuilder.CreateTable(
                name: "CreditNoteItems",
                columns: table => new
                {
                    CreditNoteItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditNoteId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPriceSnapshot = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CreditNo__58E8CE59D1A75B4A", x => x.CreditNoteItemId);
                    table.ForeignKey(
                        name: "FK__CreditNot__Credi__6EC0713C",
                        column: x => x.CreditNoteId,
                        principalTable: "CreditNotes",
                        principalColumn: "CreditNoteId");
                    table.ForeignKey(
                        name: "FK__CreditNot__Produ__6FB49575",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    MovementId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    InputQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InputUnitId = table.Column<int>(type: "int", nullable: true),
                    BaseQuantityAffected = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TypeMovement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    PurchaseId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "(getdate())"),
                    AdjustmentReasonId = table.Column<int>(type: "int", nullable: true),
                    CreditNoteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StockMov__D18224464B22E24C", x => x.MovementId);
                    table.ForeignKey(
                        name: "FK__StockMove__Adjus__7755B73D",
                        column: x => x.AdjustmentReasonId,
                        principalTable: "AdjustmentReasons",
                        principalColumn: "ReasonId");
                    table.ForeignKey(
                        name: "FK__StockMove__Credi__7849DB76",
                        column: x => x.CreditNoteId,
                        principalTable: "CreditNotes",
                        principalColumn: "CreditNoteId");
                    table.ForeignKey(
                        name: "FK__StockMove__Input__7D439ABD",
                        column: x => x.InputUnitId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK__StockMove__Produ__7C4F7684",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__StockMove__Purch__7F2BE32F",
                        column: x => x.PurchaseId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseId");
                    table.ForeignKey(
                        name: "FK__StockMove__SaleI__7E37BEF6",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntries_AccountId",
                table: "AccountingEntries",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntries_CreditNoteId",
                table: "AccountingEntries",
                column: "CreditNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntries_PurchaseId",
                table: "AccountingEntries",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingEntries_SaleId",
                table: "AccountingEntries",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingPeriods_ClosedBy",
                table: "AccountingPeriods",
                column: "ClosedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingPeriods_LastReopenedBy",
                table: "AccountingPeriods",
                column: "LastReopenedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_Period",
                table: "AccountingPeriods",
                columns: new[] { "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentAccountId",
                table: "Accounts",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "UQ__Accounts__A25C5AA7E468A306",
                table: "Accounts",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ__Categori__BC7B5FB6C008D9E8",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteItems_CreditNoteId",
                table: "CreditNoteItems",
                column: "CreditNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteItems_ProductId",
                table: "CreditNoteItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_SaleId",
                table: "CreditNotes",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_UserId",
                table: "CreditNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__CreditNo__069440D2A3F5F451",
                table: "CreditNotes",
                column: "NoteNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_PaymentMethodId",
                table: "CustomerPayments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_SaleId",
                table: "CustomerPayments",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompositions_BaseProductId",
                table: "ProductCompositions",
                column: "BaseProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompositions_ParentProductId",
                table: "ProductCompositions",
                column: "ParentProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompositions_UnitId",
                table: "ProductCompositions",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PurchaseUnitId",
                table: "Products",
                column: "PurchaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SaleUnitId",
                table: "Products",
                column: "SaleUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StockUnitId",
                table: "Products",
                column: "StockUnitId");

            migrationBuilder.CreateIndex(
                name: "UQ__Products__BC7B5FB697F2171C",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Products__CA1ECF0D1BF91785",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseExpenses_AccountId",
                table: "PurchaseExpenses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseExpenses_PurchaseId",
                table: "PurchaseExpenses",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_ProductId",
                table: "PurchaseItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_PurchaseId",
                table: "PurchaseItems",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_TaxRateId",
                table: "PurchaseOrders",
                column: "TaxRateId");

            migrationBuilder.CreateIndex(
                name: "UQ__Roles__737584F6E0E9C92F",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItems",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PaymentMethodId",
                table: "Sales",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_UserId",
                table: "Sales",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Sales__CAC5E7433F24FD0C",
                table: "Sales",
                column: "OrderNumber",
                unique: true,
                filter: "[OrderNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_AdjustmentReasonId",
                table: "StockMovements",
                column: "AdjustmentReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CreditNoteId",
                table: "StockMovements",
                column: "CreditNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_InputUnitId",
                table: "StockMovements",
                column: "InputUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductId",
                table: "StockMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_PurchaseId",
                table: "StockMovements",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SaleId",
                table: "StockMovements",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Supplier__711BE0ADCF751AFC",
                table: "Suppliers",
                column: "TaxId",
                unique: true,
                filter: "[TaxId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105345028B452",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingEntries");

            migrationBuilder.DropTable(
                name: "AccountingPeriods");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CreditNoteItems");

            migrationBuilder.DropTable(
                name: "CustomerPayments");

            migrationBuilder.DropTable(
                name: "ProductCompositions");

            migrationBuilder.DropTable(
                name: "PurchaseExpenses");

            migrationBuilder.DropTable(
                name: "PurchaseItems");

            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "StockMovements");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AdjustmentReasons");

            migrationBuilder.DropTable(
                name: "CreditNotes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UnitOfMeasures");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "TaxRates");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
