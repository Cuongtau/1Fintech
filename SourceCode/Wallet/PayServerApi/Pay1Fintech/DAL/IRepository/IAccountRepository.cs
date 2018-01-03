using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IRepository
{
    public interface IAccountRepository
    {
        int Register(Account account);
    }
}
