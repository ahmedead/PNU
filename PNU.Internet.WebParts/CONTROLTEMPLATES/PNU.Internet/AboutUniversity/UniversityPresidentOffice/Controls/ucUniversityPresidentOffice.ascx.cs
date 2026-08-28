using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    public partial class ucUniversityPresidentOffice : UserControl
    {
        public string PrimarySectionIcon { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = UpoTargetWeb.Open(site))
                        {
                            if (web == null) return;

                            // Ensure lists exist
                            UpoListProvisioner.EnsureAllListsExist();

                            // Load sections
                            var sections = LoadSections(web);
                            UpoSection primarySection = sections != null && sections.Count > 0
                                ? sections.FirstOrDefault(s => s.ShowLeadCard) ?? sections[0]
                                : null;

                            if (primarySection != null)
                            {
                                PrimarySectionIcon = !string.IsNullOrEmpty(primarySection.IconClass)
                                    ? primarySection.IconClass
                                    : "hgi-quote-down";
                                litMessageTitle.Text = primarySection.Title;
                                rptMessageParagraphs.DataSource = primarySection.Paragraphs;
                                rptMessageParagraphs.DataBind();
                            }
                            else
                            {
                                PrimarySectionIcon = "hgi-quote-down";
                                litMessageTitle.Text = UpoHelper.Enc(
                                    UpoHelper.Pick("كلمة رئيسة الجامعة", "University President's Speech"));
                                rptMessageParagraphs.DataSource = GetDefaultParagraphs();
                                rptMessageParagraphs.DataBind();
                            }

                            // Load additional sections (if any extra sections exist beyond the primary)
                            var extraSections = sections != null && primarySection != null
                                ? sections.Where(s => s.Id != primarySection.Id).ToList()
                                : new List<UpoSection>();

                            if (extraSections.Count > 0)
                            {
                                rptAdditionalSections.DataSource = extraSections;
                                rptAdditionalSections.DataBind();
                            }

                            // Load signature
                            var signatureList = LoadSignature(web);
                            var sig = signatureList != null && signatureList.Count > 0 ? signatureList[0] : null;

                            if (sig != null)
                            {
                                litSignatureName.Text = sig.Title;
                                litSignatureTitle.Text = sig.Description;

                                //if (!string.IsNullOrEmpty(sig.ImageUrl))
                                //{
                                //    imgPresidentPortrait.ImageUrl = sig.ImageUrl;
                                //}
                                //imgPresidentPortrait.AlternateText = UpoHelper.Enc(
                                //    sig.Title + (string.IsNullOrEmpty(sig.Description) ? "" : "، " + sig.Description));
                            }
                            else
                            {
                                litSignatureName.Text = UpoHelper.Enc(
                                    UpoHelper.Pick("د نجلاء بنت عبدالله التويجري", "Dr. Najla bint Abdullah Al-Tuwaijri"));
                                litSignatureTitle.Text = UpoHelper.Enc(
                                    UpoHelper.Pick("رئيسة جامعة الأميرة نورة بنت عبدالرحمن", "President of Princess Nourah bint Abdulrahman University"));
                            }

                            // Load contacts
                            var contacts = LoadContacts(web);
                            if (contacts != null && contacts.Count > 0)
                            {
                                rptContacts.DataSource = contacts;
                                rptContacts.DataBind();
                            }

                            // Set headings
                            litContactHeading.Text = UpoHelper.Enc(
                                UpoHelper.Pick("تواصل مع مكتب رئيسة الجامعة", "Contact the President's Office"));
                            litChannelHeader.Text = UpoHelper.Enc(
                                UpoHelper.Pick("القناة", "Channel"));
                            litContactDataHeader.Text = UpoHelper.Enc(
                                UpoHelper.Pick("بيانات التواصل", "Contact details"));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                UpoLog.Write("ucUniversityPresidentOffice.LoadData", ex);
            }
        }

        private List<string> GetDefaultParagraphs()
        {
            if (UpoHelper.IsArabic)
            {
                return new List<string>
                {
                    "بسم الله الرحمن الرحيم، والصلاة والسلام على نبينا محمد، وعلى آله وصحبه أجمعين.",
                    "تواصل جامعة الأميرة نورة بنت عبدالرحمن مسيرتها العلمية والتنموية بوصفها صرحًا وطنيًا متفردًا في التعليم الجامعي، وشاهدًا على ما حققته المرأة السعودية من حضور مؤثر وإسهام فاعل في نهضة الوطن.",
                    "وقد رسخت الجامعة عبر مسيرتها مكانة تستند إلى إرث عريق، وإمكانات علمية وبحثية ومؤسسية، ومجتمع جامعي يجمعه الطموح والشعور بالمسؤولية، وننطلق من هذه المكانة نحو مرحلة تتطلب منا تعزيز مكتسباتنا، وتجديد ممارساتنا، والارتقاء بأثرنا؛ بما يرسخ حضور الجامعة مؤسسة وطنية رائدة، تنتج المعرفة، وتعمل بفاعلية في خدمة المجتمع، وتحقيق أولويات الوطن ومستهدفات رؤيته الطموحة.",
                    "وتولي الجامعة جودة التعليم ومخرجاته أولوية رئيسة، من خلال تطوير البرامج الأكاديمية، وتعزيز ارتباطها بالاحتياجات التنموية والمهنية، وتنمية المعارف والمهارات التي تمكّن طالباتنا من المنافسة والقيادة، وتأهيلهن للإسهام بفاعلية في تحقيق تطلعات الوطن.",
                    "ومع ما يشهده العالم من تحولات معرفية وتقنية واقتصادية متسارعة، نواصل تطوير التجربة التعليمية، وربط المعرفة بالتطبيق، وتوسيع فرص التعلم والممارسة؛ بما يعزز جاهزية خريجاتنا، ويدعم قدرتهن على مواصلة التعلم والتكيف وصناعة الفرص.",
                    "كما تولي الجامعة البحث والابتكار اهتمامًا محوريًا، انطلاقًا من مسؤوليتهما الوطنية تجاه الإنسان والمجتمع. ومن هنا، نوجّه قدراتنا البحثية نحو الأولويات الوطنية، ونعتز بما بنته الجامعة من شراكات فاعلة مع القطاعات الوطنية والمؤسسات العلمية العالمية، وما أتاحته من تكامل للخبرات وتبادل للمعرفة، ونتطلع إلى توسيع هذه الشراكات وتعميق أثرها؛ بما يسهم في تحويل الأفكار إلى حلول مبتكرة، ويعزز إسهام الجامعة في التنمية محليًا ودوليًا.",
                    "وتستند قدرة الجامعة على أداء رسالتها إلى بنية تحتية متكاملة، تضم مرافق تعليمية وبحثية وصحية وثقافية ورياضية، تدعمها منظومات تقنية وخدمات متقدمة، وتوظف الجامعة هذه الإمكانات في تطوير التجربة التعليمية والبحثية، وتعزيز جودة الحياة الجامعية، وتوفير بيئة محفزة تمكّن أفراد مجتمعها من التعلم والعمل والإبداع بكفاءة. ويظل الإنسان محور اهتمامنا وأساس رسالتنا؛ لذلك نحرص على بناء بيئة جامعية متكاملة تدعم صحة أفراد مجتمع الجامعة ورفاههم، وتعزز الانتماء والثقة والتكامل، وتتيح لكل فرد أن ينمي قدراته، ويؤدي دوره، ويسهم بفاعلية في تحقيق رسالة الجامعة، ضمن بيئة آمنة ومحفزة تقدر التنوع وتدعم الطموح.",
                    "إن مسؤوليتنا لا تقتصر على مواكبة المستقبل، بل تمتد إلى المشاركة في صناعته، وبجهود مجتمعنا الجامعي وشركائنا، ستواصل جامعة الأميرة نورة بنت عبدالرحمن بناء القدرات بالمعرفة، وتوسيع الفرص، وصناعة أثر يعبّر عن طموح الجامعة، ويواكب تطلعات الوطن.",
                    "نسأل الله أن يبارك مسيرة الجامعة، وأن يوفقنا لأداء رسالتنا وخدمة وطننا، ومواصلة الإسهام في نهضته، وأن يحفظ وطننا وقيادته، ويديم عليه التقدم والازدهار.",
                    "والله ولي التوفيق."
                };
            }
            return new List<string>
            {
                "In the name of Allah, the Most Gracious, the Most Merciful, and peace and blessings be upon our Prophet Muhammad, and upon his family and companions.",
                "Princess Nourah bint Abdulrahman University continues its scientific and developmental journey as a unique national landmark in higher education..."
            };
        }

        private List<UpoSection> LoadSections(SPWeb web)
        {
            var result = new List<UpoSection>();
            foreach (SPListItem item in UpoHelper.GetItems(web, UpoListNames.Sections))
            {
                result.Add(new UpoSection
                {
                    Id = item.ID,
                    Title = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Title"),
                        UpoHelper.SafeString(item, "Title_EN"))),
                    Description = UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Description"),
                        UpoHelper.SafeString(item, "Description_EN")),
                    LeadText = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "LeadText"),
                        UpoHelper.SafeString(item, "LeadText_EN"))),
                    IconClass = UpoHelper.Enc(UpoHelper.SafeString(item, "IconClass")),
                    ShowLeadCard = UpoHelper.SafeBool(item, "ShowLeadCard"),
                    ImageUrl = UpoHelper.SafeUrl(item, "ImageUrl"),
                    ItemOrder = UpoHelper.SafeInt(item, "ItemOrder")
                });
            }
            return result;
        }

        private List<UpoContact> LoadContacts(SPWeb web)
        {
            var result = new List<UpoContact>();
            foreach (SPListItem item in UpoHelper.GetItems(web, UpoListNames.Contacts))
            {
                result.Add(new UpoContact
                {
                    Id = item.ID,
                    Title = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Title"),
                        UpoHelper.SafeString(item, "Title_EN"))),
                    ContactValue = UpoHelper.SafeString(item, "ContactValue"),
                    ContactType = UpoHelper.SafeString(item, "ContactType"),
                    ItemOrder = UpoHelper.SafeInt(item, "ItemOrder")
                });
            }
            return result;
        }

        private List<UpoSignatureCard> LoadSignature(SPWeb web)
        {
            var result = new List<UpoSignatureCard>();
            foreach (SPListItem item in UpoHelper.GetItems(web, UpoListNames.Signature))
            {
                result.Add(new UpoSignatureCard
                {
                    Id = item.ID,
                    Title = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Title"),
                        UpoHelper.SafeString(item, "Title_EN"))),
                    Description = UpoHelper.Enc(UpoHelper.Pick(
                        UpoHelper.SafeString(item, "Description"),
                        UpoHelper.SafeString(item, "Description_EN"))),
                    ImageUrl = UpoHelper.SafeUrl(item, "ImageUrl"),
                    ItemOrder = UpoHelper.SafeInt(item, "ItemOrder")
                });
            }
            return result;
        }

        protected void rptContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var item = (UpoContact)e.Item.DataItem;
            var lnk = (HyperLink)e.Item.FindControl("lnkContact");
            if (lnk == null || item == null) return;

            string val = item.ContactValue ?? string.Empty;
            lnk.Text = UpoHelper.Enc(val);
            lnk.NavigateUrl = item.ContactHref;

            string cType = (item.ContactType ?? "").ToLower();
            if (cType == "phone")
            {
                // Phone numbers match prompt: href="tel:..." with dir="rtl" on parent td
            }
            else if (cType == "email")
            {
                if (val.IndexOf("tawasul", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    lnk.CssClass = "external-link";
                }
            }
            else if (cType == "link")
            {
                lnk.Target = "_blank";
                lnk.CssClass = "external-link";
                lnk.Attributes["rel"] = "noopener noreferrer";
                if (val.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    lnk.Text = UpoHelper.Enc(val.Substring(8).TrimEnd('/'));
                }
                else if (val.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    lnk.Text = UpoHelper.Enc(val.Substring(7).TrimEnd('/'));
                }
            }
        }
    }
}
