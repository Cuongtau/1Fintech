using DAL.IRepository;
using DAL.Repository;
using DryIoc;
using Services.IService;
using Services.Service;
using UtilLibraries.IpAddress;

namespace Pay1Fintech
{
    public class CompositionConfig
    {
        public CompositionConfig(IRegistrator registrator)
        {
            //DAL
            registrator.Register<IAccountRepository, AccountRepository>();
            registrator.Register<IAccountService, AccountService>();

            //Utiliy
            registrator.Register<IIPAddressHelper, IPAddressHelper>();
        }
    }
}
