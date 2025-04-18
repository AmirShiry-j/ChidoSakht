using Application.CategoryService;
using Application.CategoryService.Commands;
using Application.CategoryService.Queries;
using Application.Common.AppKeyNames;
using Application.Common.Dtoes;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Domain.Categories;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebApi.Controllers;
using WebApi.Filters.Permissions;
using WebApi.ModelsAndDtoes.Categories;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
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
        /// برگردوندن همه دسته بندی ها به شکل درخت (Auth)
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Category, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Get by service
            var resultService = await _facadeCategoryService.GetAllCategoriesAsTreeService.Execute();

            if (resultService.IsSuccess)
            {
                resultService.Data.Links = new List<Link>
                    {
                        new Link
                        {
                            For="sample link for Update",
                            HttpMethod=HttpMethod.Put.ToString(),
                            Url=Url.Action(nameof(Put),nameof(CategoryController).Replace("Controller", ""),new { Area="Admin" },Request.Scheme)
                        },
                        new Link
                        {
                            For="sample link for Delete",
                            HttpMethod=HttpMethod.Delete.ToString(),
                            Url=Url.Action(nameof(Delete),nameof(CategoryController).Replace("Controller", ""),new { Area="Admin" ,CategoryId=(0).ToString() },Request.Scheme)
                        },
                    };

                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// برگشت اطلاعات مربوط ب یک دسته بندی (Auth)
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Category, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{CategoryId}")]
        public async Task<IActionResult> Get(int CategoryId)
        {
            //Get by service
            var resultService = await _facadeCategoryService.GetCategoryDetailsByIdService.Execute(CategoryId);

            if (resultService.IsSuccess)
            {
                if (resultService.Data is not null)
                {
                    //HATEOAS links
                    resultService.Data.Links = new List<Link>
                    {
                        new Link
                        {
                            For="Update",
                            HttpMethod=HttpMethod.Put.ToString(),
                            Url=Url.Action(nameof(Put),nameof(CategoryController).Replace("Controller", ""),new { Area="Admin" },Request.Scheme)
                        },
                        new Link
                        {
                            For="Delete",
                            HttpMethod=HttpMethod.Delete.ToString(),
                            Url=Url.Action(nameof(Delete),nameof(CategoryController).Replace("Controller", ""),new { Area="Admin" ,CategoryId=resultService.Data.Id },Request.Scheme)
                        },
                    };

                    return Ok(resultService.Data);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// اضافه کردن یک دسته بندی جدید (Auth)
        /// </summary>
        /// <param name="createCategoryApiDto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Category, KeyNameAction.Add, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateCategoryApiDto createCategoryApiDto)
        {
            //Map
            var dtoService = new CreateCategoryDto
            {
                Name = createCategoryApiDto.Name,
                ParentCategoryId = createCategoryApiDto.ParentCategoryId,
            };

            //Create Category by service
            var resultService = await _facadeCategoryService.CreateCategoryService.Execute(dtoService);
            if (resultService.IsSuccess)
            {
                return CreatedAtAction(nameof(Get), new { CategoryId = resultService.Data }, _localizationService.GetMessageCategory(MessageKeysCategory.CategoryCreated.ToString()));
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// آپدیت اطلاعات یک دسته بندی (Auth)
        /// </summary>
        /// <param name="updateCategoryApiDto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Category, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateCategoryApiDto updateCategoryApiDto)
        {
            //Map
            var model = new UpdateCategoryDto
            {
                Id = updateCategoryApiDto.Id,
                Name = updateCategoryApiDto.Name,
                ParentCategoryId = updateCategoryApiDto.ParentCategoryId
            };

            //Update Category by service
            var resultService = await _facadeCategoryService.UpdateCategoryService.Execute(model);

            if (resultService.IsSuccess)
            {
                if (resultService.Data is not null)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// حذف یک دسته بندی (Auth)
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Category, KeyNameAction.Delete, KeyNameArea.Admin)]
        [HttpDelete("{CategoryId}")]
        public async Task<IActionResult> Delete(int CategoryId)
        {
            //Delete Category by service
            var resultService = await _facadeCategoryService.DeleteCategoryService.Execute(CategoryId);
            if (resultService.IsSuccess)
            {
                if (resultService.Data is not null)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
    }
}
