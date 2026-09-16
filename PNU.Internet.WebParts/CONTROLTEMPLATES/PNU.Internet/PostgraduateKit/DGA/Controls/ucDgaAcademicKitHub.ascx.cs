using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls
{
    public partial class ucDgaAcademicKitHub : DgaPgSectionBase
    {
        public DgaPgHeaderModel HeaderModel { get; private set; } = new DgaPgHeaderModel();
        public DgaPgIntroModel IntroModel { get; private set; } = new DgaPgIntroModel();
        public List<DgaPgHubCardModel> HubCards { get; private set; } = new List<DgaPgHubCardModel>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Phase = DgaPgPhases.Hub;
            ExecuteSafely("LoadData", LoadData);
        }

        private void LoadData()
        {
            if (SPContext.Current == null) return;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            using (SPWeb web = OpenTargetWeb(site))
            {
                if (web == null) return;

                // 1. Header
                SPList headerList = GetList(web, DgaPgListNames.Header);
                if (headerList != null)
                {
                    SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Hub</Value></Eq></Where>" };
                    SPListItemCollection items = headerList.GetItems(q);
                    if (items.Count > 0)
                    {
                        var item = items[0];
                        HeaderModel.Title = DgaPgHelper.PickField(item, "Title");
                        HeaderModel.Description = DgaPgHelper.PickField(item, "Description");
                    }
                }
                if (string.IsNullOrEmpty(HeaderModel.Title))
                {
                    HeaderModel.Title = DgaPgHelper.Pick("الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا", "Interactive Academic Kit for Postgraduate Students");
                    HeaderModel.Description = DgaPgHelper.Pick("مرجع رقمي موحّد يواكب طالبة الدراسات العليا من القبول حتى منح الدرجة العلمية.", "A unified digital reference accompanying postgraduate students from admission to graduation.");
                }

                // 2. Intro
                SPList introList = GetList(web, DgaPgListNames.Intro);
                if (introList != null)
                {
                    SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Hub</Value></Eq></Where>" };
                    SPListItemCollection items = introList.GetItems(q);
                    if (items.Count > 0)
                    {
                        var item = items[0];
                        IntroModel.Title = DgaPgHelper.PickField(item, "Title");
                        string rawP = DgaPgHelper.PickField(item, "Paragraphs");
                        IntroModel.Paragraphs = DgaPgHelper.SplitLines(rawP);
                        string rawB = DgaPgHelper.PickField(item, "Badges");
                        IntroModel.Badges = DgaPgHelper.SplitLines(rawB);
                    }
                }
                if (string.IsNullOrEmpty(IntroModel.Title))
                {
                    IntroModel.Title = DgaPgHelper.Pick("نبذة تعريفية", "Overview");
                }

                // 3. Hub Cards
                SPList hubList = GetList(web, DgaPgListNames.Hub);
                if (hubList != null)
                {
                    SPQuery q = new SPQuery { Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                    SPListItemCollection items = hubList.GetItems(q);
                    foreach (SPListItem item in items)
                    {
                        HubCards.Add(new DgaPgHubCardModel
                        {
                            Phase = DgaPgHelper.SafeString(item, "Phase"),
                            Title = DgaPgHelper.PickField(item, "Title"),
                            Description = DgaPgHelper.PickField(item, "Description"),
                            BadgeText = DgaPgHelper.PickField(item, "BadgeText"),
                            ButtonText = DgaPgHelper.PickField(item, "ButtonText"),
                            ButtonUrl = DgaPgHelper.SafeString(item, "ButtonUrl"),
                            IconClass = DgaPgHelper.SafeString(item, "IconClass")
                        });
                    }
                }
            }
        }

        protected string GetResource(string key, string ar, string en)
        {
            return DgaPgHelper.Pick(ar, en);
        }

        protected string ResolveHomeUrl()
        {
            return IsArabic ? "/ar/Pages/default.aspx" : "/en/Pages/default.aspx";
        }

        protected string ResolveStationUrl(string url, string enUrl = null)
        {
            string target = (IsArabic || string.IsNullOrEmpty(enUrl)) ? url : enUrl;
            if (string.IsNullOrEmpty(target)) return "#";
            if (!IsArabic && target.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase))
            {
                target = "/en/" + target.Substring(4);
            }
            return DgaPgHelper.ResolveUrl(target);
        }
    }
}
