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

            // Add Admin Role to a user 
            var users = await userManager.GetUsersInRoleAsync("Admin");
            if (!users.Any())
            {
                var aUserForAdmin = await userManager.Users.Where(p => p.UserName == "09304243717").FirstOrDefaultAsync();
                if (aUserForAdmin is not null)
                    await userManager.AddToRoleAsync(aUserForAdmin, "Admin");
            }
        }
    }
}
