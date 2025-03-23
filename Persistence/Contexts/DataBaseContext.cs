using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Contexts;
using Persistence.Configurations.Users;
using Domain.Categories;
using Persistence.Configurations.Categories;

namespace Persistence.Contexts
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>, IDataBaseContext
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
