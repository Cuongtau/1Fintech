using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.IService
{
    public interface IAccountService
    {
        int Register(Account account);
    }
}
