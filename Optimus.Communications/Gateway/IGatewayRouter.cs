using Microsoft.Owin;
using RestSharp;

namespace Optimus.Communications
{
    public interface IGatewayRouter
    {
        IRestResponse RouteRequest(IOwinRequest request);
    }
}
