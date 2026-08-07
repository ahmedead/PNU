using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News
{
    public partial class ucCollegeNewsDetails : UserControl
    {
        public string RedirectURL { get; set; } = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string url = HttpContext.Current.Request.Url.ToString();
                    var queryParameters = HttpUtility.ParseQueryString(new Uri(url).Query);
                    string idValue = queryParameters["RequestID"];

                    if (url.Contains("RequestID="))
                    {
                        var ID = idValue;

                        clsRequestsList _CurrentNew = new clsRequestsList();

                        string CollegeCode = "";
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    SPList list = web.Lists["DepartmentDetails"];
                                    if (list != null)
                                    {

                                        SPListItemCollection collitem = list.GetItems();
                                        if (collitem != null)
                                        {
                                            if (collitem.Count > 0)
                                                if (collitem[0]["DeptCode"] != null)
                                                    CollegeCode = collitem[0]["DeptCode"].ToString().Trim();
                                        }


                                    }
                                }
                            }

                            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                            {
                                using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                                {
                                    SPList reqList = web.Lists["RequestsList"];

                                    SPQuery query = new SPQuery();
                                    query.Query = $@"<Where>
                                              <And>
                                                 <Eq>
                                                    <FieldRef Name='RequestStatus' />
                                                    <Value Type='Choice'>Approved</Value>
                                                 </Eq>
                                                 <And>
                                                    <Neq>
                                                       <FieldRef Name='ID' />
                                                       <Value Type='Counter'>{ID}</Value>
                                                    </Neq>
                                                    <Eq>
                                                        <FieldRef Name='FacultyName_EN' />
                                                        <Value Type='Text'>{CollegeCode}</Value>
                                                    </Eq>
                                                 </And>
                                              </And>
                                           </Where>
                                           <OrderBy>
                                              <FieldRef Name='MediaDate' Ascending='False' />
                                           </OrderBy>";


                                    
                                    query.RowLimit = Convert.ToUInt32(3);
                                    SPListItemCollection _AllData = reqList.GetItems(query);
                                    if (_AllData != null && _AllData.Count > 0)
                                    {
                                        List<clsRequestsList> _AllItems = new List<clsRequestsList>();
                                        _AllItems = SPFactory.MapListItemsToClass<clsRequestsList>(_AllData);
                                        rptNews.DataSource = _AllItems;
                                        rptNews.DataBind();

                                    }

                                    SPListItem sPListItem = reqList.GetItemById(Convert.ToInt32(ID));
                                    _CurrentNew = SPFactory.MapListItemsToClass<clsRequestsList>(sPListItem);

                                }
                            }
                        });
                            

                        if (_CurrentNew != null)
                        {
                            List<clsRequestsList> _AllData = new List<clsRequestsList>();
                            _AllData.Add(_CurrentNew);
                            rptMainData.DataSource = _AllData;
                            rptMainData.DataBind();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            if (RedirectURL != null && RedirectURL != "")
            {
                Response.Redirect(RedirectURL);
            }
            else
                Response.Redirect("default.aspx");

        }
    }
}
