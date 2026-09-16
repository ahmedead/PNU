using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// The strip the v3.0 design adds above the main navigation: information items on one side
    /// and shortcut buttons on the other, both edited through the PgTopBar list.
    /// </summary>
    public partial class ucPgSecondBar : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.TopBar; } }

        protected override string DefaultSectionKey { get { return string.Empty; } }

        /// <summary>
        /// The design places this strip between the government signature bar and the sticky
        /// navigation, and both of those live inside the shared ucHomeHeader control. Rather
        /// than editing that shared control — which every other page uses — this control moves
        /// itself to sit directly after the named control. Clear the property to leave it
        /// wherever the master page puts it.
        /// </summary>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string AnchorControlId { get; set; }

        public ucPgSecondBar()
        {
            AnchorControlId = "ucHomeDigitalStamp";
            EnableViewState = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
            MoveAfterAnchor();
        }

        private void MoveAfterAnchor()
        {
            try
            {
                if (string.IsNullOrEmpty(AnchorControlId) || Page == null) return;

                Control anchor = FindByIdRecursive(Page, AnchorControlId);
                if (anchor == null || anchor.Parent == null) return;

                int index = anchor.Parent.Controls.IndexOf(anchor);
                if (index < 0) return;

                // already in place
                if (ReferenceEquals(Parent, anchor.Parent) && Parent.Controls.IndexOf(this) == index + 1) return;

                if (Parent != null) Parent.Controls.Remove(this);
                anchor.Parent.Controls.AddAt(index + 1, this);
            }
            catch (Exception ex)
            {
                // Rendering where the master placed it is an acceptable fallback.
                PgLog.Write("ucPgSecondBar.MoveAfterAnchor", ex);
            }
        }

        private static Control FindByIdRecursive(Control root, string id)
        {
            if (root == null) return null;

            Control direct = root.FindControl(id);
            if (direct != null) return direct;

            foreach (Control child in root.Controls)
            {
                Control found = FindByIdRecursive(child, id);
                if (found != null) return found;
            }

            return null;
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var info = new List<PgTopBarItem>();
            var buttons = new List<PgTopBarItem>();

            foreach (SPListItem item in raw)
            {
                string live = PgHelper.SafeString(item, "LiveValue").Trim();
                if (live.Equals(PgListSchema.PgLiveValues.None, StringComparison.OrdinalIgnoreCase))
                    live = string.Empty;

                var entry = new PgTopBarItem
                {
                    Id = item.ID,
                    Text = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    IconUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "IconUrl")),
                    LinkUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "LinkUrl")),
                    AriaLabel = PgHelper.Enc(PgHelper.PickField(item, "AriaLabel")),
                    LiveValue = live,
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder")
                };

                if (string.IsNullOrEmpty(entry.AriaLabel)) entry.AriaLabel = entry.Text;

                bool isButton = PgListSchema.PgTopBarKinds.Button.Equals(
                    PgHelper.SafeString(item, "Kind").Trim(), StringComparison.OrdinalIgnoreCase);

                if (isButton) buttons.Add(entry); else info.Add(entry);
            }

            if (rptInfo != null)
            {
                rptInfo.DataSource = info;
                rptInfo.DataBind();
            }

            if (rptButtons != null)
            {
                rptButtons.DataSource = buttons;
                rptButtons.DataBind();
            }

            if (phBar != null) phBar.Visible = (info.Count + buttons.Count) > 0;
            if (phClock != null) phClock.Visible = info.Exists(i => i.IsLive);
        }
    }
}
