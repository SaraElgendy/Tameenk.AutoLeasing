using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using IdentitySettings = Tameenk.AutoLeasing.Identity.IdentitySettings;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Tameenk.AutoLeasing.Identity;
using Tameenk.AutoLeasing.Identity.Domain;
using Tameenk.AutoLeasing.Resources;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Tameenk.Common.Utilities;

namespace Tameenk.AutoLeasing.IdentityAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminService adminService;
        private readonly IdentitySettings IdentitySettings;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;

        public AccountController(SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager,

             IOptions<IdentitySettings> identitySettings,

             IConfiguration configuration,
             IHttpContextAccessor httpContext,
             IAdminService adminService
           )
        {
            this.adminService = adminService;
            this.SignInManager = signInManager;
            this.userManager = userManager;
            this.IdentitySettings = identitySettings.Value;
            this.configuration = configuration;
            this.httpContext = httpContext;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<LoginOutput>))]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                DateTime dtBeforeCalling = DateTime.Now;
                AdminRequestLog log = new AdminRequestLog();
                //log.UserIP = Utilities.GetUserIPAddress();
               // log.ServerIP = Utilities.GetInternalServerIP();
                //log.UserAgent = Utilities.GetUserAgent();
                log.PageName = "Login";
                log.PageURL = "/login";
                //log.ApiURL = Utilities.GetCurrentURL(httpContext);
                log.MethodName = "Login";
                log.UserID = User.GetUserId();
                log.UserName = User.GetUserName();
                var output = new Output<LoginOutput>();
                try
                {

                    var user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null && !user.IsActive)
                    {
                        output.ErrorCode = Output<LoginOutput>.ErrorCodes.NotValid;
                        output.ErrorDescription = ResourcesHepler.GetMessage("UserIsBlocked", model.Language); ;
                        return Ok(output);
                    }
                    if (model.Language != "en" && model.Language != "ar")
                    {
                        output.ErrorCode = Output<LoginOutput>.ErrorCodes.NoValidCulture;
                        output.ErrorDescription = ResourcesHepler.GetMessage("InValidCulture", "en");
                        return Ok(output);
                    }
                    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        if (!user.PasswordConfirmed)
                        {
                            output.ErrorCode = Output<LoginOutput>.ErrorCodes.ChangePassword;
                            return Ok(output);
                        }
                        var oneTimePassword = RandomOneTimePassword();
                        user.OneTimePassword = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(oneTimePassword)));
                        user.OneTimePasswordExpirationDate = DateTime.UtcNow.AddMinutes(double.Parse(configuration.GetSection("OneTimePasswordExpirationPeriodInMinutes").Value));
                        await userManager.UpdateAsync(user);
                        //var smsServiceResult = new SendSmsOutput() { StatusCode = Tameenk.SMS.Component.StatusCode.Success };// smsService.SendSMS("Medical", "123456", "BCare", user.PhoneNumber, $"Your One Time Password: {oneTimePassword}");
                        //if (smsServiceResult.StatusCode != Tameenk.SMS.Component.StatusCode.Success)
                        //{
                        //    output.ErrorCode = Output<LoginOutput>.ErrorCodes.Failed;
                        //    output.ErrorDescription = ResourcesHepler.GetMessage("OneTimePasswordSendFailedMsg", model.Language);
                        //    log.ErrorDescription = "Failed to send one time password";
                        //    log.ErrorCode = (int)output.ErrorCode;
                        //}
                        //else
                        //{
                        output.ErrorCode = Output<LoginOutput>.ErrorCodes.Success;
                        // output.ErrorDescription = ResourcesHepler.GetMessage("OneTimePasswordSendFailedMsg", model.Language);
                        output.Result = new LoginOutput { Email = user.Email, TempPassword = oneTimePassword };
                        log.ErrorDescription = "one time password Successfully sent";
                        log.ErrorCode = (int)output.ErrorCode;
                        //}
                        log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                        LogService.AddAdminRequestLogs(log);
                        return Ok(output);
                    }
                    output.ErrorCode = Output<LoginOutput>.ErrorCodes.NotFound;
                    output.ErrorDescription = ResourcesHepler.GetMessage("LoginNotCorrect", model.Language);
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = "Login data not correct";
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
                catch (Exception ex)
                {
                    output.ErrorCode = Output<LoginOutput>.ErrorCodes.ServerException;
                    output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = ex.ToString();
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOneTimePassword(OneTimePasswordModel model)
        {
            DateTime dtBeforeCalling = DateTime.Now;
            AdminRequestLog log = new AdminRequestLog();

            log.UserIP = Utilities.GetUserIPAddress();
            log.ServerIP = Utilities.GetInternalServerIP();
            log.UserAgent = Utilities.GetUserAgent();
            log.PageName = "VerifyOneTimePassword";
            log.PageURL = "/VerifyOneTimePassword";
            // log.ApiURL = Utilities.GetCurrentURL(httpContext);
            log.MethodName = "VerifyOneTimePassword";
            log.UserID = User.GetUserId();
            log.UserName = User.GetUserName();
            var output = new Output<LoginOutput>();
            try
            {
                output = model.IsValid<OneTimePasswordModel, LoginOutput>();
                if (output.ErrorCode != Output<LoginOutput>.ErrorCodes.Success)
                {
                    return Ok(output);
                }
                var user = adminService.GetUserByEmail(model);
                if (user == null)
                {
                    output.ErrorCode = Output<LoginOutput>.ErrorCodes.NotFound;
                    output.ErrorDescription = ResourcesHepler.GetMessage("LoginNotCorrect", model.Language);
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = "Login data not correct";
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
                if (user.OneTimePasswordExpirationDate <= DateTime.UtcNow)
                {
                    output.ErrorCode = Output<LoginOutput>.ErrorCodes.InvalidData;
                    output.ErrorDescription = ResourcesHepler.GetMessage("OneTimePasswordExpired", model.Language);
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = "OneTime Password Expired";
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
                await SignInManager.SignInAsync(user, true, null);
                var UserRoles = adminService.GetUserRoles(user.Id).OrderBy(x => x.Order).ToList();
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = "Logged in successfully";
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return GetToken(user, UserRoles);
            }
            catch (Exception ex)
            {
                output.ErrorCode = Output<LoginOutput>.ErrorCodes.ServerException;
                output.ErrorDescription = ResourcesHepler.GetMessage("ServerException", model.Language);
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = ex.ToString();
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return Ok(output);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            DateTime dtBeforeCalling = DateTime.Now;
            AdminRequestLog log = new AdminRequestLog();
            log.UserIP = Utilities.GetUserIPAddress();
            log.ServerIP = Utilities.GetInternalServerIP();
            log.UserAgent = Utilities.GetUserAgent();
            log.PageName = "ChangePassword";
            log.PageURL = "/ChangePassword";
            // log.ApiURL = Utilities.GetCurrentURL(httpContext);
            log.MethodName = "ChangePassword";
            log.UserID = User.GetUserId();
            log.UserName = User.GetUserName();
            var output = new Output<LoginOutput>();
            try
            {
                output = model.IsValid<ChangePasswordModel, LoginOutput>();
                if (output.ErrorCode != Output<LoginOutput>.ErrorCodes.Success)
                {
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
                var user = userManager.FindByEmailAsync(model.Email).Result;
                if (user == null)
                {
                    output.ErrorCode = Output<LoginOutput>.ErrorCodes.NotFound;
                    output.ErrorDescription = ResourcesHepler.GetMessage("UserNotFound", model.Language);
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = "User not found";
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
                user.PasswordConfirmed = true;
                string token = userManager.GeneratePasswordResetTokenAsync(user).Result;
                var res = await userManager.ResetPasswordAsync(user, token, model.Password);
                if (res.Succeeded)
                {
                    output.ErrorCode = Output<LoginOutput>.ErrorCodes.Success;
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = "password changed successfully";
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
                output.ErrorCode = Output<LoginOutput>.ErrorCodes.Failed;
                output.ErrorDescription = "changePasswordFailed";
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = "failed to change password";
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return Ok(output);
            }
            catch (Exception ex)
            {
                output.ErrorCode = Output<LoginOutput>.ErrorCodes.ServerException;
                output.ErrorDescription = ResourcesHepler.GetMessage("ServerException", model.Language);
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = ex.ToString();
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return Ok(output);
            }
        }


        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<string>))]
        public async Task<IActionResult> logout(LogoutViewModel model)
        {
            DateTime dtBeforeCalling = DateTime.Now;
            AdminRequestLog log = new AdminRequestLog();
            log.UserIP = Utilities.GetUserIPAddress();
            log.ServerIP = Utilities.GetInternalServerIP();
            log.UserAgent = Utilities.GetUserAgent();
            log.PageName = "logout";
            log.PageURL = "/logout";
            //log.ApiURL = Utilities.GetCurrentURL(httpContext);
            log.MethodName = "logout";
            log.UserID = User.GetUserId();
            log.UserName = User.GetUserName();
            var output = new Output<string>();
            try
            {
                output.ErrorCode = Output<string>.ErrorCodes.Success;
                output.ErrorDescription = ResourcesHepler.GetMessage("Success", model.Language);
                await SignInManager.SignOutAsync();
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = output.ErrorDescription;
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return Ok(output);
            }
            catch (Exception ex)
            {
                output.ErrorCode = Output<string>.ErrorCodes.ServerException;
                output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = ex.ToString();
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return Ok(output);
            }

        }
        private string RandomOneTimePassword()
        {
            var oneTimePassword = "";
            for (int index = 0; index < 6; index++)
            {
                oneTimePassword += $"{new Random().Next(0, 10)}";
            }
            return oneTimePassword;
        }
        private IActionResult GetToken(ApplicationUser user, List<ApplicationRole> roles)
        {
            var output = new Output<AdminLoginOutput>();
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                              {
                        new Claim("User" , JsonConvert.SerializeObject( user)) ,
                        new Claim("UserId",user.Id.ToString()),
                        new Claim("Email",user.Email.ToString()),
                        new Claim("UserName",user.UserName.ToString()) ,
                        new Claim("Roles" ,JsonConvert.SerializeObject( roles.Select(x=>x.Name )))
                              }),
                    Expires = DateTime.UtcNow.AddMinutes(IdentitySettings.Expires),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IdentitySettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                output.ErrorCode = Output<AdminLoginOutput>.ErrorCodes.Success;
                output.Result = new AdminLoginOutput
                {
                    Token = token,
                    Expires = IdentitySettings.Expires,
                    Email = user.Email,
                    UserName = user.UserName,
                    UserId = user.Id,
                    ExpiryDate = DateTime.Now.AddMinutes(IdentitySettings.Expires),
                    Roles = new List<LoginRole>()
                };
                foreach (var item in roles)
                {
                    output.Result.Roles.Add(new LoginRole
                    {
                        Icon = item.Icon,
                        TitleAr = item.TitleAr,
                        TitleEn = item.TitleEn,
                        ModuleAr = item.ModuleAr,
                        ModuleEn = item.ModuleEn,
                        Order = item.Order,
                        Url = item.RelativeUrl
                    });
                }
                return Ok(output);
            }
            catch (Exception ex)
            {
                output.ErrorCode = Output<AdminLoginOutput>.ErrorCodes.ServerException;
                output.ErrorDescription = ex.InnerException?.Message;
                return Ok(output);
            }
        }
    }
}