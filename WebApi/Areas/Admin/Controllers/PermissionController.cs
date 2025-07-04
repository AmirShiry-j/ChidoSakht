using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Store.AdminSection.PermissionService;
using Application.Store.AdminSection.PermissionService.Queries;
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
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [PermissionAuthorize(KeyNameController.Permission, KeyNameAction.View, KeyNameArea.Admin)]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private readonly IFacadePermissionService _FacadePermissionService;
        private readonly ILocalizationService _localizationService;
        public PermissionController(IFacadePermissionService FacadePermissionService, ILocalizationService localizationService)
        {
            _FacadePermissionService = FacadePermissionService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن لیست دسترسی ها (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PermissionDto>>> Get(string? Area, string? Controller, string? Action)
        {
            //get from service
            var resultService = await _FacadePermissionService.GetPermissionsService.Execute(new PermissionFilterDto
            {
                Area = Area,
                Controller = Controller,
                Action = Action
            });

            if (resultService.IsSuccess)
            {
                if (resultService.Data is not null)
                {

                    return Ok(resultService.Data);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
