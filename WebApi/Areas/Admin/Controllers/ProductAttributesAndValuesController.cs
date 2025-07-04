using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductAttribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters.Permissions;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class ProductAttributesAndValuesController : ControllerBase
    {
        private readonly IFacadeProductAttributeService _facadeProductAttributeService;
        private readonly ILocalizationService _localizationService;
        public ProductAttributesAndValuesController(IFacadeProductAttributeService facadeProductAttributeService, ILocalizationService localizationService)
        {
            _facadeProductAttributeService = facadeProductAttributeService;
            _localizationService = localizationService;
        }



        /// <summary>
        /// برگردوندن همه خصوصیات یک محصول به همراه مقادیرشون (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeProductAttributeService.ProductAttributeQueriesService.GetProductAttributesWithValuesByProductId(ProductId);

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
