using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tameenk.AutoLeasing.Identity.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName) where TModel : new()
        {
            var options = new TModel();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}
