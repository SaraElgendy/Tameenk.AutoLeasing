using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tameenk.AutoLeasing.Resources.Messages;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public static class ModelValidatorsExtensions
    {
        public static Output<TOutput> IsValidResult<TOutput>(this IdentityResult identityResult, string lang, out string errorKey)
        {
            errorKey = "";
            Output<TOutput> output = new Output<TOutput>();
            if (!identityResult.Succeeded)
            {
                var error = identityResult.Errors.FirstOrDefault();
                errorKey = error.Code;
                output.ErrorCode = Output<TOutput>.ErrorCodes.NotSuccess;
                output.ErrorDescription = Messages.ResourceManager.GetString(error.Code, new CultureInfo(lang));
                // output.ErrorDescription = Messages.ResourceManager.GetString("ServerError", new CultureInfo(lang));
                return output;
            }
            output.ErrorCode = Output<TOutput>.ErrorCodes.Success;
            return output;
        }
    }
}
