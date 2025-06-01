using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.Users;
using Domain.Categories;
using Persistence.Configurations.Categories;
using Domain.Products;
using Persistence.Configurations.Products;
using System.Reflection.Emit;

namespace Persistence.Contexts
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }
        //Users
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }

        //Categories
        public DbSet<Category> Categories { get; set; }
        //Permissions
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        //Products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        //
        public DbSet<Domain.Products.ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Domain.Products.ProductVariant> ProductVariants { get; set; }
        public DbSet<Domain.Products.ProductVariantAttributeValue> ProductVariantAttributeValues { get; set; }
        //
        protected override void OnModelCreating(ModelBuilder builder)
        {
            ////Relations
            //Users
            builder.Entity<Token>()
                .HasOne(p => p.User)
                .WithMany(p => p.Tokens)
                .HasForeignKey(p => p.UserId)
                .IsRequired(true);

            builder.Entity<Token>()
                .HasOne(p => p.User)
                .WithMany(p => p.Tokens)
                .HasForeignKey(p => p.UserId)
                .IsRequired(true);

            //Categories
            builder.Entity<Category>()
                .HasOne(p => p.ParentCategory)
                .WithOne()
                .HasForeignKey<Category>(p => p.ParentCategoryId)
                .IsRequired(false);

            builder.Entity<Category>()
                .HasOne<Category>(p => p.ParentCategory)
                .WithMany(p => p.ChildCategories)
                .HasForeignKey(p => p.ParentCategoryId)
                .IsRequired(false);

            //Permissions
            builder.Entity<RolePermission>()
    .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);

            //Products
            builder.Entity<Product>()
    .HasMany(p => p.ProductImages)
    .WithOne()
    .HasForeignKey(p => p.ProductId)
            .IsRequired(true);


            builder.Entity<ProductVariantAttributeValue>()
    .HasOne(v => v.ProductVariant)
    .WithMany(pv => pv.ProductVariantAttributeValues)
    .HasForeignKey(v => v.ProductVariantId)
    .OnDelete(DeleteBehavior.NoAction); // یا DeleteBehavior.NoAction

            SetConfigurations(builder);

            base.OnModelCreating(builder);
        }

        private void SetConfigurations(ModelBuilder builder)
        {
            //Users
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new TokenConfig());
            builder.ApplyConfiguration(new PermissionConfig());

            //Categories
            builder.ApplyConfiguration(new CategoryConfig());

            //Products
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new ProductImageConfig());
            builder.ApplyConfiguration(new ProductAttributeConfig());


            base.OnModelCreating(builder);
        }
    }
}
