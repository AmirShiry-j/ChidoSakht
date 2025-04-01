using Application.Interfaces.ConfigService;
using Domain.Users;
using Infrastructure.ConfigService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Contexts;
using System.Security.Claims;

namespace WebApi.Filters.Permissions
{
    public class PermissionAuthorizeAttribute : TypeFilterAttribute
    {
        public PermissionAuthorizeAttribute(string controller, string action, string? area = null)
            : base(typeof(PermissionAuthorizeFilter))
        {
            Arguments = new object[] { area, controller, action };
        }
    }

    public class PermissionAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly DataBaseContext _context;
        private readonly IConfigService _configService;
        private readonly string? _area;
        private readonly string _controller;
        private readonly string _action;

        public PermissionAuthorizeFilter(
            UserManager<User> userManager,
            DataBaseContext context,
             IConfigService configService,
            string? area, string controller, string action)
        {
            _userManager = userManager;
            _context = context;
            _configService=configService;
            _area = area;
            _controller = controller;
            _action = action;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Take user
            var userId = context.HttpContext.User.FindFirst("UserId")?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                context.Result = new ForbidResult();  // 403
                return;
            }

            //Check user is Memeber of Super Admins
            if (_configService.Config.SuperAdmins.UserNames.Contains(user.UserName))//if User Is SuperAdmin
                return;//So he has permission

            //Get User's Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            //Check Permisson
            var hasPermission = await _context.RolePermissions
                .AnyAsync(rp =>
                    userRoles.Contains(rp.Role.Name) &&
                    rp.Permission.Area == _area &&
                    rp.Permission.Controller == _controller &&
                    rp.Permission.Action == _action
                );

            if (!hasPermission)
            {
                context.Result = new ForbidResult(); // 403
            }
        }
    }
}
