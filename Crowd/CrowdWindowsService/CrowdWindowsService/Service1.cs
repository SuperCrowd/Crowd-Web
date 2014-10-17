using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Configuration;

namespace CrowdWindowsService
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {           
            _timer = new Timer(1000 * 60 * 60 * 24); // one day
            //_timer = new Timer(1000 * 60 * 30); // half hour
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.Start();
        }

        protected override void OnStop()
        {
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CrowdService.Service1Client objClient1 = new CrowdService.Service1Client();
            objClient1.TestNotification();       
        }

        public CrowdService.Service1Client GetITSCommonTableServiceClient()
        {
            string crowdUrl = ConfigurationManager.AppSettings["CrowdServiceURL"];
              
            Uri ServiceUri = new Uri(crowdUrl, UriKind.RelativeOrAbsolute);
            EndpointAddress ServiceAddress = new EndpointAddress(ServiceUri);
            BinaryMessageEncodingBindingElement BinaryMessageObject = new BinaryMessageEncodingBindingElement();

            HttpTransportBindingElement HttpTransportBindingElementObject = new HttpTransportBindingElement();
            HttpTransportBindingElementObject.MaxReceivedMessageSize = 2147483647;
            HttpTransportBindingElementObject.MaxBufferSize = 2147483647;

            BindingElementCollection elements = new BindingElementCollection();
            elements.Add(BinaryMessageObject);
            elements.Add(HttpTransportBindingElementObject);

            CustomBinding binding = new CustomBinding(elements);
            binding.CloseTimeout = new TimeSpan(0, 50, 0);
            binding.OpenTimeout = new TimeSpan(0, 50, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 50, 0);
            return new CrowdService.Service1Client(binding, ServiceAddress);
        }
    }
}
