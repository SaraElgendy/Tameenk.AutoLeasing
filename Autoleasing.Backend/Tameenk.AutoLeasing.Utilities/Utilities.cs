using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Imaging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Web;

namespace Tameenk.AutoLeasing.Utilities
{

    public class Utilities
    {
        private static readonly HttpClientService httpClient = new HttpClientService();
        private const string InternationalPhoneCode = "00";
        private const string InternationalPhoneSymbol = "+";
        private const string Zero = "0";
        private const string SaudiInternationalPhoneCode = "966";


        //public static string GetNewReferenceId()
        //{
        //    string referenceId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
        //    return referenceId;
        //}
        //public static int GetNewInvoiceNo()
        //{
        //    Random random = new Random();
        //    int InvoiceNo = random.Next(111111111, 999999999);
        //    return InvoiceNo;
        //}

        public static string GetServerIP()
        {
            var ip = httpClient?.GetCurrentContext()?.Connection?.RemoteIpAddress?.ToString();
            return ip;
        }
        public static string GetUserIP()
        {
            var ip = httpClient?.GetCurrentContext()?.Connection?.RemoteIpAddress?.ToString();
            return ip;
        }
        public static string GetUserAgent()
        {
            try
            {

                return httpClient?.GetCurrentContext()?.Request.Headers["User-Agent"];
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return string.Empty;
            }
        }
        public string SiteURL
        {
            get
            {

                string URL = httpClient.GetCurrentContext().Request.Path;
                int Port = httpClient.GetCurrentContext().Request.Host.Port.Value;
                string strPort = ":" + Port;
                string Protocol = "http://";
                {
                    if (httpClient.GetCurrentContext().Request.IsHttps ||
                        !string.IsNullOrWhiteSpace(httpClient.GetCurrentContext().Request.Headers["X-Forwarded-For"]))
                    {
                        Protocol = "https://";
                    }
                    strPort = string.Empty;
                }
                return Protocol + URL + strPort;
            }
        }
        ///// <summary>
        /// Gets a value indicating whether this instance is secure.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is secure; otherwise, <c>false</c>.
        ///// </value>
        //public static bool IsSecure
        //{
        //    get
        //    {

        //        return true;
        //        // return Utilities.accessor.HttpContext.Request.IsHttps ||
        //        // Utilities.accessor.HttpContext.Request.ServerVariables["HTTPS"].Equals("on", StringComparison.InvariantCultureIgnoreCase);
        //    }
        //}

        //public static string ValidateIban(string claimIBAN, List<BankCode> list)
        //{
        //    string bankcode = string.Empty;
        //    if (claimIBAN.Length != 22)
        //    {
        //        return bankcode;
        //    }
        //    string c = claimIBAN[2].ToString() + claimIBAN[3].ToString();
        //    var code = list.FirstOrDefault(x => x.Code == c);
        //    if (code != null)
        //    {
        //        bankcode = code.Code;
        //    }
        //    return bankcode;
        //}

        ///// <summary>
        ///// Gets the app setting.
        ///// </summary>
        ///// <param name="strKey">The STR key.</param>
        ///// <returns></returns>
        //public static string GetAppSetting(string strKey)
        //{
        //    try
        //    {
        //        string strValue = "";// System.Configuration.ConfigurationManager.AppSettings[strKey];
        //        return strValue;
        //    }
        //    catch (Exception exp)
        //    {
        //        ErrorLogger.LogError(exp.Message, exp, false);
        //        return string.Empty;
        //    }
        //}

        ///// <summary>
        ///// clear HTML tags
        ///// </summary>
        ///// <param name="Input"></param>
        ///// <returns></returns>
        //public static string ClearHTML(string Input)
        //{
        //    if (!string.IsNullOrEmpty(Input))
        //    {
        //        string strOutput = String.Empty;
        //        strOutput = Regex.Replace(Input, "<(.|\n)+?>", string.Empty);
        //        return strOutput;
        //    }
        //    return string.Empty;
        //}
        ///// <summary>
        ///// Encodes the HTML.
        ///// </summary>
        ///// <param name="Input">The input.</param>
        ///// <returns></returns>
        //public static string EncodeHTML(string Input)
        //{
        //    if (!string.IsNullOrEmpty(Input))
        //    {
        //        //return Microsoft.Security.Application.Encoder.HtmlEncode(Input);
        //    }
        //    return string.Empty;
        //}

        ///// <summary>
        ///// App_s the path.
        ///// </summary>
        ///// <returns></returns>
        //public static String App_Path()
        //{
        //    return System.AppDomain.CurrentDomain.BaseDirectory;
        //}

        ///// <summary>
        ///// Adds the cookie.
        ///// </summary>
        ///// <param name="name">The name.</param>
        ///// <param name="value">The value.</param>
        ///// <param name="isSecure">if set to <c>true</c> [is secure].</param>
        ///// <param name="expireAfterInMinutes">The expire after in minutes.</param>
        //public void AddCookie(string name, string value, bool isSecure, int expireAfterInDays, string domainName)
        //{

        //    CookieOptions cookie = new CookieOptions();
        //    cookie.HttpOnly = true;
        //    //cookie.Value = value;
        //    cookie.Secure = isSecure;
        //    if (!expireAfterInDays.Equals(0))
        //    {
        //        cookie.Expires = DateTime.Now.AddDays(expireAfterInDays);
        //    }
        //    if (!string.IsNullOrEmpty(domainName))
        //    {
        //        cookie.Domain = domainName;
        //    }
        //    httpClient.GetCurrentContext().Response.Cookies.Append(name, value, cookie);
        //}

        ///// <summary>
        ///// Adds the cookie.
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="value"></param>
        ///// <param name="isSecure"></param>
        ///// <param name="expireAfterInDays"></param>
        //public void AddCookie(string name, string value, bool isSecure, int expireAfterInDays)
        //{
        //    AddCookie(name, value, isSecure, expireAfterInDays, string.Empty);
        //}

        ///// <summary>
        ///// Gets the cookie.
        ///// </summary>
        ///// <param name="name">The name.</param>
        ///// <returns></returns>
        //public string GetCookie(string name)
        //{

        //    if (httpClient.GetCurrentContext().Request.Cookies[name] != null)
        //        return httpClient.GetCurrentContext().Request.Cookies[name].ToString();
        //    else
        //        return string.Empty;
        //}
        ///// <summary>
        ///// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        ///// </summary>
        ///// <param name="characters">Unicode Byte Array to be converted to String</param>
        ///// <returns>String converted from Unicode Byte Array</returns>
        //private static string UTF8ByteArrayToString(byte[] characters)
        //{
        //    UTF8Encoding encoding = new UTF8Encoding();
        //    string constructedString = encoding.GetString(characters);
        //    return (constructedString);
        //}

        ///// <summary>
        ///// Converts the String to UTF8 Byte array and is used in De serialization
        ///// </summary>
        ///// <param name="pXmlString"></param>
        ///// <returns></returns>
        //private static Byte[] StringToUTF8ByteArray(string pXmlString)
        //{
        //    UTF8Encoding encoding = new UTF8Encoding();
        //    byte[] byteArray = encoding.GetBytes(pXmlString);
        //    return byteArray;
        //}

        ///// <summary>
        ///// Serializes the object.
        ///// </summary>
        ///// <param name="pObject">The p object.</param>
        ///// <returns></returns>
        //public static String SerializeObject(Object pObject)
        //{
        //    String XmlizedString = null;
        //    MemoryStream memoryStream = new MemoryStream();
        //    XmlSerializer xs = new XmlSerializer(pObject.GetType());
        //    XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //    xs.Serialize(xmlTextWriter, pObject);
        //    memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        //    XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        //    memoryStream.Dispose();
        //    return XmlizedString;
        //}

        ///// <summary>
        ///// Deserializes the object.
        ///// </summary>
        ///// <param name="pXmlizedString">The p xmlized string.</param>
        ///// <param name="type">The type.</param>
        ///// <returns></returns>
        //public static Object DeserializeObject(String pXmlizedString, Type type)
        //{
        //    XmlSerializer xs = new XmlSerializer(type);
        //    using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)))
        //    {
        //        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //        return xs.Deserialize(memoryStream);
        //    }
        //}
        ///// <summary>
        ///// Deserializes the object.
        ///// </summary>
        ///// <param name="pXmlizedString">The p xmlized string.</param>
        ///// <param name="type">The type.</param>
        ///// <returns></returns>
        //public static Object DeserializeObject(String pXmlizedString, string type)
        //{
        //    XmlSerializer xs = new XmlSerializer(Type.GetType(type));
        //    using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)))
        //    {
        //        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //        return xs.Deserialize(memoryStream);
        //    }

        //}

        //public static string GetBriefString(string Body, int maxLength)
        //{
        //    string brief = Body;
        //    if (string.IsNullOrEmpty(brief))
        //    {
        //        return string.Empty;
        //    }
        //    string newBrief = brief;
        //    //cut the brief with substring
        //    if (brief.Length > maxLength)
        //    {
        //        if (brief.Substring(0, maxLength + 1).EndsWith(string.Empty))
        //            return brief.Substring(0, maxLength);
        //        for (int i = 0; i < 20; i++)
        //        {
        //            if (maxLength - 1 - i > 0)
        //                newBrief = brief.Substring(0, maxLength - i);
        //            else
        //                newBrief = string.Empty;

        //            if (newBrief.EndsWith(string.Empty))
        //            {
        //                //correct
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        newBrief = brief;
        //    }
        //    return newBrief;
        //}

        //public static int GetFileFormatCode(string servername)
        //{
        //    if (IsPdf(servername))
        //        return (int)FormatTypesEnum.PDF;
        //    else if (isImage(servername))
        //        return (int)FormatTypesEnum.Image;
        //    else if (IsExcel(servername))
        //        return (int)FormatTypesEnum.Excel;

        //    return 0;
        //}
        public static string GetCurrentURL(IHttpContextAccessor HttpContextAccessor)
        {

            var request = HttpContextAccessor.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri.ToString();

        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="imageBytes">The image bytes.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns></returns>
        //public static byte[] ResizeImage(byte[] imageBytes, int maxWidth, int maxHeight)
        //{
        //    Stream imageStream = new MemoryStream(imageBytes);
        //    Bitmap tempImage = new Bitmap(imageStream);
        //    double Width = 0;
        //    double Height = 0;
        //    if (tempImage.Width <= maxWidth && tempImage.Height <= maxHeight)
        //    {
        //        Width = tempImage.Width;
        //        Height = tempImage.Height;
        //    }
        //    else
        //    {
        //        double ratioX = Convert.ToDouble(maxWidth) / Convert.ToDouble(tempImage.Width);
        //        double ratioY = Convert.ToDouble(maxHeight) / Convert.ToDouble(tempImage.Height);
        //        double ratio = 0;
        //        if (ratioX > ratioY)
        //            ratio = ratioY;
        //        else
        //            ratio = ratioX;
        //        Width = ratio * tempImage.Width;
        //        Height = ratio * tempImage.Height;
        //    }
        //    Bitmap imgBitMap = new Bitmap(Convert.ToInt32(Width), Convert.ToInt32(Height), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //    imgBitMap.SetResolution(tempImage.HorizontalResolution, tempImage.VerticalResolution);
        //    Graphics g = Graphics.FromImage(imgBitMap);
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        //    g.DrawImage(tempImage, 0, 0, imgBitMap.Width, imgBitMap.Height);
        //    MemoryStream OutputStream = new MemoryStream();
        //    imgBitMap.Save(OutputStream, ImageFormat.Jpeg);
        //    // 
        //    imgBitMap.Dispose();
        //    g.Dispose();
        //    return OutputStream.ToArray();
        //}

        ///// <summary>
        ///// Get Value From Cache
        ///// </summary>
        ///// <param name="CacheKey"></param>
        ///// <returns></returns>
        //public object GetValueFromCache(string CacheKey)
        //{

        //    var cache = httpClient.GetCurrentContext().RequestServices.GetService<IMemoryCache>();
        //    var greeting = cache.Get(CacheKey) as string;

        //    if (httpClient.GetCurrentContext() == null || cache == null)
        //        return null;

        //    if (greeting != null)
        //    {
        //        return greeting;
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// Add Value To Cache
        ///// </summary>
        ///// <param name="CacheKey"></param>
        ///// <param name="obj"></param>
        //public void AddValueToCache(string CacheKey, object obj)
        //{

        //    var cache = httpClient.GetCurrentContext().RequestServices.GetService<IMemoryCache>();

        //    if (httpClient.GetCurrentContext() == null || cache == null)
        //        return;

        //    lock (cache)
        //    {
        //        /*          System.Web.HttpContext.Current.Cache.Add(CacheKey, obj, null, DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);*/
        //    }
        //}
        ///// <summary>
        ///// Adds the value to cache.
        ///// </summary>
        ///// <param name="CacheKey">The cache key.</param>
        ///// <param name="obj">The obj.</param>
        ///// <param name="Minutes">The minutes.</param>
        //public void AddValueToCache(string CacheKey, object obj, int Minutes)
        //{

        //    var cache = httpClient.GetCurrentContext().RequestServices.GetService<IMemoryCache>();
        //    if (httpClient.GetCurrentContext() == null || cache == null)
        //        return;

        //    //System.Web.Caching.Cache cache = System.Web.HttpContext.Current.Cache;
        //    lock (cache)
        //    {
        //        //cache.CreateEntry( CacheKey, obj, null, DateTime.Now.AddMinutes(Minutes) Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
        //    }
        //}

        ///// <summary>
        ///// Removes the cache.
        ///// </summary>
        ///// <param name="CacheKey">The cache key.</param>
        //public void RemoveCache(string CacheKey)
        //{

        //    var cache = httpClient.GetCurrentContext().RequestServices.GetService<IMemoryCache>();
        //    cache.Remove(CacheKey);

        //}

        ///// <summary>
        ///// Removes all cache key.
        ///// </summary>
        //public void RemoveAllCacheKey()
        //{
        //    var cache = httpClient.GetCurrentContext().RequestServices.GetService<IMemoryCache>();


        //    //foreach (DictionaryEntry key in cache.Get())
        //    //{
        //    //    cache.Remove(key.Key.ToString());
        //    //}
        //}

        /// <summary>
        /// Gets the current language.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.Parent.Name;
        }

        /// <summary>
        /// Gets the current culture info.
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetCurrentCultureInfo()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        static string alphaCaps = "QWERTYUPASDFGHJKZXCVBNM";
        static string alphaLow = "qwertyupasdfghjkzxcvbnm";
        static string numerics = "23456789";
        static string special = "*$-+?_&=!%{}/";
        //create another string which is a concatenation of all above

        /// <summary>
        /// Gets a random number between zero and maxValue (exclusive)
        /// </summary>
        /// <param name="rng">Random number generator</param>
        /// <param name="maxValue">max value</param>
        /// <returns></returns>
        public static int GetRandomNumber(RandomNumberGenerator rng, int maxValue)
        {
            byte[] randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            int randomNumber = BitConverter.ToInt32(randomBytes, 0);
            return Math.Abs(randomNumber) % maxValue;
        }

        /// <summary>
        /// Gets the random position.
        /// </summary>
        /// <param name="posArray">The pos array.</param>
        /// <returns></returns>
        public static int getRandomPosition(RandomNumberGenerator rng, ref string posArray)
        {
            int pos;
            string randomChar = posArray.ToCharArray()[GetRandomNumber(rng, posArray.Length)].ToString();
            pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }

        /// <summary>
        /// Gets the random char.
        /// </summary>
        /// <param name="fullString">The full string.</param>
        /// <returns></returns>
        public static string getRandomChar(RandomNumberGenerator rng, string fullString)
        {
            return fullString.ToCharArray()[GetRandomNumber(rng, fullString.Length)].ToString();
        }

        public static string GenerateStrongPasswordForOrangeMoney(int length)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            string allChars = alphaLow + numerics;
            String generatedPassword = string.Empty;

            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");

            // Generate four repeating random numbers are postions of
            // lower, upper, numeric and special characters
            // By filling these positions with corresponding characters,
            // we can ensure the password has atleast one
            // character of those types
            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            pLower = getRandomPosition(rng, ref posArray);
            pUpper = getRandomPosition(rng, ref posArray);
            pNumber = getRandomPosition(rng, ref posArray);
            pSpecial = getRandomPosition(rng, ref posArray);


            for (int i = 0; i < length; i++)
            {
                if (i == pLower)
                    generatedPassword += getRandomChar(rng, alphaCaps);
                if (i == pUpper)
                    generatedPassword += getRandomChar(rng, alphaLow);
                else if (i == pNumber)
                    generatedPassword += getRandomChar(rng, numerics);
                else if (i == pSpecial)
                    generatedPassword += getRandomChar(rng, special);
                else
                    generatedPassword += getRandomChar(rng, allChars);
            }
            return generatedPassword;
        }


        /// <summary>
        /// Generates the strong password.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string GenerateStrongPassword(int length)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            string allChars = alphaLow + numerics;
            String generatedPassword = "";

            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");

            // Generate four repeating random numbers are postions of
            // lower, upper, numeric and special characters
            // By filling these positions with corresponding characters,
            // we can ensure the password has atleast one
            // character of those types
            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            pLower = getRandomPosition(rng, ref posArray);
            pUpper = getRandomPosition(rng, ref posArray);
            pNumber = getRandomPosition(rng, ref posArray);
            pSpecial = getRandomPosition(rng, ref posArray);


            for (int i = 0; i < length; i++)
            {
                //if (i == pLower)
                //    generatedPassword += getRandomChar(rng, alphaCaps);
                if (i == pUpper)
                    generatedPassword += getRandomChar(rng, alphaLow);
                else if (i == pNumber)
                    generatedPassword += getRandomChar(rng, numerics);
                //else if (i == pSpecial)
                //    generatedPassword += getRandomChar(rng, special);
                else
                    generatedPassword += getRandomChar(rng, allChars);
            }
            return generatedPassword;
        }

        #region Input Validation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserInput">Input</param>
        /// <returns>resturns the input after removing any suspicious values</returns>
        public static string CheckUserInputText(string UserInput)
        {
            if (UserInput != null)
            {
                //if (!string.IsNullOrEmpty(UserInput))
                //{
                //    string result = FilterUsersInputURL(UserInput);
                //    return System.Web.HttpContext.Current.Server.HtmlEncode(result);
                //}
                //else
                //{
                return string.Empty;
                //}
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static string FilterUsersInputURL(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                string quote = "\"";
                val = Regex.Replace(val, "/script", string.Empty, RegexOptions.IgnoreCase);
                val = Regex.Replace(val, "script", string.Empty, RegexOptions.IgnoreCase);
                val = Regex.Replace(val, ".exe", string.Empty, RegexOptions.IgnoreCase);
                val = Regex.Replace(val, ".dll", string.Empty, RegexOptions.IgnoreCase);
                val = val.Replace("\r\n", string.Empty);
                val = val.Replace("^", string.Empty);
                val = val.Replace("%", string.Empty);
                val = val.Replace("!", string.Empty);
                val = val.Replace(";", string.Empty);
                val = val.Replace("~", string.Empty);
                val = val.Replace("--", string.Empty);
                val = val.Replace("'", string.Empty);
                val = val.Replace(quote, string.Empty);
                val = val.Replace("/>", string.Empty);
                val = val.Replace("</", string.Empty);
                val = val.Replace("<", string.Empty);
                val = val.Replace(">", string.Empty);
                val = val.Replace("(", string.Empty);
                val = val.Replace(")", string.Empty);
                val = val.Replace("[", string.Empty);
                val = val.Replace("]", string.Empty);
                val = val.Replace("#", string.Empty);
                val = val.Replace("||", string.Empty);
                val = val.Replace("&&", string.Empty);

                return val.Trim();
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// Determines whether [is safe URL] [the specified URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if [is safe URL] [the specified URL]; otherwise, <c>false</c>.
        /// </returns>
        //public static bool IsSafeUrl(string url)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        return false;
        //    }

        //    url = url.Trim().ToLower();

        //    string SafeUrls = GetAppSetting("SafeUrls");

        //    if (!string.IsNullOrEmpty(SafeUrls))
        //    {
        //        foreach (string s in SafeUrls.Split(';'))
        //        {
        //            if (url.StartsWith(s.Trim().ToLower()))
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    if (!url.StartsWith("/"))
        //        url = "/" + url;

        //    if (url.StartsWith("/english/") || url.StartsWith("/en/") || url.StartsWith("/arabic/") || url.StartsWith("/ar/"))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public static bool IsSafeUrl2(string url, out string normalizedUrl)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        normalizedUrl = "";
        //        return false;
        //    }

        //    url = url.Trim().ToLower();

        //    string SafeUrls = GetAppSetting("SafeUrls");

        //    if (!string.IsNullOrEmpty(SafeUrls))
        //    {
        //        foreach (string s in SafeUrls.Split(';'))
        //        {
        //            if (url.StartsWith(s.Trim().ToLower()))
        //            {
        //                normalizedUrl = url;
        //                return true;
        //            }
        //        }
        //    }

        //    if (!url.StartsWith("/"))
        //        url = "/" + url;

        //    if (url.StartsWith("/english/") || url.StartsWith("/en/") || url.StartsWith("/arabic/") || url.StartsWith("/ar/"))
        //    {
        //        normalizedUrl = url;
        //        return true;
        //    }

        //    normalizedUrl = "";
        //    return false;
        //}

        /// <summary>
        /// Gets the message from global resource.
        /// </summary>
        /// <param name="resourceClassName">Name of the resource class.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static string GetMessageFromGlobalResource(string resourceClassName, string message)
        {
            string messageLocal = String.Empty;
            //object objMessage = HttpContext.GetGlobalResourceObject(resourceClassName, message, Thread.CurrentThread.CurrentCulture);
            //if (objMessage != null)
            //{
            //    messageLocal = objMessage.ToString();
            //}
            return messageLocal;
        }

        /// <summary>

        //public static int BankTimeout
        //{
        //    get
        //    {
        //        int BankTimeout = 310 * 1000;
        //        if (int.TryParse(GetAppSetting("BankTimeout"), out BankTimeout))
        //        {
        //            return 310 * 1000;
        //        }
        //        return BankTimeout;
        //    }
        //}

        /// <summary>
        /// Base64s the decodein bayte.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns></returns>
        public static byte[] Base64DecodeinBayte(string base64EncodedData)
        {
            return System.Convert.FromBase64String(base64EncodedData);
        }

        ///// <summary>
        ///// Gets the internal server IP.
        ///// </summary>
        ///// <returns></returns>
        public static string GetInternalServerIP()
        {
            try
            {
                return Utilities.GetServerIP();
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return string.Empty;
            }
        }



        /// <summary>
        /// Removes the zero from dial.
        /// </summary>
        /// <param name="dial">The dial.</param>
        /// <returns></returns>
        public static string RemoveZeroFromDial(string dial)
        {
            return dial.StartsWith("0") ? dial.Remove(0, 1).Trim().ToString() : dial;
        }

        public static string AddTwoToDial(string dial)
        {
            return dial.Insert(0, "2").Trim();
        }

        /// <summary>
        /// Gets the user ip address.
        /// </summary>
        /// <returns></returns>
        public static string GetUserIPAddress()
        {
            //try
            //{
            //    //The X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a 
            //    //client connecting to a web server through an HTTP proxy or load balancer
            //    string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //    if (string.IsNullOrEmpty(ip))
            //    {
            //        ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //    }
            //    if (ip.Contains(","))
            //        ip = ip.Split(',')[0];

            //    return ip;
            //}
            //catch (Exception exp)
            //{
            //    ErrorLogger.LogError(exp.Message, exp, false);
            return string.Empty;
            //}
        }

        public static bool IsValidVatNumber(string companyVatNumber)
        {
            Regex regexDial = new Regex(@"^[0-9]{15}$");
            return regexDial.IsMatch(companyVatNumber);
        }

        public static bool IsMobileBrowser()
        {

            try
            {

                //GETS THE CURRENT USER CONTEXT
                //HttpContext context = HttpContext.Current;

                ////FIRST TRY BUILT IN ASP.NT CHECK
                //if (context.Request.Browser.IsMobileDevice)
                //{
                //    return true;
                //}
                ////THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
                //if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
                //{
                //    return true;
                //}
                ////THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
                //if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                //context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
                //{
                //    return true;
                //}
                ////AND FINALLY CHECK THE HTTP_USER_AGENT 
                ////HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
                //if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
                //{
                //    //Create a list of all mobile types
                //    string[] mobiles = new[]{
                //       "midp", "j2me", "avant", "docomo", "novarra", "palmos", "palmsource",
                //      "240x320", "opwv", "chtml", "pda", "windows ce", "mmp/",
                //      "blackberry", "mib/", "symbian","wireless", "nokia", "hand", "mobi",
                //      "phone", "cdm", "up.b", "audio","SIE-", "SEC-", "samsung", "HTC",
                //      "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx",
                //      "NEC", "philips", "mmm", "xx","panasonic", "sharp", "wap", "sch",
                //      "rover", "pocket", "benq", "java","pt", "pg", "vox", "amoi",
                //      "bird", "compal", "kg", "voda","sany", "kdd", "dbt", "sendo",
                //      "sgh", "gradi", "jb", "dddi",    "moto", "iphone"
                //    };

                //    //Loop through each item in the list created above 
                //    //and check if the header contains that text
                //    //foreach (string s in mobiles)
                //    //{
                //    //    if (context.Request.ServerVariables["HTTP_USER_AGENT"].
                //    //    ToLower().Contains(s.ToLower()))
                //    //    {
                //    //        return true;
                //    //    }
                //    //}
                //}

                //////mobile switching on red server only(not live)
                ////bool isMobileForTest = false;
                ////bool.TryParse(Utilities.GetAppSetting("IsMobileForTest"), out isMobileForTest);
                ////if (isMobileForTest)
                ////{
                ////    return true;
                ////}
                //////end of mobile switching

                return false;
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return false;
            }
        }

        /// <summary>
        /// Formats the decimal in case no fractional.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static decimal FormatDecimalInCaseNoFractional(decimal value)
        {
            try
            {
                int fractional = 0;
                decimal retValue = value;
                if (int.TryParse(value.ToString().Split('.')[1], out fractional))
                {
                    if (fractional == 0)
                        retValue = (int)value;
                }
                return retValue;
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return value;
            }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <returns></returns>


        public static bool IsValidDial(string dial)
        {
            Regex regexDial = new Regex(@"^01([0-2])\d{8}$", RegexOptions.Compiled);
            return regexDial.IsMatch(dial);
        }

        public static void InitiateSSLTrust()
        {
            try
            {
                //Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(
                delegate
                { return true; }
                );
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.InnerException?.Message, ex, false);
            }
        }

        public static string SendRequest(string url, string request, out HttpStatusCode statusCode)
        {
            statusCode = new HttpStatusCode();
            try
            {
                string response = string.Empty;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/xml";
                httpWebRequest.Method = "POST";
                try
                {
                    InitiateSSLTrust();
                }
                catch
                {

                }
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(request);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                statusCode = httpResponse.StatusCode;
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        response = streamReader.ReadToEnd();
                    }
                }
                return response;
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return null;
            }
        }
        public static string SendRequestJson(string url, string request, out HttpStatusCode statusCode, out string Exp, string token = null)
        {
            statusCode = new HttpStatusCode();
            Exp = string.Empty;
            try
            {
                string response = string.Empty;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                if (token != null)
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
                try
                {
                    InitiateSSLTrust();
                }
                catch (Exception ex)
                {
                    Exp = ex.ToString();
                }
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(request);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                statusCode = httpResponse.StatusCode;
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        response = streamReader.ReadToEnd();
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                Exp = ex.ToString();
                ErrorLogger.LogError(ex.Message, ex, false);
                return null;
            }
        }

        public static string SendComVivaRequest(string url, string request, out HttpStatusCode statusCode)
        {
            statusCode = new HttpStatusCode();
            try
            {
                string response = string.Empty;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/xml";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(request);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                statusCode = httpResponse.StatusCode;
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        response = streamReader.ReadToEnd();
                    }
                }
                return response;
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return null;
            }
        }
        /// <summary>
        /// Validates the mail.
        /// </summary>
        /// <param name="mail">The mail.</param>
        /// <returns></returns>
        public static bool IsValidMail(string mail)
        {
            try
            {
                Regex regexEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                if (regexEmail.IsMatch(mail))
                {
                    return true;
                }
                return false;
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return false;
            }
        }

        public static bool IsValidDate(string birthdate)
        {
            Regex regexDial = new Regex(@"^([1-9]|[12]\d|3[01])(\/)([1-9]|1[0-2])(\/)\d{4}$");
            return regexDial.IsMatch(birthdate);
        }
        public static bool IsValidHijriDate(string birthdate)
        {
            Regex regexDial = new Regex(@"^([1-9]|[12]\d|3[01])(\/)([1-9]|1[0-2])(\/)\d{4}$");
            return regexDial.IsMatch(birthdate);
        }
        public static bool IsValidPhoneNo(string dial)
        {

            Regex regexDial = new Regex(@"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$");
            return regexDial.IsMatch(dial);
        }
        public static bool IsValidNationalId(string id)
        {

            Regex regexDial = new Regex(@"^(1|2|3|4)[0-9]{9}$");
            return regexDial.IsMatch(id);
        }
        public static bool IsValidSponsorId(string id)
        {
            Regex regexDial = new Regex(@"^(10|70)[0-9]{8}$");
            return regexDial.IsMatch(id);
        }
        public static bool IsValidCRNumber(string id)
        {
            Regex regexDial = new Regex(@"^[0-9]{10}$");
            return regexDial.IsMatch(id);
        }
        /// <summary>
        /// Gets the delimited string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string GetDelimitedString<T>(IEnumerable<T> items, string delimiter)
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in items)
                stringBuilder.AppendFormat("{0}{1}", item, delimiter);

            var delimiterLength = delimiter.Length;
            if (stringBuilder.Length >= delimiterLength)
                stringBuilder.Remove(stringBuilder.Length - delimiterLength, delimiterLength);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Deletes the excel sheet files.
        /// </summary>
        public static void DeleteExcelSheetFiles()
        {
            try
            {
                string rootFolderPath = AppDomain.CurrentDomain.BaseDirectory;
                string filesToDelete = @"*.xlsx";   // Only delete DOC files containing "DeleteMe" in their filenames
                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                foreach (string file in fileList)
                {
                    System.IO.File.Delete(file);
                }
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
            }
        }

        /// <summary>
        /// Encodes the hex16.
        /// </summary>
        /// <param name="srcBytes">The SRC bytes.</param>
        /// <returns></returns>
        public static string EncodeHex16(Byte[] srcBytes)
        {
            if (null == srcBytes)
            {
                throw new ArgumentNullException("byteArray");
            }
            string outputString = "";

            foreach (Byte b in srcBytes)
            {
                outputString += b.ToString("X2");
            }

            return outputString;
        }

        /// <summary>
        /// Decodes the hex16.
        /// </summary>
        /// <param name="srcString">The SRC string.</param>
        /// <returns></returns>
        public static Byte[] DecodeHex16(string srcString)
        {
            if (null == srcString)
            {
                throw new ArgumentNullException("srcString");
            }

            int arrayLength = srcString.Length / 2;

            Byte[] outputBytes = new Byte[arrayLength];

            for (int index = 0; index < arrayLength; index++)
            {
                outputBytes[index] = Byte.Parse(srcString.Substring(index * 2, 2), NumberStyles.AllowHexSpecifier);
            }

            return outputBytes;
        }

        //public static string GetAbsoluteUrlOrSelf(string url)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(url))
        //            return null;

        //        return new Uri(new Uri(Utilities.GetAppSetting("PublicSiteURL")), url).AbsoluteUri;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.LogError(ex.InnerException?.Message, ex, false);
        //        return url;
        //    }
        //}

        public static string GetAbsoluteUrlOrSelf(string url, Uri baseUri)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    return null;

                if (baseUri == null)
                    return url;

                return new Uri(baseUri, url).AbsoluteUri;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.InnerException?.Message, ex, false);
                return url;
            }
        }

        //public static string GetPageOutput(string pageUrl)
        //{
        //    try
        //    {
        //        var absoluteUrl = Utilities.GetAbsoluteUrlOrSelf(pageUrl);

        //        var webResponse = WebRequest.Create(absoluteUrl).GetResponse();
        //        using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
        //            return streamReader.ReadToEnd();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.LogError(ex.InnerException?.Message, ex, false);
        //        return null;
        //    }
        //}

        /// <summary>
        /// checks browser and it's version used
        /// </summary>
        /// <returns></returns>
        public static bool IsSupportedBrowser()
        {
            try
            {
                //var utilities = new Utilities();
                //var userAgent = utilities.accessor.HttpContext.Request.Headers["User-Agent"];
                //string uaString = Convert.ToString(userAgent[0]);
                //var uaParser = Parser.GetDefault();
                //UAParser.ClientInfo c = uaParser.Parse(uaString);

                //var browser = utilities.accessor.HttpContext.Request.Headers["User-Agent"].ToString();
                //decimal numericBrowserVersion;
                //decimal.TryParse(HttpContext.Current.Request.Browser.Version, out numericBrowserVersion);
                ////check browsers and their supported versions
                //if ((browser.Browser.ToLower().Contains("ie") || browser.Browser.ToLower().Contains("internetexplorer")) && numericBrowserVersion >= 10)
                //{
                //    return true;
                //}
                //else if (browser.Browser.ToLower().Contains("firefox") && numericBrowserVersion >= 43)
                //{
                //    return true;
                //}

                //else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("android") && browser.Browser.ToLower().Contains("chrome") && (numericBrowserVersion >= 49))
                //{
                //    return true;
                //}
                //else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("android") && numericBrowserVersion >= 4.4m)
                //{
                //    return true;
                //}

                //else if (!HttpContext.Current.Request.UserAgent.ToLower().Contains("android") && HttpContext.Current.Request.UserAgent.ToLower().Contains("crios/") && !HttpContext.Current.Request.UserAgent.ToLower().Contains("opr/") && !HttpContext.Current.Request.UserAgent.ToLower().Contains("edge/"))
                //{
                //    int strartIndex = HttpContext.Current.Request.UserAgent.ToLower().IndexOf("crios/") > 0 ? HttpContext.Current.Request.UserAgent.ToLower().IndexOf("crios/") : 0;
                //    string chromeVersionForIOS = HttpContext.Current.Request.UserAgent.ToLower().Substring((strartIndex + 6), 4);
                //    decimal.TryParse(chromeVersionForIOS, out numericBrowserVersion); */
                //if (numericBrowserVersion >= 45)
                //        return true;
                //    return false;
                //}
                ////chrome in general but not android
                //else if (!HttpContext.Current.Request.UserAgent.ToLower().Contains("android") && browser.Browser.ToLower().Contains("chrome") && numericBrowserVersion >= 45 && !HttpContext.Current.Request.UserAgent.ToLower().Contains("opr/") && !HttpContext.Current.Request.UserAgent.ToLower().Contains("edge/"))
                //{
                //    return true;
                //}
                //else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("mac") && browser.Browser.ToLower().Contains("safari") && (numericBrowserVersion >= 8.4m))
                //{
                //    return true;
                //}
                //// for mobile devices
                //else if (HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone") && browser.Browser.ToLower().Contains("safari") && numericBrowserVersion >= 8.4m)
                //{
                //    return true;
                //}
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.InnerException?.Message, ex, false);
                return false;
            }
        }

        public static bool IsValidPin(string pin)
        {
            try
            {
                Regex regexDial = new Regex(@"^\d{6}$", RegexOptions.Compiled);
                return regexDial.IsMatch(pin);
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                return false;
            }
        }

        /// <summary>
        /// Gets the current browserinfo.
        /// </summary>
        /// <returns></returns>
        public static BrowserInfo GetCurrentBrowserinfo()
        {
            BrowserInfo info = new BrowserInfo();
            try
            {
                //var browser = HttpContext.Current.Request.Browser;
                //decimal numericBrowserVersion;
                //decimal.TryParse(HttpContext.Current.Request.Browser.Version, out numericBrowserVersion);

                //if ((browser.Browser.ToLower().Contains("ie") || browser.Browser.ToLower().Contains("internetexplorer")))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.IE;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //if (HttpContext.Current.Request.UserAgent.ToLower().Contains("windows phone"))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.WindowsPhone;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //if (HttpContext.Current.Request.UserAgent.ToLower().Contains("android"))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.Android;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.IPAD;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //if (HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone"))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.Iphone;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //if (browser.Browser.ToLower().Contains("firefox"))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.FireFox;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //if (browser.Browser.ToLower().Contains("chrome"))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.Chrome;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //if (HttpContext.Current.Request.UserAgent.ToLower().Contains("mac"))
                //{
                //    info.ErrorCode = BrowserInfo.ErrorCodes.Success;
                //    info.ErrorDescription = "Success";
                //    info.BrowserType = BrowserInfo.Type.MAC;
                //    info.BrowserVersion = numericBrowserVersion;
                //    return info;
                //}
                //info.ErrorCode = BrowserInfo.ErrorCodes.Other;
                //info.ErrorDescription = "Other Devices";
                //info.BrowserType = BrowserInfo.Type.None;
                //info.BrowserVersion = numericBrowserVersion;
                return info;
            }
            catch (Exception exp)
            {
                ErrorLogger.LogError(exp.Message, exp, false);
                info.ErrorCode = BrowserInfo.ErrorCodes.ServiceException;
                info.ErrorDescription = exp.Message;
                return info;
            }
        }


        public static DateTime? ConvertStringToDateTimeFromMedGulf(string strValue)
        {
            DateTime dt;
            var value = strValue;
            var dateComponents = strValue.Split('-');
            if (dateComponents.Length > 2 && dateComponents[2].Length >= 4)
            {
                string year = strValue.Substring(0, 4);
                string month = strValue.Substring(5, 2);
                string day = strValue.Substring(8, 2);
                string hour = strValue.Substring(10, 2);
                string mintues = strValue.Substring(13, 2);
                string seconds = strValue.Substring(16, 2);
                value = year + "-" + month + "-" + day + " " + hour + ":" + mintues + ":" + seconds;

            }

            if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParse(strValue, out dt))
            {
                return dt;
            }
            return null;
        }
        public static string Remove966FromMobileNumber(string mobile)
        {
            mobile = mobile.Replace("+", "00");
            if (mobile.StartsWith("966"))
            {
                Regex rex = new Regex("966", RegexOptions.IgnoreCase);
                mobile = rex.Replace(mobile, "0", 1);
            }
            if (mobile.StartsWith("00966"))
            {
                Regex rex = new Regex("00966", RegexOptions.IgnoreCase);
                mobile = rex.Replace(mobile, "0", 1);
            }
            return mobile;
        }

        public static string SaveCompanyFile(string referenceId,byte[] file,string companyName,bool isPolicy,string url,out string filename,
        bool isPdfServer = false, string domain = null, string serverIP = null, string username = null, string password = null)
        {
            FileNetworkShare fileShare = new FileNetworkShare();
            filename = string.Empty;
            try
            {
                filename = string.Format("{0}_{1}_{2}.{3}", referenceId.Replace("-", "").Substring(0, 15),
                    companyName, DateTime.Now.ToString("HHmmss"), "pdf");
                string path = url;
                if (isPolicy)
                    path = Path.Combine(path, companyName, "Policies", DateTime.Now.Date.ToString("dd-MM-yyyy"), DateTime.Now.Hour.ToString());
                else
                    path = Path.Combine(path, companyName, "Invoices", DateTime.Now.Date.ToString("dd-MM-yyyy"), DateTime.Now.Hour.ToString());
                string generatedReportFilePath = Path.Combine(path, filename);

                System.IO.File.WriteAllText("C:/inetpub/wwwroot/Medical/BackgroundServiceFinal/logs/generatedReportFilePathbeforeSave_" + ".txt", generatedReportFilePath);

                if (isPdfServer)
                {
                    string reportFilePath = generatedReportFilePath;
                    generatedReportFilePath = serverIP + "\\" + generatedReportFilePath;
                    path = serverIP + "\\" + path;
                    if (fileShare.SaveFileToShare(domain, username, password, path, generatedReportFilePath, file, serverIP, out string exception))
                    {
                        return reportFilePath;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    System.IO.File.WriteAllText("C:/inetpub/wwwroot/Medical/BackgroundServiceFinal/logs/before_" + ".txt", generatedReportFilePath);
 
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                File.WriteAllBytes(generatedReportFilePath, file);
                System.IO.File.WriteAllText("C:/inetpub/wwwroot/Medical/BackgroundServiceFinal/logs/generatedReportFilePath_" + ".txt", generatedReportFilePath);

                return generatedReportFilePath;
            }
            catch (Exception exp)
            {
                System.IO.File.WriteAllText("C:/inetpub/wwwroot/Medical/BackgroundServiceFinal/logs/exp_" + ".txt", exp.ToString());
                ErrorLogger.LogError(exp.Message, exp, false);
                return exp.ToString() ;
            }
        }

        public static string RemoveWhiteSpaces(string s)
        {
            return string.Join(" ", s.Split(new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries));
        }

        public static int GetSocialStatusId(string socialStatus)
        {
            if (socialStatus == "مطلقة" || socialStatus == "Divorced Female")
            {
                return 5;
            }
            if (socialStatus == "متزوجة" || socialStatus == "Married Female")
            {
                return 4;
            }
            if (socialStatus == "متزوج" || socialStatus == "Married Male")
            {
                return 2;
            }
            if (socialStatus == "غير متاح" || socialStatus == "Not Available")
            {
                return 0;
            }
            if (socialStatus == "غير ذلك" || socialStatus == "Other")
            {
                return 7;
            }
            if (socialStatus == "غير متزوجة" || socialStatus == "Single Female")
            {
                return 3;
            }
            if (socialStatus == "أعزب" || socialStatus == "Single Male")
            {
                return 1;
            }
            if (socialStatus == "ارملة" || socialStatus == "Widowed Female")
            {
                return 6;
            }
            return 1;
        }

        public static string Removemultiplespaces(string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// convert gorgian date to hijri date 
        /// </summary>
        /// <param name="DateConv"></param>
        /// <param name="Calendar"></param>
        /// <param name="DateLangCulture"></param>
        /// <returns></returns>
        public static string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {
                DateLangCulture = "ar-sa";
            }
            DTFormat = new CultureInfo(DateLangCulture, false).DateTimeFormat;
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new HijriCalendar();
                    break;
                case "Gregorian":
                    DTFormat.Calendar = new GregorianCalendar();
                    break;
                default:
                    return "";
            }
            DTFormat.ShortDatePattern = "dd/MM/yyyy";
            string result = DateConv.Date.ToString("f", DTFormat).Remove(10, 8);
            string result1 = DateConv.Date.ToShortDateString();
            return result;
        }

        private static HttpContext cur;
        private static int startGreg = 1900;
        private static int endGreg = 2100;
        private static string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"};
        private static CultureInfo arCul;
        private static CultureInfo enCul;
        private static HijriCalendar h;
        private static GregorianCalendar g;


        public static DateTime ConvertHijriToGorgean(string hijridate)
        {
            cur = httpClient.GetCurrentContext();
            arCul = new CultureInfo("ar-SA");
            enCul = new CultureInfo("en-US");

            h = new HijriCalendar();
            g = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
            arCul.DateTimeFormat.Calendar = h;
            var Gorgeandate = DateTime.ParseExact(hijridate, allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return Gorgeandate;
        }
        public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "application/unknown";
            string ext = (fileNameOrExtension.Contains(".")) ? Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static bool IsPdf(string filename)
        {
            string[] _validExtensions = { ".pdf" };
            if (_validExtensions.Contains(Path.GetExtension(filename).ToLower()))
                return true;
            else
                return false;
        }

        public static bool isImage(string path)
        {
            string[] _validExtensions = { ".jpg", ".bmp", ".gif", ".png", "jpeg" };
            if (_validExtensions.Contains(Path.GetExtension(path).ToLower()))
                return true;
            else
                return false;
        }
        public static bool IsExcel(string path)
        {
            string[] _validExtensions = { ".xls", ".xlsx ", ".xlsm", ".xltx", ".svc" };
            if (_validExtensions.Contains(Path.GetExtension(path).ToLower()))
                return true;
            else
                return false;
        }

        public static string CompletePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith(InternationalPhoneCode))
                phoneNumber = phoneNumber.Substring(InternationalPhoneCode.Length);
            else if (phoneNumber.StartsWith(InternationalPhoneSymbol))
                phoneNumber = phoneNumber.Substring(InternationalPhoneSymbol.Length);

            if (!phoneNumber.StartsWith(SaudiInternationalPhoneCode))
            {
                if (phoneNumber.StartsWith(Zero))
                    phoneNumber = phoneNumber.Substring(Zero.Length);
                phoneNumber = SaudiInternationalPhoneCode + phoneNumber;
            }
            return phoneNumber;
        }
        public static string GetShortUrl(string i_sLongUrl)
        {
            string finalURL = string.Empty;
            try
            {
                string i_sBitlyUserName = "bcare2019";
                string i_sBitlyAPIKey = "R_40b626d0a62049099177ffc8a415b887";
                //Construct a valid URL and parameters to connect to Bitly Server
                StringBuilder sbURL = new StringBuilder("http://api.bitly.com/v3/shorten?");
                sbURL.Append("&format=xml");
                sbURL.Append("&longUrl=");
                sbURL.Append(HttpUtility.UrlEncode(i_sLongUrl));
                sbURL.Append("&login=");
                sbURL.Append(System.Web.HttpUtility.UrlEncode(i_sBitlyUserName));
                sbURL.Append("&apiKey=");
                sbURL.Append(System.Web.HttpUtility.UrlEncode(i_sBitlyAPIKey));

                HttpWebRequest objRequest = WebRequest.Create(sbURL.ToString()) as HttpWebRequest;
                objRequest.Method = "GET";
                objRequest.ContentType = "application/x-www-form-urlencoded";
                objRequest.ServicePoint.Expect100Continue = false;
                objRequest.ContentLength = 0;
                //Send the Request and Get the Response. The Response will have the status of operation and the bitlyURL
                WebResponse objResponse = objRequest.GetResponse();
                StreamReader myXML = new StreamReader(objResponse.GetResponseStream());
                dynamic xr = XmlReader.Create(myXML);
                //Retrieve the Status and URL from the Response
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xr);

                string sStat = xdoc.ChildNodes[1].ChildNodes[1].ChildNodes[0].Value;
                if (sStat == "OK")
                {
                    finalURL = xdoc.ChildNodes[1].ChildNodes[2].ChildNodes[0].ChildNodes[0].Value;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.Message, ex, false);
            }
            return finalURL;
        }

    }


}
