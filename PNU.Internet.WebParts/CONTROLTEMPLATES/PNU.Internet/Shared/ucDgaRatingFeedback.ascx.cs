using Microsoft.SharePoint;
using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared
{
    /// <summary>
    /// Reusable DGA rating feedback block ("قيم هذه الخدمة").
    /// Drop on any page; ratings are stored in the "PageRatings" list keyed by page URL.
    /// </summary>
    public partial class ucDgaRatingFeedback : UserControl
    {
        private const string ListName = "PageRatings";
        private const string ResourceFile = "PNUres";

        /// <summary>Optional override; defaults to current page URL + query string.</summary>
        public string PageKey { get; set; }

        protected bool IsArabic { get { return SPContext.Current.Web.Language == 1025; } }

        protected int RatingsCount { get; private set; }
        protected string AverageDisplay { get; private set; }

        private string ResolvedKey
        {
            get { return string.IsNullOrEmpty(PageKey) ? Request.Url.PathAndQuery : PageKey; }
        }

        // ------------------------------------------------------------
        // Resource helper: PNUres first, hard-coded bilingual fallback
        // ------------------------------------------------------------
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
            if (!IsPostBack)
            {
                btnSubmitRating.Attributes["disabled"] = "disabled";
            }
            LoadStats();
        }

        private void SetTitles()
        {
            rateServiceBtn.InnerText = GetRes("res_RateThisService", "قيم هذه الخدمة", "Rate this service");
            rateServiceBtn.Attributes["aria-label"] = rateServiceBtn.InnerText;

            litThankYou.Text = GetRes("res_RatingThankYou", "شكرًا لك، تم تسجيل تقييمك بنجاح.", "Thank you, your rating has been submitted.");
            litTellUs.Text = GetRes("res_RatingTellUs", "أخبرنا عن رأيك في هذه الخدمة", "Tell us what you think about this service");
            litPrivacy.Text = GetRes("res_RatingPrivacyNote",
                "يرجى عدم تضمين معلومات شخصية أو مالية. سيتم إرسال تعليقك وتسجيله لغرض تحسين الخدمات.",
                "Please do not include personal or financial information. Your feedback will be recorded to improve services.");
            litHowRate.Text = GetRes("res_RatingHowRate", "كيف تقيم هذه الخدمة؟", "How do you rate this service?");
            litRateScale.Text = GetRes("res_RatingScale", "قيم تجربتك من (1) ضعيف إلى (5) ممتاز", "Rate your experience from (1) poor to (5) excellent");
            lblRatingComment.Text = GetRes("res_RatingCommentLabel", "اخبرنا عن تجربتك في هذه الخدمة", "Tell us about your experience");

            btnCancelRating.InnerText = GetRes("res_Cancel", "إلغاء", "Cancel");
            btnCancelRating.Attributes["aria-label"] = btnCancelRating.InnerText;
            btnSubmitRating.Text = GetRes("res_Submit", "إرسال", "Submit");
            btnSubmitRating.Attributes["aria-label"] = btnSubmitRating.Text;

            litFormStars.Text = BuildFormStars();
        }

        private void LoadStats()
        {
            double avg = 0;
            int count = 0;
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
                        query.ViewFields = "<FieldRef Name='RatingValue' />";
                        query.ViewFieldsOnly = true;

                        double total = 0;
                        foreach (SPListItem item in list.GetItems(query))
                        {
                            if (item["RatingValue"] == null) continue;
                            total += Convert.ToDouble(item["RatingValue"]);
                            count++;
                        }
                        if (count > 0) avg = total / count;
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucDgaRatingFeedback.LoadStats", ex.Message);
            }

            RatingsCount = count;
            AverageDisplay = count > 0 ? avg.ToString("0.0") : "0.0";

            // "تم تقييم هذه الخدمة بمتوسط 2.8"  — {0} = average
            string avgTemplate = GetRes("res_RatedAverage",
                "تم تقييم هذه الخدمة بمتوسط <span class=\"fw-semibold\">{0}</span>",
                "This service has an average rating of <span class=\"fw-semibold\">{0}</span>");
            litAvgLine.Text = string.Format(avgTemplate, AverageDisplay);

            // "12 تقييم" — {0} = count
            string countTemplate = GetRes("res_RatingsCount", "{0} تقييم", "{0} ratings");
            litCountLine.Text = string.Format(countTemplate, RatingsCount);

            litAvgStars.Text = BuildAverageStars(avg);
        }

        private const string StarPath =
            "M15.0919 1.96866C15.4494 1.19379 16.5506 1.19379 16.9081 1.96866L20.2797 9.27846C20.4254 9.59426 20.7247 9.81171 21.0701 9.85266L29.064 10.8005C29.9114 10.9009 30.2517 11.9483 29.6252 12.5277L23.7151 17.9932C23.4597 18.2293 23.3454 18.5812 23.4132 18.9223L24.982 26.8178C25.1483 27.6548 24.2574 28.3021 23.5128 27.8853L16.4884 23.9534C16.185 23.7835 15.815 23.7835 15.5116 23.9534L8.48722 27.8853C7.74261 28.3021 6.85165 27.6548 7.01795 26.8178L8.5868 18.9223C8.65458 18.5812 8.54026 18.2293 8.28492 17.9932L2.3748 12.5277C1.7483 11.9483 2.08862 10.9009 2.93601 10.8005L10.9299 9.85266C11.2753 9.81171 11.5746 9.59426 11.7203 9.27846L15.0919 1.96866Z";

        private string BuildAverageStars(double avg)
        {
            StringBuilder sb = new StringBuilder();
            int filled = (int)Math.Round(avg, MidpointRounding.AwayFromZero);
            for (int i = 1; i <= 5; i++)
            {
                sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"32\" height=\"32\" viewBox=\"0 0 32 32\" fill=\"none\">")
                  .Append("<path d=\"").Append(StarPath).Append("\" fill=\"")
                  .Append(i <= filled ? "#1B8354" : "#E5E7EB")
                  .Append("\"></path></svg>");
            }
            return sb.ToString();
        }

        private string BuildFormStars()
        {
            // "قيم بـ {0} نجمه" — {0} = star value
            string ariaTemplate = GetRes("res_RateWithStars", "قيم بـ {0} نجمه", "Rate with {0} stars");
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 5; i++)
            {
                sb.Append("<a href=\"#\" role=\"button\" class=\"dga-rating-icon d-grid place-items-center\" data-value=\"")
                  .Append(i).Append("\" aria-label=\"")
                  .Append(HttpUtility.HtmlAttributeEncode(string.Format(ariaTemplate, i)))
                  .Append("\"><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"32\" height=\"32\" viewBox=\"0 0 32 32\" fill=\"none\"><path d=\"")
                  .Append(StarPath)
                  .Append("\"></path></svg></a>");
            }
            return sb.ToString();
        }

        protected void btnSubmitRating_Click(object sender, EventArgs e)
        {
            try
            {
                int rating;
                if (!int.TryParse(hfRatingValue.Value, out rating) || rating < 1 || rating > 5) return;

                string comment = txtRatingComment.Text;
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
                        item["Title"] = "Rating " + rating;
                        item["PageUrl"] = key;
                        item["RatingValue"] = rating;
                        item["Comment"] = comment;
                        item.Update();
                        web.AllowUnsafeUpdates = false;
                    }
                });

                pnlThanks.Visible = true;
                txtRatingComment.Text = string.Empty;
                hfRatingValue.Value = "0";
                LoadStats(); // refresh average after new vote
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "ucDgaRatingFeedback.Submit", ex.Message);
            }
        }
    }

}
