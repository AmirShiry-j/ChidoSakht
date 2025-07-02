using Application.Common.AppKeyNames;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.ProductAttribute.Commands;
using Application.ProductService;
using Application.ProductVariant;
using Application.ProductVariant.Commands;
using Microsoft.AspNetCore.Authorization;
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
    public class ProductVariantController : ControllerBase
    {
        private readonly IFacadeProductVariantService _facadeProductVariantService;
        private readonly ILocalizationService _localizationService;
        public ProductVariantController(IFacadeProductVariantService facadeProductVariantService, ILocalizationService localizationService)
        {
            _facadeProductVariantService = facadeProductVariantService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن همه واریانت های یک محصول (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeProductVariantService.ProductVariantQueriesService.GetProductVariantsByProductId(ProductId);

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
        /// اضافه کردن یک واریانت (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Add, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductVariantApiDto dto)
        {
            var inputModel = new CreateProductVariantDto
            {
                Price = dto.Price,
                ProductAttributeValueIds = dto.ProductAttributeValueIds,
                ProductId = dto.ProductId,
                SpecialPrice = dto.SpecialPrice,
                Stock = dto.Stock,
                Height = dto.Height,
                Length = dto.Length,
                Weight = dto.Weight,
                Width = dto.Width,
                ProductType = Domain.Products.ProductType.Variable
            };
            var resultService = await _facadeProductVariantService.ProductVariantCommandsService.CreateProductVariant(inputModel);
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
        /// ویرایش اطلاعات یک واریانت (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProductVariantApiDto dto)
        {
            var inputModel = new UpdateProductVariantDto
            {
                ProductVariantId = dto.ProductVariantId,
                Price = dto.Price,
                SpecialPrice = dto.SpecialPrice,
                Stock = dto.Stock,
                Height = dto.Height,
                Length = dto.Length,
                Weight = dto.Weight,
                Width = dto.Width
            };
            var resultService = await _facadeProductVariantService.ProductVariantCommandsService.UpdateProductVariant(inputModel);
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
        /// حذف یک واریانت (Auth)
        /// </summary>
        /// <param name="ProductVariantId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Delete, KeyNameArea.Admin)]
        [HttpDelete("{ProductVariantId}")]
        public async Task<IActionResult> Delete(int ProductVariantId)
        {
            var resultService = await _facadeProductVariantService.ProductVariantCommandsService.DeleteProductVariant(ProductVariantId);
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
