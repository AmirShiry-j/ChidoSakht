using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductImageService;
using Application.Store.AdminSection.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Areas.Admin.ModelsAndDtoes.Products;
using WebApi.Filters.Permissions;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class ProductImageController : ControllerBase
    {
        private readonly IFacadeAdminProductImageService _facadeProductImageService;
        private readonly IFacadeAdminProductService _facadeProductService;
        private readonly ILocalizationService _localizationService;
        public ProductImageController(IFacadeAdminProductImageService facadeProductImageService, ILocalizationService localizationService, IFacadeAdminProductService facadeProductService)
        {
            _facadeProductImageService = facadeProductImageService;
            _localizationService = localizationService;
            _facadeProductService = facadeProductService;
        }

        /// <summary>
        /// برگردوندن تصاویر یک محصول (Auth)
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.View, KeyNameArea.Admin)]
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> Get(int ProductId)
        {
            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            //Get Images by service
            var resultService = await _facadeProductImageService.ProductImageQueriesService.GetImages(ProductId);
            if (resultService.IsSuccess)
            {
                if (resultService.Data.Any() == false)
                {
                    return Ok(resultService.Data);
                }

                //HATEAOS
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));
                foreach (var imageOb in resultService.Data)
                {
                    string imageUrl = domainName + "/Images/ProductImage/" + imageOb.Name;
                    imageOb.Url = imageUrl;
                }

                return Ok(resultService.Data);
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else
                    return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// اضافه کردن یک تصویر (Auth)
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPost("{ProductId}")]
        public async Task<IActionResult> Post(IFormFile file, int ProductId)
        {
            //Check size image
            var megabyte = file.Length / (1024 * 1024);
            if (megabyte > 10)
                return BadRequest("حجم تصویر بیشتر از 10 مگابایت نمیتواند باشد");

            //Check extension  jpg or png
            var extension = Path.GetExtension(file.FileName);
            if ((extension == ".jpg" || extension == ".png") == false)
                return BadRequest("فرمت تصویر پروفایل میتواند jpg یا png باشد");

            //Base Path Image
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/ProductImage");

            //Find user
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            ////Save Image in files and db
            //in files
            string imageName = "";
            string fullPath = "";
            do
            {
                imageName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                fullPath = Path.Combine(basePath, imageName);

            } while (System.IO.File.Exists(fullPath));
            using (Stream streamFile = new FileStream(fullPath, FileMode.CreateNew))
            {
                file.CopyTo(streamFile);
            }


            //Save imageName by service
            var resultService = await _facadeProductImageService.ProductImageCommandsService.CreateAImage(ProductId, imageName);

            if (resultService.IsSuccess)
            {
                //Build url of image
                string url = Request.GetDisplayUrl();
                string domainName = url.Substring(0, url.IndexOf("/api"));
                string imageUrl = domainName + "/Images/ProductImage/" + imageName;

                return Created(imageUrl, null);
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else
                    return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// حذف یک تصویر (Auth)
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpDelete("{Name}")]
        public async Task<IActionResult> Delete(string Name)
        {
            var resultService = await _facadeProductImageService.ProductImageCommandsService.DeleteAImage(Name);
            if (resultService.IsSuccess)
            {
                //Base Path Image
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/ProductImage");

                //Delete old image file
                string pathOldFile = Path.Combine(basePath, Name);
                if (System.IO.File.Exists(pathOldFile))
                {
                    System.IO.File.Delete(pathOldFile);
                }

                return NoContent();
            }
            else
            {
                return BadRequest(resultService.Message);
            }
        }

        /// <summary>
        /// ست کردن یک متن جایگزین تصویر برای محصول (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut(nameof(SetImageAltText))]
        public async Task<IActionResult> SetImageAltText(SetImageAltTextApiDto dto)
        {
            var resultService = await _facadeProductService.ProductCommandsService.SetImageAltText(dto.ProductId, dto.ImageAltText);
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
        /// قرار دادن یک تصویر به عنوان شاخص (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [PermissionAuthorize(KeyNameController.Product, KeyNameAction.Edit, KeyNameArea.Admin)]
        [HttpPut(nameof(SetIndexImage))]
        public async Task<IActionResult> SetIndexImage(SetIndexImageApiDto dto)
        {
            var resultService = await _facadeProductImageService.ProductImageCommandsService.SetIndexImage(dto.ProductId, dto.Name);
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
