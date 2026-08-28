using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public partial class ucPcHeader : PcSectionBase
    {
        protected override string ListName { get { return PcListNames.Header; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindHeader();
            }
        }

        private void BindHeader()
        {
            try
            {
                PcListProvisioner.EnsureListExists(EffectiveListName);

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = PcTargetWeb.Open(site))
                {
                    if (web == null) return;

                    List<SPListItem> items = PcHelper.GetItems(web, EffectiveListName);
                    if (items.Count > 0)
                    {
                        SPListItem item = items[0];
                        string title = PcHelper.Pick(PcHelper.SafeString(item, "Title"), PcHelper.SafeString(item, "Title_EN"));
                        string desc = PcHelper.Pick(PcHelper.SafeString(item, "Description"), PcHelper.SafeString(item, "Description_EN"));
                        string p2 = PcHelper.Pick(PcHelper.SafeString(item, "Paragraph2"), PcHelper.SafeString(item, "Paragraph2_EN"));
                        string bc = PcHelper.Pick(PcHelper.SafeString(item, "BreadcrumbText"), PcHelper.SafeString(item, "BreadcrumbText_EN"));
                        string exportBtn = PcHelper.Pick(PcHelper.SafeString(item, "ExportButtonText"), PcHelper.SafeString(item, "ExportButtonText_EN"));

                        litTitle.Text = PcHelper.Enc(string.IsNullOrEmpty(title) ? "دليل الشهادات الاحترافية" : title);
                        litDesc.Text = PcHelper.Enc(desc);

                        if (!string.IsNullOrEmpty(p2))
                        {
                            pnlParagraph2.Visible = true;
                            litParagraph2.Text = PcHelper.Enc(p2);
                        }

                        litExportBtn.Text = PcHelper.Enc(string.IsNullOrEmpty(exportBtn) ? "تصدير القائمة" : exportBtn);

                        BindBreadcrumb(bc);
                    }
                    else
                    {
                        SetFallbackHeader();
                    }
                }
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcHeader.BindHeader", ex);
                SetFallbackHeader();
            }
        }

        private void SetFallbackHeader()
        {
            litTitle.Text = PcHelper.Enc("دليل الشهادات الاحترافية");
            litDesc.Text = PcHelper.Enc("يهدف هذا الدليل إلى تسليط الضوء على الشهادات الاحترافية المهمة في مختلف التخصصات لدعم رحلتهن الأكاديمية والمهنية.");
            litExportBtn.Text = PcHelper.Enc("تصدير القائمة");
            BindBreadcrumb("الرئيسية|/ar/Pages/default.aspx;دليل الشهادات الاحترافية|");
        }

        private void BindBreadcrumb(string raw)
        {
            var list = new List<PcBreadcrumbItem>();

            if (string.IsNullOrEmpty(raw))
            {
                raw = "الرئيسية|/ar/Pages/default.aspx;دليل الشهادات الاحترافية|";
            }

            string[] parts = raw.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                string p = parts[i].Trim();
                string[] pair = p.Split('|');
                string bTitle = pair[0].Trim();
                string bUrl = pair.Length > 1 ? pair[1].Trim() : string.Empty;

                list.Add(new PcBreadcrumbItem
                {
                    Title = PcHelper.Enc(bTitle),
                    Url = string.IsNullOrEmpty(bUrl) ? "#" : bUrl,
                    IsLast = (i == parts.Length - 1)
                });
            }

            rptBreadcrumb.DataSource = list;
            rptBreadcrumb.DataBind();
        }
    }
}
