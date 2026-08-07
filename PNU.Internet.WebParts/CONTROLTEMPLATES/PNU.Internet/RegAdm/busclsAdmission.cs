using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm
{
    public static class busclsAdmission
    {
    }

    public class clsAbout
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Overview_EN { get; set; }
        public string Title_EN { get; set; }
        public string PublishingRollupImage { get; set; }
        public string VisionTitle { get; set; }
        public string OverviewTitle { get; set; }
    }

    public class clsAdmissionServices
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Desc { get; set; }
    }

    public class clsscholarshipsPrograms
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public string PublishingRollupImage { get; set; }
        public string Desc { get; set; }
        public string scholarships { get; set; }
    }
}
