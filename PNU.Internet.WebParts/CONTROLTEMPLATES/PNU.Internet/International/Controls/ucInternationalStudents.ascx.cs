using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.International.Controls
{
    /// <summary>
    /// Main container for the International Students page.
    /// Provisions all supporting lists once, then hosts the ten section controls.
    /// Each section can be switched off from the page layout / web part properties,
    /// e.g. &lt;pnu:InternationalStudents runat="server" ShowExchange="False" /&gt;
    /// </summary>
    public partial class ucInternationalStudents : UserControl
    {
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowNumbers { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowFeatures { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowAdmission { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowBeforeArrival { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowOnArrival { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowOffice { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowActivities { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowExchange { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowDocuments { get; set; }
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ShowContact { get; set; }

        /// <summary>Target page of the "Send a message" button in the contact section.</summary>
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string ContactPageUrl { get; set; }

        /// <summary>Set to "False" to skip the per-page-load list check once the site is live.</summary>
        [Browsable(true)][PersistenceMode(PersistenceMode.Attribute)] public string EnsureLists { get; set; }


        public bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                // Runs on every page load: verifies all ten lists exist on this web and
                // provisions any missing one (columns + default data) before binding.
                if (!IsFalse(EnsureLists))
                    InternationalListProvisioner.EnsureAllListsExist();

                // Set visibility in OnInit so hidden sections never query their lists.
                ucNumbers.Visible = !IsFalse(ShowNumbers);
                ucFeatures.Visible = !IsFalse(ShowFeatures);
                ucAdmission.Visible = !IsFalse(ShowAdmission);
                ucBeforeArrival.Visible = !IsFalse(ShowBeforeArrival);
                ucOnArrival.Visible = !IsFalse(ShowOnArrival);
                ucOffice.Visible = !IsFalse(ShowOffice);
                ucActivities.Visible = !IsFalse(ShowActivities);
                ucExchange.Visible = !IsFalse(ShowExchange);
                ucDocuments.Visible = !IsFalse(ShowDocuments);
                ucContact.Visible = !IsFalse(ShowContact);

                //ucIntlContact contact = ucContact as ucIntlContact;
                //if (contact != null && !string.IsNullOrEmpty(ContactPageUrl))
                //{
                //    string Ar = IsArabic ? "/ar/" : "/en/";
                //    contact.ContactPageUrl = Ar + ContactPageUrl;
                //}

                
            }
            catch (Exception ex)
            {
                
            }
        }

        private static bool IsFalse(string value)
        {
            return "False".Equals(value, StringComparison.OrdinalIgnoreCase)
                || "0".Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }

}
