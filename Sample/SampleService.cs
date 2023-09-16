using Optimus.Models;

namespace Sample
{
    public class SampleService : OptimusService
    {
        public override bool Start()
        {
            return true;
        }

        public override bool Stop()
        {
            return true;
        }
    }
}
