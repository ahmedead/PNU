using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA
{
    /// <summary>
    /// Faculty training (التدريب في الكلية) - DGA design.
    /// Intro body (FacultyTraining) rendered as an accordion item whose panel also
    /// shows the coordinators table (FacultyTrainingCoords). Body stored plain.
    /// </summary>
    public partial class ucFacultyTrainingDga : UserControl
    {
        private bool IsArabic { get { return SPContext.Current.Web.Language == 1025; } }

        public class CoordDto
        {
            public string Program { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public bool HasEmail { get; set; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current != null
                && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                TrainingProvisioner.EnsureLists();
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetTitles();
                if (!IsPostBack)
                {
                    SPWeb web = SPContext.Current.Web;
                    BindIntro(web);
                    BindCoords(web);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private void SetTitles()
        {
            //ltrHeading.Text = IsArabic ? "التدريب في الكلية" : "Training in the Faculty";
            //ltrIntro.Text = IsArabic
            //    ? "معلومات التدريب التعاوني ولجنة التدريب ومنسقي التدريب في أقسام الكلية."
            //    : "Cooperative training information, the training committee, and department coordinators.";
            ltrItemTitle.Text = IsArabic ? "التدريب التعاوني" : "Cooperative Training";
            ltrCoordsTitle.Text = IsArabic ? "منسقو التدريب في الأقسام" : "Training Coordinators by Department";
            ltrThProgram.Text = IsArabic ? "البرنامج" : "Program";
            ltrThName.Text = IsArabic ? "المنسق/ة" : "Coordinator";
            ltrThEmail.Text = IsArabic ? "البريد الإلكتروني" : "Email";
        }

        private void BindIntro(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(TrainingProvisioner.IntroListName);
            if (list == null) { rptBody.Visible = false; return; }

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);
            SPListItem first = sorted.OrderBy(x => SafeDouble(x, "SortOrder")).FirstOrDefault();
            if (first == null) { rptBody.Visible = false; return; }

            ltrItemTitle.Text = DisplayName(first, "Title", "TitleEn");
            var blocks = FacultyProvisioningHelper.ParseBody(
                IsArabic ? SafeString(first, "BodyAr") : Fallback(SafeString(first, "BodyEn"), SafeString(first, "BodyAr")));
            rptBody.DataSource = blocks;
            rptBody.DataBind();
        }

        private void BindCoords(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(TrainingProvisioner.CoordsListName);
            if (list == null) { phCoords.Visible = false; return; }

            var sorted = new List<SPListItem>();
            foreach (SPListItem it in list.Items) sorted.Add(it);

            var dtos = new List<CoordDto>();
            foreach (SPListItem it in sorted.OrderBy(x => SafeDouble(x, "SortOrder")))
            {
                string email = SafeString(it, "Email");
                dtos.Add(new CoordDto
                {
                    Program = DisplayName(it, "Title", "ProgramEn"),
                    Name = SafeString(it, "CoordName"),
                    Email = email,
                    HasEmail = !string.IsNullOrEmpty(email) && email != "-"
                });
            }

            phCoords.Visible = dtos.Count > 0;
            rptCoords.DataSource = dtos;
            rptCoords.DataBind();
        }

        // ---- helpers ----
        private string DisplayName(SPListItem item, string arField, string enField)
        {
            string ar = SafeString(item, arField);
            string en = SafeString(item, enField);
            return IsArabic ? (!string.IsNullOrEmpty(ar) ? ar : en) : (!string.IsNullOrEmpty(en) ? en : ar);
        }

        private static string Fallback(string primary, string secondary)
        {
            return string.IsNullOrEmpty(primary) ? secondary : primary;
        }

        private static string SafeString(SPListItem item, string field)
        {
            try { return item.Fields.ContainsField(field) && item[field] != null ? item[field].ToString() : string.Empty; }
            catch { return string.Empty; }
        }

        private static double SafeDouble(SPListItem item, string field)
        {
            try
            {
                double v;
                return item.Fields.ContainsField(field) && item[field] != null && double.TryParse(item[field].ToString(), out v) ? v : 0;
            }
            catch { return 0; }
        }
    }
}
