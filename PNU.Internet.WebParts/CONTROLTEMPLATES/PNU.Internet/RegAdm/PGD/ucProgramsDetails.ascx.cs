using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD
{
    public partial class ucProgramsDetails : UserControl
    {
        public bool IsArabic
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                    {
                        string url = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                        // مقطع المسار الأول لا البحث في المسار كله: Contains("/en/") كانت
                        // تُطابق رمز كلية الهندسة في /ar/Faculties/EN/... و EndsWith("/en") تُطابق
                        // أي مسار ينتهي بمجلد اسمه EN. نفس الإصلاح في SEO.ascx.cs.
                        if (url.StartsWith("/en/") || url.Equals("/en")) return false;
                    }
                    if (SPContext.Current != null && SPContext.Current.Web != null)
                    {
                        return SPContext.Current.Web.Language == 1025;
                    }
                    return System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower() != "en";
                }
                catch { return true; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // OutputCache fix for QueryString parameters
                Response.Cache.SetNoServerCaching();
                Response.Cache.VaryByParams["CatID"] = true;
                if (!IsPostBack)
                {
                    if (Page.Request.QueryString["CatID"] == null)
                        return;
                    int ID = 0;
                    ID = Convert.ToInt32(Page.Request.QueryString["CatID"]);
                    clsAdmissionServices _scholarships = new clsAdmissionServices();
                    List<clsscholarshipsPrograms> _scholarshipsPrograms = new List<clsscholarshipsPrograms>();
                    List<clsStrategicPlan> _StrategicPlan = new List<clsStrategicPlan>();
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList lstServices = web.Lists["scholarships"];
                                if (lstServices != null)
                                {
                                    //SPListItem collitem = lstServices.GetItemById(ID);

                                    SPListItem collitem = null; ;
                                    SPQuery query = new SPQuery();
                                    query.Query = $@"<Where>
                                                      <Eq>
                                                         <FieldRef Name='CatID' />
                                                         <Value Type='Number'>{ID}</Value>
                                                      </Eq>
                                                   </Where>
                                                  ";

                                    SPListItemCollection sercollitems = lstServices.GetItems(query);

                                    if (sercollitems != null && sercollitems.Count > 0)
                                    {
                                        collitem = sercollitems[0];

                                    }



                                    if (collitem != null)
                                    {
                                        _scholarships = SPFactory.MapListItemsToClass<clsAdmissionServices>(collitem);
                                        lblscholarships.Text = _scholarships.Title;
                                        SPList lstScholarshipsPrograms = web.Lists["scholarshipsPrograms"];
                                        if (lstScholarshipsPrograms != null)
                                        {
                                            query = new SPQuery();
                                            query.Query = $@"<Where>
                                                      <Eq>
                                                         <FieldRef Name='scholarships' />
                                                         <Value Type='Lookup'>{_scholarships.Title}</Value>
                                                      </Eq>
                                                   </Where>
                                                   <OrderBy>
                                                      <FieldRef Name='ItemOrder' Ascending='True' />
                                                   </OrderBy>";

                                            SPListItemCollection collitems = lstScholarshipsPrograms.GetItems(query);

                                            if (collitems != null && collitems.Count > 0)
                                            {
                                                _scholarshipsPrograms = SPFactory.MapListItemsToClass<clsscholarshipsPrograms>(collitems);

                                            }


                                        }
                                    }



                                }
                            }
                        }
                    });



                    rptServices.DataSource = _scholarshipsPrograms;
                    rptServices.DataBind();



                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }


        }

    }
}
