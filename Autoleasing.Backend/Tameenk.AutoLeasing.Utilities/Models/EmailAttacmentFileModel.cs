using System.Net.Mime;

namespace Tameenk.AutoLeasing.Utilities
{
    public class EmailAttacmentFileModel
    {
        public string FilePath { get; set; }
        public byte[] FileAsByterArray { get; set; }
        public string FileName { get; set; }
        public ContentType ContentType { get; set; }
     
    }

   
}
