using Optimus.Communications;
using System;

namespace Optimus.Registrant
{
    public static class OptimusBuilderExtensions
    {
        public static IOptimusBuilder AddWebApi(this IOptimusBuilder builder,string address = "*", int port = 5895, bool useSsl = false)
        {
            var startup = new Startup();
            startup.StartWebServer(address, port, useSsl);
            startup.RegisterArea();

            return builder;
        }

        public static IOptimusBuilder AddAuth(this IOptimusBuilder builder)
        {
            return builder;
        }
    }
}
