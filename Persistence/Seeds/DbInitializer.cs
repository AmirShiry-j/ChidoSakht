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
            var permissions = new List<Permission>
            {
                // User
                new Permission { Id=1, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.User.ToString(), Action = KeyNameAction.View.ToString(), Description = "مشاهده کاربران" },
                                 
                // Permission    
                new Permission { Id=2, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Permission.ToString(), Action = KeyNameAction.View.ToString(), Description = "مشاهده دسترسی ها" },
                                 
                //Role           
                new Permission { Id=3, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.View.ToString(), Description = "دیدن نقش ها" },
                new Permission { Id=4, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.Add.ToString(), Description = "ایجاد یک نقش" },
                new Permission { Id=5, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.Edit.ToString(), Description = "ویرایش یک نقش" },
                new Permission { Id=6, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Role.ToString(), Action = KeyNameAction.Delete.ToString(), Description = "حذف یک نقش" },
                                 
                //RolePermission 
                new Permission { Id=7, Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.RolePermission.ToString(), Action =  KeyNameAction.View.ToString(), Description = "دیدن دسترسی های اختصاص داده شده به نقش ها" },
                new Permission { Id=8, Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.RolePermission.ToString(), Action =  KeyNameAction.Add.ToString(), Description = "اضافه کردن دسترسی ها به نقش" },
                new Permission { Id=9, Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.RolePermission.ToString(), Action =  KeyNameAction.Edit.ToString(), Description = "ریست کردن و اختصاص دادن دوباره دسترسی های به نقش" },
                new Permission { Id=10, Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.RolePermission.ToString(), Action = KeyNameAction.Delete.ToString(), Description = "ریمو کردن دسترسی ها از نقش" },
                                 
                //UserRole       
                new Permission { Id=11, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.UserRole.ToString(), Action = KeyNameAction.View.ToString(), Description = "دیدن نقش های اختصاص داده شده به یک کاربر" },
                new Permission { Id=12, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.UserRole.ToString(), Action = KeyNameAction.Add.ToString(), Description = "اختصاص دادن نقش ها به یک کاربر" },
                new Permission { Id=13, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.UserRole.ToString(), Action = KeyNameAction.Edit.ToString(), Description = "ریست کردن و اختصاص دادن دوباره نقش ها به کاربر" },
                new Permission { Id=14, Area = KeyNameArea.Admin.ToString(), Controller =  KeyNameController.UserRole.ToString(), Action = KeyNameAction.Delete.ToString(), Description =  "برداشتن نقش ها از یک کاربر" },
                                 
                //RoleUser       
                new Permission { Id=15, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.RoleUser.ToString(), Action = KeyNameAction.View.ToString(), Description = "دیدن کاربران موجود در نقش ها" },
                                 
                // Categories    
                new Permission { Id=16, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.View.ToString(), Description = "مشاهده دسته بندی ها" },
                new Permission { Id=17, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.Add.ToString(), Description = "ایجاد دسته بندی" },
                new Permission { Id=18, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.Edit.ToString(), Description = "ویرایش دسته بندی" },
                new Permission { Id=19, Area = KeyNameArea.Admin.ToString(), Controller = KeyNameController.Category.ToString(), Action =  KeyNameAction.Delete.ToString(), Description = "حذف دسته بندی" }
            };

            //Insert new permissions in list if does exist in db
            var existingPermissionIdsInDb = await context.Permissions.Select(p => p.Id).ToListAsync();
            var newPermissionsForInsert = permissions.Where(p => !existingPermissionIdsInDb.Contains(p.Id)).ToList();
            if (newPermissionsForInsert.Any())
            {
                await context.Permissions.AddRangeAsync(newPermissionsForInsert);
                await context.SaveChangesAsync();
            }


            //// Add Roles
            //if (!await context.Roles.AnyAsync())
            //{
            //    var role = new Role { Name = "Admin", Description = "ادمین" };
            //    await roleManager.CreateAsync(role);
            //}

            //// add Permissions to Roles
            //var adminRole = await roleManager.FindByNameAsync("Admin");
            //if (adminRole != null)
            //{
            //    var adminPermissions = await context.Permissions.ToListAsync();
            //    foreach (var permission in adminPermissions)
            //    {
            //        var exists = await context.RolePermissions
            //            .AnyAsync(rp => rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id);

            //        if (!exists)
            //        {
            //            context.RolePermissions.Add(new RolePermission
            //            {
            //                RoleId = adminRole.Id,
            //                PermissionId = permission.Id
            //            });
            //        }
            //    }
            //    await context.SaveChangesAsync();
            //}
        }
    }
}
