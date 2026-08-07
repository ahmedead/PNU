using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency
{
    public partial class ucAllDeenShipsAndDepartments : UserControl
    {
        public string ListName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                if (ListName == "" || ListName == null)
                    ListName = "Departments";
                if (!Page.IsPostBack)
                {


                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    SPList requestsList = web.Lists["Departments"];
                                    SPQuery query = new SPQuery();
                                    query.Query = string.Concat(
                                         @"<OrderBy><FieldRef Name='ItemOrder' Ascending='False' /></OrderBy>");

                                    SPListItemCollection items = requestsList.GetItems(query);

                                    if (items == null || items.Count == 0)
                                        return;

                                    List<clsDepartments> _AllItems = SPFactory.MapListItemsToClass<clsDepartments>(items);

                                    List<clsDepartments> lst0 = new List<clsDepartments>();
                                    List<clsDepartments> lst1 = new List<clsDepartments>();
                                    List<clsDepartments> lst2 = new List<clsDepartments>();
                                    List<clsDepartments> lst3 = new List<clsDepartments>();
                                    List<clsDepartments> lst4 = new List<clsDepartments>();
                                    List<clsDepartments> lst5 = new List<clsDepartments>();


                                    if (_AllItems != null && _AllItems.Count > 0)
                                    {
                                        for (int i = 0; i < _AllItems.Count; i++)
                                        {
                                            if (i == 0)
                                            {
                                                lst0.Add(_AllItems[i]);
                                            }
                                            else if (i == 1)
                                            {
                                                lst1.Add(_AllItems[i]);
                                            }
                                            else if (i == 2)
                                            {
                                                lst2.Add(_AllItems[i]);
                                            }
                                            else if (i == 3)
                                            {
                                                lst3.Add(_AllItems[i]);
                                            }
                                            else if (i == 4)
                                            {
                                                lst4.Add(_AllItems[i]);
                                            }
                                            else
                                            {
                                                lst5.Add(_AllItems[i]);
                                            }

                                        }


                                        rpt0.DataSource = lst0;
                                        rpt0.DataBind();

                                        rpt1.DataSource = lst1;
                                        rpt1.DataBind();

                                        rpt2.DataSource = lst2;
                                        rpt2.DataBind();

                                        rpt3.DataSource = lst3;
                                        rpt3.DataBind();

                                        rpt4.DataSource = lst4;
                                        rpt4.DataBind();

                                        rpt5.DataSource = lst5;
                                        rpt5.DataBind();

                                    }



                                }
                            }
                        }
                    });








                }


            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }
    }


    public class clsDepartments
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string URL { get; set; }

        public string PublishingRollupImage { get; set; }

    }

    public static class busclsDepartments
    {
        public static List<clsDepartments> GetAllItems(string ListName)
        {
            
                List<clsDepartments> _allItems = new List<clsDepartments>();
                try
                {

                    SPListItemCollection items = null;
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPList list = web.GetList(PortalHelper.ParentLangSite + "Agencies/Lists/" + ListName);
                                if (list != null)
                                {
                                    SPQuery query = new SPQuery();
                                    query.Query = string.Concat(
                                         @"<OrderBy>
                                  <FieldRef Name='ItemOrder' Ascending='True' />
                               </OrderBy>");
                                    items = list.GetItems(query);
                                    _allItems = SPFactory.MapListItemsToClass<clsDepartments>(items);

                                }
                            }
                        }
                    });

                    if (_allItems == null || _allItems.Count == 0)
                        return null;

                }

                catch (Exception ex)
                {
                    Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsDepartments - GetAllItems", ex.Message);
                }



                return _allItems;




           
            
        }
    }
}
