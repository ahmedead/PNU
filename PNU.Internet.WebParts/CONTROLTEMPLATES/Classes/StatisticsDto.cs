using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;

namespace PNU.Internet.WebParts
{
    public class StatisticsDto
    {
        public string CNT_STD { get; set; }
        public string  CTN_COLL { get; set; }
        public string CNT_PROG { get; set; } 
        public string CNT_ALL_DEPT { get; set; }
       
    }

    // this dto to read the count of students and count of academic members from PowerPi view
    public class PowerPiStatisticsDto
    {
        public string COUNT_STUDENTS { get; set; }
        public string COUNT_ACADEMIC { get; set; }
        
    }

    public class NewStatisticsDto
    { 
        public string CNT_STD { get; set; }
        public string CNT_COLL { get; set; }
        public string CNT_PROG { get; set; }
        public string CNT_ALL_DEPT { get; set; }
        public string CNT_DEGREE_PROG { get; set; }
        public string SMRPRLE_DEGC_CODE { get; set; }
        public string STVDEGC_DESC { get; set; }
        public string DESC_EN { get; set; }
    }
}
