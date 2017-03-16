using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifiConsoleApp.MainSource.POJOS{

    /**
     * Data members to deal with.
     * This is the data members form the Json files
     * */
    public class AmplifiDataGroups {

        public string[] m_Places { get; set; }
        public string[] m_Industry { get; set; }
        public string[] m_Technology { get; set; }
        public string m_szDocSumamry { get; set; }
        public string[] m_SocialTags { get; set; }
        public string[] m_Companies { get; set; }
        public string m_szGeo1 { get; set; }
        public string m_szYear { get; set; }
        public string[] m_People { get; set; }
        public string[] m_Topics { get; set; }
        public int m_szDocID { get; set; }
        public int m_iDocBodyWordCnt { get; set; }
        public string m_szSrcUrl { get; set; }
        public string m_szSourceType { get; set; }
        public string [] m_TriGrams { get; set; }
        public string[] m_BiGrams { get; set;}
        public string m_szDocBody { get; set; }
        public int[] m_TriCnt { get; set; }
        public int[] m_BiCnt { get; set; }
        public string m_szDocTitle { get; set; }


    }
}
