using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace SMSGatewayWS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            UnityContainer container = new UnityContainer();
            UnityConfig.RegisterComponents(container);
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                container.Resolve<SmsGatewayWs>()
                //new SMSGateway()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
