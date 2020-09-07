using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


namespace Tameenk.AutoLeasing.IdentityAPI
{

    public class AuthenticateFilterAttribute : ActionFilterAttribute
    {
        public string Role { get; set; }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var claims = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Roles").Value.Replace(" ","");
            var allowedPages = JsonConvert.DeserializeObject<List<string>>(claims);
            if (!allowedPages.Contains(Role))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            return;             
        } 
    }

}
