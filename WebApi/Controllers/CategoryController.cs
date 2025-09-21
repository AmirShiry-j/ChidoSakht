using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.Dtoes;
using Application.Store.AdminSection.CategoryService;
using Application.Store.UserSection.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters.Permissions;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class CategoryController : ControllerBase
    {
        private readonly IFacadeCategoryService _facadeCategoryService;
        private readonly ILocalizationService _localizationService;
        public CategoryController(IFacadeCategoryService facadeCategoryService, ILocalizationService localizationService)
        {
            _facadeCategoryService = facadeCategoryService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن همه دسته بندی ها به شکل درخت (کاربری)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get by service
            var resultService = await _facadeCategoryService.GetAllCategoriesAsTreeService.Execute();

            if (resultService.IsSuccess)
            {

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

    }
}
