using Application.Messagers.SmsService;
using Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Helpers;
using WebApi.ModelsAndDtoes.Account;
using WebApi.ModelsAndDtoes.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly ISmsService _smsService;
        public AccountController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger,
            ISmsService smsService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _smsService = smsService;
        }

        /// <summary>
        /// ساختن حساب کاربری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            //Build user
            var newUser = new User
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserName = model.PhoneNumber,
            };

            //Register user
            if (model.Email is not null)
            {
                var resultExistEmail = await _userManager.FindByEmailAsync(model.Email);
                if (resultExistEmail is not null)
                {
                    return BadRequest("ایمیل وارد شده قبلا برای کاربر دیگری ثبت شده است");
                }
            }
            var resultRegister = await _userManager.CreateAsync(newUser, model.Password);
            if (resultRegister.Succeeded)
            {
                //Confirmation PhoneNumber
                string code = await _userManager.GenerateTwoFactorTokenAsync(newUser, TokenOptions.DefaultPhoneProvider);

                //Send code To user by PhoneNumber
                //code...
                string bodyMessage = $"کد زیر را جهت تایید حساب کاربری خود در قسمت مربوطه وارد کنید: {code}";
                var resultSendEmail = await _smsService.SendSmsAsync(newUser.PhoneNumber, bodyMessage);

                //HATEOAS links
                Link link = new Link
                {
                    Url = Url.Action("VerifyPhoneNumber", "Account", null, protocol: Request.Scheme),
                    HttpMethod = HttpMethod.Post.ToString(),
                    For = "VerifyPhoneNumber"
                };

                //Initial message
                string message = "کد تایید حساب کاربری به شماره موبایل شما ارسال شد " + $"[کد برای محیط تست: {code}]";

                return Ok(new { Message = message, Link = link, Code = code });
            }
            else
            {
                string error = resultRegister.Errors?.Select(p => p.Description).FirstOrDefault();
                return BadRequest(error);
            }
        }


    }
}
