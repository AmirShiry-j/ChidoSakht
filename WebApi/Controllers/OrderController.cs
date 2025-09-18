using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.OrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class OrderController : ControllerBase
    {
        private readonly IFacadeOrderService _facadeOrderService;
        private readonly ILocalizationService _localizationService;
        public OrderController(IFacadeOrderService facadeOrderService, ILocalizationService localizationService)
        {
            _facadeOrderService = facadeOrderService;
            _localizationService = localizationService;
        }
    }
}
