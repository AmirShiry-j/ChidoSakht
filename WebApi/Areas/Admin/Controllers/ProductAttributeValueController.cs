using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute;
using Application.ProductAttribute.Commands;
using Domain.Products;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class ProductAttributeValueController : ControllerBase
    {
        private readonly IFacadeProductAttributeService _facadeProductAttributeService;
        private readonly ILocalizationService _localizationService;
        public ProductAttributeValueController(IFacadeProductAttributeService facadeProductAttributeService, ILocalizationService localizationService)
        {
            _facadeProductAttributeService = facadeProductAttributeService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// اضافه کردن یک مقدار به خصوصیت (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateValueForAttributeApiDto dto)
        {
            var inputModel = new CreateValueForAttributeDto
            {
                Value = dto.Value,
                ProductAttributeId=dto.ProductAttributeId
            };
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.AddValueForAtribute(inputModel);
            if (resultService.IsSuccess)
            {
                //return CreatedAtAction(nameof(Get), new { ProductAttributeId = resultService.Data }, null);
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
