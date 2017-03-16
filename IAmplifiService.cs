using AmplifiConsoleApp.MainSource.POJOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.MainSource.Interface
{
    interface IAmplifiService
    {
        List<AmplifiDataGroups> ControlService(string service);
    }
}
