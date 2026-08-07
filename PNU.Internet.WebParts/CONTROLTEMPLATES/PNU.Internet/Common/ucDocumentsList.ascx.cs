using Microsoft.SharePoint;
using Microsoft.SharePoint.JSGrid;
using Microsoft.SharePoint.Publishing.Fields;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common
{
    public partial class ucDocumentsList : UserControl
    {
        public string ListName { get; set; }
        public string Category { get; set; }
        
        public string WebPartTitle { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ListName == null || ListName == "")
                    ListName = "DocumentsList";
                if (Category == null || Category == "")
                    Category = "";
                BindFolders();
            }
        }
        private void BindFolders()
        {
            SPWeb web = SPContext.Current.Web;

            // اسم الليست بالضبط كما هو في SharePoint
            SPList list = web.Lists[ListName];

            // ترتيب حسب Category ثم ItemOrder
            SPQuery query = new SPQuery();
            if(Category == "")
                query.Query =
                    @"<Where>
                         <Eq>
                            <FieldRef Name='Visibility' />
                            <Value Type='Boolean'>1</Value>
                         </Eq>

                   </Where>
                <OrderBy>
                    <FieldRef Name='Category' />
                    <FieldRef Name='ItemOrder' />
                  </OrderBy>";
            else

            query.Query = $@"<Where>
                              <And>
                                 <Contains>
                                    <FieldRef Name='Category' />
                                    <Value Type='Lookup'>{Category}</Value>
                                 </Contains>
                                 <Eq>
                                    <FieldRef Name='Visibility' />
                                    <Value Type='Boolean'>1</Value>
                                 </Eq>
                              </And>
                           </Where>
                        <OrderBy>
                    <FieldRef Name='Category' />
                    <FieldRef Name='ItemOrder' />
                  </OrderBy>";
            // لو ItemOrder الداخلي اسمه مختلف (Internal Name) عدّله هنا

            SPListItemCollection items = list.GetItems(query);

            // group by Category
            var groups = new Dictionary<string, List<SPListItem>>();

            foreach (SPListItem item in items)
            {
                string category = item["Category"] != null
                    ? item["Category"].ToString()
                    : "غير مصنّف";

                if (!groups.ContainsKey(category))
                    groups[category] = new List<SPListItem>();

                groups[category].Add(item);
            }

            var folders = new List<FolderViewModel>();

            foreach (var kvp in groups)
            {
                string categoryName = kvp.Key;

                SPFieldLookupValue cat = new SPFieldLookupValue(categoryName);
                int catId = cat.LookupId;
                string catName = cat.LookupValue;   // ده اللي هيظهر في الهيدر كـ "الأدلة الإرشادية" مثلاً
                var folderVm = new FolderViewModel
                {
                    FolderName = categoryName,


                    CategoryName = catName,
                    Files = new List<FileViewModel>()
                };

                foreach (SPListItem item in kvp.Value)
                {
                    string title = item["Title"] != null ? item["Title"].ToString() : "";

                    // --- Attachment as download file ---
                    string downloadUrl = "";
                    string sizeText = "";

                    if (item.Attachments.Count > 0)
                    {
                        // نفترض أول مرفق
                        string attachmentName = item.Attachments[0];
                        string attachmentUrl = item.Attachments.UrlPrefix + attachmentName;

                        SPFile file = web.GetFile(attachmentUrl);
                        downloadUrl = file.ServerRelativeUrl;

                        double sizeInMB = Math.Round((double)file.Length / 1024d / 1024d, 1);
                        sizeText = sizeInMB.ToString("0.0") + " MB";
                    }

                    // --- Image column (صورة الائتلاف) ---
                    string imageUrl = "https://i.ibb.co/Q3cLvXsT/ai-newsletter-ar.png"; // default

                    // انتبه: internal name للعمود العربي غالبًا يكون مختلف
                    // من List Settings > اسم العمود > شوف internal name في ال URL
                    string coalitionFieldInternalName = "PublishingRollupImage"; // عدّله للاسم الصحيح

                    if (item[coalitionFieldInternalName] != null)
                    {
                        // لو العمود من نوع Hyperlink or Picture:
                        SPFieldUrlValue urlVal = new SPFieldUrlValue(item[coalitionFieldInternalName].ToString());

                        ImageFieldValue PublishingRollupImage = (ImageFieldValue)item[coalitionFieldInternalName];
                        imageUrl = PublishingRollupImage.ImageUrl;
                    }

                    folderVm.Files.Add(new FileViewModel
                    {
                        FileName = title,
                        DownloadUrl = downloadUrl,
                        SizeMB = sizeText,
                        ImageUrl = imageUrl
                    });
                }

                folders.Add(folderVm);
            }

            rptFolders.DataSource = folders;
            rptFolders.DataBind();
        }

        protected void rptFolders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var folderVm = (FolderViewModel)e.Item.DataItem;
                var rptFiles = (Repeater)e.Item.FindControl("rptFiles");

                if (rptFiles != null)
                {
                    rptFiles.DataSource = folderVm.Files;
                    rptFiles.DataBind();
                }
            }
        }

    }


    public class FileViewModel
    {
        public string FileName { get; set; }
        public string DownloadUrl { get; set; }
        public string SizeMB { get; set; }
        public string ImageUrl { get; set; }
    }

    public class FolderViewModel
    {
        public string FolderName { get; set; }
        public List<FileViewModel> Files { get; set; }

        public string CategoryName { get; set; }


    }

}
