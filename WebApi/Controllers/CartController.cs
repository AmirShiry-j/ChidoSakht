using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.CartService;
using Application.Store.UserSection.CommentService.Commands;
using Application.Store.UserSection.CommentService.Queries;
using Application.Store.UserSection.ProductService;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;
using WebApi.Filters.Permissions;
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

        /// <summary>
        /// برگردوندن سبد خرید (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] GetCartFilterApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadeCartService.CartQueriesService.GetCartDetails(userId, (Domain.Carts.CartType)dto.CartType);

            return Ok(resultService.Data);
        }


        /// <summary>
        /// اضافه کردن یک آیتم به سبد خرید (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(AddItemToCartApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var resultService = await _facadeCartService.CartCommandsService.AddItemToCard(userId, dto.ProductId, dto.ProductVariantId);
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

        /// <summary>
        /// حذف یک آیتم از سبد خرید (Auth)
        /// </summary>
        /// <param name="CartItemId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{CartItemId}")]
        public async Task<IActionResult> Delete(long CartItemId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var resultService = await _facadeCartService.CartCommandsService.DeleteItemInCard(userId, CartItemId);
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// تغییر تعداد یک آیتم (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(UpdateQuantityAnItem))]
        public async Task<IActionResult> UpdateQuantityAnItem(UpdateQuantityOfItemInCartApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map
            var inputService = new UpdateQuantityOfItemInCartDto
            {
                CartItemId = dto.CartItemId,
                Behavior = dto.Behavior,
                Quantity = dto.Quantity
            };

            //run command
            var resultService = await _facadeCartService.CartCommandsService.UpdateQuantityAnItem(userId, inputService);
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// انتقال یک آیتم به سبد خرید بعدی یا فعلی (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(MoveAnItemInACartToAnotherCart))]
        public async Task<IActionResult> MoveAnItemInACartToAnotherCart(MoveAnItemInCartToAnotherCartApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map
            var inputService = new MoveAnItemInCartToAnotherCartDto
            {
                CartItemId = dto.CartItemId,
                MoveToCartType = (Domain.Carts.CartType)dto.MoveToCartType,
            };

            //run command
            var resultService = await _facadeCartService.CartCommandsService.MoveAnItemInACartToAnotherCart(userId, inputService);
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }

        //Update prices in the cart based on the current status with user approval
        /// <summary>
        /// به‌روزرسانی قیمت‌ها در سبد خرید بر اساس وضعیت فعلی با تأیید کاربر (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut($"{nameof(UpdatePricesInCartWithUserApproval)}/" + "{CartId}")]
        public async Task<IActionResult> UpdatePricesInCartWithUserApproval(long CartId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //run command
            var resultService = await _facadeCartService.CartCommandsService.UpdatePricesInCartWithUserApproval(userId, CartId);
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }

        //Update the number of items in the cart based on the current status with user approval
        /// <summary>
        /// تعداد اقلام موجود در سبد خرید را بر اساس وضعیت فعلی با تأیید کاربر به‌روزرسانی کنید (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut($"{nameof(UpdateNumberOfItemsWithUserApproval)}/" + "{CartId}")]
        public async Task<IActionResult> UpdateNumberOfItemsWithUserApproval(long CartId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //run command
            var resultService = await _facadeCartService.CartCommandsService.UpdateNumberOfItemsWithUserApproval(userId, CartId);
            if (resultService.IsSuccess)
            {
                return NoContent();
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
