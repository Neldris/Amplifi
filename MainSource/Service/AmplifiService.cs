using AmplifiConsoleApp.MainSource.Implementation;
using AmplifiConsoleApp.MainSource.Interface;
using AmplifiConsoleApp.MainSource.POJOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.MainSource.Service
{
    class AmplifiService: IAmplifiService {
        private AmplifiDAO amplifi;

        public AmplifiService(string dirpath) {

            amplifi = new AmplifiDAO(dirpath);
        }


        public List<AmplifiDataGroups> ControlService(string service) {

            List<string> options = new List<string>();

            //check for number of serch entries in array. if >0 fall in
            if (service.Trim().Split(new char [ ] { ',' }).Length > 0) {

                string [ ] opData = service.Trim().Split(new char [ ] { ' ' }, 2); //get the option from first split
                string func = opData [ 0 ];
                opData [ 0 ] = null;
                Console.WriteLine(func);
                //check if there is -option string
                if (func.Trim().Length < 4 && !func.Trim().StartsWith("-")) throw new Exception(" ERROR! Option mulformed. Type -help User Help information.");
                if (opData [ 1 ].Trim().Length <= 3) throw new Exception(" ERROR! Query string mulformed. Type -help User Help information."); //checnk for asking values
                string [ ] findWhat = opData [ 1 ].Split(new char [ ] { ',' });

                // TODO: NOT IMPLEMENTED
                /*int limit =  getLimit(inOption[inOption.Length]); // get the limit
                if (limit > 0) {
                    findWhat [ findWhat.Length ] = ""; // remove limit from back of array
                }
                */
                //MANUAL LIMIT SET TO 100
                return amplifi.FindPeopleRelationship(findWhat, new string [ ] { func }, 100);
            }

            else {
               /* string [ ] opData = service.Trim().Split(new char [ ] { ' ' }, 2); //get the option from first split
                string func = opData [ 0 ];
                opData [ 0 ] = null;
                */

            }
            return null;


        }


        private int getLimit( string limitStr) {
            
            string [] str = limitStr.Trim().Split(new char [ ] { ',' });
            string strFind = str [ str.Length ];
            string []val = strFind.Trim().Split(new char [ ] { ','});
            if (val[0].ToLower().Trim().Equals("-limit") || val[0].ToLower().Trim().Equals("limit")) {
                return int.Parse(val [ 1 ]);
            }
            return 0;
        }


    }
}
