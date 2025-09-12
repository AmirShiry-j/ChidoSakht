using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.CommentService.Queries;
using Application.Store.UserSection.AddressService;
using Application.Store.UserSection.AddressService.Commands;
using Application.Store.UserSection.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Address;
using WebApi.ModelsAndDtoes.Cart;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class AddressController : ControllerBase
    {
        private readonly IFacadeAddressService _facadeAddressService;
        private readonly ILocalizationService _localizationService;
        public AddressController(IFacadeAddressService facadeAddressService, ILocalizationService localizationService)
        {
            _facadeAddressService = facadeAddressService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن آدرس های ثبت شده برای یک کاربر (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadeAddressService.AddressQueriesService.GetAddressesByUserId(userId);

            return Ok(resultService.Data);
        }

        /// <summary>
        /// برگردوندن اطلاعات ثبت شده برای یک آدرس (Auth)
        /// </summary>
        /// <param name="AddressId"></param>
        /// <returns></returns>
        [HttpGet("{AddressId}")]
        public async Task<IActionResult> Get(int AddressId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get data from service
            var resultService = await _facadeAddressService.AddressQueriesService.GetAddressByAddressId(userId, AddressId);
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
        /// ثبت یک آدرس جدید (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateAddressApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //map
            var inputService = new CreateAddressDto
            {
                Name = dto.Name
            };

            //run service
            var resultService = await _facadeAddressService.AddressCommandsService.Create(userId, inputService);
            if (resultService.IsSuccess)
            {
                return CreatedAtAction(nameof(Get), new { AddressId = resultService.Data }, null);
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
        /// ویرایش یک آدرس (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateQuantityAnItem(UpdateAddressApiDto dto)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Map
            var inputService = new UpdateAddressDto
            {
                Name = dto.Name,
                AddressId = dto.AddressId
            };

            //run command
            var resultService = await _facadeAddressService.AddressCommandsService.Update(userId, inputService);
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
        /// حذف یک آدرس (Auth)
        /// </summary>
        /// <param name="AddressId"></param>
        /// <returns></returns>
        [HttpDelete("{AddressId}")]
        public async Task<IActionResult> Delete(int AddressId)
        {
            //get userid
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //run service
            var resultService = await _facadeAddressService.AddressCommandsService.Delete(userId, AddressId);
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
