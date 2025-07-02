using Application.Common.AppKeyNames;
using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute;
using Application.ProductAttribute.Commands;
using Application.ProductVariant;
using Domain.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;
using WebApi.Filters.Permissions;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
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
        /// برگردوندن همه خصوصیات یک محصول (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeProductAttributeService.ProductAttributeQueriesService.GetProductAttributesByProductId(ProductId);

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
        /// ایجاد یک خصوصیت جدید (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductAttributeApiDto dto)
        {
            var inputModel = new CreateProductAttributeDto
            {
                Name = dto.Name,
                ProductId = dto.ProductId,
                AttributeType = (AttributeType)dto.AttributeType,
                UseForVariant = dto.UseForVariant
            };
            var resultService = await _facadeProductAttributeService.ProductAttributeCommandsService.CreateAttrbite(inputModel);
            if (resultService.IsSuccess)
            {
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
        /// ویرایش یک خصوصیت (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProductAttributeApiDto dto)
        {
            var inputModel = new UpdateProductAttributeDto
            {
                Name = dto.Name,
                ProductAttributeId = dto.ProductAttributeId,
                UseForVariant = dto.UseForVariant
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
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
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
