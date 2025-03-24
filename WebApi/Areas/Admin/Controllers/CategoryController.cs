using Application.CategoryService;
using Application.CategoryService.Commands;
using Application.CategoryService.Queries;
using Application.Common;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Domain.Categories;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using WebApi.ModelsAndDtoes.Categories;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/Category/")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly IFacadeCategoryService _facadeCategoryService;
        private readonly ILocalizationService _localizationService;
        public CategoryController(IFacadeCategoryService facadeCategoryService, ILocalizationService localizationService)
        {
            _facadeCategoryService = facadeCategoryService;
            _localizationService = localizationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryApiDto createCategoryApiDto)
        {
            //Map
            var dtoService = new CreateCategoryDto
            {
                Name = createCategoryApiDto.Name,
                ParentCategoryId = createCategoryApiDto.ParentCategoryId,
            };

            //Create Category by service
            var resultService = await _facadeCategoryService.AddCategoryService.Execute(dtoService);
            if (resultService.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCategoryById), new { CategoryId = resultService.Data }, _localizationService.GetMessageCategory(MessageKeysCategory.CategoryCreated.ToString()));
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryApiDto updateCategoryApiDto)
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
                return Ok();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int CategoryId)
        {
            //Delete Category by service
            var resultService = await _facadeCategoryService.DeleteCategoryService.Execute(CategoryId);
            if (resultService.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        [HttpGet("{CategoryId}")]
        public async Task<IActionResult> GetCategoryById(int CategoryId)
        {
            //Get by service
            var resultService = await _facadeCategoryService.GetCategoryInfoByIdService.Execute(CategoryId);

            if (resultService.IsSuccess)
            {
                if (resultService.Data is not null)
                {
                    //HATEOAS links
                    //    resultService.Data.Links = new List<Link>
                    //{
                    //    new Link
                    //    {
                    //        For="Edit",
                    //        HttpMethod=HttpMethod.Put.ToString(),
                    //        Url=Url.Action(nameof(Put),nameof(CategoryController).Replace("Controller", ""),new { Area="Admin" },Request.Scheme)
                    //    },
                    //    new Link
                    //    {
                    //        For="Delete",
                    //        HttpMethod=HttpMethod.Delete.ToString(),
                    //        Url=Url.Action(nameof(Delete),nameof(CategoryController).Replace("Controller", ""),new { Area="Admin" },Request.Scheme)
                    //    },
                    //};

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
    }
}
