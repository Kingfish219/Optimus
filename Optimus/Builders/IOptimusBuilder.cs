using Optimus.Models;

namespace Optimus.Registrant
{
    public interface IOptimusBuilder
    {
        OptimusService Build<T>(string name, string displayName, string description, string[] args = null)
            where T : OptimusService, new();
        IOptimusBuilder AddWebApi(string address = "*", int port = 6095, bool useSsl = false);
        IOptimusBuilder AddAuth();
    }
}
