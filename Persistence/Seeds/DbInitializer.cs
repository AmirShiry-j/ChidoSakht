using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public static class DbInitializer
    {
        public static async Task SeedPermissionsAsync(DataBaseContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Add Permissions
            if (!await context.Permissions.AnyAsync())
            {
                var permissions = new List<Permission>
            {
                // Permissions
                new Permission { Area = "Admin", Controller = "Permission", Action = "View", Description = "مشاهده همه دسترسی ها" },
                new Permission { Area = "Admin", Controller = "PermissionRole", Action = "Add", Description = "اضافه کردن یک دسترسی به نقش" },
                new Permission { Area = "Admin", Controller = "PermissionRole", Action = "Delete", Description = "ریمو کردن یک دسترسی از نقش" },

                //Roles
                new Permission { Area = "Admin", Controller = "Role", Action = "View", Description = "دیدن همه نقش ها" },
                new Permission { Area = "Admin", Controller = "Role", Action = "Add", Description = "ایجاد یک نقش" },
                new Permission { Area = "Admin", Controller = "Role", Action = "Edit", Description = "ویرایش یک نقش" },
                new Permission { Area = "Admin", Controller = "Role", Action = "Delete", Description = "حذف یک نقش" },
                new Permission { Area = "Admin", Controller = "RoleUser", Action = "Add", Description = "اختصاص دادن یک نقش به کاربر" },
                new Permission { Area = "Admin", Controller = "RoleUser", Action = "Delete", Description =  "برداشتن یک نقش از کاربر" },


                // Users manager
                new Permission { Area = "Admin", Controller = "User", Action = "View", Description = "مشاهده کاربران" },

                // Categories
                new Permission { Area = "Admin", Controller = "Category", Action = "View", Description = "مشاهده دسته بندی ها" },
                new Permission { Area = "Admin", Controller = "Category", Action = "Add", Description = "ایجاد دسته بندی" },
                new Permission { Area = "Admin", Controller = "Category", Action = "Edit", Description = "ویرایش دسته بندی" },
                new Permission { Area = "Admin", Controller = "Category", Action = "Delete", Description = "حذف دسته بندی" }
            };

                await context.Permissions.AddRangeAsync(permissions);
                await context.SaveChangesAsync();
            }

            // Add Roles
            if (!await context.Roles.AnyAsync())
            {
                var role = new Role{Name="Admin", Description="ادمین" } ;
                await roleManager.CreateAsync(role);
            }

            // add Permissions to Roles
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var adminPermissions = await context.Permissions.ToListAsync();
                foreach (var permission in adminPermissions)
                {
                    var exists = await context.RolePermissions
                        .AnyAsync(rp => rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id);

                    if (!exists)
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = adminRole.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
