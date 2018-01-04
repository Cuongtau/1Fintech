using DAL.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class DemoService: IDemoService
    {
        private IDemo _iDemo;
        public DemoService(IDemo iDemo)
        {
            _iDemo = iDemo;
        }
        public string GetStringDI()
        {
            return _iDemo.DemoDI();
        }
    }
}
