using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared
{
    /// <summary>
    /// Reusable DGA "هل كانت هذه الصفحة مفيدة؟" survey block.
    /// Labels come from the PNUres resource file (safe fallback to built-in
    /// bilingual defaults when a key is missing).
    /// Answers are stored in the "PageSurveys" list keyed by page URL.
    /// </summary>
    public partial class ucDgaSurveyFeedback : UserControl
    {
        private const string ListName = "PageSurveys";
        private const string ResourceFile = "PNUres";

        /// <summary>Optional override; defaults to current page URL + query string.</summary>
        public string PageKey { get; set; }

        protected bool IsArabic { get { return SPContext.Current.Web.Language == 1025; } }

        protected int YesPercent { get; private set; }
        protected int SurveyCount { get; private set; }

        private string ResolvedKey
        {
            get { return string.IsNullOrEmpty(PageKey) ? Request.Url.PathAndQuery : PageKey; }
        }

        private string GetRes(string key, string fallbackAr, string fallbackEn)
        {
            try
            {
                CultureInfo culture = IsArabic ? new CultureInfo("ar-SA") : new CultureInfo("en-US");
                object val = HttpContext.GetGlobalResourceObject(ResourceFile, key, culture);
                if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    return val.ToString();
            }
            catch { /* missing key / file — use fallback */ }
            return IsArabic ? fallbackAr : fallbackEn;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetTitles();
            LoadStats();
        }

        private void SetTitles()
        {
            litWasHelpful.Text = GetRes("res_SurveyWasHelpful", "هل كانت هذه الصفحة مفيدة؟", "Was this page helpful?");
            litThankYou.Text = GetRes("res_SurveyThankYou", "شكرًا لك، تم إرسال ملاحظاتك بنجاح.", "Thank you, your feedback has been submitted.");

            btnYes.InnerText = GetRes("res_Yes", "نعم", "Yes");
            btnYes.Attributes["aria-label"] = btnYes.InnerText;
            btnNo.InnerText = GetRes("res_No", "لا", "No");
            btnNo.Attributes["aria-label"] = btnNo.InnerText;

            // "من فضلك أخبرنا بالسبب * (يمكنك تحديد خيارات متعددة)"
            litTellUsWhy.Text =
                GetRes("res_SurveyTellUsWhy", "من فضلك أخبرنا بالسبب", "Please tell us why") +
                "<span class=\"text-danger\">*</span><span class=\"small fw-normal text-muted mx-1\">(" +
                GetRes("res_SurveyMultiSelect", "يمكنك تحديد خيارات متعددة", "You can select multiple options") + ")</span>";

            // CheckBox.Text renders <label for>; add the DGA classes on input + label
            SetCheck(chkRelevant, GetRes("res_SurveyReasonRelevant", "المحتوى ذو صلة", "Content is relevant"));
            SetCheck(chkWellWritten, GetRes("res_SurveyReasonWellWritten", "لقد كانت مكتوبة بشكل جيد", "It was well written"));
            SetCheck(chkLayout, GetRes("res_SurveyReasonLayout", "جعل التخطيط من السهل القراءة", "The layout made it easy to read"));
            SetCheck(chkOther, GetRes("res_SurveyReasonOther", "شيء آخر", "Something else"));

            lblSurveyComment.Text = GetRes("res_Comment", "تعليق", "Comment");
            litIAm.Text = GetRes("res_SurveyIAm", "انا", "I am");

            SetRadio(rdMale, GetRes("res_Male", "ذكر", "Male"));
            SetRadio(rdFemale, GetRes("res_Female", "أنثى", "Female"));
            SetRadio(rdNoSay, GetRes("res_PreferNotToSay", "أفضل عدم القول", "Prefer not to say"));

            btnCancelSurvey.InnerText = GetRes("res_Cancel", "إلغاء", "Cancel");
            btnCancelSurvey.Attributes["aria-label"] = btnCancelSurvey.InnerText;
            btnSubmitSurvey.Text = GetRes("res_Submit", "إرسال", "Submit");
            btnSubmitSurvey.Attributes["aria-label"] = btnSubmitSurvey.Text;
        }

        private void SetCheck(CheckBox chk, string text)
        {
            chk.Text = text;
            chk.InputAttributes["class"] = "form-check-input";
            chk.LabelAttributes["class"] = "form-check-label";
        }

        private void SetRadio(RadioButton rd, string text)
        {
            rd.Text = text;
            rd.InputAttributes["class"] = "form-check-input";
            rd.LabelAttributes["class"] = "form-check-label";
        }

        private void LoadStats()
        {
            int yes = 0, total = 0;
            try
            {
                string key = ResolvedKey;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList(ListName);
                        if (list == null) return;

                        SPQuery query = new SPQuery();
                        query.Query = "<Where><Eq><FieldRef Name='PageUrl' /><Value Type='Text'>" +
                                      System.Security.SecurityElement.Escape(key) + "</Value></Eq></Where>";
                        query.ViewFields = "<FieldRef Name='IsHelpful' />";
                        query.ViewFieldsOnly = true;

                        foreach (SPListItem item in list.GetItems(query))
                        {
                            total++;
                            if (item["IsHelpful"] != null && Convert.ToBoolean(item["IsHelpful"])) yes++;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucDgaSurveyFeedback.LoadStats", ex.Message);
            }

            SurveyCount = total;
            YesPercent = total > 0 ? (int)Math.Round(yes * 100.0 / total) : 0;

            // "75% من المستخدمين قالوا نعم من 3 تعليقًا" — {0} = percent, {1} = count
            string statsTemplate = GetRes("res_SurveyStats",
                "<span class=\"fw-semibold\">{0}%</span> من المستخدمين قالوا نعم من <span class=\"fw-semibold\">{1}</span> تعليقًا",
                "<span class=\"fw-semibold\">{0}%</span> of users said yes from <span class=\"fw-semibold\">{1}</span> responses");
            litStatsLine.Text = string.Format(statsTemplate, YesPercent, SurveyCount);
        }

        protected void btnSubmitSurvey_Click(object sender, EventArgs e)
        {
            try
            {
                bool isHelpful = hfIsHelpful.Value == "1";

                List<string> reasons = new List<string>();
                if (chkRelevant.Checked) reasons.Add(IsArabic ? "المحتوى ذو صلة" : "Content is relevant");
                if (chkWellWritten.Checked) reasons.Add(IsArabic ? "مكتوبة بشكل جيد" : "Well written");
                if (chkLayout.Checked) reasons.Add(IsArabic ? "التخطيط سهل القراءة" : "Easy-to-read layout");
                if (chkOther.Checked) reasons.Add(IsArabic ? "شيء آخر" : "Something else");

                string gender = rdMale.Checked ? "Male" : rdFemale.Checked ? "Female" : rdNoSay.Checked ? "PreferNotToSay" : "";

                string comment = txtSurveyComment.Text;
                if (!string.IsNullOrEmpty(comment) && comment.Length > 500)
                    comment = comment.Substring(0, 500);

                string key = ResolvedKey;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList(ListName);
                        if (list == null) return;

                        web.AllowUnsafeUpdates = true;
                        SPListItem item = list.AddItem();
                        item["Title"] = isHelpful ? "Yes" : "No";
                        item["PageUrl"] = key;
                        item["IsHelpful"] = isHelpful;
                        item["Reasons"] = string.Join("; ", reasons);
                        item["Comment"] = comment;
                        item["Gender"] = gender;
                        item.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                });

                pnlThanks.Visible = true;
                txtSurveyComment.Text = string.Empty;
                chkRelevant.Checked = chkWellWritten.Checked = chkLayout.Checked = chkOther.Checked = false;
                rdMale.Checked = rdFemale.Checked = rdNoSay.Checked = false;
                LoadStats();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucDgaSurveyFeedback.Submit", ex.Message);
            }
        }
    }

}
