using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using EDITranslatorServiceLib;
using System.ServiceModel;
using System.ServiceProcess;

namespace EDIServiceHost
{
    public partial class EDIWinService : ServiceBase
    {
        private ServiceHost _host;

        public EDIWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _host?.Close();  //just to be safe
            _host = new ServiceHost(typeof(EDITranslatorService));
            _host.Open();
        }

        protected override void OnStop()
        {
            _host?.Close();
        }
    }
}
