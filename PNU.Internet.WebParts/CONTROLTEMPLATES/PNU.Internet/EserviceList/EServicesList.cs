using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public  class EServicesList
    {

        public string Title { get; set; } = "";
        public string ARServiceName { get; set; } = "";
        public string ENServiceName { get; set; } = "";
        public string Active { get; set; } = "";
        public string URL { get; set; } = "";
        public string URL_EN { get; set; } = "";
        public string ServiceURL { get; set; } = "";

        /// <summary>
        /// Returns URL_EN on an English web (same rule as the page labels: Web.Language != 1025)
        /// when it is not empty; otherwise falls back to URL.
        /// </summary>
        public string DisplayURL
        {
            get
            {
                bool isEn = false;
                try
                {
                    // Substring checks on the URL ("/en") also matched paths such as /environment
                    if (Microsoft.SharePoint.SPContext.Current != null && Microsoft.SharePoint.SPContext.Current.Web != null)
                    {
                        isEn = Microsoft.SharePoint.SPContext.Current.Web.Language != 1025;
                    }
                }
                catch { }

                if (isEn && !string.IsNullOrWhiteSpace(URL_EN))
                {
                    return URL_EN.Trim();
                }

                return URL ?? "";
            }
        }
        public string Desc { get; set; } = "";
        public string TargetGroup { get; set; } = "";
        public string DP_TargetGroup { get; set; } = "";
        public string ResponsibleParty { get; set; } = "";
        public string TimeDuration { get; set; } = "";
        public string Channels { get; set; } = "";
        public string UserManualURL { get; set; } = "";
        public string ServiceAgreement { get; set; } = "";
        public string ServiceAgreementURL { get; set; } = "";
        public string ServiceAgreementURL_EN { get; set; } = "";

        /// <summary>
        /// Displays the ServiceAgreement in EN page (using ServiceAgreementURL_EN).
        /// If ServiceAgreementURL_EN is empty, falls back to ServiceAgreement (or ServiceAgreementURL).
        /// In AR page, displays ServiceAgreement (or ServiceAgreementURL).
        /// </summary>
        public string DisplayServiceAgreement
        {
            get
            {
                bool isEn = false;
                try
                {
                    if (Microsoft.SharePoint.SPContext.Current != null && Microsoft.SharePoint.SPContext.Current.Web != null)
                    {
                        isEn = Microsoft.SharePoint.SPContext.Current.Web.Language != 1025;
                    }
                }
                catch { }

                string arUrl = !string.IsNullOrWhiteSpace(ServiceAgreement)
                    ? ServiceAgreement.Trim()
                    : (!string.IsNullOrWhiteSpace(ServiceAgreementURL) ? ServiceAgreementURL.Trim() : "");

                if (isEn)
                {
                    if (!string.IsNullOrWhiteSpace(ServiceAgreementURL_EN))
                        return ServiceAgreementURL_EN.Trim();

                    return arUrl;
                }

                return arUrl;
            }
        }
        public string _x0052_eq1 { get; set; } = "";
        public string _x0052_eq2 { get; set; } = "";
        public string _x0052_eq3 { get; set; } = "";
        public string Fav { get; set; } = "";
        public string RulesAndRegulationsURL { get; set; } = "";
        public string Desc_EN { get; set; } = "";
        public string TargetGroup_EN { get; set; } = "";
        public string DP_TargetGroup_EN { get; set; } = "";
        public string ResponsibleParty_EN { get; set; } = "";
        public string TimeDuration_EN { get; set; } = "";
        public string Channels_EN { get; set; } = "";
        public string Req1_EN { get; set; } = "";
        public string Req2_EN { get; set; } = "";
        public string Req3_EN { get; set; } = "";
        public string ID { get; set; } = "";


        
        public string Steps { get; set; }

        public string Steps_EN { get; set; }

        public string RequiredDocs { get; set; }

        public string RequiredDocs_EN { get; set; }

        public string ServiceLanguage { get; set; }

        public string ServiceLanguage_EN { get; set; }
        public string ServiceCost { get; set; }

        public string ServiceCost_EN { get; set; }
        public string Badge1 { get; set; }
        public string Badge1_EN { get; set; }
        public string Badge2 { get; set; }

        public string Badge2_EN { get; set; }
        public string Badge3 { get; set; }
        public string Badge3_EN { get; set; }
        public string NafathLogin { get; set; } = "";
        public string ItemOrder { get; set; } = "";
        public string Item_x0020_Order { get; set; } = "";
        public string Order { get; set; } = "";

        public int OrderValue
        {
            get
            {
                string raw = !string.IsNullOrWhiteSpace(ItemOrder) ? ItemOrder :
                            (!string.IsNullOrWhiteSpace(Item_x0020_Order) ? Item_x0020_Order : Order);
                int val;
                return int.TryParse(raw, out val) ? val : int.MaxValue;
            }
        }
    }
}

