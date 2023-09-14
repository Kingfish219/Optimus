using Optimus.Communications;
using Optimus.Models;
using System;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Optimus.Registrant
{
    internal class OptimusBuilder : IOptimusBuilder
    {
        public OptimusService Build<T>(string name, string displayName, string description, string[] args = null)
            where T : OptimusService, new()
        {
            try
            {
                var service = new T();

                Action<HostConfigurator> configureCallback = configure =>
                {
                    #region Input Args
                    //var inputArgs = new List<string>();
                    //var inputArgs = new Dictionary<string, string>();

                    //if (args != null)
                    //{
                    //    foreach (var pair in args)
                    //    {
                    //        configure.AddCommandLineDefinition(pair.Key, arg =>
                    //        {
                    //            inputArgs.Add(pair.Key, string.IsNullOrWhiteSpace(pair.Value) ? arg : pair.Value);
                    //            //inputArgs.Add(string.IsNullOrWhiteSpace(pair.Value) ? arg : pair.Value);
                    //        });
                    //    }
                    //}

                    //configure.ApplyCommandLine();

                    //var test = "tttt11111";

                    //configure.AddCommandLineDefinition("test", f =>
                    //{
                    //    var logger = new EventLog { Source = "KasraValidation" };
                    //    logger.WriteEntry(f);
                    //    //test = f;
                    //});
                    //configure.ApplyCommandLine();
                    #endregion

                    configure.Service<T>(configurator =>
                    {
                        configurator.ConstructUsing(s => service);
                        configurator.WhenStarted(s => s.Start());
                        configurator.WhenStopped(s => s.Stop());
                    });

                    configure.RunAsLocalSystem();
                    configure.SetServiceName(name);
                    configure.SetDisplayName(displayName);
                    configure.SetDescription(description);
                    configure.StartAutomatically();

                    //configure.EnableServiceRecovery(r =>
                    //{
                    //    r.RestartService()
                    //})
                };

                service.ConfigureCallBack = configureCallback;

                return service;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IOptimusBuilder AddWebApi(string address = "*", int port = 5895, bool useSsl = false)
        {
            var startup = new Startup();
            startup.StartWebServer(address, port, useSsl);
            startup.RegisterArea();

            return this;
        }

        public IOptimusBuilder AddAuth()
        {
            return this;
    }
    }
}
