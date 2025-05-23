using Application.CategoryService;
using Application.Common.AppKeyNames;
using Application.Common.Dtoes;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.ProductService;
using Application.ProductService.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;
using WebApi.Filters.Permissions;
using Application.Common.MessageEventTypes;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly IFacadeProductService _facadeProductService;
        private readonly ILocalizationService _localizationService;
        public ProductController(IFacadeProductService facadeProductService, ILocalizationService localizationService)
        {
            _facadeProductService = facadeProductService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// گرفتن اطلاعات یک محصول (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeProductService.ProductQueriesService.GetOneProduct(ProductId);

            if (resultService.IsSuccess)
            {
                if (resultService.Data is not null)
                {
                    //HATEOAS links
                    resultService.Data.Links = new List<Link>
                    {
                        new Link
                        {
                            For="For Create and Update Name",
                            HttpMethod=HttpMethod.Post.ToString(),
                            Url=Url.Action(nameof(Post),nameof(ProductController).Replace("Controller", ""),new { Area="Admin" },Request.Scheme)
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
        /// ایجاد اولیه و ویرایش نام محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(UpsertProductDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.Upsert(dto.Id, dto.Name);
            if (resultService.IsSuccess)
            {
                if (resultService.MessageEventType == MessageEventType.Created)
                {
                    return CreatedAtAction(nameof(Get), new { ProductId = resultService.Data }, null);
                }
                else//Updated
                {
                    return Ok();
                }
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
