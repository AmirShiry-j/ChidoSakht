using Application.CategoryService;
using Application.Interfaces.AppKeyNames;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filters.Permissions;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    [PermissionAuthorize(KeyNameController.User, KeyNameAction.View, KeyNameArea.Admin)]
    public class UserController : ControllerBase
    {
        private readonly IFacadeUserService _facadeUserService;
        public UserController(IFacadeUserService facadeUserService)
        {
            _facadeUserService = facadeUserService;
        }

        /// <summary>
        /// برگردوندن لیست تمام کاربران (موقت) (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _facadeUserService.GetAllUserService.Execute();

            return Ok(users);
        }
    }
}
