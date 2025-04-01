using Application.Interfaces.AppKeyNames;
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
using WebApi.Areas.Admin.ModelsAndDtoes.Roles;
using WebApi.Filters.Permissions;
using WebApi.ModelsAndDtoes.Common;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILocalizationService _localization;
        public RoleController(RoleManager<Role> roleManager, ILocalizationService localization)
        {
            _roleManager = roleManager;
            _localization = localization;
        }

        /// <summary>
        /// برگردوندن لیست نقش ها در سایت (Auth)
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Role, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get role list
            var roles = _roleManager.Roles.Select(p => new RoleDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                //HATEOAS links
                Links = new List<Link>
                {
                    new Link
                    {
                        For="Details",
                        HttpMethod=HttpMethod.Get.ToString(),
                        Url=Url.Action(nameof(Get),"Role",new {RoleId=p.Id, Area="Admin" },Request.Scheme)
                    },
                    new Link
                    {
                        For="Edit",
                        HttpMethod=HttpMethod.Put.ToString(),
                        Url=Url.Action(nameof(Update),"Role",new {Area="Admin" },Request.Scheme)
                    },
                    new Link
                    {
                        For="Delete",
                        HttpMethod=HttpMethod.Delete.ToString(),
                        Url=Url.Action(nameof(Delete),"Role",new {RoleId=p.Id, Area="Admin" },Request.Scheme)
                    },
                }
            }).ToList();

            return Ok(roles);
        }

        /// <summary>
        /// برگشت یک نقش سایت به وسیله آیدی اون (Auth)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Role, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{RoleId}")]
        public async Task<IActionResult> Get(string RoleId)
        {
            //Get one role by id
            var role = await _roleManager.FindByIdAsync(RoleId);

            //Map to model
            RoleDto model = null;
            if (role != null)
            {
                model = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    //HATEOAS links
                    Links = new List<Link>
                    {
                        new Link
                        {
                            For="Details",
                            HttpMethod=HttpMethod.Get.ToString(),
                            Url=Url.Action(nameof(Get),"Role",new {RoleId=role.Id, Area="Admin" },Request.Scheme)
                        },
                        new Link
                        {
                            For="Edit",
                            HttpMethod=HttpMethod.Put.ToString(),
                            Url=Url.Action(nameof(Update),"Role",new {Area="Admin" },Request.Scheme)
                        },
                        new Link
                        {
                            For="Delete",
                            HttpMethod=HttpMethod.Delete.ToString(),
                            Url=Url.Action(nameof(Delete),"Role",new {RoleId=role.Id, Area="Admin" },Request.Scheme)
                        },
                    }
                };
            }

            return Ok(model);
        }

        /// <summary>
        /// اضافه کردن نقش جدید به سایت (Auth)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Role, KeyNameAction.Add, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto model)
        {
            //Map to Role
            var newRole = new Role
            {
                Name = model.Name,
                Description = model.Description
            };

            //Add Role
            var resultAddRole = await _roleManager.CreateAsync(newRole);

            if (resultAddRole.Succeeded)
            {
                //HATEOAS link for new item
                var link = Url.Action(nameof(Get), "Role", new { RoleId = newRole.Id, Area = "Admin" }, protocol: Request.Scheme);

                return Created(link, null);
            }
            else
            {
                //Return error
                var error = resultAddRole.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }

        /// <summary>
        /// ویرایش اطلاعات نقش  (Auth)
        /// </summary>
        /// <param name="model"></param>
        [PermissionAuthorize(KeyNameController.Role, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update(EditRoleDto model)
        {
            //Find role by id
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ////Create new role for not founding role 
                //Map to Role
                var newRole = new Role
                {
                    Name = model.Name,
                    Description = model.Description
                };

                //Create new role
                var resultCreate = await _roleManager.CreateAsync(newRole);

                //HATEOAS link for new role
                var link = Url.Action(nameof(Get), "Role", new { RoleId = newRole.Id, Area = "Admin" }, protocol: Request.Scheme);

                return Created(link, null);
            }

            //Initail edits
            role.Name = model.Name;
            role.Description = model.Description;

            //Update role
            var resultEdit = await _roleManager.UpdateAsync(role);

            if (resultEdit.Succeeded)
            {
                return Ok();
            }
            else
            {
                //Return error
                var error = resultEdit.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }

        /// <summary>
        /// حذف یک نقش از سایت (Auth)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Role, KeyNameAction.Delete, KeyNameArea.Admin)]
        [HttpDelete("{RoleId}")]
        public async Task<IActionResult> Delete(string RoleId)
        {
            //Find role
            var role = await _roleManager.FindByIdAsync(RoleId);

            //Check exist role
            if (role == null)
            {
                return NotFound();
            }

            //Delete Role
            var resultDeleteRole = await _roleManager.DeleteAsync(role);

            if (resultDeleteRole.Succeeded)
            {
                return Ok();
            }
            else
            {
                //Return error
                var error = resultDeleteRole.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }
        }
    }
}
