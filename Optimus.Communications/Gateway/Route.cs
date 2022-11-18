
using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Owin;
using RestSharp;

namespace Optimus.Communications
{
    public class Route
    {
        public string Endpoint { get; set; }
        public string Protocol { get; set; }
        public Destination Destination { get; set; }
        public bool RequiresAuthentication { get; set; }

        public virtual IRestResponse SendRequest(IOwinRequest request)
        {
            var client = new RestClient($"{Protocol}://{Destination.Host}:{Destination.Port}");
            var restRequest = new RestRequest(request.Path.Value)
            {
                Method = (Method)Enum.Parse(typeof(Method), request.Method, true)
            };

            switch (restRequest.Method)
            {
                case Method.GET:
                    {
                        if (request.Query.Any())
                        {
                            foreach (var obj in request.Query)
                            {
                                restRequest.AddParameter(obj.Key, obj.Value[0]);
                            }
                        }

                        break;
                    }
                default:
                    {
                        if (restRequest.Method == Method.POST)
                        {
                            if (request.Query.Any())
                            {
                                foreach (var obj in request.Query)
                                {
                                    restRequest.AddParameter(obj.Key, obj.Value[0]);
                                }
                            }
                        }

                        if (request.Body != null)
                        {
                            string requestContent;
                            using (var receiveStream = request.Body)
                            {
                                using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                                {
                                    requestContent = readStream.ReadToEnd();
                                }
                            }

                            restRequest.AddParameter("application/json", requestContent, ParameterType.RequestBody);
                        }

                        break;
                    }
            }

            ApiGatewayRouter.AppendClientSecret(restRequest);

            if (RequiresAuthentication)
            {
                ApiGatewayRouter.AppendToken(restRequest, request);
            }

            return client.Execute(restRequest);
        }

        //public async Task<HttpResponseMessage> SendRequest1(IOwinRequest request)
        //{
        //    try
        //    {
        //        string requestContent;
        //        using (Stream receiveStream = request.Body)
        //        {
        //            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
        //            {
        //                requestContent = readStream.ReadToEnd();
        //            }
        //        }

        //        var client = new HttpClient();
        //        using (var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), @"http://127.0.0.1:5850/api/minibackup/backups"))
        //        {
        //            newRequest.Content = new StringContent(requestContent, Encoding.UTF8, request.ContentType);
        //            using (var response = await client.SendAsync(newRequest))
        //            {
        //                return response;
        //            }
        //        }


        //        //var res = new HttpResponseMessage
        //        //{
        //        //    Content = new StringContent(response.Content),
        //        //    Headers = response.Headers,

        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}
    }
    public class Destination
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
