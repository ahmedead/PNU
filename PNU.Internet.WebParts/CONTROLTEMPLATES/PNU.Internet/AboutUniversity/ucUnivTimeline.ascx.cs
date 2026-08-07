using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{
    public partial class ucUnivTimeline : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                LoadData();



            }
        }

        private void LoadData()
        {
            try
            {
                List<UniversityTimeline> _AllItems = new List<UniversityTimeline>();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/AboutUniversity"))
                        {

                            SPList list = web.Lists["UniversityTimeline"];
                            if (list != null)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = $@"<OrderBy>
                                      <FieldRef Name='ItemOrder' Ascending='True' />
                                   </OrderBy>";
                                SPListItemCollection collitem = list.GetItems(query);


                                if (collitem != null && collitem.Count > 0)
                                {
                                    _AllItems = SPFactory.MapListItemsToClass<UniversityTimeline>(collitem);
                                }

                            }
                        }
                    }
                });

                rptMainData.DataSource = _AllItems;
                rptMainData.DataBind();

            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
        }
    }

    public class UniversityTimeline
    {
        public string Title { get; set; }
        public string ItemOrder { get; set; }
        public string Visibility { get; set; }
        public string Title_EN { get; set; }
        public string Description { get; set; }
        public string Description_EN { get; set; }
        public string PublishingRollupImage { get; set; }
        public string ID { get; set; }
    }
}
