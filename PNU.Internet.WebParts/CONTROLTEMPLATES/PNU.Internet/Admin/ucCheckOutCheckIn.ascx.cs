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

        public void CheckInFolderContent()
        {
            string webUrl = txtWebURL.Text.Trim() == "" ? "https://pnu.edu.sa" : txtWebURL.Text.Trim();
            string folderRelativeUrl = txtFolderPath.Text.Trim() == "" ? "Style Library/NewStyle/UI5/" : txtFolderPath.Text.Trim();

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        SPFolder folder = web.GetFolder(folderRelativeUrl);
                        if (folder.Exists)
                        {
                            ProcessFolderCheckIn(folder);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., folder not found or permissions)
                        // Log: ex.Message
                    }
                }
            }
        }

        /// <summary>
        /// Recursively processes a folder: first iterates subfolders (depth-first),
        /// then checks in all files in the current folder.
        /// </summary>
        private void ProcessFolderCheckIn(SPFolder folder)
        {
            // First, go through all subfolders recursively
            foreach (SPFolder subFolder in folder.SubFolders)
            {
                // Skip system folders like "Forms"
                if (subFolder.Name.Equals("Forms", StringComparison.OrdinalIgnoreCase))
                    continue;

                ProcessFolderCheckIn(subFolder);
            }

            // Then, process files in the current folder
            foreach (SPFile file in folder.Files)
            {
                try
                {
                    if (file.CheckOutType != SPCheckOutType.None)
                    {
                        file.CheckIn("Batch check-in by Admin", SPCheckinType.MajorCheckIn);
                    }

                    // Publish if the library requires major versions
                    if (file.Item.ParentList.EnableMinorVersions)
                    {
                        file.Publish("Published by Admin");
                    }
                }
                catch (Exception ex)
                {
                    // Log per-file error and continue with next file
                    // Log: string.Format("Error on file {0}: {1}", file.Url, ex.Message);
                }
            }
        }

        public void CheckOutFolderContent()
        {
            string webUrl = txtWebURL.Text.Trim() == "" ? "https://pnu.edu.sa" : txtWebURL.Text.Trim();
            string folderRelativeUrl = txtFolderPath.Text.Trim() == "" ? "Style Library/NewStyle/UI5/" : txtFolderPath.Text.Trim();

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        SPFolder folder = web.GetFolder(folderRelativeUrl);
                        if (folder.Exists)
                        {
                            ProcessFolderCheckOut(folder);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., folder not found or permissions)
                        // Log: ex.Message
                    }
                }
            }
        }

        /// <summary>
        /// Recursively processes a folder: first iterates subfolders (depth-first),
        /// then checks out all files in the current folder.
        /// </summary>
        private void ProcessFolderCheckOut(SPFolder folder)
        {
            // First, go through all subfolders recursively
            foreach (SPFolder subFolder in folder.SubFolders)
            {
                // Skip system folders like "Forms"
                if (subFolder.Name.Equals("Forms", StringComparison.OrdinalIgnoreCase))
                    continue;

                ProcessFolderCheckOut(subFolder);
            }

            // Then, process files in the current folder
            foreach (SPFile file in folder.Files)
            {
                try
                {
                    if (file.CheckOutType == SPCheckOutType.None)
                    {
                        file.CheckOut();
                    }
                }
                catch (Exception ex)
                {
                    // Log per-file error and continue with next file
                    // Log: string.Format("Error on file {0}: {1}", file.Url, ex.Message);
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
    }
}