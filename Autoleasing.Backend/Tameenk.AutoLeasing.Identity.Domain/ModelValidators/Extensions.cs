
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using Tameenk.AutoLeasing.Resources.Messages;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public static class Extensions
    {
        public static Output<TOutput> IsValid<TModel, TOutput>(this ModelBase modelBase , ResourceManager resourceManager = null)
        {
            if (resourceManager == null)
                resourceManager = Messages.ResourceManager;
            var output = modelBase.IsRequiredValid<TModel, TOutput>(resourceManager);
            if(output.ErrorCode == Output<TOutput>.ErrorCodes.Success)
                output = modelBase.IsCompareValid<TModel, TOutput>(resourceManager);
            if (output.ErrorCode == Output<TOutput>.ErrorCodes.Success)
                output = modelBase.IsRegexValid<TModel, TOutput>(resourceManager);
            return output;
        }

       
        public static Output<TOutput> IsRequiredValid<TModel, TOutput>(this ModelBase modelBase, ResourceManager resourceManager)
        {
            Output<TOutput> output = new Output<TOutput>();
            PropertyInfo[] props = typeof(TModel).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(false);
                foreach (object attr in attrs)
                {
                    RequiredAttribute authAttr = attr as RequiredAttribute;
                    if (authAttr != null)
                    {
                        string ResourceName = authAttr.ErrorMessageResourceName;
                        var type = authAttr.ErrorMessageResourceType;
                        var isValid = authAttr.IsValid(prop.GetValue(modelBase));
                        if (!isValid)
                        {
                            output.ErrorCode = Output<TOutput>.ErrorCodes.EmptyInputParamter; 
                            output.ErrorDescription =  resourceManager.GetString(ResourceName, new CultureInfo(modelBase.Language));
                            output.LogDescription = resourceManager.GetString(ResourceName, new CultureInfo("en"));
                            return output;
                        }
                    }
                }
            }
            output.ErrorCode = Output<TOutput>.ErrorCodes.Success;
            return output;
        }

        public static Output<TOutput> IsCompareValid<TModel, TOutput>(this ModelBase modelBase , ResourceManager resourceManager)
        {
            Output<TOutput> output = new Output<TOutput>();
            PropertyInfo[] props = typeof(TModel).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    CompareAttribute authAttr = attr as CompareAttribute;
                    if (authAttr != null)
                    {
                        string ResourceName = authAttr.ErrorMessageResourceName;
                        var val = props.FirstOrDefault(x => x.Name == authAttr.OtherProperty).GetValue(modelBase);
                        var isValid = Equals(prop.GetValue(modelBase), val);
                        if (!isValid)
                        {
                            output.ErrorCode = Output<TOutput>.ErrorCodes.EmptyInputParamter;
                            output.ErrorDescription = resourceManager.GetString(ResourceName, new CultureInfo(modelBase.Language));
                            output.LogDescription = resourceManager.GetString(ResourceName, new CultureInfo("en"));
                            return output;
                        }
                    }
                }
            }
            output.ErrorCode = Output<TOutput>.ErrorCodes.Success;
            return output;
        }

        public static Output<TOutput> IsRegexValid<TModel, TOutput>(this ModelBase modelBase, ResourceManager resourceManager)
        {
            Output<TOutput> output = new Output<TOutput>();
            PropertyInfo[] props = typeof(TModel).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    var regexAttr = attr as RegexAttribute;
                    if (regexAttr != null)
                    {
                        string ResourceName = regexAttr.ErrorMessageResourceName;
                        var val = prop.GetValue(modelBase);
                        var isValid = regexAttr.IsValid(val);
                        if (!isValid)
                        {
                            output.ErrorCode = Output<TOutput>.ErrorCodes.NotValid;
                            output.ErrorDescription = resourceManager.GetString(ResourceName, new CultureInfo(modelBase.Language));
                            output.LogDescription = resourceManager.GetString(ResourceName, new CultureInfo("en"));
                            return output;
                        }
                    }
                    if (attr is RegularExpressionAttribute regex)
                    {
                        string ResourceName = regex.ErrorMessageResourceName;
                        var val = prop.GetValue(modelBase);
                        var isValid = regex.IsValid(val);
                        if (!isValid)
                        {
                            output.ErrorCode = Output<TOutput>.ErrorCodes.NotValid;
                            output.ErrorDescription = resourceManager.GetString(ResourceName, new CultureInfo(modelBase.Language));
                            output.LogDescription = resourceManager.GetString(ResourceName, new CultureInfo("en"));
                            return output;
                        }
                    }
                }
            }
            output.ErrorCode = Output<TOutput>.ErrorCodes.Success;
            return output;
        }
    }
}
