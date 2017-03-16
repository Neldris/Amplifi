using AmplifiConsoleApp.MainSource.POJOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmplifiConsoleApp.MainSource.Interface
{
    interface IAmplifiDAO
    {

        Boolean AddNewFile(string file);
        Boolean AddNewFiles(string [ ] files);

       // SortedSet<AmplifiDataGroups> FindPeopleRelationship(string [ ] people, string fromWhere, int limit = 0);
        List<AmplifiDataGroups> FindPeopleRelationship(string [ ] people, string [ ] options, int limit = 0);

        /*
         AmplifiGeoFinder  GeoFinder(string address, string country);
         AmplifiGeoFinder  GeoFinder(string []address, string country);
         AmplifiGeoFinder  GeoFinder(Dictionary<string,string> multiRelationa);
         */

    }
}
