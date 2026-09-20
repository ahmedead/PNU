using Microsoft.IdentityModel.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Org.BouncyCastle.Ocsp;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage;
using Portal.Main.Helper;
using Portal.Main.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{
    public partial class ucSiteMap : UserControl
    {
        private const string MENU_LEVEL_1 = "TopMenuLevel1";
        private const string MENU_LEVEL_2 = "TopMenuLevel2";
        private const string MENU_LEVEL_3 = "TopMenuLevel3";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadMenu();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ============================================================
        // Helpers used from the .ascx for conditional rendering
        // ============================================================
        protected bool HasChildren(object dataItem)
        {
            var item = dataItem as TopMenuLevel1;
            return item != null && item.LVL2 != null && item.LVL2.Count > 0;
        }

        protected bool HasL3Children(object dataItem)
        {
            var item = dataItem as TopMenuLevel2;
            return item != null && item.LVL3 != null && item.LVL3.Count > 0;
        }

        protected bool HasURL(object dataItem)
        {
            // Both TopMenuLevel1 and TopMenuLevel2 expose a URL property.
            // We use reflection-friendly DataBinder to avoid coupling to a single type.
            try
            {
                object val = DataBinder.Eval(dataItem, "URL");
                return val != null && !string.IsNullOrEmpty(val.ToString());
            }
            catch { return false; }
        }

        // ============================================================
        // Bind nested Level-2 / Level-3 repeaters
        // ============================================================
        protected void rptLevel1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var l1 = e.Item.DataItem as TopMenuLevel1;
            if (l1 == null) return;

            var rptLevel2 = e.Item.FindControl("rptLevel2") as Repeater;
            if (rptLevel2 == null) return;

            if (l1.LVL2 != null && l1.LVL2.Count > 0)
            {
                rptLevel2.DataSource = l1.LVL2;
                rptLevel2.DataBind();
            }
        }

        protected void rptLevel2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var l2 = e.Item.DataItem as TopMenuLevel2;
            if (l2 == null) return;

            var rptLevel3 = e.Item.FindControl("rptLevel3") as Repeater;
            if (rptLevel3 == null) return;

            if (l2.LVL3 != null && l2.LVL3.Count > 0)
            {
                rptLevel3.DataSource = l2.LVL3;
                rptLevel3.DataBind();
            }
        }

        // ============================================================
        // Load the menu data (levels 1, 2, 3)
        // ============================================================
        private void LoadMenu()
        {
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList mainList = null;
                    string parentWeb = web.Url + "/";
                    if (parentWeb.Contains(site.Url))
                    {
                        parentWeb = parentWeb.Replace(site.Url, "");
                    }

                    // Walk up the web tree until we find a web that has TopMenuLevel1
                    while (mainList == null)
                    {
                        try
                        {
                            using (SPWeb pWeb = site.OpenWeb(parentWeb))
                            {
                                mainList = pWeb.Lists.TryGetList(MENU_LEVEL_1);

                                if (mainList != null)
                                {
                                    List<TopMenuLevel1> level1 = LoadLevel1(pWeb, mainList);
                                    BindMenu(level1);
                                    return;
                                }

                                // Move up to the parent web
                                if (pWeb.ParentWeb == null) break;

                                parentWeb = pWeb.ParentWeb.Url + "/";
                                if (parentWeb.Contains(site.Url))
                                {
                                    parentWeb = parentWeb.Replace(site.Url, "");
                                }
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }
        }

        private List<TopMenuLevel1> LoadLevel1(SPWeb pWeb, SPList list)
        {
            // NOTE: Original code had an invalid CAML query (stray '<' before </Where>).
            // This is the corrected version.
            SPQuery query = new SPQuery
            {
                Query = @"<Where>
                            <Eq>
                                <FieldRef Name='Visibility' />
                                <Value Type='Boolean'>1</Value>
                            </Eq>
                          </Where>
                          <OrderBy>
                            <FieldRef Name='ItemOrder' Ascending='TRUE' />
                          </OrderBy>"
            };

            SPListItemCollection items = list.GetItems(query);
            List<TopMenuLevel1> level1 = SPFactory.MapListItemsToClass<TopMenuLevel1>(items);
            if (level1 == null) level1 = new List<TopMenuLevel1>();

            // For each level-1 item, fetch its level-2 children
            SPList sub = pWeb.Lists.TryGetList(MENU_LEVEL_2);
            SPList subSub = pWeb.Lists.TryGetList(MENU_LEVEL_3);

            foreach (var l1 in level1)
            {
                l1.LVL2 = new List<TopMenuLevel2>();

                if (sub != null)
                {
                    var l2Query = new SPQuery
                    {
                        Query = $@"<Where>
                                     <And>
                                       <Eq>
                                         <FieldRef Name='Parent' />
                                         <Value Type='Lookup'>{l1.Title}</Value>
                                       </Eq>
                                       <Eq>
                                         <FieldRef Name='Visibility' />
                                         <Value Type='Boolean'>1</Value>
                                       </Eq>
                                     </And>
                                   </Where>
                                   <OrderBy>
                                     <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                   </OrderBy>"
                    };

                    var l2Items = sub.GetItems(l2Query);
                    var l2List = SPFactory.MapListItemsToClass<TopMenuLevel2>(l2Items);
                    if (l2List == null) l2List = new List<TopMenuLevel2>();

                    // Fetch level-3 for each level-2
                    if (subSub != null)
                    {
                        foreach (var l2 in l2List)
                        {
                            l2.LVL3 = new List<TopMenuLevel3>();

                            var l3Query = new SPQuery
                            {
                                Query = $@"<Where>
                                             <And>
                                               <Eq>
                                                 <FieldRef Name='Parent' />
                                                 <Value Type='Lookup'>{l2.Title}</Value>
                                               </Eq>
                                               <Eq>
                                                 <FieldRef Name='Visibility' />
                                                 <Value Type='Boolean'>1</Value>
                                               </Eq>
                                             </And>
                                           </Where>
                                           <OrderBy>
                                             <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                           </OrderBy>"
                            };

                            var l3Items = subSub.GetItems(l3Query);
                            var l3List = SPFactory.MapListItemsToClass<TopMenuLevel3>(l3Items);
                            if (l3List != null) l2.LVL3 = l3List;
                        }
                    }

                    l1.LVL2 = l2List;
                }
            }

            return level1;
        }

        private void BindMenu(List<TopMenuLevel1> data)
        {
            if (data == null) data = new List<TopMenuLevel1>();
            rptLevel1.DataSource = data;
            rptLevel1.DataBind();
        }
    }

}
