using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// Definition and seed data for PgHub: the carousel slides and the station cards of the
    /// kit landing page (index.html in the design).
    /// </summary>
    public static partial class PgListSchema
    {
        /// <summary>What a PgHub row renders as.</summary>
        public static class PgHubKinds
        {
            public const string Slide = "Slide";
            public const string Card = "Card";

            public static readonly string[] All = new string[] { Slide, Card };
        }

        /// <summary>Base path of the responsive hero image used by the carousel.</summary>
        internal const string HeroBase = ImageRoot + "hero/hero-library";

        private static void BuildHubList(Dictionary<string, PgListDef> map)
        {
            map[PgListNames.Hub] = new PgListDef
            {
                Name = PgListNames.Hub,
                TitleAr = "الصفحة الرئيسة للحقيبة", TitleEn = "Kit landing page",
                Description = "Carousel slides and station cards of the kit landing page.",
                Fields = new List<PgFieldDef>
                {
                    TitleEn(),
                    new PgFieldDef("Paragraphs", "الفقرات (عربي)", "Paragraphs (AR)", SPFieldType.Note, false, 6, "فقرة في كل سطر."),
                    new PgFieldDef("Paragraphs_EN", "الفقرات (إنجليزي)", "Paragraphs (EN)", SPFieldType.Note, false, 6),
                    new PgFieldDef("ImageBase", "أساس مسار الصورة", "Image base path", SPFieldType.Text, false, 0,
                                   "بدون اللاحقة: يضيف العنصر -sm و -md و -lg مع avif و webp و jpg."),
                    new PgFieldDef("ImageAlt", "النص البديل للصورة (عربي)", "Image alt (AR)", SPFieldType.Text),
                    new PgFieldDef("ImageAlt_EN", "النص البديل للصورة (إنجليزي)", "Image alt (EN)", SPFieldType.Text),
                    Icon(),
                    new PgFieldDef("ButtonText", "نص الزر (عربي)", "Button text (AR)", SPFieldType.Text, true),
                    new PgFieldDef("ButtonText_EN", "نص الزر (إنجليزي)", "Button text (EN)", SPFieldType.Text),
                    Link(),
                    new PgFieldDef("Kind", "النوع", "Kind", SPFieldType.Choice, true, 0,
                                   "Slide = شريحة في العارض، Card = بطاقة محطة.", PgHubKinds.All),
                    Phase(), Order()
                },
                Seed = HubSeed()
            };
        }

        private static List<Dictionary<string, string>> HubSeed()
        {
            var seed = new List<Dictionary<string, string>>();

            // The design's carousel carries two further slides ("أخبار جامعة الأميرة نورة" and
            // "جامعة الأميرة نورة") that are leftovers from the university home page template,
            // so only the kit's own slide is seeded. Editors can add more rows of kind Slide.
            seed.Add(Row(
                "Title", "الحقيبة الأكاديمية التفاعلية لطلبة الدراسات العليا",
                "Title_EN", "The interactive academic kit for postgraduate students",
                "Paragraphs", "رفيقٌ رقميٌّ موحّدٌ يواكب الطالبـ/ـة من القبول حتى منح الدرجة العلمية\n١٤٤٧ - ١٤٤٨هـ\nعمادة الدراسات العليا\nتصميم وإعداد وتنفيذ\nد. ساره بنت عبدالعزيز العمار\nوكيلة عمادة الدراسات العليا للشؤون الاكاديمية",
                "Paragraphs_EN", "A single digital companion that accompanies the student from admission to the award of the degree\n1447 - 1448 AH\nDeanship of Graduate Studies\nDesigned, prepared and delivered by\nDr. Sarah bint Abdulaziz Al-Ammar\nVice Dean of Graduate Studies for Academic Affairs",
                "ImageBase", HeroBase,
                "ImageAlt", "المكتبة المركزية في جامعة الأميرة نورة",
                "ImageAlt_EN", "The Central Library at Princess Nourah University",
                "Kind", PgHubKinds.Slide,
                "Phase", PgPhases.Hub, "ItemOrder", "1"));

            seed.Add(HubCard(1, "المتقدمون", "Applicants",
                "شروط القبول لكل برنامج، والمستندات المطلوبة، والدورة التأهيلية للغة الإنجليزية",
                "Admission requirements for each program, the documents needed, and the English language preparatory course",
                "المرحلة الأولى", "Stage one",
                "/ar/Deanship/postgraduate/Pages/applicants.aspx", "gratued.png"));

            seed.Add(HubCard(2, "المقيدون", "Enrolled students",
                "رحلتك الأكاديمية، حقوقك ولوائحك، السجل الإجرائي، وحافز النشر العلمي",
                "Your academic journey, your rights and regulations, the procedures register, and the research publication incentive",
                "المرحلة الثانية", "Stage two",
                "/ar/Deanship/postgraduate/Pages/enrolled.aspx", "registered.png"));

            seed.Add(HubCard(3, "الخريجون", "Graduates",
                "إجراءات منح الدرجة وإخلاء الطرف، والوثائق، وهوية الخرّيج",
                "The award of the degree and clearance procedures, the documents, and the graduate identity",
                "المرحلة الثالثة", "Stage three",
                "/ar/Deanship/postgraduate/Pages/graduates.aspx", "apply.png"));

            return seed;
        }

        private static Dictionary<string, string> HubCard(int order, string titleAr, string titleEn,
                                                          string descAr, string descEn,
                                                          string buttonAr, string buttonEn,
                                                          string url, string icon)
        {
            return Row("Title", titleAr, "Title_EN", titleEn,
                       "Paragraphs", descAr, "Paragraphs_EN", descEn,
                       "ButtonText", buttonAr, "ButtonText_EN", buttonEn,
                       "LinkUrl", url,
                       "IconUrl", ImageRoot + icon,
                       "Kind", PgHubKinds.Card,
                       "Phase", PgPhases.Hub,
                       "ItemOrder", N(order));
        }
    }
}
