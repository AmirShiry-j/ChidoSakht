using Application.Messagers.SmsService;
using Application.TokenService;
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
using WebApi.Tools.Hasher;
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
        private readonly IUserTokenService _userTokenService;
        private readonly ILogger<AccountController> _logger;
        private readonly ISmsService _smsService;
        public AccountController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger,
            IUserTokenService userTokenService,
            ISmsService smsService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _smsService = smsService;
            _userTokenService = userTokenService;
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
                var resultSendPhoneNumber = await _smsService.SendSmsAsync(newUser.PhoneNumber, bodyMessage);

                //HATEOAS links
                Link link = new Link
                {
                    Url = Url.Action(nameof(VerifyPhoneNumber), "Account", null, protocol: Request.Scheme),
                    HttpMethod = HttpMethod.Post.ToString(),
                    For = nameof(VerifyPhoneNumber)
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

        /// <summary>
        /// ورود به حساب کاربری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            //Find user
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                return Unauthorized("کاربری با این شماره موبایل یافت نشد");
            }

            //Login user
            var resultLogin = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (resultLogin.Succeeded)
            {
                //Build and Get tokes
                var tokens = await CreateNewTokenForUser(user);

                return Ok(tokens);
            }
            else if (resultLogin.IsLockedOut)
            {
                return Unauthorized("حساب کاربری شما به علت وارد کردن رمز عبور اشتباه تا پنج دقیقه آینده قفل است");
            }
            else if (resultLogin.IsNotAllowed)
            {
                return Unauthorized("حساب کاربری شما تایید نشده. لطفا ابتدا شماره موبایل خود را تایید کنید");
            }
            else
            {
                return Unauthorized("رمز عبور وارد شده اشتباه است");
            }
        }

        /// <summary>
        ///ارسال کد به شماره موبایل برای تایید حساب کاربر
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        [HttpGet("{PhoneNumber}")]
        public async Task<IActionResult> ConfirmPhoneNumber(string PhoneNumber)
        {
            //Check bind phoneNumber
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                return BadRequest();
            }

            //Find user
            var user = await _userManager.FindByNameAsync(PhoneNumber);
            if (user == null)
            {
                return NotFound();
            }

            //Confirmation phoneNumber
            string code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);

            //Send code To user by phoneNumber
            //code...
            string bodyMessage = $"کد زیر را جهت تایید حساب کاربری خود در قسمت مربوطه وارد کنید<br/><h3>{code}</h3>";
            var resultSendPhoneNumber = await _smsService.SendSmsAsync(PhoneNumber, bodyMessage);

            //HATEOAS links
            Link link = new Link
            {
                For = nameof(VerifyPhoneNumber),
                HttpMethod = HttpMethod.Post.ToString(),
                Url = Url.Action(nameof(VerifyPhoneNumber), "Account", null, protocol: Request.Scheme)
            };

            return Ok(new { Code = code, Link = link });
        }

        /// <summary>
        /// تایید شماره موبایل با کد ارسال شده برای کاربر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberDto model)
        {
            //Find user
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                return NotFound();
            }

            //Check verify phoneNumber
            var resultConfirm = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, model.Code);
            if (resultConfirm)
            {
                //Set confirm phoneNumber user and update it
                user.PhoneNumberConfirmed = true;
                var resultConfirmedPhoneNumber = await _userManager.UpdateAsync(user);

                //Build and Get tokes
                var tokens = await CreateNewTokenForUser(user);

                return Ok(tokens);
            }
            else
            {
                return BadRequest("کد وارد شده اشتباه است");
            }
        }


        /// <summary>
        /// متد ساخت توکن jwt و رفرش توکن
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<TokenDto> CreateNewTokenForUser(User user)
        {
            ////Build token for user
            //Initial claims
            List<Claim> claims = new List<Claim>
                {
                    new Claim("UserId",user.Id),
                    new Claim("Email",user.Email),
                    new Claim("PhoneNumber",user.PhoneNumber),
                    new Claim("FullName",user.FullName)
                };

            //Add Role claims if has user
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles != null && userRoles.Any())
            {
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            //Add random value to clamis for build diffrent token in every time
            string randomValue = Guid.NewGuid().ToString();
            claims.Add(new Claim("RandomValue", randomValue));

            //Initial credentials
            var expireTime = JwtInfo.Expires;
            string key = JwtInfo.SecretKey;
            var hashKey = Encoding.UTF8.GetBytes(key);
            var secretKey = new SymmetricSecurityKey(hashKey);
            var credential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //Initial jwtSecurityToken
            var token = new JwtSecurityToken(
                issuer: JwtInfo.Issuer,
                audience: JwtInfo.Audience,
                expires: expireTime,
                notBefore: JwtInfo.NotBefore,
                claims: claims,
                signingCredentials: credential
                );

            //Initial jwtSecurityTokenHandler
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            //Create refresh token
            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpireTime = JwtInfo.ExpiresRefreshToken;

            ////Save token in db
            //Map data to dto
            var hasherService = new SecurityHasher();
            var userToken = new UserTokenDto
            {
                ExpireTime = expireTime,
                UserId = user.Id,
                TokenHash = hasherService.GetSha256Hash(jwtToken),
                RefreshExpireTime = refreshTokenExpireTime,
                RefreshTokenHash = hasherService.GetSha256Hash(refreshToken)
            };
            //Save in db
            await _userTokenService.SaveToken(userToken);

            //Map tokens for send
            var tokens = new TokenDto() { Token = jwtToken, RefreshToken = refreshToken, TokenExpireTime = expireTime, RefreshTokenExpireTime = refreshTokenExpireTime };

            return tokens;
        }

    }
}
