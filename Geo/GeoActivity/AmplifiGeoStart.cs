using Google.Maps.Geocoding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.Geo.GeoActivity
{
    class AmplifiGeoStart
    {
        private static AmplifiGeoMetadata source;
        public static async Task Run(string addrs){

            //open an HttpClient in Async mode, 
            using (var client = new HttpClient()) {

                HttpResponseMessage response = await client.GetAsync(GeoAddress(addrs)); //request and get response

                string content = await response.Content.ReadAsStringAsync(); //read response as string

                try {
                    source = JsonConvert.DeserializeObject<AmplifiGeoMetadata>(content); //Deserialize Json Object
                }
                catch (Exception e) {
                    //LOG ERROR/EXCEPTION
                }
                
            }
        }

        private static string  GeoAddress(string addr){
            return string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false",
                             Uri.EscapeDataString(addr));
             }

        public static AmplifiGeoMetadata GeoReport() {
            return source;
        }

    }
}
