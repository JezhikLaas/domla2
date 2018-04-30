using D2.Service.ServiceProvider;
using System;

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
            catch (Exception error) {
                Console.WriteLine($"Error exit: {error}");
                return 1;
            }
        }
    }
}
