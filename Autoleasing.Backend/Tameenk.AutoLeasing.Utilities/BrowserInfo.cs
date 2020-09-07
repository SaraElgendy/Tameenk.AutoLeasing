namespace Tameenk.AutoLeasing.Utilities
{
    public class BrowserInfo
    {
        /// <summary>
        /// Device or User agent type
        /// </summary>
        public enum Type
        {
            IE,
            Edge,
            Opera,
            FireFox,
            Chrome,
            Android,
            Iphone,
            IPAD,
            MAC,
            WindowsPhone,
            Other,
            None
        }

        /// <summary>
        /// Gets or sets the browser version.
        /// </summary>
        /// <value>
        /// The browser version.
        /// </value>
        public decimal BrowserVersion
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the type of the browser.
        /// </summary>
        /// <value>
        /// The type of the browser.
        /// </value>
        public Type BrowserType
        {
            get;
            set;
        }


        public enum ErrorCodes
        {
            Success,
            NullResponse,
            Other,
            ServiceException
        }
        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>
        /// The error description.
        /// </value>
        public string ErrorDescription
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public ErrorCodes ErrorCode
        {
            get;
            set;
        }
    }
}
