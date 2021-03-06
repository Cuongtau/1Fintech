﻿using DAL.Repository;
using Services.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Unity;
using Utilities;
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
                NLogLogger.PublishException(ex);
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            StartWork();
        }

        public void StartWork()
        {
            Send_MO();
        }

        public void Send_MO()
        {
            NLogLogger.LogInfo("Vao SendMO");
            var service = _container.Resolve<SMSGatewayService>();
            var totalRow = 0;
            // lấy MO
            var list = service.GetTopData_MO(0, 1000, ref totalRow).ToList();
            if (list == null || list.Count <= 0)
            {
                NLogLogger.LogInfo("------Get Data MO null");
            }
            return;

            foreach (var obj in list)
            {
                var url_Api = ConfigurationManager.AppSettings["URL_PartnerAPI"];
                var param = new
                {

                };

                var response = HttpHelper.PostProxy(param, url_Api);

                var res = response.Content.ReadAsStringAsync().Result;

                NLogLogger.LogInfo($"Send MO to Partner {res}");
            }
        }

        protected override void OnStop()
        {

        }
    }
}
