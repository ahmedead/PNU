using Microsoft.SharePoint;
using System;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public partial class ucHerbarium : UserControl
    {
        private string _targetWebUrl;

        public string TargetWebUrl
        {
            get { return string.IsNullOrEmpty(_targetWebUrl) ? HerbariumTargetWeb.Path : _targetWebUrl; }
            set { _targetWebUrl = value; }
        }

        public bool ShowHeader { get; set; }
        public bool ShowOverview { get; set; }
        public bool ShowMission { get; set; }
        public bool ShowServices { get; set; }
        public bool ShowMilestones { get; set; }
        public bool ShowContact { get; set; }

        public ucHerbarium()
        {
            ShowHeader = true;
            ShowOverview = true;
            ShowMission = true;
            ShowServices = true;
            ShowMilestones = true;
            ShowContact = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    HerbariumListProvisioner.EnsureAllListsExist(SPContext.Current.Site, TargetWebUrl);
                }
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("ucHerbarium.OnLoad", ex);
            }

            ApplyProperties();
        }

        private void ApplyProperties()
        {
            if (ucHeader != null)
            {
                ucHeader.Visible = ShowHeader;
                ucHeader.TargetWebUrl = TargetWebUrl;
            }
            if (ucOverview != null)
            {
                ucOverview.Visible = ShowOverview;
                ucOverview.TargetWebUrl = TargetWebUrl;
            }
            if (ucMission != null)
            {
                ucMission.Visible = ShowMission;
                ucMission.TargetWebUrl = TargetWebUrl;
            }
            if (ucServices != null)
            {
                ucServices.Visible = ShowServices;
                ucServices.TargetWebUrl = TargetWebUrl;
            }
            if (ucMilestones != null)
            {
                ucMilestones.Visible = ShowMilestones;
                ucMilestones.TargetWebUrl = TargetWebUrl;
            }
            if (ucContact != null)
            {
                ucContact.Visible = ShowContact;
                ucContact.TargetWebUrl = TargetWebUrl;
            }
        }
    }
}
