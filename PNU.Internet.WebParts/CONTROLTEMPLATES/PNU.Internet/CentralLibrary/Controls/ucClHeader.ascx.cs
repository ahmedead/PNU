using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClHeader : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Header; }
        }

        public ClHeaderModel Model { get; private set; }

        public ucClHeader()
        {
            Model = new ClHeaderModel();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = ClTargetWeb.Open(site))
                {
                    if (web != null)
                    {
                        List<SPListItem> raw = ClHelper.GetItems(web, EffectiveListName);
                        if (raw.Count > 0)
                        {
                            SPListItem item = raw[0];
                            Model.Title = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Title"), ClHelper.SafeString(item, "Title_EN")));
                            Model.Description = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Description"), ClHelper.SafeString(item, "Description_EN")));
                            Model.Button1Text = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Button1Text"), ClHelper.SafeString(item, "Button1Text_EN")));
                            Model.Button1Url = ClHelper.Enc(ClHelper.SafeUrl(item, "Button1Url"));
                            Model.Button2Text = ClHelper.Enc(ClHelper.Pick(ClHelper.SafeString(item, "Button2Text"), ClHelper.SafeString(item, "Button2Text_EN")));
                            Model.Button2Url = ClHelper.Enc(ClHelper.SafeUrl(item, "Button2Url"));

                            string rawBc = ClHelper.Pick(ClHelper.SafeString(item, "BreadcrumbText"), ClHelper.SafeString(item, "BreadcrumbText_EN"));
                            Model.BreadcrumbItems = ParseBreadcrumb(rawBc);
                        }
                    }
                }

                if (rptBreadcrumb != null)
                {
                    rptBreadcrumb.DataSource = Model.BreadcrumbItems;
                    rptBreadcrumb.DataBind();
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClHeader.Page_Load", ex);
            }
        }

        private List<ClBreadcrumbItem> ParseBreadcrumb(string raw)
        {
            var list = new List<ClBreadcrumbItem>();
            if (string.IsNullOrEmpty(raw)) return list;

            string[] parts = raw.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                string[] pair = parts[i].Split('|');
                string title = pair.Length > 0 ? pair[0].Trim() : string.Empty;
                string url = pair.Length > 1 ? pair[1].Trim() : string.Empty;

                if (!string.IsNullOrEmpty(title))
                {
                    list.Add(new ClBreadcrumbItem
                    {
                        Title = ClHelper.Enc(title),
                        Url = ClHelper.Enc(url),
                        IsLast = (i == parts.Length - 1)
                    });
                }
            }

            return list;
        }
    }
}
