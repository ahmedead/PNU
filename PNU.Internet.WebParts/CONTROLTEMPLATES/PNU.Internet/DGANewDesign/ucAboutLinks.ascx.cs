using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class ucAboutLinks : UserControl
    {
        private const string ListName = "SiteNavigationItems"; // اسم القائمة في شيربوينت

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnsureListExists();
                BindData();
            }
        }

        private void EnsureListExists()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))

                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        if (web.Lists.TryGetList(ListName) == null)
                        {
                            Guid listId = web.Lists.Add(ListName, "Navigation Items for Home Page", SPListTemplateType.GenericList);
                            SPList list = web.Lists[listId];
                            list.Fields.Add("Link", SPFieldType.URL, false);
                            list.Fields.Add("IconClass", SPFieldType.Text, false);
                            list.Update();
                        }
                        web.AllowUnsafeUpdates = false;
                    }
            
                }
            });
        }

        private void BindData()
        {
            List<NavItem> items = new List<NavItem>();
            SPList list = SPContext.Current.Web.Lists.TryGetList(ListName);

            if (list != null)
            {
                foreach (SPListItem item in list.Items)
                {
                    items.Add(new NavItem
                    {
                        Title = Convert.ToString(item["Title"]),
                        Link = item["Link"] != null ? new SPFieldUrlValue(item["Link"].ToString()).Url : "#",
                        IconClass = Convert.ToString(item["IconClass"])
                    });
                }
                rptNavCards.DataSource = items;
                rptNavCards.DataBind();
            }
        }
    }

    public class NavItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string IconClass { get; set; }
    }
}
