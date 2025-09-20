using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;

namespace Persistence.Configurations.Addresses
{
    public class ProvinceConfig : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.Property(p => p.ProvinceId).ValueGeneratedNever();
            builder.Property(p => p.ProvinceName).IsRequired().HasMaxLength(50);
        }
    }
}
