using System.Globalization;
using System.Resources;

namespace Tameenk.AutoLeasing.Resources
{
    public class ResourcesHepler
    {
        public static string GetMessage(string key, string lang)
        {
            return Messages.Messages.ResourceManager.GetString(key, new CultureInfo(lang));
        }

        public static string GetMessage(string key, string lang, ResourceManager resourceManager)
        {
            return resourceManager.GetString(key, new CultureInfo(lang));
        }


        //public static string GetTranslation(string key, string lang)
        //{
        //    return Translator.Translator.ResourceManager.GetString(key, new CultureInfo(lang));
        //}

    }
}
