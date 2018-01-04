using DAL.Repository;
using Ninject;
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
using Timer = System.Timers.Timer;

namespace SMSGatewayWS
{
    public partial class SMSGateway : ServiceBase
    {
        readonly Timer _timer = new Timer();
        public SMSGateway()
        {
            InitializeComponent();
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            var processor = kernel.Get<Demo>();
            processor.DemoDI();
        }

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



        }


        protected override void OnStop()
        {

        }
    }
}
