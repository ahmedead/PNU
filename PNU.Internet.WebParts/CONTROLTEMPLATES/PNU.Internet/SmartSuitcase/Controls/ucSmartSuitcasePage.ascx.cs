using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SmartSuitcase.Controls
{
    /// <summary>
    /// Main container for the Smart Suitcase page.
    /// Provisions all supporting lists once, then hosts the five section controls.
    /// </summary>
    public partial class ucSmartSuitcasePage : UserControl
    {
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowHero { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowServices { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowAccess { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowFaq { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowSupport { get; set; }

        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string EnsureLists { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                if (!IsFalse(EnsureLists))
                    SmartSuitcaseListProvisioner.EnsureAllListsExist();

                ucHero.Visible = !IsFalse(ShowHero);
                ucServices.Visible = !IsFalse(ShowServices);
                ucAccess.Visible = !IsFalse(ShowAccess);
                ucFaq.Visible = !IsFalse(ShowFaq);
                ucSupport.Visible = !IsFalse(ShowSupport);
            }
            catch (Exception ex)
            {
                SscLog.Write("ucSmartSuitcasePage.OnInit", ex);
            }
        }

        private static bool IsFalse(string value)
        {
            return "False".Equals(value, StringComparison.OrdinalIgnoreCase)
                || "0".Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
