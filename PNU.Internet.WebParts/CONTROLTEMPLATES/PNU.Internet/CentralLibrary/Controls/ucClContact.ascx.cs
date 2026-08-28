using System;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucClContact : ClSectionBase
    {
        protected override string ListName
        {
            get { return ClListNames.Contact; }
        }

        public ClCard HoursItem { get; private set; }
        public ClCard ContactItem { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadItems();
                if (Items != null && Items.Count > 0)
                {
                    HoursItem = Items.FirstOrDefault(i => i.ItemOrder == 1) ?? Items[0];
                    if (Items.Count > 1)
                        ContactItem = Items.FirstOrDefault(i => i.ItemOrder == 2) ?? Items[1];
                }
            }
            catch (Exception ex)
            {
                ClLog.Write("ucClContact.Page_Load", ex);
            }
        }
    }
}
