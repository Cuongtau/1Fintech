using DAL.Repository;
using Services.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Unity;
using Timer = System.Timers.Timer;

namespace SMSGatewayWS
{
    public partial class SmsGatewayWs : ServiceBase
    {
        readonly Timer _timer = new Timer();
        private static IUnityContainer _container;

        public SmsGatewayWs(IUnityContainer container)
        {
            InitializeComponent();
            _container = container;
            var service = _container.Resolve<DemoService>();
            service.GetStringDI();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args) 
        {
            try
            {
                _timer.AutoReset = true;
                _timer.Enabled = true;
                _timer.Interval = int.Parse(ConfigurationManager.AppSettings["TimeDelay"]);
                _timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
                StartWork();
            }
            catch (Exception ex)
            {
                // Log
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            StartWork();
        }

        public void StartWork()
        {

        }

        public void Send_MT()
        {
            var service = _container.Resolve<SMSGatewayService>();


        }


        protected override void OnStop()
        {

        }
    }
}
