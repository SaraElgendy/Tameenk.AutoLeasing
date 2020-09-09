using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Tameenk.AutoLeasing.Identity.Domain;
using Tameenk.AutoLeasing.Identity;
using Tameenk.AutoLeasing.Resources;
using System.Net;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Tameenk.Common.Utilities;

namespace Tameenk.AutoLeasing.IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdminController : ControllerBase
    {
        #region constr & probs
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminService adminService;
        // private readonly IMapper mapper;
        private readonly IdentitySettings IdentitySettings;
        private readonly IConfiguration configuration;
        // private readonly ISmsService smsService;
        // private readonly IAdminRoleService AdminRoleService;
        private readonly IHttpContextAccessor httpContext;
        public AdminController(SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager,
             IOptions<IdentitySettings> identitySettings,
             IHttpContextAccessor httpContext,
             IConfiguration configuration,
             IAdminService adminService)
        {

            this.adminService = adminService;
            this.SignInManager = signInManager;
            this.userManager = userManager;

            this.IdentitySettings = identitySettings.Value;
            this.configuration = configuration;

            this.httpContext = httpContext;
        }
        #endregion

        #region User Methods

        [HttpPost("AddUser")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<UserModel>))]
        //[AuthenticateFilter(Role = "AddUser")]
        public async Task<IActionResult> AddUser(UserModel model)
        {
            DateTime dtBeforeCalling = DateTime.Now;
            AdminRequestLog log = new AdminRequestLog();
            log.UserIP = Utilities.GetUserIPAddress();
            log.ServerIP = Utilities.GetInternalServerIP();
            log.UserAgent = Utilities.GetUserAgent();
            log.PageName = "Create-User";
            log.PageURL = "/admin/create-user";
           // log.ApiURL = Utilities.GetCurrentURL(httpContext);
            log.MethodName = "AddUser";
            log.UserID = User.GetUserId();
            log.UserName = User.GetUserName();
            var output = new Output<UserModel>();
            try
            {
                output = model.IsValid<UserModel, UserModel>();
                if (!ModelState.IsValid)
                {

                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }

                foreach (var company in model.Companies)
                {
                    if (!InsuranceCompanyService.CheckCompanyExist(company))
                    {
                        output.ErrorCode = Output<UserModel>.ErrorCodes.CompanyNotExist;
                        output.ErrorDescription = ResourcesHepler.GetMessage("CompanyNotExists", model.Language); ;
                        log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                        log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                        log.ErrorDescription = output.ErrorDescription;
                        log.ErrorCode = (int)output.ErrorCode;
                        LogService.AddAdminRequestLogs(log);
                        return Ok(output);
                    }

                }
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await userManager.CreateAsync(user, model.Userpassword);
                output = result.IsValidResult<UserModel>(model.Language, out string errorKey);
                if (output.ErrorCode == Output<UserModel>.ErrorCodes.Success)
                {
                    output.ErrorDescription = ResourcesHepler.GetMessage("Success", model.Language);
                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ErrorCode = (int)output.ErrorCode;
                    LogService.AddAdminRequestLogs(log);
                    return Ok(output);
                }
                output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = "Failed to create user";
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return Ok(output);

            }
            catch (Exception ex)
            {
                output.ErrorCode = Output<UserModel>.ErrorCodes.ServerException;
                output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
                log.ErrorDescription = ex.ToString();
                log.ErrorCode = (int)output.ErrorCode;
                LogService.AddAdminRequestLogs(log);
                return Ok(output);
            }
        }

        //    [HttpPost]
        //    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<UserModel>))]
        //    [AuthenticateFilter(Role = "EditUser")]
        //    public async Task<IActionResult> EditUser(EditUserModel model)
        //    {
        //        DateTime dtBeforeCalling = DateTime.Now;
        //        AdminRequestLog log = new AdminRequestLog();
        //        log.UserIP = Utilities.Utilities.GetUserIPAddress();
        //        log.ServerIP = Utilities.Utilities.GetInternalServerIP();
        //        log.UserAgent = Utilities.Utilities.GetUserAgent();
        //        log.PageName = "Edit-User";
        //        log.PageURL = "/admin/editUser";
        //        log.ApiURL = Utilities.Utilities.GetCurrentURL(httpContext);
        //        log.MethodName = "EditUser";
        //        log.UserID = User.GetUserId();
        //        log.UserName = User.GetUserName();
        //        var output = new Output<UserModel>();
        //        try
        //        {
        //            output = model.IsValid<EditUserModel, UserModel>();
        //            if (output.ErrorCode != Output<UserModel>.ErrorCodes.Success)
        //            {
        //                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                log.ErrorDescription = output.ErrorDescription;
        //                log.ErrorCode = (int)output.ErrorCode;
        //                LogService.AddAdminRequestLogs(log);
        //                return Ok(output);
        //            }
        //            var user = userManager.FindByIdAsync(model.Id).Result;
        //            user.Email = model.Email;
        //            user.UserName = model.UserName;
        //            user.PhoneNumber = model.PhoneNumber;
        //            user.SecurityStamp = new Random().Next(100000).ToString();
        //            var result = await userManager.UpdateAsync(user);
        //            output = result.IsValidResult<UserModel>(model.Language, out string errorKey);
        //            if (output.ErrorCode == Output<UserModel>.ErrorCodes.Success)
        //            {
        //                output.ErrorDescription = ResourcesHepler.GetMessage("Success", model.Language);
        //                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                log.ErrorDescription = "Success";
        //                log.ErrorCode = (int)output.ErrorCode;
        //                LogService.AddAdminRequestLogs(log);
        //                return Ok(output);
        //            }
        //            output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = "Failed";
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);

        //        }
        //        catch (Exception ex)
        //        {
        //            output.ErrorCode = Output<UserModel>.ErrorCodes.ServerException;
        //            output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = ex.ToString();
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);
        //        }
        //    }

        //    [HttpPost]
        //    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<UserModel>))]
        //    [AuthenticateFilter(Role = "Resetpassword")]
        //    public async Task<IActionResult> Resetpassword(ResetPasswordModel model)
        //    {
        //        DateTime dtBeforeCalling = DateTime.Now;
        //        AdminRequestLog log = new AdminRequestLog();
        //        log.UserIP = Utilities.Utilities.GetUserIPAddress();
        //        log.ServerIP = Utilities.Utilities.GetInternalServerIP();
        //        log.UserAgent = Utilities.Utilities.GetUserAgent();
        //        log.PageName = "Resetpassword";
        //        log.PageURL = "/admin/reset-password";
        //        log.ApiURL = Utilities.Utilities.GetCurrentURL(httpContext);
        //        log.MethodName = "Resetpassword";
        //        log.UserID = User.GetUserId();
        //        log.UserName = User.GetUserName();
        //        var output = new Output<UserModel>();
        //        try
        //        {
        //            output = model.IsValid<ResetPasswordModel, UserModel>();
        //            if (output.ErrorCode != Output<UserModel>.ErrorCodes.Success)
        //            {
        //                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                log.ErrorDescription = output.ErrorDescription;
        //                log.ErrorCode = (int)output.ErrorCode;
        //                LogService.AddAdminRequestLogs(log);
        //                return Ok(output);
        //            }
        //            var user = userManager.FindByIdAsync(model.Id).Result;
        //            if (user == null)
        //            {
        //                output.ErrorCode = Output<UserModel>.ErrorCodes.NotFound;
        //                output.ErrorDescription = ResourcesHepler.GetMessage("UserNotFound", model.Language);
        //                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                log.ErrorDescription = "UserNotFound";
        //                log.ErrorCode = (int)output.ErrorCode;
        //                LogService.AddAdminRequestLogs(log);
        //                return Ok(output);
        //            }
        //            user.PasswordConfirmed = false;
        //            user.SecurityStamp = new Random().Next(100000).ToString();
        //            string token = userManager.GeneratePasswordResetTokenAsync(user).Result;
        //            var result = await userManager.ResetPasswordAsync(user, token, model.Userpassword);
        //            output = result.IsValidResult<UserModel>(model.Language, out string errorKey);
        //            if (output.ErrorCode == Output<UserModel>.ErrorCodes.Success)
        //            {
        //                var smsServiceResult = new SendSmsOutput() { StatusCode = Tameenk.SMS.Component.StatusCode.Success }; //smsService.SendSMS("Medical", "123456", "BCare", user.PhoneNumber, $"Your New Password: " + model.Password);
        //                if (smsServiceResult.StatusCode != Tameenk.SMS.Component.StatusCode.Success)
        //                {
        //                    output.ErrorCode = Output<UserModel>.ErrorCodes.Failed;
        //                    output.ErrorDescription = ResourcesHepler.GetMessage("ResetPasswordFailedMsg", model.Language);
        //                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                    log.ErrorDescription = "FailedToSendSms";
        //                    log.ErrorCode = (int)output.ErrorCode;
        //                    LogService.AddAdminRequestLogs(log);
        //                }
        //                else
        //                {
        //                    output.ErrorCode = Output<UserModel>.ErrorCodes.Success;
        //                    output.ErrorDescription = ResourcesHepler.GetMessage("Success", model.Language);
        //                    log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                    log.ErrorDescription = "Success";
        //                    log.ErrorCode = (int)output.ErrorCode;
        //                    LogService.AddAdminRequestLogs(log);
        //                }
        //            }
        //            return Ok(output);
        //        }
        //        catch (Exception ex)
        //        {
        //            output.ErrorCode = Output<UserModel>.ErrorCodes.ServerException;
        //            output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = ex.ToString();
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);
        //        }
        //    }



        //    [HttpPost]
        //    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<UserListOutput>))]
        //    [AuthenticateFilter(Role = "AdminUsers")]
        //    public IActionResult GetUsersList([FromBody] UserFilterModel model)
        //    {
        //        DateTime dtBeforeCalling = DateTime.Now;
        //        AdminRequestLog log = new AdminRequestLog();
        //        log.UserIP = Utilities.Utilities.GetUserIPAddress();
        //        log.ServerIP = Utilities.Utilities.GetInternalServerIP();
        //        log.UserAgent = Utilities.Utilities.GetUserAgent();
        //        log.PageName = "UsersList";
        //        log.PageURL = "/admin/adminusers";
        //        log.ApiURL = Utilities.Utilities.GetCurrentURL(httpContext);
        //        log.MethodName = "GetUsersList";
        //        log.UserID = User.GetUserId();
        //        log.UserName = User.GetUserName();
        //        var output = new Output<UserListOutput>();
        //        try
        //        {
        //            var result = adminService.GetUserWithFilter(model);
        //            output.Result = new UserListOutput();
        //            output.Result.List = mapper.Map<List<ApplicationUser>, List<UserOutput>>(result.ToList());
        //            var userid = User.GetUserId();
        //            output.Result.List = output.Result.List.ToList();
        //            output.Result.PageSize = result.PageSize;
        //            output.Result.HasNextPage = result.HasNextPage;
        //            output.Result.HasPreviousPage = result.HasPreviousPage;
        //            output.Result.PageIndex = result.PageIndex;
        //            output.Result.TotalCount = result.TotalCount;
        //            output.Result.TotalPages = result.TotalPages;
        //            output.ErrorCode = Output<UserListOutput>.ErrorCodes.Success;
        //            output.ErrorDescription = ResourcesHepler.GetMessage("Success", model.Language);
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = "Success";
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);
        //        }
        //        catch (Exception ex)
        //        {
        //            output.ErrorCode = Output<UserListOutput>.ErrorCodes.ServerException;
        //            output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", model.Language);
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = ex.ToString();
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);
        //        }
        //    }


        //    [HttpGet]
        //    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<int>))]
        //    [AuthenticateFilter(Role = "AdminUsers")]
        //    public IActionResult ChangeUserStatus(string id, bool status)
        //    {
        //        DateTime dtBeforeCalling = DateTime.Now;
        //        AdminRequestLog log = new AdminRequestLog();
        //        log.UserIP = Utilities.Utilities.GetUserIPAddress();
        //        log.ServerIP = Utilities.Utilities.GetInternalServerIP();
        //        log.UserAgent = Utilities.Utilities.GetUserAgent();
        //        log.PageName = "UsersList";
        //        log.PageURL = "/admin/adminusers";
        //        log.ApiURL = Utilities.Utilities.GetCurrentURL(httpContext);
        //        log.MethodName = "ChangeUserStatus";
        //        log.UserID = User.GetUserId();
        //        log.UserName = User.GetUserName();
        //        var output = new Output<int>();
        //        try
        //        {
        //            output = adminService.ChangeUserStatus(id, status);
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = output.ErrorDescription;
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);
        //        }
        //        catch (Exception ex)
        //        {
        //            output.ErrorCode = Output<int>.ErrorCodes.ServerException;
        //            output.ErrorDescription = "ServerError";
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = ex.Message;
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);
        //        }

        //    }


        //    [HttpPost]
        //    [AuthenticateFilter(Role = "AdminUsers")]
        //    public IActionResult ExportUserList(UserFilterModel model)
        //    {
        //        DateTime dtBeforeCalling = DateTime.Now;
        //        AdminRequestLog log = new AdminRequestLog();
        //        log.UserIP = Utilities.Utilities.GetUserIPAddress();
        //        log.ServerIP = Utilities.Utilities.GetInternalServerIP();
        //        log.UserAgent = Utilities.Utilities.GetUserAgent();
        //        log.PageName = "UsersList";
        //        log.PageURL = "/admin/adminusers";
        //        log.ApiURL = Utilities.Utilities.GetCurrentURL(httpContext);
        //        log.MethodName = "ExportUserList";
        //        log.UserID = User.GetUserId();
        //        log.UserName = User.GetUserName();
        //        try
        //        {
        //            string filename = "Data.xlsx";
        //            var result = adminService.ExportUserWithFilter(model);
        //            var data = new List<ExportUserOutput>();
        //            int index = 0;
        //            foreach (var item in result)
        //            {
        //                index++;
        //                data.Add(new ExportUserOutput
        //                {
        //                    Id = index.ToString(),
        //                    Email = item.Email,
        //                    UserName = item.UserName,
        //                    PhoneNumber = item.PhoneNumber
        //                });
        //            }
        //            var memoryStream = ExcelService.CreateFile(model.Language, data);
        //            if (memoryStream != null)
        //            {
        //                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                log.ErrorDescription = "Success";
        //                log.ErrorCode = 1;
        //                LogService.AddAdminRequestLogs(log);
        //                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //            }
        //            return NotFound();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = ex.Message;
        //            log.ErrorCode = (int)Output<int>.ErrorCodes.ServerException;
        //            LogService.AddAdminRequestLogs(log);
        //            return NotFound();
        //        }
        //    }

        //    [HttpGet]
        //    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Output<ApplicationUser>))]
        //    [AuthenticateFilter(Role = "EditUser")]
        //    public IActionResult GetUserById(string id)
        //    {
        //        DateTime dtBeforeCalling = DateTime.Now;
        //        AdminRequestLog log = new AdminRequestLog();
        //        log.UserIP = Utilities.Utilities.GetUserIPAddress();
        //        log.ServerIP = Utilities.Utilities.GetInternalServerIP();
        //        log.UserAgent = Utilities.Utilities.GetUserAgent();
        //        log.PageName = "EditUser";
        //        log.PageURL = "/admin/editUser";
        //        log.ApiURL = Utilities.Utilities.GetCurrentURL(httpContext);
        //        log.MethodName = "GetUserById";
        //        log.UserID = User.GetUserId();
        //        log.UserName = User.GetUserName();
        //        var output = new Output<ApplicationUser>();
        //        try
        //        {
        //            var result = adminService.GetUserById(id);
        //            if (result != null)
        //            {
        //                output.Result = result;
        //                output.ErrorCode = Output<ApplicationUser>.ErrorCodes.Success;
        //                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                log.ErrorDescription = "Success";
        //                log.ErrorCode = (int)output.ErrorCode;
        //                LogService.AddAdminRequestLogs(log);
        //                return Ok(output);
        //            }
        //            else
        //            {
        //                output.ErrorDescription = ResourcesHepler.GetMessage("UserNotFound", "ar");
        //                output.ErrorCode = Output<ApplicationUser>.ErrorCodes.NotFound;
        //                log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //                log.ErrorDescription = "NotFound";
        //                log.ErrorCode = (int)output.ErrorCode;
        //                LogService.AddAdminRequestLogs(log);
        //                return Ok(output);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            output.ErrorCode = Output<ApplicationUser>.ErrorCodes.ServerException;
        //            output.ErrorDescription = ResourcesHepler.GetMessage("ServerError", "en");
        //            log.ServiceResponseTimeInSeconds = DateTime.Now.Subtract(dtBeforeCalling).TotalSeconds;
        //            log.ErrorDescription = ex.Message;
        //            log.ErrorCode = (int)output.ErrorCode;
        //            LogService.AddAdminRequestLogs(log);
        //            return Ok(output);
        //        }
        //    }

        #endregion



    }
}