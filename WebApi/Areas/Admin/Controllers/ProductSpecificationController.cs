using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductSpecificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ProductSpecificationController : ControllerBase
    {
        private readonly IFacadeAdminProductSpecificationService _facadeAdminProductSpecificationService;
        private readonly ILocalizationService _localizationService;
        public ProductSpecificationController(IFacadeAdminProductSpecificationService facadeAdminProductSpecificationService, ILocalizationService localizationService)
        {
            _facadeAdminProductSpecificationService = facadeAdminProductSpecificationService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن همه گروه ها (Auth)
        /// </summary>
        /// <param name="ProductSpecificationGroupId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{ProductSpecificationGroupId}")]
        public async Task<IActionResult> Get(long ProductSpecificationGroupId)
        {
            //Get by service
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationQueriesService.GetSpecGroupsWihtSpecsAsync((int)ProductSpecificationGroupId);

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
        public async Task<IActionResult> Post(CreateProductSpecificationApiDto dto)
        {
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationCommandsService.CreateSpecAsync(dto.ProductSpecificationGroupId, dto.Key, dto.Value);
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
        public async Task<IActionResult> Put(UpdateProductSpecificationApiDto dto)
        {
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationCommandsService.UpdateSpecAsync(dto.ProductSpecificationId, dto.Key, dto.Value);
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
        /// <param name="ProductSpecificationId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpDelete("{ProductSpecificationId}")]
        public async Task<IActionResult> Delete(long ProductSpecificationId)
        {
            var resultService = await _facadeAdminProductSpecificationService.ProductSpecificationCommandsService.DeleteSpecAsync(ProductSpecificationId);
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