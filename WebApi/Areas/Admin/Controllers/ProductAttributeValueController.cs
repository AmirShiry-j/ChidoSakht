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
        /// برگردوندن همه مقادیر یک خصوصیت (Auth)
        /// </summary>
        /// <param name="ProductAttributeId"></param>
        /// <returns></returns>
        [HttpGet("{ProductAttributeId}")]
        public async Task<IActionResult> Get(int ProductAttributeId)
        {
            //Get by service
            var resultService = await _facadeProductAttributeService.ProductAttributeQueriesService.GetProductAttributeValuesByProductAttributeId(ProductAttributeId);

            //HATEAOS
            if (resultService.IsSuccess)
            {
                return Ok(resultService.Data);
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                return
                    BadRequest(resultService.Message);
            }
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

        /// <summary>
        /// ویرایش یکی از مقادیر خصوصیت (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProductAttributeValueApiDto dto)
        {
            var inputModel = new UpdateProductAttributeValueDto
            {
                Value = dto.Value,
                ProductAttributeValueId = dto.ProductAttributeValueId
            };
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.UpdateAValueForAtribute(inputModel);
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
        /// حذف یک مقدار از خصوصیت (Auth)
        /// </summary>
        /// <param name="ProductAttributeValueId"></param>
        /// <returns></returns>
        [HttpDelete("{ProductAttributeValueId}")]
        public async Task<IActionResult> Delete(int ProductAttributeValueId)
        {
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.DeleteValueForAtribute(ProductAttributeValueId);
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
