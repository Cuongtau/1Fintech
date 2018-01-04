using DAL.IRepository;
using DAL.Repository;

namespace SMSGatewayWS
{
    class NinjectBindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {                      
            Bind<IDemo>().To<Demo>();           
        }
    }
}
