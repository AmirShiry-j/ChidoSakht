using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class ProvinceController : ControllerBase
    {
        private readonly IFacadeAddressService _facadeAddressService;
        private readonly ILocalizationService _localizationService;
        public ProvinceController(IFacadeAddressService facadeAddressService, ILocalizationService localizationService)
        {
            _facadeAddressService = facadeAddressService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن لیست استان ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get data from service
            var resultService = await _facadeAddressService.AddressQueriesService.GetProvinces();
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
