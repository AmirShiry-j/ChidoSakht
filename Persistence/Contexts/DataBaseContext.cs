using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.Users;
using Domain.Categories;
using Persistence.Configurations.Categories;

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

            SetConfigurations(builder);

            base.OnModelCreating(builder);
        }

        private void SetConfigurations(ModelBuilder builder)
        {
            //Users
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new TokenConfig());

            //Categories
            builder.ApplyConfiguration(new CategoryConfig());


            base.OnModelCreating(builder);
        }
    }
}
