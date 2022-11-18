using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Optimus.Models;
using RestSharp;

namespace Optimus.Communications
{
    public static class CommunicationsHelper
    {
        public static Task<List<T>> CallRestApiGeneric<T>(string url, string resourceUrl, Dictionary<string, object> content = null, bool sendParamsInBody = false, OptimusHttpMethods httpMethod = OptimusHttpMethods.GET, string contentType = "application/json", short timeoutInSeconds = 100, string host = "", string userAgent = "")
        {
            return Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(url)) return null;
                    var client = new RestClient($"http://{url}");

                    var request = new RestRequest(resourceUrl)
                    {
                        Timeout = timeoutInSeconds * 1000
                    };

                    switch (httpMethod)
                    {
                        case OptimusHttpMethods.GET:
                            request.Method = Method.GET;
                            break;
                        case OptimusHttpMethods.POST:
                            request.Method = Method.POST;
                            break;
                        case OptimusHttpMethods.PUT:
                            request.Method = Method.PUT;
                            break;
                        case OptimusHttpMethods.DELETE:
                            request.Method = Method.DELETE;
                            break;
                        default:
                            return null;
                    }

                    if (content != null)
                    {
                        if (sendParamsInBody)
                        {
                            var expandoObject = new ExpandoObject();
                            var collection = (ICollection<KeyValuePair<string, object>>)expandoObject;
                            foreach (var kvp in content)
                            {
                                collection.Add(kvp);
                            }
                            dynamic eoDynamic = expandoObject;
                            request.AddJsonBody(eoDynamic);
                        }
                        else
                        {

                            foreach (var obj in content)
                            {
                                request.AddParameter(obj.Key, obj.Value);
                            }
                        }
                    }

                    if (host != string.Empty)
                    {
                        request.AddHeader("Host", host);
                    }
                    if (userAgent != string.Empty)
                    {
                        request.AddHeader("User-Agent", userAgent);
                    }

                    var response = client.Execute<List<T>>(request);
                    return (int)response.StatusCode >= 400 ? null : response.Data;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public static Task<Result> CallRestApi(string url, string resourceUrl, Dictionary<string, object> content = null, bool sendParamsInBody = false, OptimusHttpMethods httpMethod = OptimusHttpMethods.GET, string contentType = "application/json", short timeoutInSeconds = 100)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(url))
                        return new Result
                        {
                            Success = false
                        };

                    var client = new RestClient($"http://{url}");
                    var request = new RestRequest(resourceUrl)
                    {
                        Timeout = timeoutInSeconds * 1000
                    };

                    switch (httpMethod)
                    {
                        case OptimusHttpMethods.GET:
                            request.Method = Method.GET;
                            break;
                        case OptimusHttpMethods.POST:
                            request.Method = Method.POST;
                            break;
                        case OptimusHttpMethods.PUT:
                            request.Method = Method.PUT;
                            break;
                        case OptimusHttpMethods.DELETE:
                            request.Method = Method.DELETE;
                            break;
                        default:
                            return new Result
                            {
                                Success = false
                            };
                    }

                    if (content != null)
                    {
                        if (sendParamsInBody)
                        {
                            var expandoObject = new ExpandoObject();
                            var collection = (ICollection<KeyValuePair<string, object>>)expandoObject;
                            foreach (var kvp in content)
                            {
                                collection.Add(kvp);
                            }
                            dynamic eoDynamic = expandoObject;
                            request.AddJsonBody(eoDynamic);
                        }
                        else
                        {

                            foreach (var obj in content)
                            {
                                request.AddParameter(obj.Key, obj.Value);
                            }
                        }
                    }

                    var response = client.Execute(request);
                    return new Result
                    {
                        Success = (int)response.StatusCode < 400
                    };
                }
                catch (Exception)
                {
                    return new Result
                    {
                        Success = false
                    };
                }
            });
        }

        public static bool CheckTcpPortStatus(int port)
        {
            var listener = new TcpListener(IPAddress.Loopback, port);
            try
            {
                listener.Start();
            }
            catch (SocketException)
            {
                return false;
            }

            listener.Stop();

            return true;
        }

        public static int GetFreeTcpPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public static string GetLocalIpAddress()
        {
            string localIp;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                localIp = endPoint?.Address.ToString();
            }

            if (!string.IsNullOrEmpty(localIp))
                return localIp;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        public static List<string> GetLocalIpAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var output = new List<string>();
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    output.Add(ip.ToString());
                }
            }

            //_serviceEventLoggingHelper.LogEvent("No network adapters with an IPv4 address in the system!", EventLogEntryType.Error);
            return output;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum OptimusHttpMethods
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public static class ContentTypes
    {
        public const string Json = "application/json";
    }
}

