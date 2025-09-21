using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.AddressService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class CityController : ControllerBase
    {
        private readonly IFacadeAddressService _facadeAddressService;
        private readonly ILocalizationService _localizationService;
        public CityController(IFacadeAddressService facadeAddressService, ILocalizationService localizationService)
        {
            _facadeAddressService = facadeAddressService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن لیست شهرستان های یک استان
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <returns></returns>
        [HttpGet("{ProvinceId}")]
        public async Task<IActionResult> Get(int ProvinceId)
        {
            //Get data from service
            var resultService = await _facadeAddressService.AddressQueriesService.GetCitiesByProvinceId(ProvinceId);
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
