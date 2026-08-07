using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.NouraStudents.Controls
{
    public partial class ucNsStudentServices : NsSectionBase
    {
        protected override string ListName { get { return NsListNames.StudentServices; } }

        /// <summary>Section image. Defaults to the Style Library asset.</summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ImageUrl { get; set; }

        /// <summary>Intro paragraph shown above the service list.</summary>
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
                    ? NsHelper.GetRes("Ns_StudentServicesIntro",
                        "توفر لك جامعة نورة خدمات متكاملة تدعم رحلتك وحياتك الأكاديمية.",
                        "PNU offers integrated services that support your academic journey.")
                    : IntroText);

                string image = string.IsNullOrEmpty(ImageUrl)
                    ? "/Style Library/DGA/images/students/services.png"
                    : ImageUrl;

                imgSection.ImageUrl = image;
                imgSection.AlternateText = NsHelper.GetRes("Ns_StudentServices", "الخدمات الطلابية", "Student services");
                imgSection.Attributes["loading"] = "lazy";
                imgSection.Attributes["decoding"] = "async";

                rptServices.DataSource = Items;
                rptServices.DataBind();

                secStudentServices.Visible = Items.Count > 0;
            }
            catch (Exception ex)
            {
                NsLog.Write("ucNsStudentServices.Bind", ex);
                secStudentServices.Visible = false;
            }
        }
    }
}
