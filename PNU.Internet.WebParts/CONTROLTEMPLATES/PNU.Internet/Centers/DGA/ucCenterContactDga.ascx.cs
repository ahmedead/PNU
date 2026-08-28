using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterContactDga : UserControl
    {
        public const string LIST_CONTACT_INFO = "CenterContactInfo";
        public const string LIST_CONTACT_DIRECTORY = "CenterContactDirectory";

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class DirectoryItem
        {
            public string EntityName { get; set; }
            public string Phone { get; set; }
            public string EmailHtml { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindData();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterContactDga.Page_Load", ex.Message);
            }
        }

        private void BindData()
        {
            litMainHeading.Text = IsArabic ? "تواصل مع المركز" : "Contact the Center";
            litInfoTitle.Text = IsArabic ? "بيانات التواصل" : "Contact Details";
            litEmailLabel.Text = IsArabic ? "البريد الإلكتروني" : "Email";
            litPhoneLabel.Text = IsArabic ? "الهاتف" : "Phone";
            litLocationLabel.Text = IsArabic ? "الموقع" : "Location";

            litDirectoryTitle.Text = IsArabic ? "دليل التواصل" : "Contact Directory";
            litThEntity.Text = IsArabic ? "الجهة" : "Entity";
            litThPhone.Text = IsArabic ? "الهاتف" : "Phone";
            litThEmail.Text = IsArabic ? "البريد الإلكتروني" : "Email";

            litUnifiedTitle.Text = IsArabic ? "أو عن طريق نموذج التواصل الموحد" : "Or via the Unified Contact Form";
            litFormCardTitle.Text = IsArabic ? "نموذج التواصل الموحد" : "Unified Contact Form";
            litFormCardDesc.Text = IsArabic ? "أرسل استفسارك أو مقترحك إلى المركز من خلال نموذج التواصل." : "Send your inquiry or suggestion to the Center through the contact form.";

            SPWeb web = SPContext.Current.Web;
            CenterProvisioner.EnsureContactLists(web);

            // 1. Load Info Cards
            Dictionary<string, string> info = LoadContactInfo();
            string email = Get(info, "Email", "cdc@pnu.edu.sa");
            string phone = Get(info, "Phone", "01182/37141");
            string location = Get(info, "Location", IsArabic ? "محطة (A4) – مبنى 190 – المركز الترفيهي – الدور الأرضي – مكتب رقم (0.404)." : "Station (A4) – Building 190 – Ground Floor – Office (0.404).");

            litEmailValue.Text = string.Format("<a href=\"mailto:{0}\" dir=\"ltr\">{0}</a>", email);
            litPhoneValue.Text = string.Format("<a href=\"tel:+966{0}\" dir=\"ltr\">{1}</a>", phone.Replace("/", "").Replace("-", "").TrimStart('0'), phone);
            litLocationValue.Text = location;

            // 2. Load Directory Table
            var dirList = LoadDirectory();
            rptDirectory.DataSource = dirList;
            rptDirectory.DataBind();
        }

        private Dictionary<string, string> LoadContactInfo()
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPList list = web.Lists.TryGetList(LIST_CONTACT_INFO);
                if (list != null && list.ItemCount > 0)
                {
                    foreach (SPListItem item in list.Items)
                    {
                        string key = Convert.ToString(item["Title"] ?? item["Key"] ?? "").Trim();
                        string val = Convert.ToString(IsArabic ? (item["Value"] ?? item["Body"]) : (item["Value_EN"] ?? item["Body_EN"] ?? item["Value"] ?? item["Body"]));
                        if (!string.IsNullOrEmpty(key))
                            dict[key] = val;
                    }
                }
            }
            catch { }
            return dict;
        }

        private List<DirectoryItem> LoadDirectory()
        {
            var list = new List<DirectoryItem>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPList spList = web.Lists.TryGetList(LIST_CONTACT_DIRECTORY);
                if (spList != null && spList.ItemCount > 0)
                {
                    SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                    foreach (SPListItem item in spList.GetItems(query))
                    {
                        string entity = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                        string phone = Convert.ToString(item["Phone"] ?? "");
                        string emailRaw = Convert.ToString(item["Email"] ?? "");

                        StringBuilder sbEmail = new StringBuilder();
                        if (!string.IsNullOrEmpty(emailRaw))
                        {
                            string[] emails = emailRaw.Split(new[] { ';', ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < emails.Length; i++)
                            {
                                string em = emails[i].Trim();
                                if (!string.IsNullOrEmpty(em))
                                {
                                    if (i > 0) sbEmail.Append("<br />");
                                    sbEmail.AppendFormat("<a href=\"mailto:{0}\" dir=\"ltr\">{0}</a>", em);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(entity))
                        {
                            list.Add(new DirectoryItem { EntityName = entity, Phone = phone, EmailHtml = sbEmail.ToString() });
                        }
                    }
                }
            }
            catch { }

            if (list.Count == 0)
            {
                if (IsArabic)
                {
                    list.Add(new DirectoryItem
                    {
                        EntityName = "إدارة برنامج سموق",
                        Phone = "01182/42635",
                        EmailHtml = "<a href=\"mailto:dsa-doff-dao@pnu.edu.sa\" dir=\"ltr\">dsa-doff-dao@pnu.edu.sa</a><br /><a href=\"mailto:cdc-td-sm@pnu.edu.sa\" dir=\"ltr\">cdc-td-sm@pnu.edu.sa</a>"
                    });
                    list.Add(new DirectoryItem
                    {
                        EntityName = "إدارة الإرشاد المهني",
                        Phone = "01182/23148",
                        EmailHtml = "<a href=\"mailto:cdc-ccu@pnu.edu.sa\" dir=\"ltr\">cdc-ccu@pnu.edu.sa</a>"
                    });
                    list.Add(new DirectoryItem
                    {
                        EntityName = "إدارة الخريجات",
                        Phone = "01182/44254",
                        EmailHtml = "<a href=\"mailto:asss-ug@pnu.edu.sa\" dir=\"ltr\">asss-ug@pnu.edu.sa</a>"
                    });
                    list.Add(new DirectoryItem
                    {
                        EntityName = "سجل سموق المهاري",
                        Phone = "01182/38845",
                        EmailHtml = "<a href=\"mailto:cdc@pnu.edu.sa\" dir=\"ltr\">cdc@pnu.edu.sa</a>"
                    });
                }
                else
                {
                    list.Add(new DirectoryItem
                    {
                        EntityName = "Smoc Program Administration",
                        Phone = "01182/42635",
                        EmailHtml = "<a href=\"mailto:cdc-td-sm@pnu.edu.sa\" dir=\"ltr\">cdc-td-sm@pnu.edu.sa</a>"
                    });
                    list.Add(new DirectoryItem
                    {
                        EntityName = "Career Guidance Administration",
                        Phone = "01182/23148",
                        EmailHtml = "<a href=\"mailto:cdc-ccu@pnu.edu.sa\" dir=\"ltr\">cdc-ccu@pnu.edu.sa</a>"
                    });
                    list.Add(new DirectoryItem
                    {
                        EntityName = "Graduates Administration",
                        Phone = "01182/44254",
                        EmailHtml = "<a href=\"mailto:asss-ug@pnu.edu.sa\" dir=\"ltr\">asss-ug@pnu.edu.sa</a>"
                    });
                }
            }

            return list;
        }

        private static string Get(Dictionary<string, string> dict, string key, string fallback = "")
        {
            if (dict != null && dict.TryGetValue(key, out string val) && !string.IsNullOrWhiteSpace(val))
                return val;
            return fallback;
        }
    }
}
