using Optimus.Registrant;

namespace Sample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var service = OptimusRegistrant
                .CreateDefaultBuilder()
                .AddWebApi()
                .Build<SampleService>("MySampleService", "My Sample Service", "My sample service powered by Optimus", args);

            service.Run();
        }
    }
}
