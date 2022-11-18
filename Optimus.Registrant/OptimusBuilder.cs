using System;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Optimus.Registrant
{
    internal class OptimusBuilder : IOptimusBuilder
    {
        public Action<HostConfigurator> ConfigureCallback { get; set; }

        public IOptimusBuilder Build<T>(string name, string displayName, string description, string[] args = null)
            where T : OptimusService, new()
        {
            try
            {
                ConfigureCallback = configure =>
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

                    configure.Service<T>(service =>
                    {
                        service.ConstructUsing(s => new T());
                        service.WhenStarted(s => s.Start());
                        service.WhenStopped(s => s.Stop());
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

                return this;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Run()
        {
            HostFactory.Run(ConfigureCallback);
        }
    }
}
