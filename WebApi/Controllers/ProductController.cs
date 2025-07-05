using Application.Store.AdminSection.ProductService.Queries;
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
        //[HttpGet]
        //public async Task<IActionResult> Get([FromQuery] ProductFilterForUserSectionApiDto productFilterForUserSectionApiDto)
        //{
        //    //map
        //    var inputService = new ProductFilterDto
        //    {
        //        Name = searchProductApiDto.Name,
        //        CountInPage = searchProductApiDto.CountInPage,
        //        Page = searchProductApiDto.Page,
        //    };

        //    //Get data from service
        //    var resultService = await _facadeProductService.ProductQueriesService.GetProducts(inputService);

        //    //HATEAOS
        //    //Build url of image
        //    string url = Request.GetDisplayUrl();
        //    string domainName = url.Substring(0, url.IndexOf("/api"));

        //    foreach (var product in resultService.Data.Products)
        //    {
        //        if (product.NameIndexImage != null)
        //        {
        //            string imageUrl = domainName + "/Images/ProductImage/" + product.NameIndexImage;
        //            product.UrlNameIndexImage = imageUrl;
        //        }

        //        product.Link = new Link
        //        {
        //            For = "Details",
        //            HttpMethod = HttpMethod.Get.ToString(),
        //            Url = Url.Action(nameof(Get), nameof(ProductController).Replace("Controller", ""), new { ProductId = product.Id }, Request.Scheme)
        //        };
        //    }

        //    return Ok(resultService.Data);
        //}
    }
}
