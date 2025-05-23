using Application.Interfaces.Localization;
using Application.ProductImageService;
using Application.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    //[Authorize]
    public class ProductImageController : ControllerBase
    {
        private readonly IFacadeProductImageService _facadeProductImageService;
        private readonly ILocalizationService _localizationService;
        public ProductImageController(IFacadeProductImageService facadeProductImageService, ILocalizationService localizationService)
        {
            _facadeProductImageService = facadeProductImageService;
            _localizationService = localizationService;
        }


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
                if (resultService.MessageEventType == Application.Common.MessageEventTypes.MessageEventType.NotFound)
                    return NotFound();
                else
                    return BadRequest(resultService.Message);
            }
        }

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
                string imageUrl = domainName + "/Images/ProductImage/" + resultService.Data;

                return Created(imageUrl, null);
            }
            else
            {
                if (resultService.MessageEventType == Application.Common.MessageEventTypes.MessageEventType.NotFound)
                    return NotFound();
                else
                    return BadRequest(resultService.Message);
            }
        }

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
    }
}
