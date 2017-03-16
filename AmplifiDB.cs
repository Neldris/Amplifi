using AmplifiConsoleApp.MainSource.POJOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.MainSource.DataSource
{
    class AmplifiDB
    {
        private static readonly AmplifiDB INSTANCE = new AmplifiDB();

        private static List<AmplifiDataGroups> listAmpGroup = new List<AmplifiDataGroups>();
        private static object obj = new object();

        private AmplifiDB() { }

        public static AmplifiDB getInstance()
        {
            return INSTANCE;
        }

        public  void AddDB(AmplifiDataGroups grp) {
            lock (obj) { listAmpGroup.Add(grp); }
        }

        public  List<AmplifiDataGroups> GetAllDB() { return listAmpGroup; }

    }
}
