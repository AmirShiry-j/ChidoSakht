using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Products
{
    public class ProductAttributeConfig : IEntityTypeConfiguration<Domain.Products.ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<Domain.Products.ProductAttribute> builder)
        {
            var converterForAttributeTypeEnum = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.EnumToStringConverter<Domain.Products.AttributeType>();


            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.AttributeType).IsRequired().HasConversion(converterForAttributeTypeEnum);
        }
    }
}
