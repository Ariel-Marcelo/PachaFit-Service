using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PACHA_FIT.Core.Domain.Entities;

namespace PACHA_FIT.src.Core.Domain.Entities;

public partial class PachaFitContext : DbContext
{
    public PachaFitContext(DbContextOptions<PachaFitContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountingEntry> AccountingEntries { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductComposition> ProductCompositions { get; set; }

    public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<StockMovement> StockMovements { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TaxRate> TaxRates { get; set; }

    public virtual DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A6FE8E4FCF");

            entity.HasIndex(e => e.Code, "UQ__Accounts__A25C5AA7E468A306").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<AccountingEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__Accounti__F57BD2F79E3E7104");

            entity.Property(e => e.Credit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Debit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntryDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountingEntries)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Accountin__Accou__07C12930");

            entity.HasOne(d => d.Purchase).WithMany(p => p.AccountingEntries)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK__Accountin__Purch__09A971A2");

            entity.HasOne(d => d.Sale).WithMany(p => p.AccountingEntries)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK__Accountin__SaleI__08B54D69");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BDE4C94FA");

            entity.HasIndex(e => e.Slug, "UQ__Categori__BC7B5FB6C008D9E8").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Slug).HasMaxLength(150);

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK__Categorie__Paren__5070F446");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1D323A7DDC2");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD396E8750");

            entity.HasIndex(e => e.Slug, "UQ__Products__BC7B5FB697F2171C").IsUnique();

            entity.HasIndex(e => e.Sku, "UQ__Products__CA1ECF0D1BF91785").IsUnique();

            entity.Property(e => e.CostPrice).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPublished).HasDefaultValue(false);
            entity.Property(e => e.MainImageUrl).HasColumnName("MainImageURL");
            entity.Property(e => e.ManageStockDirectly).HasDefaultValue(true);
            entity.Property(e => e.MinStockLevel)
                .HasDefaultValue(5m)
                .HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.ProductUuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("SKU");
            entity.Property(e => e.Slug).HasMaxLength(250);
            entity.Property(e => e.StockQty)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__5629CD9C");

            entity.HasOne(d => d.PurchaseUnit).WithMany(p => p.ProductPurchaseUnits)
                .HasForeignKey(d => d.PurchaseUnitId)
                .HasConstraintName("FK__Products__Purcha__5812160E");

            entity.HasOne(d => d.SaleUnit).WithMany(p => p.ProductSaleUnits)
                .HasForeignKey(d => d.SaleUnitId)
                .HasConstraintName("FK__Products__SaleUn__59063A47");

            entity.HasOne(d => d.StockUnit).WithMany(p => p.ProductStockUnits)
                .HasForeignKey(d => d.StockUnitId)
                .HasConstraintName("FK__Products__StockU__59FA5E80");
        });

        modelBuilder.Entity<ProductComposition>(entity =>
        {
            entity.HasKey(e => e.CompositionId).HasName("PK__ProductC__B8E2331F8834F3C0");

            entity.Property(e => e.QuantityUsed).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.BaseProduct).WithMany(p => p.ProductCompositionBaseProducts)
                .HasForeignKey(d => d.BaseProductId)
                .HasConstraintName("FK__ProductCo__BaseP__03F0984C");

            entity.HasOne(d => d.ParentProduct).WithMany(p => p.ProductCompositionParentProducts)
                .HasForeignKey(d => d.ParentProductId)
                .HasConstraintName("FK__ProductCo__Paren__02FC7413");

            entity.HasOne(d => d.Unit).WithMany(p => p.ProductCompositions)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__ProductCo__UnitI__04E4BC85");
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => e.PurchaseItemId).HasName("PK__Purchase__B48BB687CE5BA814");

            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TaxRateSnapshot).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitCostSnapshot).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__PurchaseI__Produ__68487DD7");

            entity.HasOne(d => d.Purchase).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK__PurchaseI__Purch__6754599E");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BBE3B4C16E6");

            entity.Property(e => e.PurchaseDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalTax)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__PurchaseO__Suppl__619B8048");

            entity.HasOne(d => d.TaxRate).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.TaxRateId)
                .HasConstraintName("FK__PurchaseO__TaxRa__6383C8BA");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AD6C9EA39");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F6E0E9C92F").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C3FFBED21984");

            entity.HasIndex(e => e.OrderNumber, "UQ__Sales__CAC5E7433F24FD0C").IsUnique();

            entity.Property(e => e.OrderNumber).HasMaxLength(50);
            entity.Property(e => e.Origin)
                .HasMaxLength(20)
                .HasDefaultValue("POS");
            entity.Property(e => e.SaleDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SaleUuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ShippingCost)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Completed");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalTax).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Sales)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Sales__PaymentMe__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.Sales)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Sales__UserId__70DDC3D8");
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(e => e.SaleItemId).HasName("PK__SaleItem__C605940140441B44");

            entity.Property(e => e.CostPriceSnapshot).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TaxRateSnapshot).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitPriceSnapshot).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__SaleItems__Produ__797309D9");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleItems)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK__SaleItems__SaleI__787EE5A0");
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.HasKey(e => e.MovementId).HasName("PK__StockMov__D18224464B22E24C");

            entity.Property(e => e.BaseQuantityAffected).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.InputQuantity).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TypeMovement).HasMaxLength(50);

            entity.HasOne(d => d.InputUnit).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.InputUnitId)
                .HasConstraintName("FK__StockMove__Input__7D439ABD");

            entity.HasOne(d => d.Product).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__StockMove__Produ__7C4F7684");

            entity.HasOne(d => d.Purchase).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("FK__StockMove__Purch__7F2BE32F");

            entity.HasOne(d => d.Sale).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK__StockMove__SaleI__7E37BEF6");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4EBC13550");

            entity.HasIndex(e => e.TaxId, "UQ__Supplier__711BE0ADCF751AFC").IsUnique();

            entity.Property(e => e.ContactEmail).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.TaxId).HasMaxLength(20);
        });

        modelBuilder.Entity<TaxRate>(entity =>
        {
            entity.HasKey(e => e.TaxRateId).HasName("PK__TaxRates__B114CEC1F99DCA29");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Rate).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<UnitOfMeasure>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__UnitOfMe__44F5ECB5BFBA4837");

            entity.Property(e => e.Abbreviation).HasMaxLength(10);
            entity.Property(e => e.ConversionFactor)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(18, 8)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C54F69319");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345028B452").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleId__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
