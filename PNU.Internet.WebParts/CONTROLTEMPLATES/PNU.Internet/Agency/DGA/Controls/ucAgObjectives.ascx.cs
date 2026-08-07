using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// "الأهداف" - one full-width card whose bullets come from the AgObjectives list.
    /// The card heading comes from the schema title, the icon from the first row.
    /// </summary>
    public partial class ucAgObjectives : AgSectionBase
    {
        private const string DefaultIcon = "hgi-target-02";

        protected override string ListName { get { return AgListNames.Objectives; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                if (Items.Count == 0)
                {
                    colObjectives.Visible = false;
                    return;
                }

                ltSectionTitle.Text = AgHelper.Enc(HeadingText);
                phHeading.Visible = HeadingVisible && !string.IsNullOrEmpty(HeadingText);

                string icon = AgHelper.Icon(FirstItem.IconClass, DefaultIcon);
                ltIcon.Text = "<i class=\"hgi hgi-stroke " + icon + " fs-3\" aria-hidden=\"true\"></i>";

                rptObjectives.DataSource = Items;
                rptObjectives.DataBind();

                colObjectives.Visible = true;
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgObjectives.Bind", ex);
                colObjectives.Visible = false;
            }
        }
    }
}
