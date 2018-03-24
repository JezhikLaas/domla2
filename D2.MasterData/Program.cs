using D2.Service.ServiceProvider;

namespace D2.MasterData
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try {
                ServiceHost
                    .CreateDefaultBuilder()
                    .UseStartup<Startup>()
                    .Build()
                    .Run();

                return 0;
            }
            catch {
                return 1;
            }
        }
    }
}
