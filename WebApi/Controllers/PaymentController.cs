using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.CartService;
using Application.Store.UserSection.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IFacadePaymentService _facadePaymentService;
        private readonly ILocalizationService _localizationService;
        public PaymentController(IFacadePaymentService facadePaymentService, ILocalizationService localizationService)
        {
            _facadePaymentService = facadePaymentService;
            _localizationService = localizationService;
        }

        [HttpPost("{OrderId}")]
        [Authorize]
        public async Task<IActionResult> Post([Required] long OrderId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadePaymentService.PaymentCommandsService.PayingForAnOrder(userId, OrderId);

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
    }
}
