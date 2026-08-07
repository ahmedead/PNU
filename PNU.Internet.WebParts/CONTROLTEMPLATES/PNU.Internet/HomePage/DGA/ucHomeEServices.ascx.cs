using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm;
using Portal.Main.Helper;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public partial class ucHomeEServices : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string RowsCount { get; set; } = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            IsArabic = DetectArabic();
            if (!IsPostBack)
            {
                BindServices();
            }

            
        }

        protected bool IsArabic { get; private set; }
        private static bool DetectArabic()
        {
            try
            {
                int lcid = Thread.CurrentThread.CurrentUICulture.LCID;
                if (lcid == 1025) return true;
                if (lcid == 1033) return false;
            }
            catch { }
            return true;
        }
        private void BindServices()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["EservicesList"];
                        if (list != null)
                        {
                            SPQuery query = new SPQuery();
                            if (RowsCount != "0")
                                query.RowLimit = Convert.ToUInt32(RowsCount);
                            query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='Active' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                                 <Eq>
                                                    <FieldRef Name='IsHome' />
                                                    <Value Type='Boolean'>1</Value>
                                                 </Eq>
                                              </And>
                                           </Where>";

                            // Fetch all items matching the query
                            SPListItemCollection collitem = list.GetItems(query);

                            if (collitem != null)
                            {
                                // Pagination logic
                                int pageSize = 6; // Number of items per page
                                int totalItems = collitem.Count;
                                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                                
                                // Map items to your custom class
                                List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(collitem);

                                // Bind data to the repeater
                                rptAllData.DataSource = eserviceList;
                                rptAllData.DataBind();


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

    }
}
