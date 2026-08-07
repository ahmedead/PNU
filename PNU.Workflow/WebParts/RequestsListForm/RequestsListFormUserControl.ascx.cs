using Microsoft.SharePoint;
using PNU.Workflow.Classes;
using PNU.Workflow.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Workflow.WebParts.RequestsListForm
{
    public partial class RequestsListFormUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetRequestsList();
            }
        }

        private void GetRequestsList()
        {
            
            List<RequestModel> reqList = new List<RequestModel>();
            if(SPContext.Current.Web.CurrentUser == null)
            {

                return;
            }
            var currentUser = SPContext.Current.Web.CurrentUser.LoginName;

            if(currentUser == null )
            {
                return;
            }

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("/ar/MediaCenter/News/"))
                        {
                            
                            
                            var isApprovers = Helper.GetUsersInGroup("PNU-WF").Any(a => a.LoginName == currentUser);

                            SPList requestsList = web.Lists["RequestsList"];
                            SPQuery query = new SPQuery();
                            if (!isApprovers)
                            {
                                query.Query = "<Eq><FieldRef Name='Author'/><Value Type='Text'>" + currentUser + "</Value></Eq>" +
                                    "<OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy>";
                            }

                            SPListItemCollection listItemColl = requestsList.GetItems(query);
                            if (listItemColl == null || listItemColl.Count <= 0)
                                return;

                            foreach (SPListItem item in listItemColl)
                            {
                                var reqeust = new RequestModel
                                {
                                    CreateDate = Convert.ToDateTime(item["Created"]),
                                    RequestId = Convert.ToInt32(item["ID"]),
                                    Requester = Convert.ToString(item["Author"]),
                                    RequestStatus = Convert.ToString(item["RequestStatus"]),
                                    Title= Convert.ToString(item["Title"])

                                };

                                reqList.Add(reqeust);
                            }
                           
                        }
                    }
                }
            });

            rep.DataSource = reqList;
            rep.DataBind();

        }
    
    }
}
