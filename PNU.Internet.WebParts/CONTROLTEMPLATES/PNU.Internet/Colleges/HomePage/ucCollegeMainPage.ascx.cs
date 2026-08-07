using Microsoft.SharePoint;
using Org.BouncyCastle.Ocsp;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.HomePage
{
    public partial class ucCollegeMainPage : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string CollegeCode = "";
                if(Page.Request.QueryString["Source"] != null)
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
                                    if(collitem.Count > 0)
                                        if (collitem[0]["College_Code"] != null)
                                            CollegeCode = collitem[0]["College_Code"].ToString().Trim();


                                }


                            }
                        }
                    }


                }
                if (CollegeCode != "")
                {
                    var Source = CollegeCode;
                    using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList _CollegesList = web.Lists["AllFaculties"];
                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where>
                                                      <Eq>
                                                         <FieldRef Name='Code' />
                                                         <Value Type='Text'>" + Source + @"</Value>
                                                      </Eq>
                                                   </Where>");



                            List<AllFaculties> _Data = SPFactory.GetAllItemsByQuery<AllFaculties>("Admin", Settings.AllFaculties, query);
                            if (_Data != null && _Data.Count > 0)
                            {
                                rptMainData.DataSource = _Data;
                                rptMainData.DataBind();


                            }


                            SPList _DeptList = web.Lists["AllFacultyDepartments"];
                            query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where>
                                                      <Eq>
                                                         <FieldRef Name='COLL_CODE' />
                                                         <Value Type='Text'>" + Source + @"</Value>
                                                      </Eq>
                                                   </Where>"
                            );


                            //SPListItemCollection objNew = null;
                            //objNew = _DeptList.GetItems(query);
                            //if (objNew != null && objNew.Count > 0)
                            //{
                            //    List<AllFacultyDepartments> _DeptData = SPFactory.MapListItemsToClass<AllFacultyDepartments>(objNew);
                            //    if (_DeptData != null && _DeptData.Count > 0)
                            //    {
                            //        rptSections.DataSource = _DeptData;
                            //        rptSections.DataBind();


                            //    }
                            //}



                        }
                    }


                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }

            
        }
    
    }
}
