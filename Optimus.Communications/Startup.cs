using System;
using System.Collections.Generic;
using System.Diagnostics;
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
             //_logger = new DefaultLogger();
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
                 Debug.WriteLine(ex.Message);
                 Debug.WriteLine(ex.InnerException?.Message);

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
                 Debug.WriteLine(ex.Message);
                 Debug.WriteLine(ex.InnerException?.Message);
             }


             //Config.Routes.MapHttpRoute(
             //    name: "v2",
             //    routeTemplate: "api/{controller}/{action}/{id}",
             //    defaults: new { id = RouteParameter.Optional }
             //);
         }

         public void Configuration(IAppBuilder app)
         {
             try
             {

                 //RegisterArea(_clientSecret);

                 //var container = _iocConfigurator.RegisterServices(new ContainerBuilder(), Config);
                 //Config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
                 //app.UseAutofacMiddleware(container);

                 //ConfigureAuth(app);

                 //app.UseAutofacWebApi(Config);
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

                         //var response = router.RouteRequest1(context.Request);
                         //await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
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
                 Debug.WriteLine(ex.Message);
                 Debug.WriteLine(ex.InnerException?.Message);
             }
         }
     }
 }
//=======
//﻿using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Runtime.ExceptionServices;
//using System.Text;
//using System.Web.Http;
//using System.Web.Http.Filters;
//using Microsoft.Owin.Cors;
//using Microsoft.Owin.Hosting;
//using Owin;

//namespace Optimus.Communications
//{
//    public class Startup
//    {
//        private static readonly HttpConfiguration Config = new HttpConfiguration();
//        private int _port;

//        public Startup()
//        {
//            Config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
//            //_logger = new DefaultLogger();
//            //_logger = new DefaultLogger();
//        }

//        [HandleProcessCorruptedStateExceptions]
//        public bool StartWebServer(string address, int port = default(int), bool useSsl = false)
//        {
//            try
//            {
//                _port = port;
//                var protocol = useSsl ? "https" : "http";

//                WebApp.Start<Startup>(new StartOptions
//                {
//                    Urls =
//                    {
//                        $"{protocol}://{address}:{_port}/"
//                    }
//                });

//                return true;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex.Message);
//                Debug.WriteLine(ex.InnerException?.Message);

//                return false;
//            }
//        }

//        public string GetWebApiAddress()
//        {
//            var localIp = CommunicationsHelper.GetLocalIpAddress();
//            if (string.IsNullOrEmpty(localIp))
//            {
//                return "localhost:" + _port;
//            }

//            return localIp + ":" + _port;
//        }

//        public void RegisterArea(List<FilterAttribute> filterAttributes = default)
//        {
//            try
//            {
//                Config.MapHttpAttributeRoutes();
//                Config.Routes.MapHttpRoute(
//                    name: "DefaultApi",
//                    routeTemplate: "api/{controller}/{id}",
//                    defaults: new { id = RouteParameter.Optional }
//                );

//                filterAttributes?.ForEach(filter =>
//                {
//                    Config.Filters.Add(filter);
//                });
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex.Message);
//                Debug.WriteLine(ex.InnerException?.Message);
//            }


//            //Config.Routes.MapHttpRoute(
//            //    name: "v2",
//            //    routeTemplate: "api/{controller}/{action}/{id}",
//            //    defaults: new { id = RouteParameter.Optional }
//            //);
//        }

//        public void Configuration(IAppBuilder app)
//        {
//            try
//            {

//                //RegisterArea(_clientSecret);

//                //var container = _iocConfigurator.RegisterServices(new ContainerBuilder(), Config);
//                //Config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
//                //app.UseAutofacMiddleware(container);

//                //ConfigureAuth(app);

//                //app.UseAutofacWebApi(Config);
//                app.UseCors(CorsOptions.AllowAll);

//                app.Use(async (context, next) =>
//                {
//                    try
//                    {
//                        var reader = new StreamReader(context.Request.Body);
//                        var body = await reader.ReadToEndAsync();
//                        var requestData = Encoding.UTF8.GetBytes(body);
//                        context.Request.Body = new MemoryStream(requestData);

//                        await next.Invoke();

//                        //var response = router.RouteRequest1(context.Request);
//                        //await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
//                    }
//                    catch (Exception exception)
//                    {
//                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                        await context.Response.WriteAsync($"Error occured: {exception.Message}");
//                    }
//                });

//                app.UseWebApi(Config);
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex.Message);
//                Debug.WriteLine(ex.InnerException?.Message);
//            }
//        }
//    }
//>>>>>>> 6655e7b12484828f62ef56ae8934b7b641d5770c
//}
