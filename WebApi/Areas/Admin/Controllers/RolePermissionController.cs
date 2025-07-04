using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.PermissionService;
using Application.Store.AdminSection.PermissionService.Commands;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Permissions;
using WebApi.Areas.Admin.ModelsAndDtoes.Roles;
using WebApi.Filters.Permissions;
using WebApi.ModelsAndDtoes.Categories;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[Controller]/")]
    [Authorize]
    public class RolePermissionController : ControllerBase
    {
        private readonly IFacadePermissionService _FacadePermissionService;
        private readonly ILocalizationService _localizationService;
        public RolePermissionController(IFacadePermissionService FacadePermissionService, ILocalizationService localizationService)
        {
            _FacadePermissionService = FacadePermissionService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن همه نقش ها با دسترسی های اختصاص داده شده بهشون (Auth)
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.RolePermission, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            //Map
            var resultService = await _FacadePermissionService.GetAssignedPermissionsInAllRoleService.Execute();

            if (resultService.IsSuccess)
            {
                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برگردوندن دسترسی های اختصاص داده شده به یک نقش (Auth)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>

        [PermissionAuthorize(KeyNameController.RolePermission, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{RoleId}")]
        public async Task<ActionResult> Get(string RoleId)
        {
            //Map
            var resultService = await _FacadePermissionService.GetAssignedPermissionsInARoleService.Execute(RoleId);

            //return result
            if (resultService.IsSuccess)
            {
                return Ok(resultService.Data);
            }
            else
            {
                if (resultService.MessageEventType.HasValue && resultService.MessageEventType.Value.Equals(MessageEventType.NotFound))
                {
                    return NotFound(resultService.Message);
                }
                else
                {
                    return BadRequest(resultService.Message);
                }
            }
        }

        /// <summary>
        /// اضافه کردن دسترسی ها به نقش (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.RolePermission, KeyNameAction.Add, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<ActionResult> Post(RolePermissionsApiDto Dto)
        {
            //Map
            var resultService = await _FacadePermissionService.AssignPermissionsService.Execute(new RolePermissionsDto
            {
                PermissionIds = Dto.PermissionIds,
                RoleId = Dto.RoleId,
            });

            //return result
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType.HasValue && resultService.MessageEventType.Value.Equals(MessageEventType.NotFound))
                {
                    return NotFound(resultService.Message);
                }
                else
                {
                    return BadRequest(resultService.Message);
                }
            }
        }

        /// <summary>
        /// ریست کردن و اختصاص دادن دوباره دسترسی ها به نقش (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.RolePermission, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut]
        public async Task<ActionResult> Put(RolePermissionsApiDto Dto)
        {
            //Map
            var resultService = await _FacadePermissionService.ResetAndAssignPermissionsService.Execute(new RolePermissionsDto
            {
                PermissionIds = Dto.PermissionIds,
                RoleId = Dto.RoleId,
            });

            //return result
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType.HasValue && resultService.MessageEventType.Value.Equals(MessageEventType.NotFound))
                {
                    return NotFound(resultService.Message);
                }
                else
                {
                    return BadRequest(resultService.Message);
                }
            }
        }

        /// <summary>
        /// ریمو کردن دسترسی ها از نقش (Auth)
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.RolePermission, KeyNameAction.Delete, KeyNameArea.Admin)]
        [HttpDelete]
        public async Task<ActionResult> Delete(RolePermissionsApiDto Dto)
        {
            //Map
            var resultService = await _FacadePermissionService.UnAssignPermissionsService.Execute(new RolePermissionsDto
            {
                PermissionIds = Dto.PermissionIds,
                RoleId = Dto.RoleId,
            });

            //return result
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType.HasValue && resultService.MessageEventType.Value.Equals(MessageEventType.NotFound))
                {
                    return NotFound(resultService.Message);
                }
                else
                {
                    return BadRequest(resultService.Message);
                }
            }
        }
    }
}
