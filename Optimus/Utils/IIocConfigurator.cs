using System.Web.Http;
using Autofac;

namespace Optimus.Utils
{
    public interface IIocConfigurator
    {
        IContainer RegisterServices(ContainerBuilder builder, HttpConfiguration config);
    }
}