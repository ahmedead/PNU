using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static Microsoft.SharePoint.SPFile;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin
{
    public partial class ucCheckOutCheckIn : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GetSiteAndWeb(out SPSite site, out SPWeb web)
        {
            string webUrl = txtWebURL.Text.Trim();
            if (!string.IsNullOrEmpty(webUrl))
            {
                site = new SPSite(webUrl);
                web = site.OpenWeb(new Uri(webUrl).AbsolutePath);
            }
            else
            {
                site = new SPSite(SPContext.Current.Site.ID);
                web = site.OpenWeb();
            }
        }

        public void CheckInFolderContent()
        {
            string folderRelativeUrl = txtFolderPath.Text.Trim() == "" ? "Style Library/NewStyle/UI5/" : txtFolderPath.Text.Trim();

            SPSite site = null;
            SPWeb web = null;
            try
            {
                GetSiteAndWeb(out site, out web);
                SPFolder folder = web.GetFolder(folderRelativeUrl);
                if (folder.Exists)
                {
                    int total = 0, checkedIn = 0, published = 0, approved = 0, errors = 0;
                    ProcessFolderCheckIn(folder, ref total, ref checkedIn, ref published, ref approved, ref errors);
                    lblResult.Text = string.Format("Completed Check-In & Approval on '{0}'. Total: {1}, CheckedIn: {2}, Published: {3}, Approved: {4}, Errors: {5}",
                        folderRelativeUrl, total, checkedIn, published, approved, errors);
                    lblResult.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblResult.Text = string.Format("Folder '{0}' does not exist in Web '{1}'.", folderRelativeUrl, web.Url);
                    lblResult.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error during Check-In: " + ex.Message;
                lblResult.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                if (web != null) web.Dispose();
                if (site != null) site.Dispose();
            }
        }

        private void ProcessFolderCheckIn(SPFolder folder, ref int total, ref int checkedIn, ref int published, ref int approved, ref int errors)
        {
            foreach (SPFolder subFolder in folder.SubFolders)
            {
                if (subFolder.Name.Equals("Forms", StringComparison.OrdinalIgnoreCase))
                    continue;

                ProcessFolderCheckIn(subFolder, ref total, ref checkedIn, ref published, ref approved, ref errors);
            }

            foreach (SPFile file in folder.Files)
            {
                total++;
                try
                {
                    if (file.CheckOutType != SPCheckOutType.None)
                    {
                        file.CheckIn("Batch check-in by Admin", SPCheckinType.MajorCheckIn);
                        checkedIn++;
                    }
                    else if (file.Level == SPFileLevel.Draft)
                    {
                        file.Publish("Published by Admin");
                        published++;
                    }

                    // Content Approval (Moderation)
                    if (file.Item != null && file.Item.ParentList.EnableModeration)
                    {
                        if (file.Item.ModerationInformation != null &&
                            file.Item.ModerationInformation.Status != SPModerationStatusType.Approved)
                        {
                            file.Approve("Approved by Admin");
                            approved++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors++;
                }
            }
        }

        public void ApproveFolderContent()
        {
            string folderRelativeUrl = txtFolderPath.Text.Trim() == "" ? "_catalogs/masterpage" : txtFolderPath.Text.Trim();

            SPSite site = null;
            SPWeb web = null;
            try
            {
                GetSiteAndWeb(out site, out web);
                SPFolder folder = web.GetFolder(folderRelativeUrl);
                if (folder.Exists)
                {
                    int total = 0, approved = 0, errors = 0;
                    ProcessFolderApprove(folder, ref total, ref approved, ref errors);
                    lblResult.Text = string.Format("Completed Approval on '{0}'. Total: {1}, Approved: {2}, Errors: {3}",
                        folderRelativeUrl, total, approved, errors);
                    lblResult.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblResult.Text = string.Format("Folder '{0}' does not exist in Web '{1}'.", folderRelativeUrl, web.Url);
                    lblResult.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error during Approval: " + ex.Message;
                lblResult.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                if (web != null) web.Dispose();
                if (site != null) site.Dispose();
            }
        }

        private void ProcessFolderApprove(SPFolder folder, ref int total, ref int approved, ref int errors)
        {
            foreach (SPFolder subFolder in folder.SubFolders)
            {
                if (subFolder.Name.Equals("Forms", StringComparison.OrdinalIgnoreCase))
                    continue;

                ProcessFolderApprove(subFolder, ref total, ref approved, ref errors);
            }

            foreach (SPFile file in folder.Files)
            {
                total++;
                try
                {
                    if (file.Item != null && file.Item.ParentList.EnableModeration)
                    {
                        if (file.Item.ModerationInformation != null &&
                            file.Item.ModerationInformation.Status != SPModerationStatusType.Approved)
                        {
                            file.Approve("Approved by Admin");
                            approved++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors++;
                }
            }
        }

        public void CheckOutFolderContent()
        {
            string folderRelativeUrl = txtFolderPath.Text.Trim() == "" ? "Style Library/NewStyle/UI5/" : txtFolderPath.Text.Trim();

            SPSite site = null;
            SPWeb web = null;
            try
            {
                GetSiteAndWeb(out site, out web);
                SPFolder folder = web.GetFolder(folderRelativeUrl);
                if (folder.Exists)
                {
                    int total = 0, checkedOut = 0, errors = 0;
                    ProcessFolderCheckOut(folder, ref total, ref checkedOut, ref errors);
                    lblResult.Text = string.Format("Completed Check-Out on '{0}'. Total: {1}, CheckedOut: {2}, Errors: {3}",
                        folderRelativeUrl, total, checkedOut, errors);
                    lblResult.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblResult.Text = string.Format("Folder '{0}' does not exist in Web '{1}'.", folderRelativeUrl, web.Url);
                    lblResult.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error during Check-Out: " + ex.Message;
                lblResult.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                if (web != null) web.Dispose();
                if (site != null) site.Dispose();
            }
        }

        private void ProcessFolderCheckOut(SPFolder folder, ref int total, ref int checkedOut, ref int errors)
        {
            foreach (SPFolder subFolder in folder.SubFolders)
            {
                if (subFolder.Name.Equals("Forms", StringComparison.OrdinalIgnoreCase))
                    continue;

                ProcessFolderCheckOut(subFolder, ref total, ref checkedOut, ref errors);
            }

            foreach (SPFile file in folder.Files)
            {
                total++;
                try
                {
                    if (file.CheckOutType == SPCheckOutType.None)
                    {
                        file.CheckOut();
                        checkedOut++;
                    }
                }
                catch (Exception ex)
                {
                    errors++;
                }
            }
        }

        protected void btnCheckIn_Click(object sender, EventArgs e)
        {
            CheckInFolderContent();
        }

        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            CheckOutFolderContent();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            ApproveFolderContent();
        }
    }
}