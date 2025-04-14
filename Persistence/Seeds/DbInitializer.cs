using Application.Common.AppKeyNames;
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
            //context.RolePermissions.RemoveRange(context.RolePermissions.ToList());
            //context.Permissions.RemoveRange(context.Permissions.ToList());
            //context.UserRoles.RemoveRange(context.UserRoles.ToList());
            //context.Roles.RemoveRange(context.Roles.ToList());
            //await context.SaveChangesAsync();

            //Add Permissions
            if (!await context.Permissions.AnyAsync())
            {
                var permissions = new List<Permission>
            {
                // Permissions
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Permission.ToString(), Action = KeyNameAction.View.ToString(), Description = "مشاهده همه دسترسی ها" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.RolePermission.ToString(), Action =  KeyNameAction.Add.ToString(), Description = "اضافه کردن یک دسترسی به نقش" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.RolePermission.ToString(), Action = KeyNameAction.Delete.ToString(), Description = "ریمو کردن یک دسترسی از نقش" },

                //Roles
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.View.ToString(), Description = "دیدن همه نقش ها" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.Add.ToString(), Description = "ایجاد یک نقش" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.Edit.ToString(), Description = "ویرایش یک نقش" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.Delete.ToString(), Description = "حذف یک نقش" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.UserRole.ToString(), Action = KeyNameAction.Add.ToString(), Description = "اختصاص دادن یک نقش به کاربر" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.UserRole.ToString(), Action = KeyNameAction.Delete.ToString(), Description =  "برداشتن یک نقش از کاربر" },


                // Users manager
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.User.ToString(), Action = KeyNameAction.View.ToString(), Description = "مشاهده کاربران" },

                // Categories
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.View.ToString(), Description = "مشاهده دسته بندی ها" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.Add.ToString(), Description = "ایجاد دسته بندی" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.Edit.ToString(), Description = "ویرایش دسته بندی" },
                new Permission { Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.Delete.ToString(), Description = "حذف دسته بندی" }
            };

                await context.Permissions.AddRangeAsync(permissions);
                await context.SaveChangesAsync();
            }

            // Add Roles
            if (!await context.Roles.AnyAsync())
            {
                var role = new Role { Name = "Admin", Description = "ادمین" };
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
