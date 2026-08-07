using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsAcademicServices : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.AcademicServices; } }

        /// <summary>Target of the "دليل الخدمات الأكاديمية" button. Empty hides the button.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string GuideUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                ltSectionTitle.Text = NsHelper.Enc(HeadingText);

                string guide = GuideUrl == null ? "#" : GuideUrl;
                lnkGuide.Visible = !string.IsNullOrEmpty(guide);
                lnkGuide.NavigateUrl = guide;
                lnkGuide.Text = NsHelper.GetRes("Ns_AcademicGuide",
                    "دليل الخدمات الأكاديمية", "Academic services guide");

                rptServices.DataSource = Items;
                rptServices.DataBind();

                secAcademicServices.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsAcademicServices.Bind", ex);
                secAcademicServices.Visible = false;
            }
        }
    }
}
