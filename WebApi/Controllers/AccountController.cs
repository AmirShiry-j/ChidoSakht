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
                string code = await _userManager.GenerateChangePhoneNumberTokenAsync(newUser, newUser.PhoneNumber);

                //Send code To user by PhoneNumber
                //code...
                string bodyMessage = $"کد زیر را جهت تایید حساب کاربری خود در قسمت مربوطه وارد کنید: {code}";
                var resultSendPhoneNumber = await _smsService.SendSmsAsync(newUser.PhoneNumber, bodyMessage);

                //HATEOAS links
                Link link = new Link
                {
                    Url = Url.Action(nameof(VerifyPhoneNumber), nameof(AccountController).Replace("Controller", ""), null, protocol: Request.Scheme),
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
                return Unauthorized("شماره موبایل یا رمز عبور اشتباه است");
            }

            //Check confirmed Account by PhoneNumber
            if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
            {
                return Unauthorized("حساب کاربری شما تایید نشده. لطفا ابتدا شماره موبایل خود را تایید کنید");

            }

            //Check IsLockedOut
            if (await _userManager.IsLockedOutAsync(user))
            {
                return Unauthorized("حساب کاربری شما به علت بیش از حد وارد کردن رمز عبور اشتباه تا پنج دقیقه قفل میباشد");
            }

            if (model.LoginType == LoginType.ByOTP)
            {
                //Confirmation phoneNumber
                string code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);

                //Send code To user by phoneNumber
                //code...
                string bodyMessage = $"کد زیر را جهت تایید حساب کاربری خود در قسمت مربوطه وارد کنید<br/><h3>{code}</h3>";
                var resultSendPhoneNumber = await _smsService.SendSmsAsync(model.PhoneNumber, bodyMessage);

                //HATEOAS links
                Link link = new Link
                {
                    For = nameof(VerifyOTP),
                    HttpMethod = HttpMethod.Post.ToString(),
                    Url = Url.Action(nameof(VerifyOTP), nameof(AccountController).Replace("Controller", ""), null, protocol: Request.Scheme)
                };

                return Ok(new { Code = code, Link = link });
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
                return Unauthorized("حساب کاربری شما به علت بیش از حد وارد کردن رمز عبور اشتباه تا پنج دقیقه قفل میباشد");
            }
            //else if (resultLogin.IsNotAllowed)
            //{
            //    return Unauthorized("حساب کاربری شما تایید نشده. لطفا ابتدا شماره موبایل خود را تایید کنید");
            //}
            else
            {
                return Unauthorized("شماره موبایل یا رمز عبور اشتباه است");
            }
        }

        /// <summary>
        /// چک کردن رمز یکبار مصرف
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(VerifyOTPDto model)
        {
            //Find user
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (user == null)
            {
                return NotFound();
            }

            //Check verify Code of OTP
            var resultConfirm = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, model.Code);
            if (resultConfirm)
            {

                //Build and Get tokes
                var tokens = await CreateNewTokenForUser(user);

                return Ok(tokens);
            }
            else
            {
                return Unauthorized("شماره موبایل یا رمز موقت است");
            }
        }



        /// <summary>
        ///ارسال کد به شماره موبایل برای تایید حساب کاربری
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
            string code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, PhoneNumber);

            //Send code To user by phoneNumber
            //code...
            string bodyMessage = $"کد زیر را جهت تایید حساب کاربری خود در قسمت مربوطه وارد کنید<br/><h3>{code}</h3>";
            var resultSendPhoneNumber = await _smsService.SendSmsAsync(PhoneNumber, bodyMessage);

            //HATEOAS links
            Link link = new Link
            {
                For = nameof(VerifyPhoneNumber),
                HttpMethod = HttpMethod.Post.ToString(),
                Url = Url.Action(nameof(VerifyPhoneNumber), nameof(AccountController).Replace("Controller", ""), null, protocol: Request.Scheme)
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
            var resultConfirm = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Code, model.PhoneNumber);
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
        /// رفرش توکن
        /// </summary>
        /// <param name="RefreshToken"></param>
        /// <returns></returns>
        [HttpGet("{RefreshToken}")]
        public async Task<IActionResult> RefreshToken(string RefreshToken)
        {
            //Find token by refresh token
            var securityHasher = new SecurityHasher();
            var token = _userTokenService.FindTokenByRefreshToken(securityHasher.GetSha256Hash(RefreshToken));

            ////Check refresh token
            //Check exist refresh token
            if (token == null)
            {
                return Unauthorized("رفرش توکن ارسال شده موجود نیست");
            }
            //Check expire refresh token
            if (token.RefreshTokenExpireTime < DateTime.Now)
            {
                return Unauthorized("زمان انقضای رفرش توکن به اتمام رسیده");
            }

            //Delete old token
            _userTokenService.DeleteToken(token);

            //Create new Token
            var tokens = await CreateNewTokenForUser(token.User);

            return Ok(tokens);
        }

        /// <summary>
        /// برای خروج از حساب کاربری (Auth)
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            //For find token
            var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer", null)?.Trim();

            if (string.IsNullOrEmpty(token))
            {
                return Ok();
            }

            //Delete it
            _userTokenService.DeleteToken(new SecurityHasher().GetSha256Hash(token));

            return Ok();
        }


        /// <summary>
        /// تغییر رمز عبور (Auth)
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            //Find user by claims
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;
            User user;
            if (userId != null)
            {
                user = await _userManager.FindByIdAsync(userId);
            }
            else
            {
                return BadRequest();
            }

            //Change password user
            var resultChangePassword = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
                                                                            changePasswordDto.NewPassword);
            //Check success changes
            if (resultChangePassword.Succeeded)
            {
                return Ok();
            }
            else
            {
                //Return error if unsuccess
                var error = resultChangePassword.Errors?.Select(p => p.Description)?.Aggregate((p1, p2) => p1 + "+" + p2);
                return BadRequest(error);
            }
        }


        /// <summary>
        /// فراموشی رمز عبور
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        [HttpGet("{PhoneNumber}")]
        public async Task<IActionResult> ForgetPassword(string PhoneNumber)
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

            //Build new password by random class
            int newPassword = new Random().Next(100000, 999999);

            //Save in user account
            var resultRemoveOldPass = await _userManager.RemovePasswordAsync(user);

            //Check was successed
            if (resultRemoveOldPass.Succeeded)
            {
                var resultAddNewPass = await _userManager.AddPasswordAsync(user, newPassword.ToString());

                //Return error add new password if UnSuccess
                if (resultAddNewPass.Succeeded == false)
                {
                    var error = resultAddNewPass.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                    return BadRequest(error);
                }
            }
            else
            {
                //Return error remove old password
                var error = resultRemoveOldPass.Errors.Select(p => p.Description).Aggregate((p1, p2) => p1 + "," + p2);
                return BadRequest(error);
            }

            ////Changes was successed
            //Send new password for user by PhoneNumber
            //Code...
            string bodyMessage = $"کد زیر رمز عبور جدید شما در سایت است. لطفا پس از ورود رمز خود را تغییر دهید   {newPassword}   ";
            var resultSendSms = await _smsService.SendSmsAsync(user.PhoneNumber, bodyMessage);

            //HATEOAS
            List<Link> links = new List<Link>
            {
                new Link
                {
                    For="Login",
                    HttpMethod=HttpMethod.Post.ToString(),
                    Url= Url.Action(nameof(Login),"Account",null,Request.Scheme)
                },
                new Link
                {
                    For="ChangePassword",
                    HttpMethod=HttpMethod.Post.ToString(),
                    Url=Url.Action(nameof(ChangePassword),"Account",null,Request.Scheme)
                }
            };

            //Message for user
            string message = "رمز عبور جدید به شماره موبایل شما ارسال شد. لطفا پس از ورود رمز عبور خود را تغییر دهید" + newPassword;

            return Ok(new { Message = message, Links = links });
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
                    new Claim("Email",user.Email is null?"":user.Email),
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
