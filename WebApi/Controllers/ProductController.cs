using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.ProductService;
using Application.Store.UserSection.ProductService.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WebApi.Filters.Permissions;
using WebApi.ModelsAndDtoes.Product;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class ProductController : ControllerBase
    {
        private readonly IFacadeProductService _facadeProductService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHostEnvironment _env;
        public ProductController(IFacadeProductService facadeProductService, ILocalizationService localizationService, IWebHostEnvironment env)
        {
            _facadeProductService = facadeProductService;
            _localizationService = localizationService;
            _env = env;
        }
        /// <summary>
        /// برگردوندن محصولات با فیلتر های مختلف (کاربری)
        /// </summary>
        /// <param name="productFilterForUserSectionApiDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductFilterForUserSectionApiDto productFilterForUserSectionApiDto)
        {
            //map
            var inputService = new ProductFilterForUserSectionDto
            {
                ProductName = productFilterForUserSectionApiDto.ProductName,
                ToPrice = productFilterForUserSectionApiDto.ToPrice,
                FromPrice = productFilterForUserSectionApiDto.FromPrice,
                TypeOrderByForProduct = productFilterForUserSectionApiDto.TypeOrderByForProduct,
                CategoryId = productFilterForUserSectionApiDto.CategoryId,
                OnlyAvailableGoods = productFilterForUserSectionApiDto.OnlyAvailableGoods,
                Ascending = productFilterForUserSectionApiDto.Ascending,

                CountInPage = productFilterForUserSectionApiDto.CountInPage,
                Page = productFilterForUserSectionApiDto.Page,
            };

            //Get data from service
            var resultService = await _facadeProductService.ProductQueriesService.GetProducts(inputService);

            //HATEOAS
            //Build url of image
            string url = Request.GetDisplayUrl();
            string domainName = url.Substring(0, url.IndexOf("/api"));
            string baseImageUrl = domainName + "/Images/ProductImage/";

            foreach (var product in resultService.Data.Products.Where(p => p.NameIndexImage is not null))
            {
                string imageUrl = baseImageUrl + product.NameIndexImage;
                product.UrlNameIndexImage = imageUrl;
            }

            return Ok(resultService.Data);
        }


        /// <summary>
        /// برگردوندن اطلاعات یک محصول (کاربری)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Get by service
            var resultService = await _facadeProductService.ProductQueriesService.GetOneProductWithDetails(ProductId);

            if (resultService.IsSuccess)
            {
                //HATEOAS
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));
                string baseImageUrl = domainName + "/Images/ProductImage/";

                if (resultService.Data.NameIndexImage is not null)
                {
                    string imageUrl = baseImageUrl + resultService.Data.NameIndexImage;
                    resultService.Data.UrlNameIndexImage = imageUrl;
                }

                foreach (var img in resultService.Data.ProductImages)
                {
                    string imageUrl = baseImageUrl + img.Name;
                    img.Url = imageUrl;
                }

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
        /// برگردوندن اطلاعات یک محصول (کاربری)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet("GetRelatedProducts/{ProductId}")]
        public async Task<IActionResult> GetRelatedProducts(int ProductId)
        {
            //Get by service
            var resultService = await _facadeProductService.ProductQueriesService.GetRelatedProducts(ProductId);

            if (resultService.IsSuccess)
            {
                //HATEOAS
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));
                string baseImageUrl = domainName + "/Images/ProductImage/";

                foreach (var product in resultService.Data)
                {
                    if (product.NameIndexImage is not null)
                    {
                        string imageUrl = baseImageUrl + product.NameIndexImage;
                        product.UrlNameIndexImage = imageUrl;
                    }
                }

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
    }
}
