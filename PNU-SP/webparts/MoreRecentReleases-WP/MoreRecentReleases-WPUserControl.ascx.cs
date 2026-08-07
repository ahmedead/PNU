using Microsoft.SharePoint;
using Portal.Main.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU_SP.webparts.MoreRecentReleases_WP
{
    public partial class MoreRecentReleases_WPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["ItemId"] == null)
                return;
            if (!Page.IsPostBack)
            {
                GetRecentReleases();
            }

        }
        private void GetRecentReleases()
        {

            ArrayList qryParam = new ArrayList();

            if (Page.Request.QueryString["ItemId"] == null)
                return;

            var ItemId = Convert.ToInt32(Page.Request.QueryString["ItemId"]);
            qryParam.Add("<Neq><FieldRef Name='ID' /><Value Type='Number'>" + ItemId + "</Value></Neq>");
            string whereQry = "<Where>{0}</Where>";

            for (int i = 0; i < qryParam.Count; i++)
            {
                if (i < qryParam.Count - 1)
                    whereQry = String.Format(whereQry, "<And>" + qryParam[i] +
                      "{0}</And>");
                else
                    whereQry = String.Format(whereQry, qryParam[i]);
            }

            whereQry = String.Format(whereQry, String.Empty);
            SPQuery qry = new SPQuery();

            qry.Query = @"<Where>
                  <Neq>
                     <FieldRef Name='ItemId' />
                     <Value Type='Number'>" + ItemId + @"</Value>
                  </Neq>
               </Where>";
            qry.RowLimit = 3;
            //var result = Helper.LoadListDynamicByCML(SPContext.Current.Web.Url, MoreRecentReleases_WP._ListName, qryParam, 3);

            SPListItemCollection ItemCol = null;
            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(PortalHelper.ParentLangSite + "MediaCenter/RecentReleases/"))
                {
                    SPList list = web.Lists.TryGetList(MoreRecentReleases_WP._ListName);
                    if (list != null)
                    {

                        ItemCol = list.GetItems(qry);
                        if (ItemCol == null || ItemCol.Count == 0)
                        {
                            rep.DataSource = null;
                            rep.DataBind();
                            return;
                        }
                        rep.DataSource = ItemCol.GetDataTable();
                        rep.DataBind();
                    }
                        //ItemCol = web.Lists[MoreRecentReleases_WP._ListName].GetItems(qry);
                    
                }
            }



        }
        protected void rep_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "download")
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                downloadFile(itemId);
            }
        }
        protected void downloadFile(int itemId)
        {
            List<AttachmentFile> AttList = new List<AttachmentFile>();
            ; SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb oSPWeb = site.OpenWeb())
                    {
                        oSPWeb.AllowUnsafeUpdates = true;

                        //Get the List
                        SPList list = oSPWeb.Lists[MoreRecentReleases_WP._ListName];

                        //Get all list items
                        SPListItemCollection itemCollection = list.Items;



                        SPListItem item = list.GetItemById(itemId);
                        SPAttachmentCollection attachments = item.Attachments;



                        SPFile file = oSPWeb.GetFile(attachments.UrlPrefix + attachments[0]);


                        byte[] binFile = file.OpenBinary();

                        Response.Clear();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Length", binFile.Length.ToString());
                        Response.AddHeader("Content-Disposition", "attachment; filename=" +
                        item["Title"]);
                        Response.BinaryWrite(binFile);
                        Response.Flush();


                    }


                }
            });

        }


    }
}
