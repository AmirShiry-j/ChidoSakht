using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.OrderService;
using Application.Store.UserSection.OrderService.Commands;
using Domain.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.ModelsAndDtoes.Cart;
using WebApi.ModelsAndDtoes.Orders;

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

        /// <summary>
        /// برگردوندن جزئیات یک سفارش (Auth)
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpGet("{OrderId}")]
        [Authorize]
        public async Task<IActionResult> Get([Required] long OrderId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadeOrderService.OrderQueriesService.GetOrderDetailsById(userId, OrderId);

            if (resultService.IsSuccess)
            {
                return Ok(resultService.Data);
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
        /// برگردوندن سفارش‌های یک کاربر (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] GetOrdersByFilterApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadeOrderService.OrderQueriesService.GetOrders(userId, dto.OrderStatus is null ? null : (Domain.Orders.OrderStatus)dto.OrderStatus);

            return Ok(resultService.Data);
        }


        /// <summary>
        /// ایجاد سفارش (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //define input
            var inputService = new CreateOrderDto
            {
                AddressId = dto.AddressId,
                CartId = dto.CartId,
                SendBy = (SendBy)dto.SendBy
            };

            var resultService = await _facadeOrderService.OrderCommandsService.CreateOrder(userId, inputService);
            if (resultService.IsSuccess)
            {
                return Created("", resultService.Data);
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }


        [HttpDelete("{OrderId}")]
        [Authorize]
        public async Task<IActionResult> Delete([Required] long OrderId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadeOrderService.OrderQueriesService.GetOrderDetailsById(userId, OrderId);

            if (resultService.IsSuccess)
            {
                return Ok(resultService.Data);
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
