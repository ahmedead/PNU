using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Controls
{
    public partial class ucPgCategoryPrograms : UserControl
    {
        private string _programCategoryString;
        private PgProgramCategory _programCategory = PgProgramCategory.NouraStaff;
        private bool _isCategorySetExplicitly = false;
        private bool _showHero = true;
        private bool _showBanner = true;
        private bool _showQuickCards = true;
        private PgCategoryRepository _repository;

        public string ProgramCategory
        {
            get { return _programCategoryString; }
            set
            {
                _programCategoryString = value;
                _programCategory = PgCategoryConstants.ParseCategory(value);
                _isCategorySetExplicitly = true;
            }
        }

        public PgProgramCategory CategoryEnum
        {
            get { return _programCategory; }
            set
            {
                _programCategory = value;
                _programCategoryString = value.ToString();
                _isCategorySetExplicitly = true;
            }
        }

        public bool ShowHero
        {
            get { return _showHero; }
            set { _showHero = value; }
        }

        public bool ShowBanner
        {
            get { return _showBanner; }
            set { _showBanner = value; }
        }

        public bool ShowQuickCards
        {
            get { return _showQuickCards; }
            set { _showQuickCards = value; }
        }

        public bool IsArabic
        {
            get
            {
                if (_repository != null) return _repository.IsArabic;
                try
                {
                    return SPContext.Current != null && SPContext.Current.Web != null
                        ? SPContext.Current.Web.Language == 1025
                        : true;
                }
                catch { return true; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // OutputCache fix for QueryString parameters
            Response.Cache.SetNoServerCaching();
            Response.Cache.VaryByParams["Cat"] = true;
            Response.Cache.VaryByParams["Category"] = true;
            _repository = new PgCategoryRepository(SPContext.Current != null ? SPContext.Current.Web : null);

            if (!_isCategorySetExplicitly)
            {
                ResolveCategoryFromContext();
            }

            if (!IsPostBack)
            {
                try
                {
                    PgCategoryProvisioner.EnsureListsOnCurrentWeb();
                }
                catch { }

                BindCategoryData();
            }
        }

        private void ResolveCategoryFromContext()
        {
            // 1. Check QueryString ?Category=... or ?Cat=...
            string qCat = Request.QueryString["Category"] ?? Request.QueryString["Cat"];
            if (!string.IsNullOrEmpty(qCat))
            {
                _programCategory = PgCategoryConstants.ParseCategory(qCat);
                _programCategoryString = _programCategory.ToString();
                return;
            }

            // 2. Check URL page name
            string path = Request.Url.AbsolutePath.ToLowerInvariant();
            if (path.Contains("pnur.aspx"))
            {
                _programCategory = PgProgramCategory.NouraStaff;
            }
            else if (path.Contains("humanitarianpror.aspx") || path.Contains("humanitarian"))
            {
                _programCategory = PgProgramCategory.Humanitarian;
            }
            else if (path.Contains("interfaceprogr.aspx") || path.Contains("interface"))
            {
                _programCategory = PgProgramCategory.Interdisciplinary;
            }
            else if (path.Contains("sciencesprogr.aspx") || path.Contains("sciences"))
            {
                _programCategory = PgProgramCategory.Scientific;
            }
            else if (path.Contains("healthprogr1.aspx") || path.Contains("healthprogr.aspx") || path.Contains("health"))
            {
                _programCategory = PgProgramCategory.Health;
            }
            else
            {
                _programCategory = PgProgramCategory.NouraStaff;
            }

            _programCategoryString = _programCategory.ToString();
        }

        private void BindCategoryData()
        {
            bool isAr = IsArabic;
            litDir.Text = isAr ? "rtl" : "ltr";
            litLang.Text = isAr ? "ar-SA" : "en-US";
            litUnivName.Text = isAr ? "جامعة الأميرة نورة بنت عبدالرحمن" : "Princess Nourah bint Abdulrahman University";

            string arrowClass = isAr ? "hgi-arrow-left-02" : "hgi-arrow-right-02";
            litArrowIcon1.Text = arrowClass;
            litArrowIcon2.Text = arrowClass;

            pnlHero.Visible = _showHero;
            pnlBanner.Visible = _showBanner;
            pnlQuickCards.Visible = _showQuickCards;

            // 1. Bind Overview
            var overview = _repository.GetOverview(_programCategory);
            if (overview != null)
            {
                litHeroSubTag.Text = overview.GetHeroSubTag(isAr);
                litHeroTag.Text = overview.GetHeroTag(isAr);
                litHeroTitle.Text = overview.GetHeroTitle(isAr);
                litHeroDesc.Text = overview.GetHeroDescription(isAr);

                litBannerTitle.Text = overview.GetBannerTitle(isAr);
                litBannerDesc.Text = overview.GetBannerDescription(isAr);
                litBannerBtnText.Text = overview.GetBannerButtonText(isAr);
                btnBannerApply.HRef = !string.IsNullOrEmpty(overview.BannerUrl) ? overview.BannerUrl : PgCategorySeedData.DefaultBannerUrl;
                btnBannerApply.Attributes["aria-label"] = isAr ? "التقديم عبر بوابة القبول الموحدة (يفتح في نافذة جديدة)" : "Apply via Electronic Admission Portal (opens in new window)";

                litCard1Title.Text = overview.GetCard1Title(isAr);
                litCard1Desc.Text = overview.GetCard1Desc(isAr);
                lnkCard1.HRef = !string.IsNullOrEmpty(overview.Card1Url) ? overview.Card1Url : PgCategorySeedData.DefaultReqUrl;
                lnkCard1.Attributes["aria-label"] = overview.GetCard1Title(isAr);

                litCard2Title.Text = overview.GetCard2Title(isAr);
                litCard2Desc.Text = overview.GetCard2Desc(isAr);
                lnkCard2.HRef = !string.IsNullOrEmpty(overview.Card2Url) ? overview.Card2Url : PgCategorySeedData.DefaultScheduleUrl;
                lnkCard2.Attributes["aria-label"] = overview.GetCard2Title(isAr);
            }

            // 2. Bind Program Groups
            var groups = _repository.GetGroupedPrograms(_programCategory, true);
            rptDegreeGroups.DataSource = groups;
            rptDegreeGroups.DataBind();
        }

        protected void rptDegreeGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var groupModel = e.Item.DataItem as PgDegreeGroupViewModel;
                if (groupModel != null)
                {
                    Repeater rptPrograms = e.Item.FindControl("rptPrograms") as Repeater;
                    if (rptPrograms != null)
                    {
                        rptPrograms.DataSource = groupModel.Programs;
                        rptPrograms.DataBind();
                    }
                }
            }
        }
    }
}
