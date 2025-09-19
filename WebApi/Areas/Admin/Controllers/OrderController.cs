using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.OrderService;
using Domain.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Areas.Admin.ModelsAndDtoes.Orders;
using WebApi.ModelsAndDtoes.Orders;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IFacadeAdminOrderService _facadeAdminOrderService;
        private readonly ILocalizationService _localizationService;
        public OrderController(IFacadeAdminOrderService facadeAdminOrderService, ILocalizationService localizationService)
        {
            _facadeAdminOrderService = facadeAdminOrderService;
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
            //Get data from service
            var resultService = await _facadeAdminOrderService.OrderQueriesService.GetOrderDetailsById(OrderId);

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
        /// برگردوندن سفارش‌ها (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] GetOrdersByFilterAdminApiDto dto)
        {
            //Get data from service
            var resultService = await _facadeAdminOrderService.OrderQueriesService.GetOrders(dto.UserId, dto.OrderStatus is null ? null : (Domain.Orders.OrderStatus)dto.OrderStatus);

            return Ok(resultService.Data);
        }

        /// <summary>
        /// تغییر وضعیت یک سفارش (Auth)
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpPut("{OrderId}")]
        [Authorize]
        public async Task<IActionResult> Put([Required] long OrderId, [Required] OrderStatusApiEnum orderStatus)
        {
            //Get data from service
            var resultService = await _facadeAdminOrderService.OrderCommandsService.UpdateStatusOfAnOrder(OrderId, (OrderStatus)orderStatus);

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
