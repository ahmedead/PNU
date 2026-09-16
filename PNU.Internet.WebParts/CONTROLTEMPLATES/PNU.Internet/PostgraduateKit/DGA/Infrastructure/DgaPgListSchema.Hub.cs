using Microsoft.SharePoint;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    public static partial class DgaPgListSchema
    {
        private static void RegisterHubLists()
        {
            var hub = new DgaPgListDef(DgaPgListNames.Hub, "الصفحة الرئيسية للحقيبة (DGA)", "بطاقات ومسارات الفئات الثلاث للحقيبة");
            hub.AddField("Title_EN", "Title (EN)", SPFieldType.Text, false);
            hub.AddField("Phase", "Phase", SPFieldType.Choice, true, DgaPgPhases.Stations);
            hub.AddField("Description", "Description (AR)", SPFieldType.Note, false);
            hub.AddField("Description_EN", "Description (EN)", SPFieldType.Note, false);
            hub.AddField("BadgeText", "Badge Text (AR)", SPFieldType.Text, false);
            hub.AddField("BadgeText_EN", "Badge Text (EN)", SPFieldType.Text, false);
            hub.AddField("ButtonText", "Button Text (AR)", SPFieldType.Text, false);
            hub.AddField("ButtonText_EN", "Button Text (EN)", SPFieldType.Text, false);
            hub.AddField("ButtonUrl", "Button URL", SPFieldType.Text, false);
            hub.AddField("IconClass", "Icon Class", SPFieldType.Text, false);
            hub.AddField("ItemOrder", "Item Order", SPFieldType.Number, false);
            hub.SeedDataFactory = GetHubSeed;
            Register(hub);
        }

        private static List<Dictionary<string, object>> GetHubSeed()
        {
            var list = new List<Dictionary<string, object>>();

            // Applicants Station Card
            list.Add(new Dictionary<string, object> {
                { "Title", "المتقدمون" },
                { "Title_EN", "Applicants" },
                { "Phase", DgaPgPhases.Applicants },
                { "Description", "شروط القبول لكل برنامج، والمستندات المطلوبة، والدورة التأهيلية للغة الإنجليزية" },
                { "Description_EN", "Admission criteria, required documents, and English qualifying courses." },
                { "BadgeText", "المرحلة الأولى" },
                { "BadgeText_EN", "Stage 1" },
                { "ButtonText", "عرض المسار" },
                { "ButtonText_EN", "View Track" },
                { "ButtonUrl", "/ar/Deanship/postgraduate/Pages/applicants.aspx" },
                { "IconClass", "hgi-login-03" },
                { "ItemOrder", 1 }
            });

            // Enrolled Station Card
            list.Add(new Dictionary<string, object> {
                { "Title", "المقيدون" },
                { "Title_EN", "Enrolled Students" },
                { "Phase", DgaPgPhases.Enrolled },
                { "Description", "رحلتك الأكاديمية، حقوقك ولوائحك، السجل الإجرائي، وحافز النشر العلمي" },
                { "Description_EN", "Academic journey, student rights & bylaws, procedures, and publication incentives." },
                { "BadgeText", "المرحلة الثانية" },
                { "BadgeText_EN", "Stage 2" },
                { "ButtonText", "عرض المسار" },
                { "ButtonText_EN", "View Track" },
                { "ButtonUrl", "/ar/Deanship/postgraduate/Pages/enrolled.aspx" },
                { "IconClass", "hgi-book-open-01" },
                { "ItemOrder", 2 }
            });

            // Graduates Station Card
            list.Add(new Dictionary<string, object> {
                { "Title", "الخريجون" },
                { "Title_EN", "Graduates" },
                { "Phase", DgaPgPhases.Graduates },
                { "Description", "إجراءات منح الدرجة وإخلاء الطرف، والوثائق، وهوية الخرّيج" },
                { "Description_EN", "Degree conferral, clearance procedures, official diplomas, and graduate identity." },
                { "BadgeText", "المرحلة الثالثة" },
                { "BadgeText_EN", "Stage 3" },
                { "ButtonText", "عرض المسار" },
                { "ButtonText_EN", "View Track" },
                { "ButtonUrl", "/ar/Deanship/postgraduate/Pages/graduates.aspx" },
                { "IconClass", "hgi-graduate-female" },
                { "ItemOrder", 3 }
            });

            return list;
        }
    }
}
