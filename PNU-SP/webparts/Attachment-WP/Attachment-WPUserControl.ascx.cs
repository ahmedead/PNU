using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;


namespace PNU_SP.webparts.Attachment_WP
{
    public partial class Attachment_WPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetList();
        }
        protected void GetList()
        {
            //string downloadLocation = @"c:\\";
            List<AttachmentFile> AttList = new List<AttachmentFile>();
;            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb oSPWeb = site.OpenWeb())
                    {
                        oSPWeb.AllowUnsafeUpdates = true;

                        //Get the List
                        SPList list = oSPWeb.Lists["test"];

                        //Get all list items
                        SPListItemCollection itemCollection = list.Items;

                        //Loop through each list item
                        foreach (SPListItem item in itemCollection)
                        {
                            var attFile = new AttachmentFile();
                            //string destinationFolder = downloadLocation + "\\" + item.ID;
                            //if (!Directory.Exists(destinationFolder))
                            //{
                            //    Directory.CreateDirectory(destinationFolder);
                            //}

                            //Get all attachments
                            SPAttachmentCollection attachmentsColl = item.Attachments;

                            attFile.ItemId = item.ID;
                            attFile.Title = item.Title;
                            //Loop through each attachment
                            //foreach (string attachment in attachmentsColl)
                            //{
                            //    SPFile file = oSPWeb.GetFile(attachmentsColl.UrlPrefix + attachment);
                            //    string filePath = destinationFolder + "\\" + attachment;

                            //    byte[] binFile = file.OpenBinary();

                            //    attFile.FileUrl = file.ServerRelativeUrl;
                            //}

                            SPFile file = oSPWeb.GetFile(attachmentsColl.UrlPrefix + attachmentsColl[0]);
                            attFile.FileUrl = file.ServerRelativeUrl;


                            AttList.Add(attFile);
                        }

                        rep.DataSource = AttList;
                        rep.DataBind();

                    }


                }
            });
         
        }

        protected void downloadFile(int itemId)
        {
          //  string downloadLocation = @"c:\\";
            List<AttachmentFile> AttList = new List<AttachmentFile>();
            ; SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb oSPWeb = site.OpenWeb())
                    {
                        oSPWeb.AllowUnsafeUpdates = true;

                        //Get the List
                        SPList list = oSPWeb.Lists["test"];

                        //Get all list items
                        SPListItemCollection itemCollection = list.Items;



                        SPListItem item = list.GetItemById(itemId);
                        SPAttachmentCollection attachments = item.Attachments;

                      //  string destinationFolder = downloadLocation + "\\" + item.ID;

                        SPFile file = oSPWeb.GetFile(attachments.UrlPrefix + attachments[0]);
                       // string filePath = destinationFolder + "\\" + attachments[0];

                        byte[] binFile = file.OpenBinary();
                       // System.IO.FileStream fs = System.IO.File.Create(filePath);
                        //fs.Write(binFile, 0, binFile.Length);
                        //fs.Close();

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

       
        protected void rep_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "download")
            {
                //string filename = e.CommandArgument.ToString();
                //string path = MapPath("~/Docfiles/" + filename);
                //byte[] bts = System.IO.File.ReadAllBytes(path);
                //Response.Clear();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Type", "Application/octet-stream");
                //Response.AddHeader("Content-Length", bts.Length.ToString());
                //Response.AddHeader("Content-Disposition", "attachment; filename=" +
                //filename);
                //Response.BinaryWrite(bts);
                //Response.Flush();
                int itemId = Convert.ToInt32(e.CommandArgument.ToString());
                downloadFile(itemId);
            }
        }
    }
    public class AttachmentFile
    {
        public int ItemId { get; set; }
        public string  Title { get; set; }
        public string  FileUrl { get; set; }
        //public List<string> FileUrl { get; set; }
    }
}
