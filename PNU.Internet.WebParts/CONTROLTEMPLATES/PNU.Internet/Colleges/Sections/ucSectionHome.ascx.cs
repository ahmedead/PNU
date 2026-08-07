using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Portal.Main.Helper;
using System.Runtime.Remoting;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections
{
    public partial class ucSectionHome : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string CollegeCode = "";
                if (Page.Request.QueryString["Source"] != null)
                {
                    CollegeCode = Page.Request.QueryString["Source"].ToString();
                }
                else
                {
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists["AboutCollege"];
                            if (list != null)
                            {

                                SPListItemCollection collitem = list.GetItems();
                                if (collitem != null)
                                {
                                    if (collitem.Count > 0)
                                        if (collitem[0]["College_Code"] != null)
                                            CollegeCode = collitem[0]["College_Code"].ToString().Trim();


                                }


                            }
                        }
                    }


                }

                if (Page.Request.QueryString["SecCode"] != null)
                {
                    var Source = Page.Request.QueryString["SecCode"].ToString();
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList _CollegesList = web.Lists["AllFacultyDepartments"];
                            SPQuery query = new SPQuery();
                            

                            query.Query = $@"<Where>
                                          <And>
                                             <Eq>
                                                <FieldRef Name='Code' />
                                                <Value Type='Text'>{Source}</Value>
                                             </Eq>
                                             <Eq>
                                                <FieldRef Name='COLL_CODE' />
                                                <Value Type='Text'>{CollegeCode}</Value>
                                             </Eq>
                                          </And>
                                       </Where>";



                            List<AllFacultyDepartments> _Data = SPFactory.GetAllItemsByQuery<AllFacultyDepartments>("Admin", Settings.AllFacultyDepartments, query);
                            if (_Data != null && _Data.Count > 0)
                            {
                                string localizedTitle = SPFactory.GetLocalizedTitle(_Data[0].COLL_DESC + " - " + _Data[0].Title, _Data[0].COLL_DESC_EN + " - " + _Data[0].Title_EN);
                                SetBrowserTitle(localizedTitle);
                                //var head = FindPlaceHolder(Page.Master, "PlaceHolderAdditionalPageHead");
                                //if (head != null)
                                //{
                                //    string desc = HttpUtility.HtmlEncode(
                                //        Publics.TruncateText(SPFactory.GetLocalizedTitle(_Data[0].Description, _Data[0].Description_EN), 160));
                                //    head.Controls.Add(new LiteralControl(
                                //        $"<meta name=\"description\" content=\"{desc}\" />"));
                                //}
                                rptMainData.DataSource = _Data;
                                rptMainData.DataBind();


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
        private ContentPlaceHolder FindPlaceHolder(System.Web.UI.MasterPage master, string id)
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
