using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class ucFAQ : UserControl
    {
        // Cached in-memory data so nested repeaters can filter without new queries
        private List<FAQQuestionItem> _allQuestions = new List<FAQQuestionItem>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        var initializer = new FAQListInitializer();
                        initializer.EnsureLists(web);

                        var repo = new FAQRepository();

                        var categories = repo.GetCategories(web);
                        _allQuestions  = repo.GetQuestions(web);
                        var allAnswers = repo.GetAnswerItems(web);

                        // In-memory join: attach answer rows to their parent question.
                        repo.AttachAnswersToQuestions(_allQuestions, allAnswers);

                        if (categories != null && categories.Count != 0)
                        {
                            rptCategoriesTabs.DataSource = categories;
                            rptCategoriesTabs.DataBind();

                            rptCategoriesPanes.DataSource = categories;
                            rptCategoriesPanes.DataBind();
                        }
                    }
                }
            });
        }

        public string GetTabButtonClass(int index)
        {
            string baseCls = "nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2";
            return index == 0 ? baseCls + " active" : baseCls;
        }

        public string GetTabPaneClass(int index)
        {
            return index == 0 ? "tab-pane fade show active" : "tab-pane fade";
        }

        protected void rptCategoriesPanes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var category = (FAQCategoryItem)e.Item.DataItem;
            var inner = (Repeater)e.Item.FindControl("rptQuestions");
            if (inner == null) return;

            var repo = new FAQRepository();
            var questions = repo.GetQuestionsByCategory(_allQuestions, category.Title);

            inner.DataSource = questions;
            inner.DataBind();
        }

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var q = (FAQQuestionItem)e.Item.DataItem;
            var litAnswer = (Literal)e.Item.FindControl("litAnswer");
            if (litAnswer != null)
            {
                litAnswer.Text = RenderAnswer(q.AnswerItems);
            }
        }

        /// <summary>
        /// Renders a list of answer rows into HTML. Consecutive 'bullet' rows are
        /// grouped into a single &lt;ul&gt;; 'paragraph' and 'intro' rows break the list
        /// and render as &lt;p&gt; blocks.
        /// </summary>
        private string RenderAnswer(List<FAQAnswerItem> items)
        {
            if (items == null || items.Count == 0) return string.Empty;

            var sb = new StringBuilder();
            bool listOpen = false;

            for (int i = 0; i < items.Count; i++)
            {
                var a = items[i];
                string type = (a.ItemType ?? "").ToLowerInvariant();
                string content = a.ContentTextAr ?? string.Empty;

                if (type == FAQAnswerTypes.Bullet)
                {
                    if (!listOpen)
                    {
                        // Start a new UL. Add mb-0 if this is the tail of the answer.
                        bool isLastGroup = IsLastBulletGroup(items, i);
                        sb.Append(isLastGroup
                            ? "<ul class='mb-0 ps-3'>"
                            : "<ul class='ps-3'>");
                        listOpen = true;
                    }
                    sb.Append("<li>").Append(content).Append("</li>");
                }
                else
                {
                    // Close any open UL before rendering a non-bullet block
                    if (listOpen)
                    {
                        sb.Append("</ul>");
                        listOpen = false;
                    }

                    // Intro and paragraph both render as <p>; the last paragraph gets mb-0
                    bool isLast = (i == items.Count - 1);
                    sb.Append(isLast ? "<p class='mb-0'>" : "<p>")
                      .Append(content)
                      .Append("</p>");
                }
            }

            if (listOpen) sb.Append("</ul>");
            return sb.ToString();
        }

        /// <summary>
        /// True if there are no more bullet rows after index i, meaning this UL ends the answer.
        /// </summary>
        private bool IsLastBulletGroup(List<FAQAnswerItem> items, int startIndex)
        {
            for (int j = startIndex + 1; j < items.Count; j++)
            {
                string t = (items[j].ItemType ?? "").ToLowerInvariant();
                if (t != FAQAnswerTypes.Bullet) return false;
            }
            return true;
        }
    }
}
