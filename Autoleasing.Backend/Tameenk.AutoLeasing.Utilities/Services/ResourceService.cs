using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace Tameenk.AutoLeasing.Utilities
{
    public class ResourceService
    {
        public Dictionary<string, string> GetResources(Type type, string lang)
        {
            var Binding = new Dictionary<string, string>();
            ResourceManager MyResource = new ResourceManager(type);
            ResourceSet resourceSet = MyResource.GetResourceSet(CultureInfo.CreateSpecificCulture(lang), true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                Binding.Add(entry.Key.ToString(), entry.Value.ToString());
            }
            return Binding;
        }
        public  string GetResourcesName(Type type, string key,  string lang)
        {
            var Binding = new Dictionary<string, string>();
            ResourceManager MyResource = new ResourceManager(type);
            string resoucename = MyResource.GetString(key , CultureInfo.CreateSpecificCulture(lang));
             
            return resoucename;
        }
    }
}
