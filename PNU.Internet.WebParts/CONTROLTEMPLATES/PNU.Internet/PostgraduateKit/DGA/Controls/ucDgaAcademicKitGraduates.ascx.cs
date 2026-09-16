using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls
{
    public partial class ucDgaAcademicKitGraduates : DgaPgSectionBase
    {
        public DgaPgHeaderModel HeaderModel { get; private set; } = new DgaPgHeaderModel();
        public DgaPgIntroModel IntroModel { get; private set; } = new DgaPgIntroModel();
        public List<DgaPgJourneyStepModel> JourneySteps { get; private set; } = new List<DgaPgJourneyStepModel>();
        public List<DgaPgResourceCardModel> GraduationRequirements { get; private set; } = new List<DgaPgResourceCardModel>();
        public List<DgaPgProcedureModel> Procedures { get; private set; } = new List<DgaPgProcedureModel>();
        public List<DgaPgFaqItemModel> FaqItems { get; private set; } = new List<DgaPgFaqItemModel>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Phase = DgaPgPhases.Graduates;
            ExecuteSafely("LoadData", LoadData);
        }

        private void LoadData()
        {
            if (SPContext.Current != null)
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                using (SPWeb web = OpenTargetWeb(site))
                {
                    if (web != null)
                    {
                        // 1. Sections dictionary
                        LoadSections(web);

                        // 2. Header
                        SPList headerList = GetList(web, DgaPgListNames.Header);
                        if (headerList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Graduates</Value></Eq></Where>" };
                            SPListItemCollection items = headerList.GetItems(q);
                            if (items.Count > 0)
                            {
                                var item = items[0];
                                HeaderModel.Title = DgaPgHelper.PickField(item, "Title");
                                HeaderModel.Description = DgaPgHelper.PickField(item, "Description");
                                HeaderModel.Breadcrumbs = ParseBreadcrumbs(DgaPgHelper.PickField(item, "BreadcrumbText"));
                            }
                        }

                        // 3. Intro
                        SPList introList = GetList(web, DgaPgListNames.Intro);
                        if (introList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Graduates</Value></Eq></Where>" };
                            SPListItemCollection items = introList.GetItems(q);
                            if (items.Count > 0)
                            {
                                var item = items[0];
                                IntroModel.Title = DgaPgHelper.PickField(item, "Title");
                                IntroModel.SubTitle = DgaPgHelper.PickField(item, "SubTitle");
                                IntroModel.LeadText = DgaPgHelper.PickField(item, "LeadText");
                                string rawP = DgaPgHelper.PickField(item, "Paragraphs");
                                IntroModel.Paragraphs = DgaPgHelper.SplitLines(rawP);
                                string rawB = DgaPgHelper.PickField(item, "Badges");
                                IntroModel.Badges = DgaPgHelper.SplitLines(rawB);
                                IntroModel.ReturnText = DgaPgHelper.PickField(item, "ReturnText");
                                IntroModel.ReturnUrl = DgaPgHelper.SafeString(item, "ReturnUrl");
                            }
                        }

                        // 4. Journey Steps
                        SPList journeyList = GetList(web, DgaPgListNames.Journey);
                        if (journeyList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Graduates</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            SPListItemCollection items = journeyList.GetItems(q);
                            foreach (SPListItem item in items)
                            {
                                JourneySteps.Add(new DgaPgJourneyStepModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-route-03"),
                                    StepNumber = DgaPgHelper.SafeInt(item, "StepNumber")
                                });
                            }
                        }

                        // 5. Graduation Requirements
                        SPList resList = GetList(web, DgaPgListNames.Resources);
                        if (resList != null)
                        {
                            SPQuery qReq = new SPQuery { Query = "<Where><Eq><FieldRef Name='GroupKey' /><Value Type='Choice'>GradRequirements</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            SPListItemCollection reqItems = resList.GetItems(qReq);
                            foreach (SPListItem item in reqItems)
                            {
                                GraduationRequirements.Add(new DgaPgResourceCardModel
                                {
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    Description = DgaPgHelper.PickField(item, "Description"),
                                    BadgeText = DgaPgHelper.PickField(item, "BadgeText"),
                                    IconClass = DgaPgHelper.SafeString(item, "IconClass", "hgi-check-list")
                                });
                            }
                        }

                        // 6. Graduation Procedures
                        SPList procList = GetList(web, DgaPgListNames.Procedures);
                        if (procList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Graduates</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            SPListItemCollection items = procList.GetItems(q);
                            foreach (SPListItem item in items)
                            {
                                var proc = new DgaPgProcedureModel
                                {
                                    Id = item.ID,
                                    Title = DgaPgHelper.PickField(item, "Title"),
                                    ShortDescription = DgaPgHelper.PickField(item, "ShortDescription"),
                                    Requirements = DgaPgHelper.SplitLines(DgaPgHelper.PickField(item, "Requirements")),
                                    Forms = DgaPgHelper.ParseLinks(DgaPgHelper.PickField(item, "Forms")),
                                    FlowText = DgaPgHelper.PickField(item, "FlowText"),
                                    DirectServiceUrl = DgaPgHelper.SafeString(item, "DirectServiceUrl"),
                                    IconUrl = DgaPgHelper.SafeString(item, "IconUrl"),
                                    BadgeText = DgaPgHelper.PickField(item, "BadgeText")
                                };

                                string rawArticles = DgaPgHelper.PickField(item, "ArticleText");
                                if (!string.IsNullOrEmpty(rawArticles))
                                {
                                    var lines = DgaPgHelper.SplitLines(rawArticles);
                                    foreach (var line in lines)
                                    {
                                        if (line.Contains("|"))
                                        {
                                            var parts = line.Split(new[] { '|' }, 2);
                                            proc.Articles.Add(new DgaPgArticleBlock { Heading = parts[0].Trim(), Body = parts[1].Trim() });
                                        }
                                        else
                                        {
                                            proc.Articles.Add(new DgaPgArticleBlock { Heading = DgaPgHelper.Pick("نص المادة", "Article Text"), Body = line });
                                        }
                                    }
                                }

                                Procedures.Add(proc);
                            }
                        }

                        // 7. FAQ
                        SPList faqList = GetList(web, DgaPgListNames.Faq);
                        if (faqList != null)
                        {
                            SPQuery q = new SPQuery { Query = "<Where><Eq><FieldRef Name='Phase' /><Value Type='Choice'>Graduates</Value></Eq></Where><OrderBy><FieldRef Name='ItemOrder' Ascending='True' /></OrderBy>" };
                            SPListItemCollection items = faqList.GetItems(q);
                            foreach (SPListItem item in items)
                            {
                                FaqItems.Add(new DgaPgFaqItemModel
                                {
                                    Id = item.ID,
                                    Question = DgaPgHelper.PickField(item, "Title"),
                                    Answer = DgaPgHelper.PickField(item, "Answer"),
                                    Category = DgaPgHelper.SafeString(item, "Category")
                                });
                            }
                        }
                    }
                }
            }

            EnsureDefaults();
        }

        private void EnsureDefaults()
        {
            if (string.IsNullOrEmpty(HeaderModel.Title))
            {
                HeaderModel.Title = GetResource("Graduates", "الخريجون", "Graduates");
                HeaderModel.Description = GetResource("GraduatesHeaderDesc", "رحلة التخرّج ومتطلباتها وإجراءات منح الدرجة وإخلاء الطرف.", "Graduation journey, requirements, degree conferral and clearance procedures.");
            }

            if (HeaderModel.Breadcrumbs.Count == 0)
            {
                HeaderModel.Breadcrumbs.Add(new DgaPgLinkItem { Title = GetResource("Home", "الرئيسية", "Home"), Url = ResolveSiteHomeUrl() });
                HeaderModel.Breadcrumbs.Add(new DgaPgLinkItem { Title = GetResource("KitTitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا", "Interactive Academic Kit for Postgraduate Students"), Url = ResolveKitHubUrl() });
                HeaderModel.Breadcrumbs.Add(new DgaPgLinkItem { Title = HeaderModel.Title, Url = string.Empty });
            }

            if (string.IsNullOrEmpty(IntroModel.Title))
            {
                IntroModel.Title = GetResource("Overview", "نبذة تعريفية", "Overview");
                IntroModel.LeadText = GetResource("GradIntroLead",
                    "وأنتِ على أعتاب التخرّج بعد رحلةٍ من الجدّ والعطاء، تأتي هذه المحطّة لتكون دليلكِ في إتمام مسيرتكِ باستحقاقٍ ووضوح؛ حرصاً من العمادة على أن تُكلَّل جهودكِ بإجراءاتٍ ميسّرةٍ ومعلومةٍ بيّنة. وهي تبيّن لكِ خطوات التخرّج كاملةً من اكتمال المناقشة حتى استلام الوثيقة، ومتطلّبات التخرّج، وإجراءات منح الدرجة العلمية وإخلاء الطرف بنماذجها وجهاتها المعتمدة؛ لتغادري الجامعة وقد أتممتِ مسيرتكِ على أكمل وجهٍ وأيسر سبيل.",
                    "As you approach graduation after a journey of dedication and achievement, this station serves as your guide to completing your academic journey with clarity and ease.");
                IntroModel.SubTitle = GetResource("KitSubtitle", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا", "Interactive Academic Kit for Postgraduate Students");
                IntroModel.Paragraphs = new List<string>
                {
                    GetResource("KitDesc", "رفيقٌ رقميٌّ موحّدٌ يواكب الطالبة من القبول حتى منح الدرجة العلمية — بالضغط على أي إجراءٍ تظهر شروطه ونصّ مادته ونماذجه.", "A unified digital companion supporting students from admission to degree conferral.")
                };
                IntroModel.Badges = new List<string>
                {
                    GetResource("BadgeUni", "جامعة الأميرة نورة بنت عبد الرحمن • عمادة الدراسات العليا • وكالة الشؤون الأكاديمية", "Princess Nourah University • Deanship of Postgraduate Studies"),
                    GetResource("BadgeYear", "١٤٤٧ - ١٤٤٨هـ", "1447 - 1448 AH")
                };
                IntroModel.ReturnText = GetResource("BackToHub", "⟶ العودة لاختيار الفئة", "⟶ Back to Kit Selection");
            }

            if (JourneySteps.Count == 0)
            {
                JourneySteps.Add(new DgaPgJourneyStepModel
                {
                    StepNumber = 1,
                    IconClass = "hgi-route-03",
                    Title = GetResource("Step1GradTitle", "اكتمال المناقشة والتعديلات", "Completion of Defense and Amendments"),
                    Description = GetResource("Step1GradDesc", "استكمال مناقشة الرسالة وإجراء التعديلات المطلوبة والإقرار بها وفق توصية لجنة المناقشة.", "Completing the thesis defense and making the required modifications in accordance with the defense committee recommendation.")
                });
                JourneySteps.Add(new DgaPgJourneyStepModel
                {
                    StepNumber = 2,
                    IconClass = "hgi-route-03",
                    Title = GetResource("Step2GradTitle", "تسليم الرسالة العلمية إلكترونياً", "Electronic Submission of Thesis"),
                    Description = GetResource("Step2GradDesc", "تسليم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ورفعها إلى عمادة الدراسات العليا بصورتها المعتمدة.", "Submitting the final electronic thesis to the department and college, and uploading it in its approved format to the Deanship of Postgraduate Studies.")
                });
                JourneySteps.Add(new DgaPgJourneyStepModel
                {
                    StepNumber = 3,
                    IconClass = "hgi-route-03",
                    Title = GetResource("Step3GradTitle", "رفع التوصية بمنح الدرجة", "Recommendation for Degree Conferral"),
                    Description = GetResource("Step3GradDesc", "يرفع القسم تقرير اللجنة إلى الكلية، وتضمّ وحدة الخريجين بالعمادة اسم الطالبة إلى مذكرة الخريجين تمهيداً لرفعها إلى مجلس الجامعة.", "The department submits the committee report to the college, and the Graduate Unit adds the student to the graduation memo for submission to the University Council.")
                });
                JourneySteps.Add(new DgaPgJourneyStepModel
                {
                    StepNumber = 4,
                    IconClass = "hgi-route-03",
                    Title = GetResource("Step4GradTitle", "اعتماد رئيسة الجامعة المكلّفة", "Approval by the University President"),
                    Description = GetResource("Step4GradDesc", "اعتماد مذكرة الخريجين ومنح الدرجة العلمية وفق الصلاحيات النظامية المقرّرة.", "Approving the graduation memo and conferring the academic degree in accordance with statutory powers.")
                });
                JourneySteps.Add(new DgaPgJourneyStepModel
                {
                    StepNumber = 5,
                    IconClass = "hgi-route-03",
                    Title = GetResource("Step5GradTitle", "رفع خطاب لإصدار الوثيقة", "Issuance Request Letter"),
                    Description = GetResource("Step5GradDesc", "يُرفع خطابٌ إلى إدارة القبول والتسجيل لإصدار وثيقة التخرّج بعد اعتماد منح الدرجة.", "Submitting a formal letter to Admissions and Registration to issue the graduation certificate after degree approval.")
                });
                JourneySteps.Add(new DgaPgJourneyStepModel
                {
                    StepNumber = 6,
                    IconClass = "hgi-route-03",
                    Title = GetResource("Step6GradTitle", "إخلاء الطرف", "Clearance"),
                    Description = GetResource("Step6GradDesc", "استكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة الواردة فيه، تمهيداً لاستلام وثيقة التخرّج.", "Completing the clearance form certified by the relevant entities before receiving the graduation certificate.")
                });
            }

            if (GraduationRequirements.Count == 0)
            {
                GraduationRequirements.Add(new DgaPgResourceCardModel
                {
                    Title = GetResource("Req1Title", "تسليم الرسالة العلمية إلكترونياً", "Electronic Thesis Submission"),
                    Description = GetResource("Req1Desc", "تسليم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ليتولّيا رفعها إلى عمادة الدراسات العليا بصورتها المعتمدة.", "Submitting the final electronic copy of the thesis to the department and college for submission to the Deanship."),
                    BadgeText = "1",
                    IconClass = "hgi-check-list"
                });
                GraduationRequirements.Add(new DgaPgResourceCardModel
                {
                    Title = GetResource("Req2Title", "تسليم نسختين ورقيتين للعمادة", "Submitting Two Hard Copies"),
                    Description = GetResource("Req2Desc", "تسليم نسختين ورقيتين من الرسالة العلمية إلى عمادة الدراسات العليا.", "Submitting two printed copies of the thesis to the Deanship of Postgraduate Studies."),
                    BadgeText = "2",
                    IconClass = "hgi-check-list"
                });
                GraduationRequirements.Add(new DgaPgResourceCardModel
                {
                    Title = GetResource("Req3Title", "إخلاء الطرف", "University Clearance"),
                    Description = GetResource("Req3Desc", "استكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة: مكتبة الجامعة، وسكن الطالبات، ووحدة الشؤون المالية بالعمادة، ووكالة الشؤون الأكاديمية للدراسات العليا؛ تمهيداً لاستلام وثيقة التخرّج من إدارة القبول والتسجيل.", "Completing clearance signed by: Library, Housing, Deanship Finance Unit, and Academic Affairs Vice-Deanship, prior to collecting the graduation certificate."),
                    BadgeText = "3",
                    IconClass = "hgi-check-list"
                });
            }

            if (Procedures.Count == 0)
            {
                // Proc 1: Degree Conferral
                var proc1 = new DgaPgProcedureModel
                {
                    Id = 1,
                    Title = GetResource("Proc1Title", "منح الدرجة العلمية", "Conferral of Academic Degree"),
                    BadgeText = GetResource("Proc1Badge", "المواد ٥٣ و٥٤ و٥٦", "Articles 53, 54 & 56"),
                    IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/graduate-procedure-degree.png",
                    ShortDescription = GetResource("Proc1Summary", "تخرّج الطالب/ـة بعد إنهاء متطلبات البرنامج بنجاح، ورفع التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.", "Graduation after completing all program requirements successfully and submitting the recommendation to the University Council."),
                    Requirements = new List<string>
                    {
                        GetResource("Proc1Req1", "اكتمال متطلبات التخرج بنجاح حسب خطة البرنامج، وألا يقلّ المعدل التراكمي عن «جيد جداً».", "Completion of graduation requirements with GPA not less than Very Good."),
                        GetResource("Proc1Req2", "استكمال نماذج اللجنة والإقرار بالتعديلات وإخلاء الطرف، وتسليم نسخ الرسالة المعتمدة.", "Completion of committee forms, modification approvals, clearance, and approved thesis copies."),
                        GetResource("Proc1Req3", "رفع الوكيل المختص التوصية بمنح الدرجة إلى مجلس الجامعة (المفوض لمعالي رئيسة الجامعة).", "Submission of recommendation to the University Council (delegated to the University President).")
                    },
                    Articles = new List<DgaPgArticleBlock>
                    {
                        new DgaPgArticleBlock
                        {
                            Heading = GetResource("Art53Heading", "المادة الثالثة والخمسون", "Article Fifty-Three"),
                            Body = GetResource("Art53Body", "يرفع رئيس القسم المختص تقرير لجنة المناقشة إلى عميد الكلية في مدة لا تتجاوز أسبوعين من تاريخ المناقشة؛ لرفعه إلى الإدارة التنفيذية.", "The department chair submits the defense report to the dean within two weeks of the defense.")
                        },
                        new DgaPgArticleBlock
                        {
                            Heading = GetResource("Art54Heading", "المادة الرابعة والخمسون", "Article Fifty-Four"),
                            Body = GetResource("Art54Body", "يتخرج الطالب بعد إنهاء متطلبات التخرج بنجاح حسب خطة البرنامج، على ألا يقل معدله التراكمي عن المعدل الذي يحدده مجلس الجامعة لكل برنامج، على ألا يقل في كل الأحوال عن تقدير (جيد جداً).", "The student graduates after fulfilling all requirements with a GPA not less than Very Good.")
                        },
                        new DgaPgArticleBlock
                        {
                            Heading = GetResource("Art56Heading", "المادة السادسة والخمسون", "Article Fifty-Six"),
                            Body = GetResource("Art56Body", "يرفع النائب أو الوكيل المختص التوصية بمنح الدرجة العلمية إلى مجلس الجامعة لاتخاذ القرار.", "The Vice President submits the degree conferral recommendation to the University Council.")
                        }
                    },
                    Forms = new List<DgaPgLinkItem>
                    {
                        new DgaPgLinkItem { Title = GetResource("FormDefenseReport", "نموذج تقرير لجنة مناقشة الرسالة العلمية", "Defense Committee Report Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%aa%d9%82%d8%b1%d9%8a%d8%b1%20%d9%84%d8%ac%d9%86%d8%a9%20%d9%85%d9%86%d8%a7%d9%82%d8%b4%d8%a9%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx" },
                        new DgaPgLinkItem { Title = GetResource("FormAmendments", "نموذج الإقرار بإجراء التعديلات المطلوبة", "Amendments Acknowledgment Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d8%a5%d9%82%d8%b1%d8%a7%d8%b1%20%d8%a8%d8%a5%d8%ac%d8%b1%d8%a7%d8%a1%20%d8%a7%d9%84%d8%aa%d8%b9%d8%af%d9%8a%d9%84%d8%a7%d8%aa%20%d8%a7%d9%84%d9%85%d8%b7%d9%84%d9%88%d8%a8%d8%a9%20%d8%b9%d9%84%d9%89%20%d8%a7%d9%84%d8%b1%d8%b3%d8%a7%d9%84%d8%a9%20%d8%a7%d9%84%d8%b9%d9%84%d9%85%d9%8a%d8%a9.docx" },
                        new DgaPgLinkItem { Title = GetResource("FormClearance", "نموذج إخلاء طرف", "Clearance Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%ae%d9%84%d8%a7%d8%a1%20%d8%b7%d8%b1%d9%81.docx" }
                    },
                    FlowText = GetResource("Proc1Flow", "مسار الإجراء الأكاديمي: تقرير لجنة المناقشة ← رئيس القسم ← عميد الكلية ← الإدارة التنفيذية ← الوكيل المختص ← مجلس الجامعة.", "Academic Workflow: Defense Report → Department Chair → College Dean → Executive Administration → Vice President → University Council.")
                };
                Procedures.Add(proc1);

                // Proc 2: Clearance
                var proc2 = new DgaPgProcedureModel
                {
                    Id = 2,
                    Title = GetResource("Proc2Title", "إخلاء الطرف", "Clearance"),
                    BadgeText = GetResource("Proc2Badge", "إجراءات معتمدة", "Approved Procedures"),
                    IconUrl = "/Style%20Library/DGA/PostgraduateKit/public/images/academic-kit/ui/graduate-procedure-exit.png",
                    ShortDescription = GetResource("Proc2Summary", "استكمال الطالب/ـة براءة ذمّتها تجاه وحدات الجامعة عند الانسحاب أو التخرج أو إلغاء القيد.", "Completing clearance with all university units upon graduation, withdrawal, or status termination."),
                    Requirements = new List<string>
                    {
                        GetResource("Proc2Req1", "استكمال براءة الذمة تجاه جميع وحدات الجامعة ذات العلاقة.", "Fulfilling financial and asset obligations across all relevant departments."),
                        GetResource("Proc2Req2", "يُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب أو التخرج أو إلغاء القيد ومنح الدرجة.", "A mandatory concluding step prior to receiving the official certificate.")
                    },
                    Articles = new List<DgaPgArticleBlock>
                    {
                        new DgaPgArticleBlock
                        {
                            Heading = GetResource("ArtClearanceHeading", "الأساس الإجرائي", "Procedural Basis"),
                            Body = GetResource("ArtClearanceBody", "يُستكمل إخلاء الطرف وفق الإجراءات المعتمدة بالعمادة، ويُعدّ خطوةً ختاميةً مرتبطةً بالانسحاب والتخرج وإلغاء القيد ومنح الدرجة.", "Clearance is processed per approved Deanship guidelines and is required for degree issuance.")
                        }
                    },
                    Forms = new List<DgaPgLinkItem>
                    {
                        new DgaPgLinkItem { Title = GetResource("FormClearance", "نموذج إخلاء طرف", "Clearance Form"), Url = "https://pnu.edu.sa/ar/Deanship/postgraduate/Documents/%d9%86%d9%85%d9%88%d8%b0%d8%ac%20%d8%a7%d9%95%d8%ae%d9%84%d8%a7%d8%a1%20%d8%b7%d8%b1%d9%81.docx" }
                    },
                    FlowText = GetResource("Proc2Flow", "مسار الإجراء الأكاديمي: الطالب/ـة ← الوحدات ذات العلاقة ← اعتماد إخلاء الطرف.", "Academic Workflow: Student → Relevant University Units → Clearance Approval.")
                };
                Procedures.Add(proc2);
            }

            if (FaqItems.Count == 0)
            {
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ1", "ما المتطلّبات اللازمة لإتمام تخرّجي؟", "What are the requirements to finalize my graduation?"),
                    Answer = GetResource("GradFaqA1", "تسليم الرسالة العلمية بصورتها النهائية إلى القسم والكلية لرفعها إلى العمادة، وتسليم نسختين ورقيتين من الرسالة لعمادة الدراسات العليا، واستكمال نموذج إخلاء الطرف باعتماد الجهات المعنيّة الواردة فيه.", "Submitting the final thesis electronically to the department/college for submission to the Deanship, delivering two hard copies, and completing the clearance form.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ2", "كيف تُرفع التوصية بمنح الدرجة العلمية؟", "How is the recommendation for degree conferral submitted?"),
                    Answer = GetResource("GradFaqA2", "يرفع القسم تقرير لجنة المناقشة إلى الكلية، ثم تضمّ وحدة الخريجين بالعمادة اسمكِ إلى مذكرة الخريجين، وتُرفع إلى مجلس الجامعة، وتُعتمد من رئيس الجامعة المكلّفة وفق الصلاحيات النظامية.", "The department submits the defense report to the college, the Graduate Unit includes your name in the graduation memo, which is submitted to the University Council and approved by the University President.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ3", "كيف أستلم وثيقة التخرّج؟", "How do I receive my graduation certificate?"),
                    Answer = GetResource("GradFaqA3", "بعد اعتماد منح الدرجة، تطبعين نموذج إخلاء الطرف وتستكملينه، ثم تراجعين عمادة القبول والتسجيل لاستلام وثيقة التخرّج.", "After degree approval, complete the clearance form and visit the Deanship of Admissions and Registration to collect your certificate.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ4", "ما المقصود بإخلاء الطرف؟", "What is meant by university clearance?"),
                    Answer = GetResource("GradFaqA4", "هو استكمال إنهاء التزاماتكِ تجاه وحدات الجامعة عبر النموذج المعتمد، وهو خطوةٌ ختاميةٌ لازمةٌ قبل استلام وثيقة التخرّج.", "It is settling all commitments across university departments using the approved form, which is mandatory before collecting the certificate.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ5", "كيف أتابع تغيّر حالتي إلى متخرّجة؟", "How can I track my status change to graduate?"),
                    Answer = GetResource("GradFaqA5", "يمكنكِ متابعة وضعكِ على بوابة النظام الأكاديمي حتى تتأكّدي من تغيّر حالتكِ من منتظمة إلى متخرّجة.", "You can track your academic status via the Academic System Portal until it changes from active to graduated.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ6", "كيف أسلّم رسالتي العلمية للعمادة؟", "How do I submit my thesis to the Deanship?"),
                    Answer = GetResource("GradFaqA6", "تُسلَّم الرسالة العلمية إلكترونياً بصيغتها النهائية إلى القسم والكلية، ليتولّيا رفعها إلى عمادة الدراسات العليا بصورتها المعتمدة، مع تسليم نسختين ورقيتين للعمادة.", "Submit the final electronic thesis to your department/college for official transmission, along with two printed copies to the Deanship.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ7", "ما الجهات التي يلزم اعتمادها في نموذج إخلاء الطرف؟", "Which departments must sign the clearance form?"),
                    Answer = GetResource("GradFaqA7", "يلزم اعتماد النموذج من: مكتبة الجامعة، وسكن الطالبات، ووحدة الشؤون المالية بالعمادة، ووكالة الشؤون الأكاديمية للدراسات العليا.", "Signatures required: University Library, Student Housing, Deanship Finance Unit, and Postgraduate Academic Affairs.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ8", "ما دور رئيسة الجامعة المكلّفة في منح الدرجة؟", "What is the role of the University President in conferring the degree?"),
                    Answer = GetResource("GradFaqA8", "بعد رفع مذكرة الخريجين إلى مجلس الجامعة، تُعتمد الدرجة العلمية وفق الصلاحيات النظامية المقرّرة، تمهيداً لإصدار وثيقة التخرّج.", "The President approves degree conferral per statutory authorizations following submission to the University Council.")
                });
                FaqItems.Add(new DgaPgFaqItemModel
                {
                    Question = GetResource("GradFaqQ9", "من الجهة التي تُصدر وثيقة التخرّج؟", "Which entity issues the graduation certificate?"),
                    Answer = GetResource("GradFaqA9", "بعد اعتماد منح الدرجة، يُرفع خطابٌ إلى إدارة القبول والتسجيل لإصدار وثيقة التخرّج، وتراجعينها لاستلامها بعد استكمال إخلاء الطرف.", "The Deanship of Admissions and Registration issues the graduation certificate upon receiving the approval letter.")
                });
            }
        }

        // ---------------------------------------------------------------
        // Section anchors and table of contents
        // ---------------------------------------------------------------

        /// <summary>
        /// Fixed anchor id per section key. academic-kit-pages.css styles
        /// #graduate-section-1..4 individually, so these ids must never be derived
        /// from list position — reordering or hiding a section in SharePoint would
        /// otherwise shift the styling onto the wrong content.
        /// </summary>
        protected override string SectionAnchor(string sectionKey)
        {
            switch (sectionKey)
            {
                case DgaPgSectionKeys.Intro:            return "graduate-intro";
                case DgaPgSectionKeys.Journey:          return "graduate-section-1";
                case DgaPgSectionKeys.GradRequirements: return "graduate-section-2";
                case DgaPgSectionKeys.Procedures:       return "graduate-section-3";
                case DgaPgSectionKeys.Faq:              return "graduate-section-4";
                case DgaPgSectionKeys.Reference:        return "graduate-reference";
                default:                                return "graduate-" + (sectionKey ?? string.Empty).ToLowerInvariant();
            }
        }

        /// <summary>
        /// TOC entries for the sections this page actually renders, so an empty or
        /// hidden section never leaves a dead button behind.
        /// </summary>
        public List<DgaPgLinkItem> TocItems
        {
            get
            {
                var toc = new List<DgaPgLinkItem>();

                AddToc(toc, DgaPgSectionKeys.Intro, IntroModel.Title, true);
                AddToc(toc, DgaPgSectionKeys.Journey, GetSectionTitle(DgaPgSectionKeys.Journey, GetResource("GradAxis1", "المحور الأول: رحلة التخرّج", "First Axis: Graduation Journey")), JourneySteps.Count > 0);
                AddToc(toc, DgaPgSectionKeys.GradRequirements, GetSectionTitle(DgaPgSectionKeys.GradRequirements, GetResource("GradAxis2", "المحور الثاني: متطلّبات التخرّج", "Second Axis: Graduation Requirements")), GraduationRequirements.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Procedures, GetSectionTitle(DgaPgSectionKeys.Procedures, GetResource("GradAxis3", "المحور الثالث: إجراءات التخرّج", "Third Axis: Graduation Procedures")), Procedures.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Faq, GetSectionTitle(DgaPgSectionKeys.Faq, GetResource("GradAxis4", "المحور الرابع: الأسئلة الأكثر شيوعاً", "Fourth Axis: Frequently Asked Questions")), FaqItems.Count > 0);
                AddToc(toc, DgaPgSectionKeys.Reference, ReferenceTitle, !string.IsNullOrEmpty(ReferenceLead));

                return toc;
            }
        }

        protected string ResolveHomeUrl()
        {
            return IsArabic ? "/ar/Pages/default.aspx" : "/en/Pages/default.aspx";
        }
    }
}
