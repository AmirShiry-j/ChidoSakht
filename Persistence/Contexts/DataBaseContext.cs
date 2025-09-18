using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.Users;
using Domain.Categories;
using Persistence.Configurations.Categories;
using Domain.Products;
using Persistence.Configurations.Products;
using System.Reflection.Emit;
using Domain.SymbolicShoppingCarts;
using Persistence.Configurations.Carts;
using Domain.Carts;
using Domain.Orders;

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
        public DbSet<Address> Addresses { get; set; }

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
        public DbSet<ProductVariantTransportation> ProductVariantTransportations { get; set; }
        //
        public DbSet<SymbolicOrderOrSymbolicShoppingCartItem> SymbolicOrderOrSymbolicShoppingCartItems { get; set; }
        //
        public DbSet<RelatedProduct> RelatedProducts { get; set; }
        //
        public DbSet<ProductSpecificationGroup> ProductSpecificationGroups { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        //
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Helpful> Helpfuls { get; set; }
        //
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        //
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Filters
            builder.Entity<Product>().HasQueryFilter(p => p.IsPublished);

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

            builder.Entity<Category>()
    .HasMany(p => p.Products)
    .WithOne(p => p.Category)
    .HasForeignKey(p => p.CategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

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
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Product>()
.HasMany(p => p.ProductAttributes)
.WithOne(p => p.Product)
.HasForeignKey(p => p.ProductId)
.IsRequired(true)
.OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Domain.Products.ProductVariant>()
    .HasMany(p => p.ProductVariantAttributeValues)
    .WithOne(p => p.ProductVariant)
    .HasForeignKey(v => v.ProductVariantId)
    .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Domain.Products.ProductAttribute>()
    .HasMany(p => p.ProductAttributeValues)
    .WithOne(p => p.ProductAttribute)
    .HasForeignKey(v => v.ProductAttributeId)
    .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Domain.Products.Product>()
    .HasMany(p => p.ProductVariants)
    .WithOne(p => p.Product)
    .HasForeignKey(v => v.ProductId)
    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Domain.Products.ProductAttributeValue>()
    .HasMany<Domain.Products.ProductVariantAttributeValue>()
    .WithOne(p => p.ProductAttributeValue)
    .HasForeignKey(v => v.ProductAttributeValueId)
    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Domain.Products.ProductVariant>()
    .HasOne(p => p.ProductVariantTransportation)
    .WithOne(p => p.ProductVariant)
    .HasForeignKey<Domain.Products.ProductVariantTransportation>(v => v.ProductVariantId)
    .OnDelete(DeleteBehavior.Cascade);

            //

            builder.Entity<Domain.Products.ProductVariant>()
    .HasMany<Domain.SymbolicShoppingCarts.SymbolicOrderOrSymbolicShoppingCartItem>()
    .WithOne(p => p.ProductVariant)
    .HasForeignKey(p => p.ProductVariantId)
    .OnDelete(DeleteBehavior.NoAction);
            //


            builder.Entity<RelatedProduct>()
    .HasKey(rp => new { rp.ProductId, rp.RelatedProductId });

            builder.Entity<RelatedProduct>()
                .HasOne(rp => rp.Product)
                .WithMany(p => p.RelatedProducts)
                .HasForeignKey(rp => rp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RelatedProduct>()
                .HasOne(rp => rp.RelatedTo)
                .WithMany(p => p.RelatedToProducts)
                .HasForeignKey(rp => rp.RelatedProductId)
                .OnDelete(DeleteBehavior.Restrict);
            //
            builder.Entity<ProductSpecificationGroup>()
                .HasMany(g => g.Specifications)
                .WithOne(s => s.ProductSpecificationGroup)
                .HasForeignKey(s => s.ProductSpecificationGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            //

            builder.Entity<Domain.Users.User>()
    .HasMany<Comment>()
    .WithOne(p => p.User)
    .HasForeignKey(v => v.UserId)
    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Domain.Users.User>()
.HasMany <Order>()
.WithOne(p => p.User)
.HasForeignKey(v => v.UserId)
.OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Domain.Users.User>()
.HasMany<Cart>()
.WithOne(p => p.User)
.HasForeignKey(v => v.UserId)
.OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
.HasOne(p=>p.Cart)
.WithOne()
.HasForeignKey<Order>(p=> p.CartId)
.OnDelete(DeleteBehavior.NoAction);


            builder.Entity<Payment>()
.HasOne(p => p.Order)
.WithOne(p=>p.Payment)
.HasForeignKey<Payment>(p => p.OrderId)
.OnDelete(DeleteBehavior.NoAction);


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
            builder.ApplyConfiguration(new CommentConfig());

            //Carts
            builder.ApplyConfiguration(new CartConfig());

            base.OnModelCreating(builder);
        }
    }
}
