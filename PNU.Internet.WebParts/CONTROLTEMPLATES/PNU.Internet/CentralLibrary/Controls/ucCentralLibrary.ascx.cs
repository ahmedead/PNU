using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls
{
    public partial class ucCentralLibrary : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowHeaderSection { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowAbout { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowAwards { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowServices { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowCollections { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowFacilities { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowFaq { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowContact { get; set; }

        public ucCentralLibrary()
        {
            ShowHeaderSection = true;
            ShowAbout = true;
            ShowAwards = true;
            ShowServices = true;
            ShowCollections = true;
            ShowFacilities = true;
            ShowFaq = true;
            ShowContact = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClListProvisioner.EnsureAllListsExist();

                if (phHeader != null) phHeader.Visible = ShowHeaderSection;
                if (phAbout != null) phAbout.Visible = ShowAbout;
                if (phAwards != null) phAwards.Visible = ShowAwards;
                if (phServices != null) phServices.Visible = ShowServices;
                if (phCollections != null) phCollections.Visible = ShowCollections;
                if (phFacilities != null) phFacilities.Visible = ShowFacilities;
                if (phFaq != null) phFaq.Visible = ShowFaq;
                if (phContact != null) phContact.Visible = ShowContact;
            }
            catch (Exception ex)
            {
                ClLog.Write("ucCentralLibrary.Page_Load", ex);
            }
        }
    }
}
