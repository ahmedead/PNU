using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>
    /// "الرؤية" and "الرسالة" cards. Each row carries its own Bootstrap column class,
    /// so editors can widen or narrow a card without touching the markup.
    /// </summary>
    public partial class ucAgCards : AgSectionBase
    {
        protected override string ListName { get { return AgListNames.Cards; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                rptCards.DataSource = Items;
                rptCards.DataBind();
            }
            catch (Exception ex)
            {
                AgLog.Write("ucAgCards.Bind", ex);
            }
        }
    }
}
