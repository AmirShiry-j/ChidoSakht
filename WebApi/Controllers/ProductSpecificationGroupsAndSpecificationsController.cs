using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductSpecificationService;
using Application.Store.UserSection.ProductSpecificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters.Permissions;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class ProductSpecificationGroupsAndSpecificationsController : ControllerBase
    {
        private readonly IFacadeProductSpecificationService _facadeProductSpecificationService;
        private readonly ILocalizationService _localizationService;
        public ProductSpecificationGroupsAndSpecificationsController(IFacadeProductSpecificationService facadeProductSpecificationService, ILocalizationService localizationService)
        {
            _facadeProductSpecificationService = facadeProductSpecificationService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن همه گروه های اطلاعات با مشخصه هاشون (کاربری)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeProductSpecificationService.ProductSpecificationQueriesService.GetSpecGroupsWihtSpecsAsync(ProductId);

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