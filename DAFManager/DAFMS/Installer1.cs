using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace DAFMS
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        ServiceInstaller si;
        ServiceProcessInstaller spi;

        public Installer1()
        {
            InitializeComponent();

            si = new ServiceInstaller();
            si.StartType = ServiceStartMode.Automatic;
            si.ServiceName = "DAFMS";
            si.Description = "Служба синхронизации DAFM";
            spi = new ServiceProcessInstaller();
            spi.Account = ServiceAccount.LocalSystem;

            Installers.Add(si);
            Installers.Add(spi);
        }
    }
}
