using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA
{
    public partial class ucCenterDigitalChannelsDga : UserControl
    {
        [WebBrowsable(true),
         Category("List Settings"),
         DefaultValue("CenterDigitalChannels"),
         Description("Name of the SharePoint list containing Center Digital Channels & Platforms.")]
        public string ListName { get; set; }

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public class ChannelItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string IconClass { get; set; }
            public string Url { get; set; }
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
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterDigitalChannelsDga.Page_Load", ex.Message);
            }
        }

        private void BindData()
        {
            litChannelsTitle.Text = IsArabic ? "المنصات والخدمات المرتبطة" : "Linked Platforms & Services";

            string targetListName = string.IsNullOrWhiteSpace(ListName) ? "CenterDigitalChannels" : ListName.Trim();

            var channelItems = new List<ChannelItem>();
            try
            {
                SPWeb web = SPContext.Current.Web;
                CenterProvisioner.EnsureDigitalChannelsList(web, targetListName);

                Guid siteId = web.Site.ID;
                Guid webId = web.ID;

                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (var site = new SPSite(siteId))
                    using (var elevatedWeb = site.OpenWeb(webId))
                    {
                        SPList channelList = elevatedWeb.Lists.TryGetList(targetListName);
                        if (channelList != null && channelList.ItemCount > 0)
                        {
                            SPQuery query = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True'/></OrderBy>" };
                            foreach (SPListItem item in channelList.GetItems(query))
                            {
                                string title = Convert.ToString(IsArabic ? item["Title"] : (item["Title_EN"] ?? item["Title"]));
                                string desc = Convert.ToString(IsArabic ? item["Description"] : (item["Description_EN"] ?? item["Description"]));
                                string icon = Convert.ToString(item["IconClass"] ?? "hgi-award-01");
                                string url = "";
                                if (item["URL"] != null)
                                {
                                    var uv = new SPFieldUrlValue(Convert.ToString(item["URL"]));
                                    url = uv.Url;
                                }
                                if (!string.IsNullOrEmpty(title))
                                    channelItems.Add(new ChannelItem { Title = title, Description = desc, IconClass = icon, Url = url });
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current?.Request?.Url?.ToString() ?? "", "ucCenterDigitalChannelsDga.BindData", ex.Message);
            }

            if (channelItems.Count == 0)
            {
                if (IsArabic)
                {
                    channelItems.Add(new ChannelItem
                    {
                        Title = "منصة سموق",
                        Description = "منصة قياس المهارات والقيم المهنية وتوجيه الطالبة إلى البرامج والأنشطة الملائمة لاحتياجاتها.",
                        IconClass = "hgi-award-01",
                        Url = "https://smoc.pnu.edu.sa/login"
                    });
                    channelItems.Add(new ChannelItem
                    {
                        Title = "بوابة الخريجات",
                        Description = "بوابة التواصل مع خريجات الجامعة واستعراض الفرص الوظيفية والخدمات وقصص النجاح.",
                        IconClass = "hgi-graduation-scroll",
                        Url = "https://graduates.pnu.edu.sa/ar/Pages/default.aspx"
                    });
                    channelItems.Add(new ChannelItem
                    {
                        Title = "خدمة سجل سموق المهاري",
                        Description = "الخدمة الإلكترونية لتوثيق المهارات والدورات والورش التي اكتسبتها الطالبة خلال حياتها الجامعية.",
                        IconClass = "hgi-license",
                        Url = "https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=205"
                    });
                }
                else
                {
                    channelItems.Add(new ChannelItem
                    {
                        Title = "Smoc Platform",
                        Description = "Platform for measuring skills and values and directing students to matching development programs.",
                        IconClass = "hgi-award-01",
                        Url = "https://smoc.pnu.edu.sa/login"
                    });
                    channelItems.Add(new ChannelItem
                    {
                        Title = "Graduates Portal",
                        Description = "Portal connecting with university alumni and showcasing opportunities and success stories.",
                        IconClass = "hgi-graduation-scroll",
                        Url = "https://graduates.pnu.edu.sa/ar/Pages/default.aspx"
                    });
                    channelItems.Add(new ChannelItem
                    {
                        Title = "Smoc Skills Record Service",
                        Description = "E-service for documenting skills and workshops acquired during university life.",
                        IconClass = "hgi-license",
                        Url = "https://pnu.edu.sa/ar/Pages/service-details.aspx?eti=205"
                    });
                }
            }

            rptChannels.DataSource = channelItems;
            rptChannels.DataBind();
        }
    }
}
