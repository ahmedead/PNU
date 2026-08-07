using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls
{
    /// <summary>
    /// Main container for the Contact / تواصل (Tawasul) page.
    /// Checks/provisions all supporting lists on every load, then hosts the three section
    /// controls. Each section can be switched off from the page layout, e.g.
    /// &lt;pnu:Tawasul runat="server" ShowLocation="False" /&gt;
    /// </summary>
    public partial class ucTawasul : UserControl
    {
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowContactCards { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowEntities { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowLocation { get; set; }

        /// <summary>Set to "False" to skip the per-page-load list check once the site is live.</summary>
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string EnsureLists { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                // Runs on every page load: verifies all lists exist on the target web and
                // provisions any missing one (columns + default data) before binding.
                if (!IsFalse(EnsureLists))
                    TwListProvisioner.EnsureAllListsExist();

                // Set visibility in OnInit so hidden sections never query their lists.
                ucContactCards.Visible = !IsFalse(ShowContactCards);
                ucEntities.Visible     = !IsFalse(ShowEntities);
                ucLocation.Visible     = !IsFalse(ShowLocation);
            }
            catch (Exception ex)
            {
                TwLog.Write("ucTawasul.OnInit", ex);
            }
        }

        private static bool IsFalse(string value)
        {
            return "False".Equals(value, StringComparison.OrdinalIgnoreCase)
                || "0".Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
