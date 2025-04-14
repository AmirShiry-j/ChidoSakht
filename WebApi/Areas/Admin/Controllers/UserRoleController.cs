using Application.Common.AppKeyNames;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Areas.Admin.ModelsAndDtoes.Permissions;
using WebApi.Filters.Permissions;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[Controller]")]
    [Authorize]
    public class UserRoleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILocalizationService _localization;
        public UserRoleController(UserManager<User> userManager, RoleManager<Role> roleManager, ILocalizationService localization)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localization = localization;
        }

        /// <summary>
        /// اختصاص دادن نقش ها به یک کاربر (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.UserRole, KeyNameAction.Add, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post(UserRolesApiDto Dto)
        {
            //Find user
            var user = await _userManager.FindByIdAsync(Dto.UserId);
            if (user is null)
            {
                return NotFound(_localization.GetMessageAccount(MessageKeysAccount.UserIdNotFound.ToString()));
            }

            //Find roles
            var notFoundRoleIds = new List<string>();
            var roles = new List<Role>();
            {
                foreach (var roleId in Dto.RoleIds)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);
                    if (role is not null)
                        roles.Add(role);
                    else
                        notFoundRoleIds.Add(roleId);
                }
                if (notFoundRoleIds.Any())
                {
                    var str = notFoundRoleIds.Select(p => "'" + p + "'").Aggregate((p1, p2) => p1 + "," + p2);
                    return NotFound(string.Format(_localization.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString()), str));
                }
            }

            //Add role to user
            foreach (var role in roles)
            {
                var resultAddRoleToUser = await _userManager.AddToRoleAsync(user, role.Name);
            }

            return NoContent();
        }

        /// <summary>
        /// برداشتن نقش ها از یک کاربر (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.UserRole, KeyNameAction.Delete, KeyNameArea.Admin)]
        [HttpDelete]
        public async Task<IActionResult> Delete(UserRolesApiDto Dto)
        {
            //Find user
            var user = await _userManager.FindByIdAsync(Dto.UserId);
            if (user == null)
            {
                return NotFound(_localization.GetMessageAccount(MessageKeysAccount.UserIdNotFound.ToString()));
            }

            //Find roles
            var notFoundRoleIds = new List<string>();
            var roles = new List<Role>();
            {
                foreach (var roleId in Dto.RoleIds)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);
                    if (role is not null)
                        roles.Add(role);
                    else
                        notFoundRoleIds.Add(roleId);
                }
                if (notFoundRoleIds.Any())
                {
                    var str = notFoundRoleIds.Select(p => "'" + p + "'").Aggregate((p1, p2) => p1 + "," + p2);
                    return NotFound(string.Format(_localization.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString()), str));
                }
            }

            //Remove role to user
            foreach (var role in roles)
            {
                var resultAddRoleToUser = await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            return NoContent();
        }
    }
}
