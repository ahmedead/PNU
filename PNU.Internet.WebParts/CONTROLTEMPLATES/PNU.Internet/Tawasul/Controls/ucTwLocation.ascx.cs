using System;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// The "الموقع" section: national address block and the embedded map, read from the
    /// shared TwLocation list (the same item the University card reuses).
    /// </summary>
    public partial class ucTwLocation : TwSectionBase
    {
        protected override string ListName { get { return TwListNames.Location; } }
        protected override string SectionKey { get { return TwListSchema.KeyLocation; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            try
            {
                LoadItems();
                TwCard loc = FirstItem;

                // Heading: the "الموقع" section title, or the address item's own title.
                string heading = HeadingText;
                if (string.IsNullOrEmpty(heading) && loc != null) heading = loc.Title;
                ltSectionTitle.Text = heading;

                ltAddress.Text = loc != null ? loc.AddressHtml : string.Empty;

                ltMap.Text = loc != null ? loc.MapHtml : string.Empty;
                phMap.Visible = loc != null && loc.HasMap;

                secLocation.Visible = loc != null;
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTwLocation.Bind", ex);
                secLocation.Visible = false;
            }
        }
    }
}
