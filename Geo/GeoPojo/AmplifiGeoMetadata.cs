using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.Geo
{
    [Serializable]
    class AmplifiGeoMetadata
    {
        public List<Address> results { get; set; }
    }

    [Serializable]
    public class Address
    {
        public List<InnerAddr> address_components { get; set; }
        public Geometry geometry { get; set; }
    }

    [Serializable]
    public class InnerAddr
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    [Serializable]
    public class Geometry
    {
        public MyLocation location { get; set; }
    }

    [Serializable]
    public class MyLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

}
