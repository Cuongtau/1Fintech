using DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class Demo: IDemo
    {
        public string DemoDI()
        {
            return "Demo DI";
        }
    }
}
