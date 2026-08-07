using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Org.BouncyCastle.Ocsp;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucServiceDetails : UserControl
    {
        private int uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string testParameter = Request.QueryString["eti"];
                uid = Convert.ToInt32(testParameter);
                //   lblquerys.Text = testParameter;
                Bindservices();

                
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            

        }
        private void Bindservices()
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
                            //query.Query = "<Where><Eq><FieldRef Name='Active' /><Value Type='Boolean'>1</Value></Eq></Where>";
                            query.Query = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + uid + "</Value></Eq></Where>";



                            SPListItemCollection collitem = list.GetItems(query);
                            if (collitem != null)
                            {
                                //SPItem itemX = collitem.GetItemById(uid);

                                List<EServicesList> eserviceList = SPFactory.MapListItemsToClass<EServicesList>(collitem);
                                EServicesList eservice = new EServicesList();

                                string localizedTitle = SPFactory.GetLocalizedTitle(eserviceList[0].ARServiceName, eserviceList[0].ENServiceName);
                                SetBrowserTitle(localizedTitle);
                                //var head = FindPlaceHolder(Page.Master, "PlaceHolderAdditionalPageHead");
                                //if (head != null)
                                //{
                                //    string desc = HttpUtility.HtmlEncode(
                                //        Publics.TruncateText(SPFactory.GetLocalizedTitle(eserviceList[0].Desc, eserviceList[0].Desc_EN), 160));
                                //    head.Controls.Add(new LiteralControl(
                                //        $"<meta name=\"description\" content=\"{desc}\" />"));
                                //}

                                rptAllData.DataSource = eserviceList;
                                rptAllData.DataBind();

                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected void rptAllData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Find the <li> element
                HtmlGenericControl req1 = (HtmlGenericControl)e.Item.FindControl("req1");
                HtmlGenericControl req2 = (HtmlGenericControl)e.Item.FindControl("req2");
                HtmlGenericControl req3 = (HtmlGenericControl)e.Item.FindControl("req3");
                HtmlGenericControl UserManualLink = (HtmlGenericControl)e.Item.FindControl("UserManualLink");

                // Get the data
                string req1String = SPFactory.GetLocalizedTitle(DataBinder.Eval(e.Item.DataItem, "_x0052_eq1"), DataBinder.Eval(e.Item.DataItem, "Req1_EN"));

                string req2String = SPFactory.GetLocalizedTitle(DataBinder.Eval(e.Item.DataItem, "_x0052_eq2"), DataBinder.Eval(e.Item.DataItem, "Req2_EN"));

                string req3String = SPFactory.GetLocalizedTitle(DataBinder.Eval(e.Item.DataItem, "_x0052_eq3"), DataBinder.Eval(e.Item.DataItem, "Req3_EN"));

                string UserManualURL = DataBinder.Eval(e.Item.DataItem, "UserManualURL")?.ToString();
                
                if (string.IsNullOrEmpty(UserManualURL))
                {
                    UserManualLink.Visible = false;
                }

                if (string.IsNullOrEmpty(req1String))
                {
                    //  preq1.Disabled = true;
                    req1.Visible = false;
                }

                if (string.IsNullOrEmpty(req2String))
                {
                    //  preq1.Disabled = true;
                    req2.Visible = false;
                }

                if (string.IsNullOrEmpty(req3String))
                {
                    //  preq1.Disabled = true;
                    req3.Visible = false;
                }
                
            }
        }

        private void SetBrowserTitle(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title)) return;
                // make the dynamic title available to ucHomeHeader
                HttpContext.Current.Items["PNU_BrowserTitle"] = title;
                var placeholder = FindPlaceHolder(Page.Master, "PlaceHolderPageTitle");
                if (placeholder != null)
                {
                    placeholder.Controls.Clear();
                    placeholder.Controls.Add(new LiteralControl(HttpUtility.HtmlEncode(title)));
                }
                else
                {
                    // fallback if the master exposes a normal <head runat="server">
                    Page.Title = title;
                }
            }
            catch (Exception ex)
            {
                //Publics.WriteToLog("ucMediaDetails.SetBrowserTitle", ex);
            }
        }

        // Handles nested master pages (e.g. DGA_Internal.master under a root master)
        private ContentPlaceHolder FindPlaceHolder(MasterPage master, string id)
        {
            while (master != null)
            {
                var ph = master.FindControl(id) as ContentPlaceHolder;
                if (ph != null) return ph;
                master = master.Master;
            }
            return null;
        }



    }
}
