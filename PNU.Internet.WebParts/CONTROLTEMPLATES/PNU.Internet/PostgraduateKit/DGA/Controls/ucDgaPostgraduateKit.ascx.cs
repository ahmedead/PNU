using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure;
using System;
using System.Web;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Controls
{
    public partial class ucDgaPostgraduateKit : DgaPgSectionBase
    {
        public override string Phase { get; set; } = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteSafely("RoutePhase", RoutePhase);
        }

        private void RoutePhase()
        {
            string targetPhase = Phase;
            if (string.IsNullOrEmpty(targetPhase))
            {
                targetPhase = DetectPhaseFromUrl();
            }

            phHub.Visible = false;
            phApplicants.Visible = false;
            phEnrolled.Visible = false;
            phGraduates.Visible = false;

            if (targetPhase.Equals(DgaPgPhases.Applicants, StringComparison.OrdinalIgnoreCase))
            {
                phApplicants.Visible = true;
            }
            else if (targetPhase.Equals(DgaPgPhases.Enrolled, StringComparison.OrdinalIgnoreCase))
            {
                phEnrolled.Visible = true;
            }
            else if (targetPhase.Equals(DgaPgPhases.Graduates, StringComparison.OrdinalIgnoreCase))
            {
                phGraduates.Visible = true;
            }
            else
            {
                phHub.Visible = true;
            }
        }

        private string DetectPhaseFromUrl()
        {
            try
            {
                string url = HttpContext.Current?.Request?.RawUrl?.ToLowerInvariant() ?? string.Empty;
                if (url.Contains("applicant"))
                    return DgaPgPhases.Applicants;
                if (url.Contains("enrolled") || url.Contains("student"))
                    return DgaPgPhases.Enrolled;
                if (url.Contains("graduate"))
                    return DgaPgPhases.Graduates;
            }
            catch { }

            return DgaPgPhases.Hub;
        }
    }
}
