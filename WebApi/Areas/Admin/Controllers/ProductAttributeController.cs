using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute;
using Application.ProductAttribute.Commands;
using Application.ProductVariant;
using Domain.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IFacadeProductAttributeService _facadeProductAttributeService;
        private readonly ILocalizationService _localizationService;
        public ProductAttributeController(IFacadeProductAttributeService facadeProductAttributeService, ILocalizationService localizationService)
        {
            _facadeProductAttributeService = facadeProductAttributeService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// ایجاد یک خصوصیت جدید (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductAttributeApiDto dto)
        {
            var inputModel = new CreateProductAttributeDto
            {
                Name = dto.Name,
                ProductId = dto.ProductId,
                AttributeType = (AttributeType)dto.AttributeType
            };
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.Create(inputModel);
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


        /// <summary>
        /// ایجاد یک خصوصیت جدید (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProductAttributeApiDto dto)
        {
            var inputModel = new UpdateProductAttributeDto
            {
                Name = dto.Name
            };
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.Update(inputModel);
            if (resultService.IsSuccess)
            {
                return Ok();
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
