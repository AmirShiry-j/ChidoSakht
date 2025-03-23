using Application.CategoryService;
using Application.CategoryService.Commands;
using Application.CategoryService.Queries;
using Domain.Categories;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
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
        public CategoryController(IFacadeCategoryService facadeCategoryService)
        {
            _facadeCategoryService = facadeCategoryService;
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
                return CreatedAtAction(nameof(GetCategoryById), new { CategoryId = resultService.Data }, "دسته بندی شما ثبت شد");
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
                    //HATEAOS
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
