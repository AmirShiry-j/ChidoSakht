using Domain.Carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Carts
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            var converterForCartStatusEnum = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.EnumToStringConverter<CartType>();
            builder.Property(p => p.CartType).IsRequired().HasConversion(converterForCartStatusEnum);
        }
    }
}
