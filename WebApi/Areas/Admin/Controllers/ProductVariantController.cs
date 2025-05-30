using Application.Interfaces.Localization;
using Application.ProductService;
using Application.ProductVariant;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class ProductVariantController : ControllerBase
    {
        private readonly IFacadeProductVariantService _facadeProductVariantService;
        private readonly ILocalizationService _localizationService;
        public ProductVariantController(IFacadeProductVariantService facadeProductVariantService, ILocalizationService localizationService)
        {
            _facadeProductVariantService = facadeProductVariantService;
            _localizationService = localizationService;
        }
    }
}
