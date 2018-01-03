using DAL.IRepository;
using DAL.Model;
using Services.IService;

namespace Services.Service
{
    public class AccountService: IAccountService
    {
        private IAccountRepository _iAccountRepository;
        public AccountService(IAccountRepository iAccountRepository)
        {
            _iAccountRepository = iAccountRepository;
        }
        public int Register(Account account)
        {
            return _iAccountRepository.Register(account);
        }
    }
}
