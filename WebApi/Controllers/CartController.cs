using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.CartService;
using Application.Store.UserSection.CommentService.Commands;
using Application.Store.UserSection.CommentService.Queries;
using Application.Store.UserSection.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Cart;
using WebApi.ModelsAndDtoes.Product;

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] GetCartFilterApiDto dto)
        {
            //map
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadeCartService.CartQueriesService.GetCartDetails(userId, dto.CartStatus);

            return Ok(resultService.Data);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(AddItemToCartApiDto dto)
        {
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var resultService = await _facadeCartService.CartCommandsService.AddItemToCard(userId, dto.ProductId, dto.ProductVariantId, dto.CartStatus);
            if (resultService.IsSuccess)
            {
                return Created();
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }
    }
}
