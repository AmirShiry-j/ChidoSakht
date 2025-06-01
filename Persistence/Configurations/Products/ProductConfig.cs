using Domain.Categories;
using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Products
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.CreateTime).HasDefaultValueSql("getdate()");
            builder.HasIndex(p => p.UniqeLink).IsUnique();
            builder.HasIndex(p => p.UniCode).IsUnique();
            //builder.Property(p=>p.ParentCategory).
        }
    }
}
