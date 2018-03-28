using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DContainer.ServiceModel.ConsoleHost.Services;

namespace DContainer.ServiceModel.ConsoleHost
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // Using Service Location to adapter IoC container
                var register = Locator.Register;
                register
                    .RegisterCurrentLocatorProvider()
                    .RegisterTypeIfMissing<IResourceService, ResourceService>();

                Console.Out.WriteLine("Server listening...");
                IServiceHostFactory hostFactory = new DContainerServiceHostFactory();
                using (var hosts = hostFactory.CreateServiceHosts(typeof(ResourceService)))
                {
                    hosts.Open();

                    Console.Out.WriteLine("--- Press <return> to quit ---");
                    Console.ReadLine();
                }

                //Console.Out.WriteLine("--- Press <return> to quit ---");
                //Console.ReadLine();

                //using (var hosts = new ServiceHostCollection())
                //{
                //    hosts.Add(new ServiceHost(typeof(AuthorizationService)));

                //    hosts.Open();

                //    Console.Out.WriteLine("Server listening...");
                //    Console.ReadLine();
                //}

                //using(var host = new ServiceLocationServiceHost(typeof(AuthorizationService)))
                //{
                //    host.Open();

                //    Console.Out.WriteLine("Server listening...");
                //    Console.ReadLine();
                //}
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
                Console.Out.WriteLine("--- Press <return> to quit ---");
                Console.ReadLine();
            }
        }
    }
}
