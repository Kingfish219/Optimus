using Optimus.Registrant;

namespace Sample
{
    public class Program
    {
        static void Main(string[] args)
        {
            OptimusRegistrant
                .CreateDefaultBuilder()
                .Build<SampleService>("Optimus", "Optimus", "Optimus sample service", args)
                .AddWebApi()
                .Run();
        }
    }
}
