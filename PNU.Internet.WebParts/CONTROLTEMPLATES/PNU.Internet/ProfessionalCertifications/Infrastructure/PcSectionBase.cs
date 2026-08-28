using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public abstract class PcSectionBase : UserControl
    {
        protected abstract string ListName { get; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListNameOverride { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int MaxItems { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ShowTitle { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string SectionTitle { get; set; }

        protected string EffectiveListName
        {
            get { return string.IsNullOrEmpty(ListNameOverride) ? ListName : ListNameOverride; }
        }

        protected List<PcCertificationModel> Items { get; private set; }

        protected PcSectionBase()
        {
            Items = new List<PcCertificationModel>();
            ShowTitle = "True";
        }

        protected string HeadingText
        {
            get
            {
                if (!string.IsNullOrEmpty(SectionTitle)) return SectionTitle;
                PcListDef def = PcListSchema.Get(EffectiveListName);
                return def != null ? def.Display : string.Empty;
            }
        }

        protected bool HeadingVisible
        {
            get { return !"False".Equals(ShowTitle, StringComparison.OrdinalIgnoreCase); }
        }

        protected void LoadItems()
        {
            try
            {
                PcListProvisioner.EnsureListExists(EffectiveListName);

                if (SPContext.Current == null) return;

                Guid siteId = SPContext.Current.Site.ID;
                List<SPListItem> raw = new List<SPListItem>();

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = PcTargetWeb.Open(site))
                    {
                        if (web == null) return;
                        raw = PcHelper.GetItems(web, EffectiveListName);
                    }
                });

                Items = raw.Select(Project).ToList();

                if (MaxItems > 0 && Items.Count > MaxItems)
                    Items = Items.Take(MaxItems).ToList();
            }
            catch (Exception ex)
            {
                PcLog.Write(GetType().Name + ".LoadItems:" + EffectiveListName, ex);
                Items = new List<PcCertificationModel>();
            }
        }

        protected virtual PcCertificationModel Project(SPListItem item)
        {
            string relatedStr = PcHelper.SafeString(item, "RelatedCourses");
            string c1 = "", c2 = "", c3 = "", c4 = "";
            if (!string.IsNullOrEmpty(relatedStr))
            {
                string[] arr = relatedStr.Split(new char[] { ',', '،', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0) c1 = arr[0].Trim();
                if (arr.Length > 1) c2 = arr[1].Trim();
                if (arr.Length > 2) c3 = arr[2].Trim();
                if (arr.Length > 3) c4 = arr[3].Trim();
            }

            var cert = new PcCertificationModel
            {
                id = item.ID,
                college = PcHelper.Pick(PcHelper.SafeString(item, "College"), PcHelper.SafeString(item, "College_EN")),
                department = PcHelper.Pick(PcHelper.SafeString(item, "Department"), PcHelper.SafeString(item, "Department_EN")),
                Program = PcHelper.Pick(PcHelper.SafeString(item, "Program"), PcHelper.SafeString(item, "Program_EN")),
                title_ar = PcHelper.SafeString(item, "Title"),
                title_en = PcHelper.SafeString(item, "Title_EN"),
                description = PcHelper.Pick(PcHelper.SafeString(item, "Description"), PcHelper.SafeString(item, "Description_EN")),
                level = PcHelper.Pick(PcHelper.SafeString(item, "Level"), PcHelper.SafeString(item, "Level_EN")),
                language = PcHelper.Pick(PcHelper.SafeString(item, "Language"), PcHelper.SafeString(item, "Language_EN")),
                cost = PcHelper.SafeString(item, "Cost"),
                mode = PcHelper.Pick(PcHelper.SafeString(item, "Mode"), PcHelper.SafeString(item, "Mode_EN")),
                is_supported = PcHelper.Pick(PcHelper.SafeString(item, "IsSupported"), PcHelper.SafeString(item, "IsSupported_EN")),
                topics = PcHelper.SafeString(item, "Topics"),
                requirements = PcHelper.SafeString(item, "Requirements"),
                registration_steps = PcHelper.SafeString(item, "RegistrationSteps"),
                provider = PcHelper.SafeString(item, "Provider"),
                resources = PcHelper.SafeString(item, "Resources"),
                validity = PcHelper.SafeString(item, "Validity"),
                exam_duration = PcHelper.SafeString(item, "ExamDuration"),
                questions_count = PcHelper.SafeString(item, "QuestionsCount"),
                questions_type = PcHelper.SafeString(item, "QuestionsType"),
                ContactPerson = PcHelper.SafeString(item, "ContactPerson"),
                course_1 = c1,
                course_2 = c2,
                course_3 = c3,
                course_4 = c4,
                link = PcHelper.SafeUrl(item, "OfficialLink"),
                item_order = PcHelper.SafeInt(item, "ItemOrder")
            };

            return cert;
        }
    }
}
