using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsCampusLife : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.CampusLife; } }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ImageUrl { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string IntroText { get; set; }

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

                ltIntro.Text = NsHelper.Enc(string.IsNullOrEmpty(IntroText)
                    ? NsHelper.GetRes("Ns_CampusLifeIntro",
                        "تجربة جامعية متكاملة تثري مهارات الطالبات وتدعم مشاركتهن داخل الحرم الجامعي وخارجه.",
                        "A complete university experience that enriches students' skills on and off campus.")
                    : IntroText);

                imgCampus.ImageUrl = string.IsNullOrEmpty(ImageUrl)
                    ? "/Style Library/DGA/images/hero/hero-campus.webp"
                    : ImageUrl;
                imgCampus.AlternateText = NsHelper.GetRes("Ns_CampusLife", "الحياة الجامعية", "Campus life");
                imgCampus.Attributes["loading"] = "lazy";
                imgCampus.Attributes["decoding"] = "async";

                rptCampusLife.DataSource = Items;
                rptCampusLife.DataBind();

                secCampusLife.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsCampusLife.Bind", ex);
                secCampusLife.Visible = false;
            }
        }
    }
}
