using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    /// <summary>
    /// Shared base class for all Herbarium section user controls.
    /// Handles list provisioning checks, target web opening, and exception handling.
    /// </summary>
    public abstract class HerbariumSectionBase : UserControl
    {
        private string _targetWebUrl;

        /// <summary>
        /// Optional override for target web URL.
        /// Defaults to HerbariumTargetWeb.Path (/ar/Faculties/SC).
        /// </summary>
        public string TargetWebUrl
        {
            get { return string.IsNullOrEmpty(_targetWebUrl) ? HerbariumTargetWeb.Path : _targetWebUrl; }
            set { _targetWebUrl = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureListsProvisioned();
            if (!IsPostBack)
            {
                BindDataSafe();
            }
        }

        protected void EnsureListsProvisioned()
        {
            try
            {
                if (SPContext.Current != null && SPContext.Current.Site != null)
                {
                    HerbariumListProvisioner.EnsureAllListsExist(SPContext.Current.Site, TargetWebUrl);
                }
            }
            catch (Exception ex)
            {
                HerbariumLog.Write("HerbariumSectionBase.EnsureListsProvisioned", ex);
            }
        }

        private void BindDataSafe()
        {
            try
            {
                SPWeb contextWeb = SPContext.Current != null ? SPContext.Current.Web : null;
                if (contextWeb == null) return;

                using (SPWeb web = HerbariumTargetWeb.Open(contextWeb.Site))
                {
                    if (web == null) return;
                    BindSectionData(web);
                }
            }
            catch (Exception ex)
            {
                HerbariumLog.Write(GetType().Name + ".BindDataSafe", ex);
            }
        }

        protected abstract void BindSectionData(SPWeb web);
    }
}
