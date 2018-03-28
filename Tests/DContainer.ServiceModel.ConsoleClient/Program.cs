using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using DContainer.ServiceModel.ConsoleHost.Services;

namespace DContainer.ServiceModel.ConsoleClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Demo starting...");
            Console.Out.WriteLine("Pleaes enter return to continue...");
            Console.ReadLine();

            do
            {
                using (var channelFactory = new ChannelFactory<IResourceService>("resourceEndpoint"))
                {
                    var serviceProxy = channelFactory.CreateChannel();
                    Console.WriteLine("Begin invoke service IResourceService/GetResource...");
                    var resource = serviceProxy.GetResource(-1);
                    Console.WriteLine(resource.ResourceName);
                    var resource2 = serviceProxy.GetResource(-1);
                    Console.WriteLine(resource.ResourceName);
                    Console.WriteLine("End invoke service IResourceService/GetResource...");
                }
                Console.WriteLine("Please enter 'r' to restart the demo.");
            } while ("r" == Console.ReadLine());
        }

        private static Dictionary<string, Type> GetServiceChannels()
        {
            var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServiceModelSectionGroup serviceModel = ServiceModelSectionGroup.GetSectionGroup(appConfig);
            ChannelEndpointElementCollection endpoints = serviceModel.Client.Endpoints;
            Dictionary<string, Type> serviceTypes = new Dictionary<string, Type>(endpoints.Count);
            foreach (ChannelEndpointElement endpoint in endpoints)
            {
                var serviceType = Type.GetType(endpoint.Contract);
                var endpointName = endpoint.Name;

                serviceTypes[endpointName] = serviceType;
            }
            return serviceTypes;
        }

        //private static void CurrentTimeCallback(IAsyncResult asyncResult)
        //{
        //    var service = asyncResult.AsyncState as IAsyncAuthorizationService;
        //    var serverTime = service.EndCurrentTime(asyncResult);
        //    Console.WriteLine(serverTime);
        //    Console.WriteLine("End invoke service IResourceService/GetResource...");
        //}
    }
}
