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

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/Role/{RoleId}/User/{UserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
        /// اختصاص دادن یک نقش به کاربر (Auth)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(string RoleId, string UserId)
        {
            //Find role
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                return NotFound(_localization.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString()));
            }

            //Find user
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound(_localization.GetMessageAccount(MessageKeysAccount.UserIdNotFound.ToString()));
            }

            //Add role to user
            var resultAddRoleToUser = await _userManager.AddToRoleAsync(user, role.Name);
            if (resultAddRoleToUser.Succeeded)
            {
                return Ok();
            }
            else
            {
                //Return error
                var error = resultAddRoleToUser.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }

        /// <summary>
        /// برداشتن یک نقش از کاربر (Auth)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string RoleId, string UserId)
        {
            //Find role
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                return NotFound(_localization.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString()));
            }

            //Find user
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound(_localization.GetMessageAccount(MessageKeysAccount.UserIdNotFound.ToString()));
            }

            //Check how many Admin is exist
            if (role.Name == "Admin" &&
                _userManager.GetUsersInRoleAsync("Admin").Result.Count == 1 &&
                _userManager.IsInRoleAsync(user, "Admin").Result)
            {
                //Return error for cant delete only admin in site
                return BadRequest(_localization.GetMessagePermission(MessageKeysPermission.AtleastOneAdmin.ToString()));
            }

            //Remove role to user
            var resultRemoveRoleToUser = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (resultRemoveRoleToUser.Succeeded)
            {
                return Ok();
            }
            else
            {
                //Return error
                var error = resultRemoveRoleToUser.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }
    }
}
