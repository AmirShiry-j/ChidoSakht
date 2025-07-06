using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductAttribute.Commands;
using Application.Store.AdminSection.ProductVariant;
using Application.Store.AdminSection.RelatedProduct;
using Application.Store.AdminSection.RelatedProduct.Commands;
using Domain.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.RelatedProduct;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class RelatedProductsController : ControllerBase
    {
        private readonly IFacadeAdminRelatedProductService _FacadeAdminRelatedProductService;
        private readonly ILocalizationService _localizationService;
        public RelatedProductsController(IFacadeAdminRelatedProductService FacadeAdminRelatedProductService, ILocalizationService localizationService)
        {
            _FacadeAdminRelatedProductService = FacadeAdminRelatedProductService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// اضافه کردن محصولات مرتبط یک محصول - خودکار دو طرفه - (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(AddRelatedProductsApiDto dto)
        {
            var inputModel = new AddRelatedProductsDto
            {
                ProductId = dto.ProductId,
                RelatedProductIds = dto.RelatedProductIds,
            };

            var resultService = await _FacadeAdminRelatedProductService.RelatedProductCommandsService.AddRelatedProductsAsync(inputModel);
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
        /// حذف محصول مرتبط یک محصول - دو طرفه (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveRelation(RemoveRelatedProductApiDto dto)
        {
            var inputModel = new RemoveRelatedProductDto
            {
                ProductId = dto.ProductId,
                RelatedProductId = dto.RelatedProductId,
            };

            var resultService = await _FacadeAdminRelatedProductService.RelatedProductCommandsService.RemoveRelationAsync(inputModel);
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