using Application.CategoryService;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.ProductService.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;
using WebApi.ModelsAndDtoes.Common;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly IProductCommandsService _productService;
        private readonly ILocalizationService _localizationService;
        public ProductController(IProductCommandsService productService, ILocalizationService localizationService)
        {
            _productService = productService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// ایجاد اولیه و ویرایش نام محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(UpsertProductDto dto)
        {
            var resultService = await _productService.Upsert(dto.Id, dto.Name);
            if (resultService.IsSuccess)
            {
                //var link = Url.Action(nameof(Put), nameof(ProductController).Replace("Controller", ""), new { Area = "Admin" }, Request.Scheme);
                //return CreatedAtAction(nameof(Put), new { CategoryId = resultService.Data }, _localizationService.GetMessageCategory(MessageKeysCategory.CategoryCreated.ToString()));

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
