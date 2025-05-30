using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute;
using Application.ProductVariant;
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


        [HttpPost]
        public async Task<IActionResult> Post(CreateProductAttributeApiDto dto)
        {
            //var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.Upsert(dto.ProductId, dto.Name);
            //if (resultService.IsSuccess)
            //{
            //    if (resultService.MessageEventType == MessageEventType.Created)
            //    {
            //        return CreatedAtAction(nameof(Get), new { ProductId = resultService.Data }, null);
            //    }
            //    else//Updated
            //    {
                    return NoContent();
            //    }
            //}
            //else
            //{
            //    if (resultService.MessageEventType == MessageEventType.NotFound)
            //        return NotFound();
            //    else //bad request
            //        return BadRequest(resultService.Message);
            //}
        }
    }
}
