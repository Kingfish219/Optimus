
using System;
using Topshelf.HostConfigurators;

namespace Optimus.Registrant
{
    public interface IOptimusBuilder
    {
        Action<HostConfigurator> ConfigureCallback { get; set; }
        IOptimusBuilder Build<T>(string name, string displayName, string description, string[] args = null)
            where T : OptimusService, new();
        //IOptimusBuilder AddWebApi(string address = "*", int port = 5895, bool useSsl = false);
        //IOptimusBuilder AddAuth();
        void Run();
    }
}
