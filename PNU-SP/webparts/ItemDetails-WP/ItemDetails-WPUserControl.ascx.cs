using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU_SP.webparts.ItemDetails_WP
{
    public partial class ItemDetails_WPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["ItemId"] == null)
                return;
            if (!IsPostBack)
            {
                
                GetItemDetailsById();
            }
        }

        private void GetItemDetailsById()
        {

            if (Page.Request.QueryString["ItemId"] == null)
                return;

            var ItemId = Convert.ToInt32(Page.Request.QueryString["ItemId"]);
            ArrayList qryParam = new ArrayList();
            qryParam.Add("<Eq><FieldRef Name='ID'/><Value Type='Number'>" + ItemId + "</Value></Eq>");

            SPListItemCollection listItemColl = Helper.LoadListDynamicByCML(SPContext.Current.Web.Url,ItemDetails_WP._ListName, qryParam,null)
;           if (listItemColl == null || listItemColl.Count <= 0)
                return;

            SPListItem item = listItemColl[0];
            if (item == null) return;

            lblTitle.Text = Convert.ToString(item["Title"]);
            lblDate.Text = Convert.ToString(item["Date"]);
            lblDesc.Text = Convert.ToString(item["Desc"]);
            lblTag.Text = Convert.ToString(item["Tags2"]); //Convert.ToString(item["_x0054_ag1"]);
            //Convert.ToString(item["Tags2"]);

            img.ImageUrl = Convert.ToString(item["ImageUrl"]);



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
                        SPList list = oSPWeb.Lists[ItemDetails_WP._ListName];

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

        protected void btnDownload_Click(object sender, EventArgs e)
        {

            if (Page.Request.QueryString["ItemId"] == null)
                return;

            var ItemId = Convert.ToInt32(Page.Request.QueryString["ItemId"]);
            downloadFile(ItemId);
        }
    }
}
