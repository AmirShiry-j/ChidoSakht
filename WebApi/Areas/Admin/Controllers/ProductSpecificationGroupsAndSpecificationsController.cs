using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductSpecificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters.Permissions;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class ProductSpecificationGroupsAndSpecificationsController : ControllerBase
    {
        private readonly IFacadeAdminProductSpecificationService _facadeAdminProductSpecificationService;
        private readonly ILocalizationService _localizationService;
        public ProductSpecificationGroupsAndSpecificationsController(IFacadeAdminProductSpecificationService facadeAdminProductSpecificationService, ILocalizationService localizationService)
        {
            _facadeAdminProductSpecificationService = facadeAdminProductSpecificationService;
            _localizationService = localizationService;
        }


        /// <summary>
        /// برگردوندن همه گروه های اطلاعات با مشخصه هاشون (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationQueriesService.GetSpecGroupsWihtSpecsAsync(ProductId);

            //HATEAOS
            if (resultService.IsSuccess)
            {
                return Ok(resultService.Data);
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                return
                    BadRequest(resultService.Message);
            }
        }
    }
}
