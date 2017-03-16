using AmplifiConsoleApp.Geo;
using AmplifiConsoleApp.Geo.GeoActivity;
using AmplifiConsoleApp.MainSource.DataSource;
using AmplifiConsoleApp.MainSource.POJOS;
using AmplifiConsoleApp.MainSource.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.View
{
    class AmplifiResultsViewer
    {
        private static  AmplifiService service = new AmplifiService("");
        private AmplifiGeoStart geoStart = new AmplifiGeoStart();
        private AmplifiFileManager init = new AmplifiFileManager();
        private SortedSet<AmplifiDataGroups> resultsGroup = new SortedSet<AmplifiDataGroups>();

        private AmplifiDB ampDB = AmplifiDB.getInstance();

        public void Start(string filePath) {
            try {       
                   init.init(filePath);

                   Thread.Sleep(1000); // WAIT FOR FILES TO LOAD
                   Thread t2 = new Thread(MainOP);
                   t2.Start();
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                Console.Clear();  // Clear the screen
            }
            int x = ampDB.GetAllDB().Count();
            
            Greetings(x);// Greetings 

            //RUN MAIN OPERATION ON ITS OWN THREAD
           


        }

        private static void Greetings( int fcount=0) {

            string abr = "---------------------AMPLIFI SYS-------------------------\n" +
                           "\n\n -Total files loaded {0}\n\n" +
                           "For help and how to use the system\n" +
                           "Type -help or -h\n";
            Console.WriteLine(abr,fcount);
            Thread.Sleep(2000);

        }
        
        // Get the comand string and output results 
        public static void ShowResults(string request, string seacrhvalue, string theComand) {
            try {
                Console.Clear();
                Console.WriteLine("\nOPTION TYPE: {0}", theComand);
                string [ ] SRC = seacrhvalue.Split(new char [ ] { ',' });
                Console.WriteLine("\nYOU SEARCHED FOR: {0}", seacrhvalue);

                //GET SERACH RESULTS
                List<AmplifiDataGroups> fiGroupresults = service.ControlService(request);

                if (fiGroupresults.Count > 1) {
                    StringBuilder build = new StringBuilder();


                    int ad = fiGroupresults.Count();
                    Console.WriteLine("\n\nQUERY TOTAL RESULST :{0}", ad);
                    Console.Clear();
                    Console.WriteLine("\n\n-------------PEOPLE: ----------");
                    int nppl = 0;
                    foreach (var value in fiGroupresults) {

                        foreach (var ppl in value.m_People) {
                            if (nppl % 4 == 0) {
                                Console.WriteLine();
                            }
                            Console.Write(" {0}, ", ppl);
                            nppl++;
                        }
                    }

                    Console.WriteLine("\n\n-------------COMPANIES: ----------");
                    int cc = 0;
                    foreach (var value in fiGroupresults) {

                        foreach (var ppl in value.m_Companies) {
                            if (cc % 4 == 0) {
                                Console.WriteLine();
                            }
                            Console.Write(" {0}, ", ppl);
                            cc++;
                        }
                    }


                    Console.WriteLine("\n\n-------------TECHNOLOGY: ----------");
                    int ttt = 0;
                    foreach (var value in fiGroupresults) {

                        foreach (var ppl in value.m_Technology) {
                            if (ttt % 4 == 0) {
                                Console.WriteLine();
                            }
                            Console.Write(" {0} ", ppl);
                            ttt++;
                        }
                    }

                    Console.WriteLine("\n\n-------------TOPICS: ----------");
                    int ttp = 0;
                    foreach (var value in fiGroupresults) {

                        foreach (var ppl in value.m_Topics) {
                            if (ttp % 4 == 0) {
                                Console.WriteLine();
                            }
                            Console.Write(" {0} ", ppl);
                            ttp++;
                        }
                    }

                    Console.WriteLine("\n\n-------------LOCATIONS: ----------");
                    int cct = 0;
                    List<string> theLoc = new List<string>();
                    foreach (var value in fiGroupresults) {

                        foreach (var ppl in value.m_Places) {
                          if(ppl.Length > 0) {
                            if (cct % 4 == 0) {
                                Console.WriteLine();
                              }
                                // TOO MUCH DATA FOR VIEW IN CONSOLE
                                if (cct == 10) break;

                                //check for duplications
                                if (theLoc.Contains(ppl) == false) {
                                    theLoc.Add(ppl);
                                    Console.Write("** {0}\n", ppl.ToUpper());

                                    // RUN GEOLOCATION     
                                    Task.WaitAll(AmplifiGeoStart.Run(ppl));
                                    AmplifiGeoMetadata data = AmplifiGeoStart.GeoReport();

                                    InnerAddr indr = new InnerAddr();
                                    foreach (var d in data.results) {
                                        if (d != null) { 
                                            foreach (var dd in d.address_components) {

                                                indr = dd;
                                                //CHECK BEFORE CALL VALUES
                                                if (indr.types.Contains("town")) Console.WriteLine("City: {0}", indr.long_name);
                                                if (indr.types.Contains("locality")) Console.WriteLine("Area: {0}", indr.long_name);
                                                if (indr.types.Contains("administrative_area_level_2")) Console.WriteLine("County : {0}", indr.long_name);
                                                if (indr.types.Contains("administrative_area_level_1")) Console.WriteLine("State : {0}", indr.long_name);
                                                if (indr.types.Contains("country")) Console.WriteLine("Country : {0}", indr.long_name);
                                            }
                                        //CALL LOCATION LAT AND LNG
                                        MyLocation location = d.geometry.location;
                                        Console.WriteLine("LNG: {0}\nLAT: {1}", location.lng, location.lat);
                                        Console.WriteLine();
                                    }
                                }
                            }

                                 cct++;

                          }
                        }
                    }
                    

                    //WAS THINKING OUT FILE OUTPUT RESULTS
                    // using (StreamWriter file = new StreamWriter(@"C:\Users\Public\TestFolder\AmplifiOutPut.txt")) {}

                }
                else {
                    Console.WriteLine(" Search term [ {0} ] yeilded 0 reuslts!.", seacrhvalue);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                // neeeds further twweeks
                // TODO: COME BACK AND FINISH THIS

              

            }

        }


        private  static void MainOP() {

            bool operate = true;
            string finalReuest = string.Empty;
            string secondValue = string.Empty;
            string chk = string.Empty;
            Console.WriteLine("Start........\n");
            Console.Write("\nSearch# ");
            while (operate) {
                
                try {
                    finalReuest = Console.ReadLine();
                    string [ ] searchVals = finalReuest.Trim().Split(new char [ ] { ' ' }, 2);

                    chk = searchVals [ 0 ];
                    secondValue = searchVals.Length > 1 ? searchVals [ 1 ] : "";

                    Console.WriteLine(chk);
                    if (chk.ToLower().Equals("-help") || chk.ToLower().Equals("-h") || chk.ToLower().Equals("help")) {
                        MainMenu();

                    }
                    else if(chk.ToLower().Equals("-geo") || chk.ToLower().Equals("-getStat")) {
                        GeoStats(secondValue);
                    }
                    else if (chk.ToLower().Equals("exit") || chk.ToLower().Equals("-exit")) {
                        operate = false;
                        break;

                    }
                    else {
                        if (chk.StartsWith("-")) {
                            ShowResults(finalReuest, secondValue, chk);
                           
                        }
                        else {
                            Greetings();
                        }

                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);

                }
                finally {
                    Console.Write("\nSearch# ");
                }
            }

        }

        private static void MainMenu() {
            try {
                StreamReader sr = new StreamReader("Menu.txt");
                string ss = sr.ReadToEnd();
                Console.WriteLine(ss);
                sr.Close();


            }
            catch (Exception c) { Console.WriteLine(c); }
        }

        //GEO LOCATION STATS
        public static void GeoStats(string linkVal) {

            string [ ] linkVals = linkVal.Trim().Split(new char [ ] { ','});
            if(linkVals.Length > 0) { 
            // RUN GEOLOCATION     
            foreach (var ppl in linkVals) {
                Task.WaitAll(AmplifiGeoStart.Run(ppl.Trim()));
                AmplifiGeoMetadata data = AmplifiGeoStart.GeoReport();

                InnerAddr indr = new InnerAddr();
                foreach (var d in data.results) {
                        if (d != null) {
                            foreach (var dd in d.address_components) {

                                indr = dd;
                                //CHECK BEFORE CALL VALUES
                                if (indr.types.Contains("town")) Console.WriteLine("City: {0}", indr.long_name);
                                if (indr.types.Contains("locality")) Console.WriteLine("Area: {0}", indr.long_name);
                                if (indr.types.Contains("administrative_area_level_2")) Console.WriteLine("County : {0}", indr.long_name);
                                if (indr.types.Contains("administrative_area_level_1")) Console.WriteLine("State : {0}", indr.long_name);
                                if (indr.types.Contains("country")) Console.WriteLine("Country : {0}", indr.long_name);
                            }
                            //CALL LOCATION LAT AND LNG
                            MyLocation location = d.geometry.location;
                            Console.WriteLine("LNG: {0}\nLAT: {1}", location.lng, location.lat);
                            Console.WriteLine();
                        }
                     }
                }
            }
        }


    }
}
