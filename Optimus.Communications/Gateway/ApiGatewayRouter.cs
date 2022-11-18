using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Owin;
using Newtonsoft.Json;
using RestSharp;

namespace Optimus.Communications
{
    public class ApiGatewayRouter : IGatewayRouter
    {
        private static ApiGatewayRouter _instance;

        public List<Route> Routes { get; set; }
        public Destination AuthenticationService { get; set; }
        private readonly string _clientSecret;

        private ApiGatewayRouter(string routeConfigFilePath, string clientSecret)
        {
            _clientSecret = clientSecret;
            var router = JsonLoader.LoadFromFile<dynamic>(routeConfigFilePath);
            Routes = JsonLoader.Deserialize<List<Route>>(Convert.ToString(router.routes));
            AuthenticationService = JsonLoader.Deserialize<Destination>(Convert.ToString(router.authenticationService));
        }

        public static ApiGatewayRouter ReturnInstance()
        {
            return _instance;
        }

        public static ApiGatewayRouter ReturnInstance(string routeConfigFilePath, string clientSecret)
        {
            return _instance ?? (_instance = new ApiGatewayRouter(routeConfigFilePath, clientSecret));
        }

        public static void AppendClientSecret(IRestRequest request)
        {
            request.AddHeader("X-Forwarded-Host", _instance._clientSecret);
        }

        public static void AppendToken(IRestRequest newRequest, IOwinRequest request)
        {
            newRequest.AddHeader("Authorization", request.Headers["Authorization"] ?? "");
        }

        public IRestResponse RouteRequest(IOwinRequest request)
        {
            var route = Routes.FirstOrDefault(x => request.Path.Value.StartsWith(x.Endpoint));
            if (route is null)
            {
                return new RestResponse
                {
                    Content = "No endpoints were found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return route.SendRequest(request);
        }

        //public IRestResponse SendRequest(Route endpoint)
        //{
        //    var route = Routes.FirstOrDefault(x => x.Equals(endpoint));
        //    if (route is null)
        //    {
        //        return new RestResponse
        //        {
        //            Content = "No endpoints were found",
        //            StatusCode = HttpStatusCode.NotFound
        //        };
        //    }

        //    //if (destination.RequiresAuthentication)
        //    //{
        //    //    string token = request.Headers["token"];
        //    //    request.Query.Append(new KeyValuePair<string, StringValues>("token", new StringValues(token)));
        //    //    HttpResponseMessage authResponse = await AuthenticationService.SendRequest(request);
        //    //    if (!authResponse.IsSuccessStatusCode) return ConstructErrorMessage("Authentication failed.");
        //    //}

        //    return route.SendRequest(new OwinRequest
        //    {
        //        Method = 
        //    });
        //}
    }

    public class JsonLoader
    {
        public static T LoadFromFile<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        public static T Deserialize<T>(object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(Convert.ToString(jsonObject));
        }
    }
}
