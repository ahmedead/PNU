using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{
    public partial class ucHelpSupport : UserControl
    {
        private const string ListName = "AboutHelpSupport";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnsureListAndSeedData();
                BindData();
            }
        }

        private void EnsureListAndSeedData()
        {
            try
            {
                if (SPContext.Current == null || SPContext.Current.Web == null) return;

                Guid siteId = SPContext.Current.Site.ID;
                Guid webId = SPContext.Current.Web.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb(webId))
                        {
                            bool prevUnsafe = web.AllowUnsafeUpdates;
                            web.AllowUnsafeUpdates = true;
                            try
                            {
                                SPList list = web.Lists.TryGetList(ListName);

                                if (list == null)
                                {
                                    Guid listId = web.Lists.Add(ListName, "Help and Support items list", SPListTemplateType.GenericList);
                                    list = web.Lists[listId];
                                    list.OnQuickLaunch = false;
                                    list.Update();
                                }

                                EnsureField(list, "TitleEn", SPFieldType.Text);
                                EnsureField(list, "Description", SPFieldType.Note);
                                EnsureField(list, "DescEn", SPFieldType.Note);
                                EnsureField(list, "Link", SPFieldType.URL);
                                EnsureField(list, "IconClass", SPFieldType.Text);
                                EnsureField(list, "ItemOrder", SPFieldType.Number);

                                EnsureDefaultViewFields(list, "Title", "TitleEn", "Description", "DescEn", "Link", "IconClass", "ItemOrder");

                                SeedData(list);
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = prevUnsafe;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHelpSupport - EnsureListAndSeedData", ex.Message);
            }
        }

        private void EnsureField(SPList list, string fieldName, SPFieldType fieldType)
        {
            if (!list.Fields.ContainsField(fieldName))
            {
                list.Fields.Add(fieldName, fieldType, false);
                list.Update();
            }
        }

        private void EnsureDefaultViewFields(SPList list, params string[] fieldNames)
        {
            if (list == null || fieldNames == null || fieldNames.Length == 0) return;
            try
            {
                SPView view = list.DefaultView;
                if (view == null) return;

                bool updated = false;
                foreach (string name in fieldNames)
                {
                    if (list.Fields.ContainsField(name) && !view.ViewFields.Exists(name))
                    {
                        view.ViewFields.Add(name);
                        updated = true;
                    }
                }

                if (updated)
                {
                    view.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucHelpSupport - EnsureDefaultViewFields", ex.Message);
            }
        }

        private void SeedData(SPList list)
        {
            var seedItems = new[]
            {
                new
                {
                    Title = "الأسئلة الشائعة",
                    TitleEn = "FAQs",
                    Description = "استعرض إجابات أكثر الأسئلة شيوعاً حول القبول والخدمات والأنظمة الجامعية.",
                    DescEn = "Browse answers to the most common questions regarding admission, services, and university systems.",
                    LinkUrl = "/ar/FAQs/Pages/default.aspx",
                    IconClass = "hgi-message-question",
                    ItemOrder = 1
                },
                new
                {
                    Title = "تواصل معنا",
                    TitleEn = "Contact Us",
                    Description = "تواصل مع الجامعة عبر القنوات الرسمية لرفع الاستفسارات والملاحظات وطلبات الدعم.",
                    DescEn = "Connect with the university through official channels to submit inquiries, feedback, and support requests.",
                    LinkUrl = "/ar/Pages/ContactUsForm.aspx",
                    IconClass = "hgi-customer-service-01",
                    ItemOrder = 2
                },
                new
                {
                    Title = "تيك كير لتقنية المعلومات",
                    TitleEn = "IT TechCare",
                    Description = "نظام مخصص لطلبات الدعم التقني ومتابعة البلاغات والخدمات المرتبطة بتقنية المعلومات.",
                    DescEn = "A dedicated system for technical support requests, tracking tickets, and IT-related services.",
                    LinkUrl = "#",
                    IconClass = "hgi-tools",
                    ItemOrder = 3
                }
            };

            foreach (var seed in seedItems)
            {
                bool exists = false;
                foreach (SPListItem existingItem in list.Items)
                {
                    string title = Convert.ToString(existingItem["Title"]);
                    string titleEn = list.Fields.ContainsField("TitleEn") ? Convert.ToString(existingItem["TitleEn"]) : "";
                    if (title == seed.Title || titleEn == seed.TitleEn)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    SPListItem item = list.Items.Add();
                    item["Title"] = seed.Title;
                    if (list.Fields.ContainsField("TitleEn")) item["TitleEn"] = seed.TitleEn;
                    if (list.Fields.ContainsField("Description")) item["Description"] = seed.Description;
                    if (list.Fields.ContainsField("DescEn")) item["DescEn"] = seed.DescEn;
                    if (list.Fields.ContainsField("Link"))
                    {
                        item["Link"] = new SPFieldUrlValue { Url = seed.LinkUrl, Description = seed.Title };
                    }
                    if (list.Fields.ContainsField("IconClass")) item["IconClass"] = seed.IconClass;
                    if (list.Fields.ContainsField("ItemOrder")) item["ItemOrder"] = seed.ItemOrder;
                    item.Update();
                }
            }
        }

        private void BindData()
        {
            try
            {
                List<HelpSupportItem> items = new List<HelpSupportItem>();
                SPList list = SPContext.Current.Web.Lists.TryGetList(ListName);

                if (list != null)
                {
                    SPQuery query = new SPQuery();
                    query.Query = @"<OrderBy>
                                      <FieldRef Name='ItemOrder' Ascending='True' />
                                    </OrderBy>";

                    SPListItemCollection coll = list.GetItems(query);
                    if (coll != null && coll.Count > 0)
                    {
                        foreach (SPListItem item in coll)
                        {
                            string linkStr = "#";
                            if (item.Fields.ContainsField("Link") && item["Link"] != null)
                            {
                                linkStr = new SPFieldUrlValue(item["Link"].ToString()).Url;
                            }

                            items.Add(new HelpSupportItem
                            {
                                ID = item.ID.ToString(),
                                Title = item["Title"] != null ? item["Title"].ToString() : string.Empty,
                                TitleEn = item.Fields.ContainsField("TitleEn") && item["TitleEn"] != null ? item["TitleEn"].ToString() : string.Empty,
                                Description = item.Fields.ContainsField("Description") && item["Description"] != null ? item["Description"].ToString() : string.Empty,
                                DescEn = item.Fields.ContainsField("DescEn") && item["DescEn"] != null ? item["DescEn"].ToString() : string.Empty,
                                Link = linkStr,
                                IconClass = item.Fields.ContainsField("IconClass") && item["IconClass"] != null ? item["IconClass"].ToString() : string.Empty,
                                ItemOrder = item.Fields.ContainsField("ItemOrder") && item["ItemOrder"] != null ? Convert.ToInt32(item["ItemOrder"]) : 0
                            });
                        }
                    }

                    rptHelpSupport.DataSource = items;
                    rptHelpSupport.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucHelpSupport - BindData", ex.Message);
            }
        }
    }

    public class HelpSupportItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public string DescEn { get; set; }
        public string Link { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }

        public string ArrowClass
        {
            get
            {
                return Convert.ToString(SPFactory.GetLocalizedTitle("hgi-arrow-left-02", "hgi-arrow-right-02"));
            }
        }
    }
}
