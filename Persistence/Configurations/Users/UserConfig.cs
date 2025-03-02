using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Users
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FullName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Email).IsRequired(false);
            builder.Property(p => p.PhoneNumber).IsRequired(true);
            builder.Property(p => p.ImageName).IsRequired(false);
            builder.Property(p => p.TimeCreate).HasDefaultValueSql("getdate()");
        }
    }
}
