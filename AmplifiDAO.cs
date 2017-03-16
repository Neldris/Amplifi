using AmplifiConsoleApp.MainSource.DataSource;
using AmplifiConsoleApp.MainSource.Interface;
using AmplifiConsoleApp.MainSource.POJOS;
using AmplifiConsoleApp.MainSource.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.MainSource.Implementation
{
    class AmplifiDAO : IAmplifiDAO {

        private AmplifiDB ampDataBase;
        private List<AmplifiDataGroups> dataSource;
        private AmplifiFileManager fileManager = new AmplifiFileManager();
        private AmplifiArrayChecker AAC = new AmplifiArrayChecker();

        public AmplifiDAO(string path) {
            ampDataBase = AmplifiDB.getInstance();
            dataSource = ampDataBase.GetAllDB();
        }

        public bool AddNewFile(string file)
        {
            if (file.Length == 0) return false;
            try {
                fileManager.Add(file);
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        /**
         * ADD AN NEW JSONFILE PATH TO READ
         * @param string []files
         * */
        public bool AddNewFiles(string [ ] files){
            if (files.Length == 0) return false;
            try {
                foreach (var x in files) {
                    if (x.Length != 0) {
                        fileManager.Add(x);
                    }
                }
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        /**
         * FIND PEOPLE RELATION
         * @Para String [], int limit
         * 
         * */
        public List<AmplifiDataGroups> FindPeopleRelationship(string [ ] findWhat,string []func, int limit=0)
        {
            List<AmplifiDataGroups> finalValue = new List<AmplifiDataGroups>();
            AAC.ResetAdataGroupResults();//reset entries

            if (findWhat.Length == 0) return null;
            foreach(var op in findWhat) {
                foreach (var x in func) {
                    AAC.AmplifiDataGroups( op, x.Trim().Split(new char [ ] { '-' }) [ 1 ]);
                    if (limit > 0)
                        if (AAC.GetDataGroupresults().Count == limit) break;
                }
            }
            Thread.Sleep(3000);
            finalValue = AAC.GetDataGroupresults();
            return finalValue;

        }
    }
}
 
 