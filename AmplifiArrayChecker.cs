using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

using AmplifiConsoleApp.MainSource.POJOS;
using Fastenshtein;
using System.Collections.Concurrent;
using AmplifiConsoleApp.MainSource.DataSource;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.MainSource.Util
{
    class AmplifiArrayChecker {
        object mylock = new object();
        object dgrLock = new object();

        private AmplifiDB manager = AmplifiDB.getInstance();
        private List<AmplifiDataGroups> dataSource;
        private static List<AmplifiDataGroups> dataGroupResults = new List<AmplifiDataGroups>();
        private const int THREADPOOL = 200;
        private static BlockingCollection<Thread> threadList = new BlockingCollection<Thread>(THREADPOOL);

        public AmplifiArrayChecker() { dataSource = manager.GetAllDB(); }


        /**
         * FIND GENERAL RELATIONSHIP
         * @params string findwhat ,
         * 
         * @return   void
         * */
        public void GenralSearch(string findwhat) {

            bool docID = false;
            string place = string.Empty;
            string people = string.Empty;
            string company = string.Empty;
            string topic = string.Empty;
            string industry = string.Empty;
            string socialTags = string.Empty;
            int docTitle = 0;
            bool url = false;
            bool docSummery = false;
            bool sourceType = false;
            foreach (var grp in dataSource) {
                try {
                    Monitor.TryEnter(mylock);


                    place = Array.Find(grp.m_Places, s => s.Equals(findwhat));
                    people = Array.Find(grp.m_People, s => s.Equals(findwhat));
                    company = Array.Find(grp.m_Companies, s => s.Equals(findwhat));
                    topic = Array.Find(grp.m_Topics, s => s.Equals(findwhat));
                    industry = Array.Find(grp.m_Industry, s => s.Equals(findwhat));
                    socialTags = Array.Find(grp.m_SocialTags, s => s.Equals(findwhat));
                    docTitle = Levenshtein.Distance(findwhat, grp.m_szDocTitle);
                    url = findwhat.Equals(grp.m_szSrcUrl);
                    docSummery = findwhat.ToArray().Intersect(grp.m_szDocSumamry.ToArray()).Any();
                    sourceType = findwhat.ToArray().Intersect(grp.m_szSourceType.ToArray()).Any();

                    try {
                        int docid = int.Parse(findwhat);
                        docID = grp.m_szDocID.Equals(docid);
                    }
                    catch (ArithmeticException e) {
                        docID = false;
                    }

                }
                catch (Exception e) {
                    // lEAVE IT
                }
                finally {
                    Monitor.Exit(mylock);
                }
            
                if (docID || place != null || people != null || company != null || docTitle < 5)
                    dataGroupResults.Add(grp);
                
            }
        }

        /**
         * FIND PEOPLE WITH NAME OR STRING
         * @param string ppl
         * @return void
         * */
        public void FindPeople(string ppl) {
            foreach (var data in dataSource) { 
                foreach (var x in data.m_People) {
                   int dist = Levenshtein.Distance(ppl.ToLower(), x.ToLower());
                  if (dist <= 3) {
                    Monitor.Enter(dgrLock);
                        try {
                            //TODO: CHECK FOR DOUBLE ENTRIES BEFORE ADD
                            //IF PERSON  SKIP ENTRY
                            dataGroupResults.Add(data);      
                        }
                        catch (Exception c) {
                            //LOG EXCEPTION
                        }
                        finally {
                            Monitor.Exit(dgrLock); 
                        }
                       }

                }
            }
        }

        // SortedSet<AmplifiDataGroups> dataGroupResults

        /**
         * FIND COMPANY WITH NAME OR STRING
         * @param string ppl
         * @return void
         * */
        public void FindCompanies(string ppl)
        {
            foreach (var data in dataSource) {
                foreach (var x in data.m_Companies) {
                    int dist = Levenshtein.Distance(ppl.ToLower(), x.ToLower());
                    if (dist <= 3) {
                        Monitor.Enter(dgrLock);
                        try {
                            //TODO: CHECK FOR DOUBLE ENTRIES BEFORE ADD
                            dataGroupResults.Add(data);
                        }
                        catch (Exception c) {
                            //LOG EXCEPTION
                        }
                        finally { Monitor.Exit(dgrLock); }
                    }

                }
            }
        }



        /**
         * FIND INDUSTRY WITH NAME OR STRING
         * @param string ppl
         * @retun void
         * */
        public void FindIndustry(string ppl)
        {
            foreach (var data in dataSource) {
                foreach (var x in data.m_Industry) {
                    int dist = Levenshtein.Distance(ppl.ToLower(), x.ToLower());
                    if (dist <= 3) {
                        Monitor.Enter(dgrLock);
                        try {
                            //TODO: CHECK FOR DOUBLE ENTRIES BEFORE ADD
                            dataGroupResults.Add(data);
                        }
                        catch (Exception c) {
                            //LOG EXCEPTION
                        }
                        finally { Monitor.Exit(dgrLock); }
                    }

                }
            }
        }

        /**
         * FIND INDUSTRY WITH NAME OR STRING
         * @param string ppl
         * @retun void
         * */
        public void FindTechnology(string ppl)
        {
            foreach (var data in dataSource) {
                foreach (var x in data.m_Technology) {
                    int dist = Levenshtein.Distance(ppl.ToLower(), x.ToLower());
                    if (dist <= 3) {
                        Monitor.Enter(dgrLock);
                        try {
                            //TODO: CHECK FOR DOUBLE ENTRIES
                            dataGroupResults.Add(data);
                        }
                        catch (Exception c) {
                            //LOG EXCEPTION
                        }
                        finally { Monitor.Exit(dgrLock); }
                    }

                }
            }
        }


        /**
         * FIND PLACES WITH NAME OR STRING
         * @param string ppl
         * @retun void
         * */
        public void FindPlaces(string ppl)
        {
            foreach (var data in dataSource) {
                foreach (var x in data.m_Places) {
                    int dist = Levenshtein.Distance(ppl.ToLower(), x.ToLower());
                    if (dist <= 3) {
                        Monitor.Enter(dgrLock);
                        try {
                            //TODO: CHECK FOR DOUBLE ENTRIES
                            dataGroupResults.Add(data);
                        }
                        catch (Exception c) {
                            //LOG EXCEPTION
                        }
                        finally { Monitor.Exit(dgrLock); }
                    }

                }
            }
        }

        /**
         * FIND PLACES WITH NAME OR STRING
         * @param string ppl
         * @retun void
         * */
        public void FindTopics(string ppl)
        {
            foreach (var data in dataSource) {
                foreach (var x in data.m_Topics) {
                    int dist = Levenshtein.Distance(ppl.ToLower(), x.ToLower());
                    if (dist <= 3) {
                        Monitor.Enter(dgrLock);
                        try {
                            //TODO: CHECK FOR DOUBLE ENTRIES
                            dataGroupResults.Add(data);
                        }
                        catch (Exception c) {
                            //LOG EXCEPTION
                        }
                        finally { Monitor.Exit(dgrLock); }
                    }

                }
            }
        }

        /**
         * FIND DOCSUMMERY WITH NAME OR STRING
         * @param string ppl
         * @retun void
         * */
        public void FindDocSummery(string ppl)
        {
            
            foreach (var data in dataSource) {
                string [ ] args = data.m_szDocSumamry.Split(new char [ ] {' '});
                foreach (var x in args) {
                    int dist = Levenshtein.Distance(ppl.ToLower(), x.ToLower());
                    if (dist <= 3) {
                        Monitor.Enter(dgrLock);
                        try {
                            //TODO: CHECK FOR DOUBLE ENTRIES
                            dataGroupResults.Add(data);
                            
                        }
                        catch (Exception c) {
                            //LOG EXCEPTION
                        }
                        finally { Monitor.Exit(dgrLock); }
                    }

                }
            }
        }

        /**
         * FIND DOCID WITH NAME OR STRING
         * @param object docid,
         * @retun void
         * */
        public void FindDocID(object docid)
        {
            try {
                int dID = (int)docid;
                foreach (var data in dataSource) {
                    if (dID == data.m_szDocID) {
                            dataGroupResults.Add(data);
                     
                    }
                           
                }
            }
            catch (Exception e) {
                //LOG EXCEPTION
            }
        }

        /**
         * FIND DOCSUMMERY WITH NAME OR STRING
         * @param string year,
         * @retun void
         * */
        public void FindYear(string year)
        {
            foreach (var data in dataSource) {
                    int dist = Levenshtein.Distance(data.m_szYear, year);
                    if (dist < 5) dataGroupResults.Add(data);
            }
        }


        /**
         * ADD DATAGROU PRESULTS
         * SOLE DATA GROUP ADD FUNC
         * */
        public void AddDataGroupResults(AmplifiDataGroups adg) {
            lock (mylock) {
                dataGroupResults.Add(adg);
            }
        }

        /**
         *  VOID GETDATAGROUPRESULTS
         *  @return dataGroupResults
         * */
        public List<AmplifiDataGroups> GetDataGroupresults() {
            return dataGroupResults;
        }

        /**
         * VOID RESET DATAGROUP RESULTS
         * */
        public void ResetAdataGroupResults() {
            dataGroupResults.Clear();
        }

        /**
         * THREAD WALKER
         * @parem object fs
         * */
        private void groupFind(object ob)
        {
            MyThreadPoolArgs myArgs = (MyThreadPoolArgs)ob;
            string fs = myArgs.findWhat;
            string funcName = myArgs.FunctName;

            string FuncName = (string)funcName;
            switch (FuncName.ToUpper()) {

                case "PEOPLE":
                    string n = (string)fs;
                    FindPeople(n);
                    break;
                case "COMPANIES":
                     string nc = (string)fs;
                     FindCompanies(nc);
                     break;
                case "INDUSTRY":
                     string ni = (string)fs;
                     FindIndustry(ni);
                     break;
                case "TECHNOLOGY":
                     string ntg = (string)fs;
                     FindTechnology(ntg);
                     break;
                case "PLACES":
                     string np = (string)fs;
                     FindPlaces(np);
                     break;
                case "TOPICS":
                     string nt = (string)fs;
                     FindTopics(nt);
                     break;
                case "DOCSUMMERY":
                     string nd = (string)fs;
                     FindDocSummery(nd);
                     break;
                case "YEAR":
                      string ny = (string)fs;
                      FindYear(ny);
                      break;
                case "DOCID":
                    FindDocID(fs);
                    break;
                default:
                     string nsrc = (string)fs;
                     GenralSearch(nsrc);
                break;
            }
        }

        /**
         *  AMPLIFIDATAGROUP THREAD TASKS
         *  @Param string findwhat
         * */
        public void AmplifiDataGroups(string findwhat, string FuncName)
        {
            MyThreadPoolArgs myArgs = new MyThreadPoolArgs();
            myArgs.findWhat = findwhat;
            myArgs.FunctName = FuncName;
           producer(myArgs);

        }
        private static void producer(object fp)
        {
            AmplifiArrayChecker cker = new AmplifiArrayChecker();
            ThreadPool.SetMaxThreads(THREADPOOL, THREADPOOL);
            ThreadPool.QueueUserWorkItem(new WaitCallback(cker.groupFind), fp);

        }
    }

    public class MyThreadPoolArgs {
        public string findWhat { get; set; }
        public string FunctName { get; set; }
    }
}
