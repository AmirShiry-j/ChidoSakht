using Application.CategoryService;
using Application.TokenService;
using Application.UserService;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IFacadeUserService _facadeUserService;
        public ProfileController(IFacadeUserService facadeUserService)
        {
            _facadeUserService = facadeUserService;
        }


        /// <summary>
        /// بر گردوندن اطلاعات پروفایل (Auth)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;
            if (userId == null)
            {
                return BadRequest();
            }

            //Get Profil infoes
            var resultService = await _facadeUserService.GetUserProfileService.Execute(userId);
            if (resultService.IsSuccess == false)
                return Problem();

            ////HATEOAS links
            //resultService.Data.Link = new Application.Common.Link
            //{
            //    For = "Edit",
            //    HttpMethod = HttpMethod.Put.ToString(),
            //    Url = Url.Action(nameof(Put), "Profile", null, Request.Scheme)
            //};

            return Ok(resultService.Data);
        }

    }
}
