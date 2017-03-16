using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using AmplifiConsoleApp.MainSource.POJOS;
using AmplifiConsoleApp.MainSource.Interface;
using System.Text.RegularExpressions;

namespace AmplifiConsoleApp.MainSource.DataSource
{

    class AmplifiFileManager {

        private static  int THREADPOOL = 200;
        private  BlockingCollection<Thread> threadList = new BlockingCollection<Thread>(THREADPOOL);
        private static AmplifiDB ampDB = AmplifiDB.getInstance();
        private static object fileJsonLock = new object(); 
        private  object instanceLock = new object();
        private  ReaderWriterLockSlim threadListCatch = new ReaderWriterLockSlim();
        private static string[] filesPaths;

       // private string filP;
        public  AmplifiFileManager(){}


        /**
         * Get file paths
         * */
        private static void AmpFiles(string path) {
            List<string> files = new List<string>();
            try {
                files = new List<string>(Directory.GetFiles(path));
                Console.WriteLine("File path recieved as {0}", path);
                filesPaths = new string [ files.Count ];
                filesPaths = files.ToArray();
            }
            catch (IOException e) {
                Console.WriteLine("File path recieved ERROR, file path {0}", path);
                filePathDetector(path);
            }
        }

        /**
         * Get AmplifiDataGroups Object after deserialization
         *   Object does not need a lock as mutilple object are needed per file.
         * */
        private static void getFileJson(object val) {
            AmplifiDataGroups ampGroup = new AmplifiDataGroups();
            string file = (string)val;
            try {
                
                    using (StreamReader stream = new StreamReader(File.Open(file, FileMode.Open))) {
                    lock (fileJsonLock) {
                        string forjson = stream.ReadToEnd();
                        try {
                            ampGroup = JsonConvert.DeserializeObject<AmplifiDataGroups>(forjson);
                            ampDB.AddDB(ampGroup);
                            Console.WriteLine("-Adding Current File {0}", file);
                        }
                        catch (JsonException e) {
                            Console.WriteLine(e.Message);

                        }
                        
                    }
                }
            }
            catch (IOException e) {
                Console.WriteLine("Error reading file :{0} ", file);
            }
            
            Thread.SpinWait(THREADPOOL);
        }

        /**
         * File Detector, if there is no file set or Error
         * App may aske you to put a director JSON in you Home directory
         * */
        public static void filePathDetector(string filepath) {
            string filePath = filepath;
            Boolean b = true;
            while (b) {
                try {
                    //if (string.IsNullOrEmpty(filePath)) throw new Exception("Error! path provided is not a  direct");
                    if (Directory.Exists(filePath)) { 
                        Console.WriteLine("File  Found paths as: {0}", filePath);
                        b = false;
                   }
                    else {
                        throw new IOException("Error! path provided is not a  directory.. ");
                    }

                } catch (IOException e) {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Put the JSON directory in your user home directory and press Enter");
                    Console.WriteLine("OR \nYou may type in the file path and Enter");
                    filePath = Console.ReadLine();
                    
                    // LOGIC NOT FULLY TESTED BUT WORKS ON WIN's
                    if (filePath.Length == 0) {
                        string pathname = "USERPROFILE";
                        string dirName = "\\JSON\\";
                        string os = Environment.GetEnvironmentVariable("OS");
                        if (string.IsNullOrEmpty(os)) {
                            pathname = "HOME";
                            dirName = "/JSON/";
                        }
                        filePath = Path.GetDirectoryName(Environment.GetEnvironmentVariable(pathname) + dirName);
                        Console.WriteLine("One moment please...... Found paths as: {0}", filePath);
                        filePathDetector(filePath);
                       // Thread.Sleep(1000);
                    }
                     filePathDetector(filePath);
                
                     }
                }
            AmpFiles(filePath);
       
        }

        /**
         * Populator Runs multiple threads to populate the listGroup with Json deserialized object
         *  # see threadProducer method
         *  */
        private static void populator() {
            int fCount = filesPaths.Length;
            for (int x = 0; x < fCount; x++) {
                producer(filesPaths [ x ]);
            }
          
        }

        //ADD NEW JSON FILE
        public void Add( string path) {
            try {
                string name = Path.GetFileName(path);
                if (File.Exists(path)) {
                    if (Regex.IsMatch(path, name, RegexOptions.IgnoreCase)) {
                        Thread t = new Thread(new ParameterizedThreadStart(getFileJson));
                        t.Start();
                    }
                }
                throw new IOException("File :"+name +" does not Exist......");
            }
            catch (Exception e) {
                Console.WriteLine(" Error Occured ....{0} " + e.Message);
            }
        }


        /**
         * BASIC THREAD POOL OPERATION
         **/
        private static void producer(string fp)
        {
            ThreadPool.SetMaxThreads(THREADPOOL, THREADPOOL);
            ThreadPool.QueueUserWorkItem(new WaitCallback(getFileJson),(object) fp);
          
        }


        /**
         * Innitializer
         * 
         */
        public  void init(object path) {
            filePathDetector((string)path);
            populator();
            
        }
    }
}
