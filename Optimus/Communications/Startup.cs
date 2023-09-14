using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace Optimus.Communications
{
    public class Startup
    {
        private static readonly HttpConfiguration Config = new HttpConfiguration();
        private int _port;

        public Startup()
        {
            Config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        [HandleProcessCorruptedStateExceptions]
        public bool StartWebServer(string address, int port = default(int), bool useSsl = false)
        {
            try
            {
                _port = port;
                var protocol = useSsl ? "https" : "http";

                WebApp.Start<Startup>(new StartOptions
                {
                    Urls =
                     {
                         $"{protocol}://{address}:{_port}/"
                     }
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);

                return false;
            }
        }

        public string GetWebApiAddress()
        {
            var localIp = CommunicationsHelper.GetLocalIpAddress();
            if (string.IsNullOrEmpty(localIp))
            {
                return "localhost:" + _port;
            }

            return localIp + ":" + _port;
        }

        public void RegisterArea(List<FilterAttribute> filterAttributes = default)
        {
            try
            {
                Config.MapHttpAttributeRoutes();
                Config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                filterAttributes?.ForEach(filter => { Config.Filters.Add(filter); });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
            }
        }

        public void Configuration(IAppBuilder app)
        {
            try
            {
                app.UseCors(CorsOptions.AllowAll);

                app.Use(async (context, next) =>
                {
                    try
                    {
                        var reader = new StreamReader(context.Request.Body);
                        var body = await reader.ReadToEndAsync();
                        var requestData = Encoding.UTF8.GetBytes(body);
                        context.Request.Body = new MemoryStream(requestData);

                        await next.Invoke();
                    }
                    catch (Exception exception)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync($"Error occured: {exception.Message}");
                    }
                });

                app.UseWebApi(Config);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
            }
        }
    }
}
