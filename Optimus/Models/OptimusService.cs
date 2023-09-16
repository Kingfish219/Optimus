using System;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Optimus.Models
{
    public abstract class OptimusService
    {
        internal Action<HostConfigurator> ConfigureCallBack;
        public abstract bool Start();
        public abstract bool Stop();
        public void Run()
        {
            HostFactory.Run(ConfigureCallBack);
        }
    }
}
