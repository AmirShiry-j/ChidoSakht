using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.CartService;
using Application.Store.UserSection.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class CartController : ControllerBase
    {
        private readonly IFacadeCartService _facadeCartService;
        private readonly ILocalizationService _localizationService;
        public CartController(IFacadeCartService facadeCartService, ILocalizationService localizationService)
        {
            _facadeCartService = facadeCartService;
            _localizationService = localizationService;
        }


    }
}
