using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class ucAboutPNU : UserControl
    {
        private bool IsArabic
        {
            get { return AboutPnuHelper.IsArabic; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AboutPnuListProvisioner.EnsureAllListsExist();

                if (!Page.IsPostBack)
                {
                    LoadAboutData();
                }
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPNU.Page_Load", ex);
            }
        }

        private void LoadAboutData()
        {
            try
            {
                if (SPContext.Current == null)
                {
                    RenderFallbackData();
                    return;
                }

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = AboutPnuTargetWeb.Open(site))
                {
                    if (web == null || !web.Exists)
                    {
                        RenderFallbackData();
                        return;
                    }

                    LoadOverview(web);
                    LoadMilestones(web);
                    LoadPillars(web);
                }
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPNU.LoadAboutData", ex);
                RenderFallbackData();
            }
        }

        private void LoadOverview(SPWeb web)
        {
            try
            {
                List<SPListItem> items = AboutPnuHelper.GetItems(web, AboutPnuListNames.Overview);
                if (items == null || items.Count == 0)
                {
                    RenderDefaultOverview();
                    return;
                }

                // First overview item is for History
                SPListItem histItem = items.FirstOrDefault(x =>
                {
                    string t = AboutPnuHelper.SafeString(x, "Title");
                    return t.Contains("تاريخ") || t.Equals("History", StringComparison.OrdinalIgnoreCase) || AboutPnuHelper.SafeInt(x, "ItemOrder") == 1;
                }) ?? items[0];

                if (histItem != null && AboutPnuHelper.SafeBool(histItem, "Visibility"))
                {
                    string hTitle = IsArabic
                        ? AboutPnuHelper.SafeString(histItem, "Title")
                        : AboutPnuHelper.SafeString(histItem, "Title_EN");
                    if (string.IsNullOrEmpty(hTitle))
                        hTitle = IsArabic ? "تاريخ الجامعة" : "University History";
                    litHistoryTitle.Text = AboutPnuHelper.Enc(hTitle);

                    string hDesc = IsArabic
                        ? AboutPnuHelper.SafeString(histItem, "Description")
                        : AboutPnuHelper.SafeString(histItem, "Description_EN");
                    litHistoryDescription.Text = FormatParagraphs(hDesc);

                    string imgUrl = AboutPnuHelper.SafeUrl(histItem, "ImageUrl");
                    if (string.IsNullOrEmpty(imgUrl))
                    {
                        imgUrl = "/style%20library/dga/public/images/hero/hero-library-lg.avif";
                    }

                    imgFacility.ImageUrl = imgUrl;

                    string alt = IsArabic
                        ? AboutPnuHelper.SafeString(histItem, "ImageAlt")
                        : AboutPnuHelper.SafeString(histItem, "ImageAlt_EN");
                    imgFacility.AlternateText = alt;

                    string caption = IsArabic
                        ? AboutPnuHelper.SafeString(histItem, "ImageCaption")
                        : AboutPnuHelper.SafeString(histItem, "ImageCaption_EN");
                    litImageCaption.Text = AboutPnuHelper.Enc(caption);
                    pnlFacility.Visible = true;
                }
                else
                {
                    RenderDefaultOverview();
                }

                // Second overview item is for Pillars
                SPListItem pilItem = items.FirstOrDefault(x =>
                {
                    string t = AboutPnuHelper.SafeString(x, "Title");
                    return t.Contains("مرتكزات") || t.Equals("Pillars", StringComparison.OrdinalIgnoreCase) || AboutPnuHelper.SafeInt(x, "ItemOrder") == 2;
                });

                if (pilItem != null)
                {
                    string pTitle = IsArabic
                        ? AboutPnuHelper.SafeString(pilItem, "Title")
                        : AboutPnuHelper.SafeString(pilItem, "Title_EN");
                    if (string.IsNullOrEmpty(pTitle))
                        pTitle = IsArabic ? "مرتكزات الجامعة" : "University Pillars";
                    litPillarsTitle.Text = AboutPnuHelper.Enc(pTitle);

                    string pSub = IsArabic
                        ? AboutPnuHelper.SafeString(pilItem, "Subtitle")
                        : AboutPnuHelper.SafeString(pilItem, "Subtitle_EN");
                    litPillarsSubtitle.Text = AboutPnuHelper.Enc(pSub);
                }
                else
                {
                    litPillarsTitle.Text = AboutPnuHelper.Enc(IsArabic ? "مرتكزات الجامعة" : "University Pillars");
                    litPillarsSubtitle.Text = AboutPnuHelper.Enc(IsArabic
                        ? "بطاقات مختصرة تلخص المحاور الأساسية التي تتكرر في الصفحات المرجعية المرتبطة بالجامعة."
                        : "Brief cards summarizing core pillars recurring across university reference pages.");
                }
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPNU.LoadOverview", ex);
                RenderDefaultOverview();
            }
        }

        private void LoadMilestones(SPWeb web)
        {
            try
            {
                List<SPListItem> items = AboutPnuHelper.GetItems(web, AboutPnuListNames.Milestones);
                List<AboutPnuMilestoneItem> list = new List<AboutPnuMilestoneItem>();

                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem item in items)
                    {
                        if (!AboutPnuHelper.SafeBool(item, "Visibility")) continue;

                        list.Add(new AboutPnuMilestoneItem
                        {
                            Id = item.ID,
                            TitleAr = AboutPnuHelper.SafeString(item, "Title"),
                            TitleEn = AboutPnuHelper.SafeString(item, "Title_EN"),
                            DescriptionAr = AboutPnuHelper.SafeString(item, "Description"),
                            DescriptionEn = AboutPnuHelper.SafeString(item, "Description_EN"),
                            PeriodAr = AboutPnuHelper.SafeString(item, "Period"),
                            PeriodEn = AboutPnuHelper.SafeString(item, "Period_EN"),
                            DateAttribute = AboutPnuHelper.SafeString(item, "DateAttribute"),
                            IconClass = AboutPnuHelper.SafeString(item, "IconClass"),
                            ItemOrder = AboutPnuHelper.SafeInt(item, "ItemOrder"),
                            Visibility = true
                        });
                    }
                }

                if (list.Count == 0)
                {
                    list = GetDefaultMilestones();
                }

                rptMilestones.DataSource = list;
                rptMilestones.DataBind();
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPNU.LoadMilestones", ex);
                rptMilestones.DataSource = GetDefaultMilestones();
                rptMilestones.DataBind();
            }
        }

        private void LoadPillars(SPWeb web)
        {
            try
            {
                List<SPListItem> items = AboutPnuHelper.GetItems(web, AboutPnuListNames.Pillars);
                List<AboutPnuPillarItem> list = new List<AboutPnuPillarItem>();

                if (items != null && items.Count > 0)
                {
                    foreach (SPListItem item in items)
                    {
                        if (!AboutPnuHelper.SafeBool(item, "Visibility")) continue;

                        list.Add(new AboutPnuPillarItem
                        {
                            Id = item.ID,
                            TitleAr = AboutPnuHelper.SafeString(item, "Title"),
                            TitleEn = AboutPnuHelper.SafeString(item, "Title_EN"),
                            DescriptionAr = AboutPnuHelper.SafeString(item, "Description"),
                            DescriptionEn = AboutPnuHelper.SafeString(item, "Description_EN"),
                            IconClass = AboutPnuHelper.SafeString(item, "IconClass"),
                            ItemOrder = AboutPnuHelper.SafeInt(item, "ItemOrder"),
                            Visibility = true
                        });
                    }
                }

                if (list.Count == 0)
                {
                    list = GetDefaultPillars();
                }

                rptPillars.DataSource = list;
                rptPillars.DataBind();
            }
            catch (Exception ex)
            {
                AboutPnuLog.Write("ucAboutPNU.LoadPillars", ex);
                rptPillars.DataSource = GetDefaultPillars();
                rptPillars.DataBind();
            }
        }

        protected void rptMilestones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            AboutPnuMilestoneItem item = e.Item.DataItem as AboutPnuMilestoneItem;
            if (item == null) return;

            HtmlGenericControl icon = e.Item.FindControl("iconEl") as HtmlGenericControl;
            Literal litTitle = e.Item.FindControl("litTitle") as Literal;
            Literal litDesc = e.Item.FindControl("litDesc") as Literal;
            HtmlGenericControl timeEl = e.Item.FindControl("timeEl") as HtmlGenericControl;
            Literal litPeriod = e.Item.FindControl("litPeriod") as Literal;

            if (icon != null)
            {
                string iconClass = string.IsNullOrEmpty(item.IconClass) ? "hgi hgi-stroke hgi-target-01" : item.IconClass;
                if (!iconClass.Contains("hgi")) iconClass = "hgi hgi-stroke " + iconClass;
                icon.Attributes["class"] = iconClass;
            }

            if (litTitle != null)
            {
                litTitle.Text = AboutPnuHelper.Enc(item.Title);
            }

            if (litDesc != null)
            {
                litDesc.Text = AboutPnuHelper.Enc(item.Description);
            }

            if (timeEl != null && !string.IsNullOrEmpty(item.DateAttribute))
            {
                timeEl.Attributes["datetime"] = item.DateAttribute;
            }

            if (litPeriod != null)
            {
                litPeriod.Text = AboutPnuHelper.Enc(item.Period);
            }
        }

        protected void rptPillars_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            AboutPnuPillarItem item = e.Item.DataItem as AboutPnuPillarItem;
            if (item == null) return;

            HtmlGenericControl icon = e.Item.FindControl("iconEl") as HtmlGenericControl;
            Literal litTitle = e.Item.FindControl("litTitle") as Literal;
            Literal litDesc = e.Item.FindControl("litDesc") as Literal;

            if (icon != null)
            {
                string iconClass = string.IsNullOrEmpty(item.IconClass) ? "hgi hgi-stroke hgi-target-01" : item.IconClass;
                if (!iconClass.Contains("hgi")) iconClass = "hgi hgi-stroke " + iconClass;
                icon.Attributes["class"] = iconClass;
            }

            if (litTitle != null)
            {
                litTitle.Text = AboutPnuHelper.Enc(item.Title);
            }

            if (litDesc != null)
            {
                litDesc.Text = AboutPnuHelper.Enc(item.Description);
            }
        }

        private string FormatParagraphs(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return string.Empty;

            // If the content already contains HTML tags (<p>, <br>), return as is or sanitized
            if (raw.Contains("<p>") || raw.Contains("<br") || raw.Contains("<div>"))
            {
                return raw;
            }

            // Otherwise wrap double newlines into <p> paragraphs
            string[] paragraphs = raw.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string p in paragraphs)
            {
                string clean = p.Trim().Replace("\r\n", "<br/>").Replace("\n", "<br/>");
                if (clean.Length > 0)
                {
                    sb.Append("<p class=\"mb-4\">").Append(clean).Append("</p>");
                }
            }
            return sb.ToString();
        }

        private void RenderDefaultOverview()
        {
            litHistoryTitle.Text = AboutPnuHelper.Enc(IsArabic ? "تاريخ الجامعة" : "University History");
            string defaultDesc = IsArabic
                ? "شهد تعليم المرأة في المملكة العربية السعودية اهتمامًا كبيرًا مكّنها من تحقيق إنجازات مميزة محليًا وعالميًا، حيث برزت نماذج نسائية رائدة في مختلف مجالات العلم والمعرفة.\n\nوتُعد جامعة الأميرة نورة بنت عبد الرحمن من أبرز ثمار هذا الاهتمام؛ إذ بدأت مسيرة تعليم المرأة مبكرًا بإنشاء أول كلية تربوية للبنات عام 1970م، ثم توسع التعليم ليشمل عشرات الكليات في مختلف مناطق المملكة. وفي عام 1427هـ صدر الأمر الملكي بإنشاء أول جامعة متكاملة للبنات في الرياض، وتم تفعيلها عام 1428هـ.\n\nوفي عام 1429هـ، وُضع حجر الأساس للمدينة الجامعية، ليُطلق عليها لاحقًا اسم \"جامعة الأميرة نورة بنت عبد الرحمن\"، تخليدًا لاسم شقيقة الملك عبد العزيز -رحمه الله-، لتصبح اليوم صرحًا علميًا رائدًا يعكس تمكين المرأة ودورها في التنمية."
                : "Women's education in Saudi Arabia has received tremendous attention, enabling women to achieve distinguished achievements locally and globally, bringing forth leading female models across fields of science and knowledge.\n\nPrincess Nourah bint Abdulrahman University stands as one of the prominent fruits of this attention. The journey began early with the establishment of the first women's educational college in 1970, expanding to dozens of colleges across the Kingdom. In 1427 AH, a royal decree was issued establishing the first comprehensive university for women in Riyadh, activated in 1428 AH.\n\nIn 1429 AH, the cornerstone was laid for the university campus, later named Princess Nourah bint Abdulrahman University, commemorating the sister of King Abdulaziz, to become a leading academic monument reflecting women's empowerment and role in development.";
            litHistoryDescription.Text = FormatParagraphs(defaultDesc);

            imgFacility.ImageUrl = "/style%20library/dga/public/images/hero/hero-library-lg.avif";
            imgFacility.AlternateText = IsArabic ? "واجهة من مرافق جامعة الأميرة نورة بنت عبد الرحمن" : "Princess Nourah University campus facilities facade";
            litImageCaption.Text = AboutPnuHelper.Enc(IsArabic
                ? "مشهد من مرافق الجامعة يعكس اتساع الحرم الجامعي والطابع المعماري للمباني الرئيسة."
                : "A scene of university facilities reflecting the vast campus and main architectural design.");
            pnlFacility.Visible = true;

            litPillarsTitle.Text = AboutPnuHelper.Enc(IsArabic ? "مرتكزات الجامعة" : "University Pillars");
            litPillarsSubtitle.Text = AboutPnuHelper.Enc(IsArabic
                ? "بطاقات مختصرة تلخص المحاور الأساسية التي تتكرر في الصفحات المرجعية المرتبطة بالجامعة."
                : "Brief cards summarizing core pillars recurring across university reference pages.");
        }

        private List<AboutPnuMilestoneItem> GetDefaultMilestones()
        {
            return new List<AboutPnuMilestoneItem>
            {
                new AboutPnuMilestoneItem
                {
                    Id = 1,
                    TitleAr = "البداية الأكاديمية",
                    TitleEn = "Academic Inception",
                    DescriptionAr = "إنشاء أول كلية تربوية للبنات إيذانًا بانطلاقة المسار الأكاديمي المنظم لتعليم المرأة.",
                    DescriptionEn = "Establishment of the first educational college for women, marking the launch of an organized academic path for women's education.",
                    PeriodAr = "1390 هـ / 1970 م",
                    PeriodEn = "1390 AH / 1970 AD",
                    DateAttribute = "1970",
                    IconClass = "hgi hgi-stroke hgi-target-01",
                    ItemOrder = 1,
                    Visibility = true
                },
                new AboutPnuMilestoneItem
                {
                    Id = 2,
                    TitleAr = "تأسيس الجامعة",
                    TitleEn = "University Establishment",
                    DescriptionAr = "صدور الأمر الملكي بإنشاء أول جامعة للبنات بالرياض تحت إشراف وزارة التعليم العالي.",
                    DescriptionEn = "Issuance of the Royal Decree establishing the first women's university in Riyadh under the Ministry of Higher Education.",
                    PeriodAr = "1427 هـ",
                    PeriodEn = "1427 AH",
                    DateAttribute = "2006",
                    IconClass = "hgi hgi-stroke hgi-hierarchy",
                    ItemOrder = 2,
                    Visibility = true
                },
                new AboutPnuMilestoneItem
                {
                    Id = 3,
                    TitleAr = "التفعيل والمدينة الجامعية",
                    TitleEn = "Activation & University City",
                    DescriptionAr = "تفعيل الجامعة، ثم وضع حجر الأساس للمدينة الجامعية واعتماد اسمها الحالي.",
                    DescriptionEn = "Activating the university, laying the foundation stone of the university city, and approving its current name.",
                    PeriodAr = "1428 - 1429 هـ",
                    PeriodEn = "1428 - 1429 AH",
                    DateAttribute = "2007/2008",
                    IconClass = "hgi hgi-stroke hgi-chart",
                    ItemOrder = 3,
                    Visibility = true
                }
            };
        }

        private List<AboutPnuPillarItem> GetDefaultPillars()
        {
            return new List<AboutPnuPillarItem>
            {
                new AboutPnuPillarItem
                {
                    Id = 1,
                    TitleAr = "الرؤية",
                    TitleEn = "Vision",
                    DescriptionAr = "هوية أكاديمية ومعرفية تقود إلى أثر مؤسسي ومجتمعي أوسع.",
                    DescriptionEn = "An academic and knowledge identity driving broader institutional and societal impact.",
                    IconClass = "hgi hgi-stroke hgi-target-01",
                    ItemOrder = 1,
                    Visibility = true
                },
                new AboutPnuPillarItem
                {
                    Id = 2,
                    TitleAr = "الرسالة",
                    TitleEn = "Mission",
                    DescriptionAr = "تجربة جامعية موثوقة تدعم التعليم والبحث والخدمة المجتمعية.",
                    DescriptionEn = "A trusted university experience supporting education, research, and community service.",
                    IconClass = "hgi hgi-stroke hgi-globe",
                    ItemOrder = 2,
                    Visibility = true
                },
                new AboutPnuPillarItem
                {
                    Id = 3,
                    TitleAr = "القيم",
                    TitleEn = "Values",
                    DescriptionAr = "التميز والاعتزاز بالهوية والمسؤولية والتعاون والشفافية.",
                    DescriptionEn = "Excellence, pride in identity, responsibility, collaboration, and transparency.",
                    IconClass = "hgi hgi-stroke hgi-star",
                    ItemOrder = 3,
                    Visibility = true
                }
            };
        }

        private void RenderFallbackData()
        {
            RenderDefaultOverview();
            rptMilestones.DataSource = GetDefaultMilestones();
            rptMilestones.DataBind();
            rptPillars.DataSource = GetDefaultPillars();
            rptPillars.DataBind();
        }
    }
}
