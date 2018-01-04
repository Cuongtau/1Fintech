using DAL.IRepository;
using DAL.Repository;
using Services.IService;
using Services.Service;
using Unity;

namespace SMSGatewayWS
{
    public static class UnityConfig
    {
        public static void RegisterComponents(UnityContainer container)
        {
            container.RegisterType<IDemo, Demo>();
            container.RegisterType<IDemoService, DemoService>();

            container.RegisterType<ISMSGateway, DAL.Repository.SMSGateway>();
            container.RegisterType<ISMSGatewayService, SMSGatewayService>();           
        }
    }
}