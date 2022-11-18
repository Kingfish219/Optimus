using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace Optimus.Communications.MessageHandlers
{
    public class IpFilterHandler : DelegatingHandler
    {
        private readonly string _whiteListedClient;
        public IpFilterHandler(string whiteListedClient)
        {
            _whiteListedClient = whiteListedClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (ClientIsWhiteListed(request))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            return request.CreateErrorResponse(HttpStatusCode.Unauthorized
                    , "Client is not authorized");
        }

        public bool ClientIsWhiteListed(HttpRequestMessage request)
        {
            try
            {
                if (string.IsNullOrEmpty(_whiteListedClient))
                {
                    return true;
                }

                var forwardedHost = request.Headers.FirstOrDefault(x => x.Key == "X-Forwarded-Host");
                if (forwardedHost.Key is null || forwardedHost.Value is null)
                {
                    return false;
                }

                //if (_whiteListedIps == null || _whiteListedIps.Length == 0)
                //{
                //    return true;
                //}

                var addressString = forwardedHost.Value.FirstOrDefault();

                return string.Equals(_whiteListedClient, addressString);
                //return _whiteListedIps.Any(a => a.Trim().Equals(addressString, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }


    public static class HttpRequestExtensionHelper
    {
        public static string GetRequestAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                var owinRequest = ((OwinContext)request.Properties["MS_OwinContext"]).Request;
                return $"{owinRequest.Host.Value}";
            }

            return string.Empty;
        }
    }
}
