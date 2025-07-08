using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductSpecificationService;
using Domain.Products;
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
    public class ProductSpecificationGroupController : ControllerBase
    {
        private readonly IFacadeAdminProductSpecificationService _facadeAdminProductSpecificationService;
        private readonly ILocalizationService _localizationService;
        public ProductSpecificationGroupController(IFacadeAdminProductSpecificationService facadeAdminProductSpecificationService, ILocalizationService localizationService)
        {
            _facadeAdminProductSpecificationService = facadeAdminProductSpecificationService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن همه گروه ها (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationQueriesService.GetSpecGroupsWihtSpecsAsync(ProductId);

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
        /// ایجاد یک گروه (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductSpecificationGroupApiDto dto)
        {
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationCommandsService.CreateSpecGroupAsync(dto.ProductId, dto.Titile);
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
        /// ویرایش گروه (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProductSpecificationGroupApiDto dto)
        {
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationCommandsService.UpdateSpecGroupAsync(dto.ProductSpecificationGroupId, dto.Titile);
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
        /// حذف یک گروه (Auth)
        /// </summary>
        /// <param name="ProductSpecificationGroupId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpDelete("{ProductSpecificationGroupId}")]
        public async Task<IActionResult> Delete(long ProductSpecificationGroupId)
        {
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationCommandsService.DeleteSpecGroupAsync(ProductSpecificationGroupId);
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
