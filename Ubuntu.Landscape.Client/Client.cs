using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Ubuntu.Landscape.Exceptions;

namespace Ubuntu.Landscape
{
    /// <summary>
    /// Ubuntu Landscape Client
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Hostname
        /// </summary>
        public string hostname { get; set; } = "";

        /// <summary>
        /// Access Key
        /// </summary>
        public string accessKey { get; set; } = "";

        /// <summary>
        /// Secret Key
        /// </summary>
        public string secretKey { get; set; } = "";

        /// <summary>
        /// Action URL String
        /// </summary>
        public string actionString { get; set; } = "";

        /// <summary>
        /// Use HTTPS or HTTP
        /// </summary>
        protected bool _sslEnabled = true;

        /// <summary>
        /// sslEnabled property
        /// </summary>
        public bool sslEnabled
        {
            get { return _sslEnabled; }
            set { _sslEnabled = value; }
        }

        /// <summary>
        /// Ignore invalid SSL Certificates
        /// </summary>
        protected bool _ignoreInvalidCerts = false;

        public bool ignoreInvalidCerts
        {
            get { return _ignoreInvalidCerts; }
            set { _ignoreInvalidCerts = value; }
        }

        public string requestUrl
        {
            get { return this.requestString(); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Client()
        {
        }

        /// <summary>
        /// Input check
        /// </summary>
        /// <returns></returns>
        protected bool checkInputs()
        {
            if (this.hostname.Trim() == "") throw new InputException("Missing host name!");
            if (this.accessKey.Trim() == "") throw new InputException("Missing access key!");
            if (this.secretKey.Trim() == "") throw new InputException("Missing secret key!");
            if (this.actionString.Trim() == "") throw new InputException("Missing action string!");
            return true;
        }

        /// <summary>
        /// Calculate Hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        /// <summary>
        /// Build Request String
        /// </summary>
        /// <returns></returns>
        protected string requestString()
        {
            string tmp = "GET\n";
            tmp += this.hostname + "\n";
            tmp += "/api/\n";
            tmp += "access_key_id=" + this.accessKey;
            tmp += "&action=" + this.actionString;
            tmp += "&signature_method=HmacSHA256";
            tmp += "&signature_version=2";
            tmp += "&timestamp=" + HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")).ToUpper();
            tmp += "&version=2011-08-01";
            return tmp.Substring(tmp.IndexOf("access_key_id")) + "&signature=" + HttpUtility.UrlEncode(Convert.ToBase64String(HashHMAC(Encoding.ASCII.GetBytes(this.secretKey), Encoding.ASCII.GetBytes(tmp))));
        }

        /// <summary>
        /// Get Result Data as String
        /// </summary>
        /// <returns></returns>
        public string getResult()
        {
            if (this.checkInputs())
            {
                string retVal = "{}";
                using (new ignoreInvalidCerts(this._ignoreInvalidCerts))
                {
                    using (WebClient client = new WebClient())
                    {
                        var protocol = (this._sslEnabled) ? "https" : "http";
                        using (Stream data = client.OpenRead(protocol + "://" + this.hostname + "/api/?" + this.requestString()))
                        {
                            using (StreamReader reader = new StreamReader(data))
                            {
                                try
                                {
                                    retVal = reader.ReadToEnd();
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message, ex);
                                }
                                finally
                                {
                                    reader.Close();
                                }
                            }
                            data.Close();
                        }
                    }
                }
                return retVal;
            }
            return null;
        }
    }
}
