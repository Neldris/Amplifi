using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Maps.Geocoding;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AmplifiConsoleApp.View;
using AmplifiConsoleApp.Geo.GeoActivity;
using AmplifiConsoleApp.Geo;
using System.IO;

namespace AmplifiConsoleApp
{
    class Program
    {

        static void Main(string [ ] args) {
            AmplifiResultsViewer view = new AmplifiResultsViewer();

            //Get File path 
            string fPath = "Use auto";
            if (args.Length >1)
                fPath = args [ 0 ];
            view.Start(Path.GetDirectoryName(fPath)); //set path
           
           
        }
        
    }
}
