using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute;
using Application.ProductAttribute.Commands;
using Application.ProductVariant;
using Domain.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
        /// برگردوندن اطلاعات یک خصوصیت (Auth)
        /// </summary>
        /// <param name="ProductAttributeId"></param>
        /// <returns></returns>
        [HttpGet("{ProductAttributeId}")]
        public async Task<IActionResult> Get(int ProductAttributeId)
        {
            //Get by service
            var resultService = await _facadeProductAttributeService.ProductAttributeQueriesService.GetOnProductAttribute(ProductAttributeId);

            //HATEAOS
            if (resultService.IsSuccess)
            {

                resultService.Data.Links = new List<Link>
                    {
                        new Link
                        {
                            For="For Update",
                            HttpMethod=HttpMethod.Put.ToString(),
                            Url=Url.Action(nameof(Put),nameof(ProductAttributeController).Replace("Controller", ""),new { Area="Admin" },Request.Scheme)
                        },
                        new Link
                        {
                            For="For Delete",
                            HttpMethod=HttpMethod.Delete.ToString(),
                            Url=Url.Action(nameof(Delete),nameof(ProductAttributeController).Replace("Controller", ""),new { Area="Admin" , ProductAttributeId=resultService.Data.ProductAttributeId },Request.Scheme)
                        },
                    };

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
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.CreateAttrbite(inputModel);
            if (resultService.IsSuccess)
            {
                return CreatedAtAction(nameof(Get), new { ProductAttributeId = resultService.Data }, null);
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
        /// ویرایش یک خصوصیت (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProductAttributeApiDto dto)
        {
            var inputModel = new UpdateProductAttributeDto
            {
                Name = dto.Name,
                ProductAttributeId = dto.ProductAttributeId
            };
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.UpdateAttrbite(inputModel);
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
        /// حذف یک خصوصیت (Auth)
        /// </summary>
        /// <param name="ProductAttributeId"></param>
        /// <returns></returns>
        [HttpDelete("{ProductAttributeId}")]
        public async Task<IActionResult> Delete(int ProductAttributeId)
        {
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.DeleteAttrbite(ProductAttributeId);
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
