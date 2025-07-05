using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.ProductService;
using Application.Store.UserSection.ProductService.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Product;

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
                OnlyAvailableGoods= productFilterForUserSectionApiDto.OnlyAvailableGoods,

                CountInPage = productFilterForUserSectionApiDto.CountInPage,
                Page = productFilterForUserSectionApiDto.Page,
            };

            //Get data from service
            var resultService = await _facadeProductService.ProductQueriesService.GetProducts(inputService);

            //HATEAOS
            //Build url of image
            string url = Request.GetDisplayUrl();
            string domainName = url.Substring(0, url.IndexOf("/api"));

            return Ok(resultService.Data);
        }
    }
}
