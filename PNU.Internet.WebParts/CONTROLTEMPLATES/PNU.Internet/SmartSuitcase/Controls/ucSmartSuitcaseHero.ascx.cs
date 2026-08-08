using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    public partial class ucSmartSuitcaseHero : SscSectionBase
    {
        protected override string ListName { get { return SscListNames.Hero; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();

                if (Items != null && Items.Count > 0 && Items[0].HasImageUrl)
                {
                    imgHero.Src = Items[0].ImageUrl;
                    imgHero.Alt = string.IsNullOrEmpty(Items[0].ImageAlt)
                        ? "تطبيقات وخدمات Microsoft 365 المتاحة ضمن الحقيبة الذكية"
                        : Items[0].ImageAlt;
                }
                else
                {
                    imgHero.Src = "https://cdn.cs.1worldsync.com/syndication/mediaserverredirect/ca2b962c678f2d76b6169f3e69e10176/original.png";
                    imgHero.Alt = "تطبيقات وخدمات Microsoft 365 المتاحة ضمن الحقيبة الذكية";
                }
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSmartSuitcaseHero.Bind", ex);
                secHero.Visible = false;
            }
        }
    }
}
