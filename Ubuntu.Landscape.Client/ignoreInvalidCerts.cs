using System;
using System.Net;
using System.Net.Security;

namespace Ubuntu.Landscape
{
    class ignoreInvalidCerts : IDisposable
    {
        /// <summary>
        /// Ignore invalid SSL Certificates
        /// </summary>
        protected bool _ignoreInvalidCerts = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="yesNo"></param>
        public ignoreInvalidCerts(bool yesNo)
        {
            this._ignoreInvalidCerts = yesNo;
            if (this._ignoreInvalidCerts)
            {
                ServicePointManager.ServerCertificateValidationCallback += this.getSslFailureCallback();
            }
        }

        /// <summary>
        /// Check whether callback is active
        /// </summary>
        /// <returns></returns>
        private bool hasCallBackDelegation()
        {
            if (ServicePointManager.ServerCertificateValidationCallback != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remote Certificate Validation Callback
        /// </summary>
        protected RemoteCertificateValidationCallback getSslFailureCallback()
        {
            return new RemoteCertificateValidationCallback(delegate { return this._ignoreInvalidCerts; });
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            if (this.hasCallBackDelegation())
            {
                ServicePointManager.ServerCertificateValidationCallback -= this.getSslFailureCallback();
            }
        }
    }
}
