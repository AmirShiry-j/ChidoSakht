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
using Microsoft.AspNetCore.Http.Extensions;
using Application.ProductService.Queries;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IFacadeProductService _facadeProductService;
        private readonly ILocalizationService _localizationService;
        public ProductController(IFacadeProductService facadeProductService, ILocalizationService localizationService)
        {
            _facadeProductService = facadeProductService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// برگردوندن محصولات (Auth)
        /// </summary>
        /// <param name="searchProductApiDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductFilterApiDto searchProductApiDto)
        {
            //map
            var inputService = new ProductFilterDto
            {
                Name = searchProductApiDto.Name,
                CountInPage = searchProductApiDto.CountInPage,
                Page = searchProductApiDto.Page,
            };

            //Get data from service
            var resultService = await _facadeProductService.ProductQueriesService.GetProducts(inputService);

            //HATEAOS
            //Build url of image
            string url = Request.GetDisplayUrl();
            string domainName = url.Substring(0, url.IndexOf("/api"));

            foreach (var product in resultService.Data.Products)
            {
                if (product.NameIndexImage != null)
                {
                    string imageUrl = domainName + "/Images/ProductImage/" + product.NameIndexImage;
                    product.UrlNameIndexImage = imageUrl;
                }

                product.Link = new Link
                {
                    For = "Details",
                    HttpMethod = HttpMethod.Get.ToString(),
                    Url = Url.Action(nameof(Get), nameof(ProductController).Replace("Controller", ""), new { ProductId = product.Id }, Request.Scheme)
                };
            }

            return Ok(resultService.Data);
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

            //HATEAOS
            //Build url of image


            if (resultService.IsSuccess)
            {
                //HATEOAS links
                if (resultService.Data.NameIndexImage is not null)
                {
                    string url = Request.GetDisplayUrl();
                    string domainName = url.Substring(0, url.IndexOf("/api"));
                    string mainProductImageUrl = domainName + "/Images/ProductImage/" + resultService.Data.NameIndexImage;

                    resultService.Data.UrlNameIndexImage = mainProductImageUrl;
                }

                resultService.Data.Links = new List<Link>
                    {
                        new Link
                        {
                            For="For Update Name",
                            HttpMethod=HttpMethod.Post.ToString(),
                            Url=Url.Action(nameof(Post),nameof(ProductController).Replace("Controller", ""),new { Area="Admin" },Request.Scheme)
                        },
                    };

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
        /// ایجاد اولیه محصول و یا ویرایش نام آن (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(UpsertProductApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.Upsert(dto.ProductId, dto.Name);
            if (resultService.IsSuccess)
            {
                if (resultService.MessageEventType == MessageEventType.Created)
                {
                    return CreatedAtAction(nameof(Get), new { ProductId = resultService.Data }, null);
                }
                else//Updated
                {
                    return NoContent();
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

        /// <summary>
        /// ست کردن یک توضیح برای محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut(nameof(SetDescription))]
        public async Task<IActionResult> SetDescription(SetDescriptionApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.SetDescription(dto.ProductId, dto.Description);
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
        /// ست کردن یک پیوند یکتا برای محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut(nameof(SetUniqeLink))]
        public async Task<IActionResult> SetUniqeLink(SetUniqeLinkApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.SetUniqeLink(dto.ProductId, dto.UniqeLink);
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
        /// بررسی یکتا بودن پیوند محصول (Auth)
        /// </summary>
        /// <param name="UniqeLink"></param>
        /// <returns></returns>
        [HttpGet(nameof(ValidateUniqeLink))]
        public async Task<IActionResult> ValidateUniqeLink([Required] int ProductId, [Required] string UniqeLink)
        {
            var resultService = await _facadeProductService.ProductQueriesService.ValidateUniqeLink(ProductId, UniqeLink);
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
