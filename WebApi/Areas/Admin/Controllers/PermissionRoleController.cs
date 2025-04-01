using Application.CategoryService;
using Application.CategoryService.Commands;
using Application.Common.AppKeyNames;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.PermissionService.Commands;
using Application.PermissionService.Queries;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Roles;
using WebApi.Filters.Permissions;
using WebApi.ModelsAndDtoes.Categories;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/Permission/{PermissionId}/Role/{RoleId}")]
    [Authorize]
    public class PermissionRoleController : ControllerBase
    {
        private readonly IFacadePermissionService _FacadePermissionService;
        private readonly ILocalizationService _localizationService;
        public PermissionRoleController(IFacadePermissionService FacadePermissionService, ILocalizationService localizationService)
        {
            _FacadePermissionService = FacadePermissionService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// اضافه کردن یک دسترسی به نقش (Auth)
        /// </summary>
        /// <param name="PermissionId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.PermissionRole, KeyNameAction.Add, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<ActionResult> Post(int PermissionId, string RoleId)
        {
            //Map
            var resultService = await _FacadePermissionService.AssignPermissionService.Execute(new RolePermissionDto
            {
                PermissionId = PermissionId,
                RoleId = RoleId,
            });

            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// ریمو کردن یک دسترسی از نقش (Auth)
        /// </summary>
        /// <param name="PermissionId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.PermissionRole, KeyNameAction.Delete, KeyNameArea.Admin)]
        [HttpDelete]
        public async Task<ActionResult> Delete(int PermissionId, string RoleId)
        {
            //Map
            var resultService = await _FacadePermissionService.UnAssignPermissionService.Execute(new RolePermissionDto
            {
                PermissionId = PermissionId,
                RoleId = RoleId,
            });

            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
