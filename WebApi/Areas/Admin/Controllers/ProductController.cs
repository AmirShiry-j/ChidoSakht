using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;
using WebApi.Filters.Permissions;
using Microsoft.AspNetCore.Http.Extensions;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using Domain.Products;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.ProductService;
using Application.Store.AdminSection.ProductService.Queries;
using Application.Store.AdminSection.ProductService.Commands;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IFacadeAdminProductService _facadeProductService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHostEnvironment _env;
        public ProductController(IFacadeAdminProductService facadeProductService, ILocalizationService localizationService, IWebHostEnvironment env)
        {
            _facadeProductService = facadeProductService;
            _localizationService = localizationService;
            _env = env;
        }

        /// <summary>
        /// برگردوندن محصولات (Auth)
        /// </summary>
        /// <param name="searchProductApiDto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductFilterApiDto searchProductApiDto)
        {
            //map
            var inputService = new ProductFilterDto
            {
                Name = searchProductApiDto.Name,
                CategoryId = searchProductApiDto.CategoryId,
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
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
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
        /// ایجاد اولیه محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Add, KeyNameArea.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.Create(dto.Name, (ProductType)dto.ProductType);
            if (resultService.IsSuccess)
            {
                return CreatedAtAction(nameof(Get), new { ProductId = resultService.Data }, null);
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
        /// آپدیت کردن نام برای محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut(nameof(SetName))]
        public async Task<IActionResult> SetName(SetNameProductApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.SetName(dto.ProductId, dto.Name);
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
        /// ثبت و آپدیت اطلاعات برای محصول ساده (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put(UpdateInfoProductSampleApiDto dto)
        {
            var inputModel = new UpdateInfoProductSampleDto
            {
                Height = dto.Height,
                Length = dto.Length,
                Price = dto.Price,
                ProductId = dto.ProductId,
                SpecialPrice = dto.SpecialPrice,
                Stock = dto.Stock,
                Weight = dto.Weight,
                Width = dto.Width
            };
            var resultService = await _facadeProductService.ProductCommandsService.UpdateInfoProductSample(inputModel);
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
        /// ست کردن یک توضیح برای محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
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
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
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
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
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


        /// <summary>
        /// بررسی یکتا بودن فیلد یونیکد محصول (Auth)
        /// </summary>
        /// <param name="UniCode"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpGet(nameof(ValidateUniCode))]
        public async Task<IActionResult> ValidateUniCode([Required] int ProductId, [Required] string UniCode)
        {
            var resultService = await _facadeProductService.ProductQueriesService.ValidateUniCode(ProductId, UniCode);
            if (resultService.IsSuccess)
            {
                return Ok(resultService.Data);
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// ست کردن کد یکتا برای محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut(nameof(SetUniCode))]
        public async Task<IActionResult> SetUniCode(SetUniCodeApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.SetUniCode(dto.ProductId, dto.UniCode);
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
        /// ست کردن دسته بندی برای محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut(nameof(SetCategoryId))]
        public async Task<IActionResult> SetCategoryId(SetCategoryIdApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.SetCategoryId(dto.ProductId, dto.CategoryId);
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
        /// حذف یک محصول (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Delete, KeyNameArea.Admin)]
        [HttpDelete("{ProductId}")]
        public async Task<IActionResult> Delete(int ProductId)
        {
            var basePathImages = Path.Combine(_env.ContentRootPath, "Images", "ProductImage");

            var resultService = await _facadeProductService.ProductCommandsService.DeleteAProduct(ProductId, basePathImages);
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
