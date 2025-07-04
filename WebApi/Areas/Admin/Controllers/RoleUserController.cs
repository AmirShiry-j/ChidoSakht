using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Services.UserService.Queries;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Areas.Admin.ModelsAndDtoes.Roles;
using WebApi.Filters.Permissions;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[Controller]")]
    [Authorize]
    public class RoleUserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILocalizationService _localization;
        public RoleUserController(UserManager<User> userManager, RoleManager<Role> roleManager, ILocalizationService localization)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localization = localization;
        }

        /// <summary>
        /// برگردوندن همه نقش ها و کاربران موجود در اونها (Auth)
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.RoleUser, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            //get roles
            var roles = await _roleManager.Roles.Select(p => new RoleUserApiDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            }).ToListAsync();

            //get users in roles
            foreach (var role in roles)
            {
                role.Users = (await _userManager.GetUsersInRoleAsync(role.Name)).Select(p => new UserDto
                {
                    Id = p.Id,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    FullName = p.FullName
                }).ToList();
            }

            return Ok(roles);
        }

        /// <summary>
        /// برگردوندن کاربران موجود در یک نقش (Auth)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.RoleUser, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{RoleId}")]
        public async Task<ActionResult> Get(string RoleId)
        {
            //Find Role
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role is null)
            {
                return NotFound(_localization.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString()));
            }

            //get users in role
            var users = (await _userManager.GetUsersInRoleAsync(role.Name)).Select(p => new UserDto
            {
                Id = p.Id,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                FullName = p.FullName
            }).ToList();

            return Ok(users);
        }
    }
}
