using System;
using System.Collections.Generic;
using Backend_ThucTap.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_ThucTap.Data;

public partial class WebBadmintonContext : DbContext
{
    public WebBadmintonContext()
    {
    }

    public WebBadmintonContext(DbContextOptions<WebBadmintonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Function> Functions { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductSerial> ProductSerials { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleModuleFunction> RoleModuleFunctions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherCondition> VoucherConditions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-S80KR1F\\SQLEXPRESS;Initial Catalog=Web_Badminton1;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brands__DAD4F3BE675F08E6");

            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.BrandName).HasMaxLength(100);
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BFB219196");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Function>(entity =>
        {
            entity.HasKey(e => e.FunctionId).HasName("PK__Function__31ABF9187B9A470E");

            entity.Property(e => e.FunctionId).HasColumnName("FunctionID");
            entity.Property(e => e.FunctionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Modules__2B7477878179404A");

            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.ModuleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED8AE96C4F");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.DiscountPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Products__BrandI__628FA481");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__6383C8BA");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__ProductD__135C314DC8C543C4");

            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.BalancePoint).HasMaxLength(50);
            entity.Property(e => e.GripSize).HasMaxLength(20);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Stiffness).HasMaxLength(50);
            entity.Property(e => e.StockQuantity).HasDefaultValue(0);
            entity.Property(e => e.WeightClass).HasMaxLength(20);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDe__Produ__60A75C0F");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__7516F4ECC39AF844");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductIm__Produ__619B8048");
        });

        modelBuilder.Entity<ProductSerial>(entity =>
        {
            entity.HasKey(e => e.SerialId).HasName("PK__ProductS__5E5B3EC40A49B63E");

            entity.Property(e => e.SerialId).HasColumnName("SerialID");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.ImportDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Detail).WithMany(p => p.ProductSerials)
                .HasForeignKey(d => d.DetailId)
                .HasConstraintName("FK_ProductSerials_ProductDetails");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A555E3E5F");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<RoleModuleFunction>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.ModuleId, e.FunctionId }).HasName("PK__RoleModu__F17C22BB814BFEC7");

            entity.ToTable("RoleModuleFunction");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.FunctionId).HasColumnName("FunctionID");

            entity.HasOne(d => d.Function).WithMany(p => p.RoleModuleFunctions)
                .HasForeignKey(d => d.FunctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModuleFunction_Functions");

            entity.HasOne(d => d.Module).WithMany(p => p.RoleModuleFunctions)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModuleFunction_Modules");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleModuleFunctions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleModuleFunction_Roles");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACDA11DB34");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105340900DFCB").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRoles__RoleI__6754599E"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRoles__UserI__68487DD7"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF27604F85DA9AD6");
                        j.ToTable("UserRoles");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                        j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");
                    });
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__UserProf__290C8884BCECCDC6");

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DetailedAddress).HasMaxLength(255);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserProfiles_Users");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Vouchers__3AEE79C16B90EEE2");

            entity.Property(e => e.VoucherId).HasColumnName("VoucherID");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.MaxDiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinOrderValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.VoucherCode).HasMaxLength(50);
        });

        modelBuilder.Entity<VoucherCondition>(entity =>
        {
            entity.HasKey(e => e.ConditionId).HasName("PK__VoucherC__37F5C0EFA7AA7130");

            entity.Property(e => e.ConditionId).HasColumnName("ConditionID");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

            entity.HasOne(d => d.Brand).WithMany(p => p.VoucherConditions)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_VoucherConditions_Brands");

            entity.HasOne(d => d.Category).WithMany(p => p.VoucherConditions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_VoucherConditions_Categories");

            entity.HasOne(d => d.Product).WithMany(p => p.VoucherConditions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_VoucherConditions_Products");

            entity.HasOne(d => d.Voucher).WithMany(p => p.VoucherConditions)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_VoucherConditions_Vouchers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
