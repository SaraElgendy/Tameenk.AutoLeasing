using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;

namespace Tameenk.AutoLeasing.Utilities
{
    public class FileNetworkShare
    {
        public bool SaveFileToShare(string domain, string userID, string password, string generatedReportDirPath, string filepath, byte[] pdfData, string serverIP, out string exception)
        {
            try
            {
                exception = string.Empty;
                filepath = filepath.Replace(":", "$");
                generatedReportDirPath = generatedReportDirPath.Replace(":", "$");
                FileDownloads unc = new FileDownloads(@filepath, userID, domain, password);
                if (!Directory.Exists(generatedReportDirPath))
                    Directory.CreateDirectory(generatedReportDirPath);
                File.WriteAllBytes(filepath, pdfData);
                return true;
                 
            }
            catch (Exception exp)
            {
                exception = " generatedReportDirPath="+ generatedReportDirPath + " filepath="+ filepath+" " + exp.ToString();
                return false;
            }
        }
        public byte[] GetFile(string filepath, out string exception)
        {
            byte[] fileData = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    exception = string.Empty;

                    string URL = "http://172.16.46.5:9000/api/policy/GetFile";
                    string urlParameters = "?filePath=" + filepath;
                    client.BaseAddress = new Uri(URL);

                    HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        fileData = JsonConvert.DeserializeObject<byte[]>(response.Content.ReadAsStringAsync().Result);

                    }
                    return fileData;
                }
            }
            catch (Exception exp)
            {
                exception = "filedata is " + fileData + " " + exp.ToString();
                return null;
            }
        }
    }
    

}