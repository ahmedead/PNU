using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.Infrastructure
{
    public enum PgProgramCategory
    {
        All = 0,
        NouraStaff = 1,        // كوادر نورة
        Humanitarian = 2,      // البرامج الإنسانية
        Interdisciplinary = 3, // البرامج البينية
        Scientific = 4,        // البرامج العلمية
        Health = 5             // البرامج الصحية
    }

    public static class PgCategoryConstants
    {
        public const string ListCategoryOverview = "PgCategoryOverview";
        public const string ListCategoryPrograms = "PgCategoryPrograms";
        public const string ListProgramDetails = "PgProgramDetails";

        public static string GetCategoryDisplayNameAr(PgProgramCategory cat)
        {
            switch (cat)
            {
                case PgProgramCategory.NouraStaff: return "كوادر نورة";
                case PgProgramCategory.Humanitarian: return "البرامج الإنسانية";
                case PgProgramCategory.Interdisciplinary: return "البرامج البينية";
                case PgProgramCategory.Scientific: return "البرامج العلمية";
                case PgProgramCategory.Health: return "البرامج الصحية";
                default: return "جميع البرامج";
            }
        }

        public static string GetCategoryDisplayNameEn(PgProgramCategory cat)
        {
            switch (cat)
            {
                case PgProgramCategory.NouraStaff: return "Nourah Cadres";
                case PgProgramCategory.Humanitarian: return "Humanitarian Programs";
                case PgProgramCategory.Interdisciplinary: return "Interdisciplinary Programs";
                case PgProgramCategory.Scientific: return "Scientific Programs";
                case PgProgramCategory.Health: return "Health Programs";
                default: return "All Programs";
            }
        }

        public static PgProgramCategory ParseCategory(string value)
        {
            if (string.IsNullOrEmpty(value)) return PgProgramCategory.NouraStaff;

            string normalized = value.Trim().ToLowerInvariant();

            if (normalized == "nourastaff" || normalized == "pnur" || normalized.Contains("كوادر"))
                return PgProgramCategory.NouraStaff;
            if (normalized == "humanitarian" || normalized == "humanitarianpror" || normalized.Contains("إنسانية") || normalized.Contains("انسانية"))
                return PgProgramCategory.Humanitarian;
            if (normalized == "interdisciplinary" || normalized == "interfaceprogr" || normalized.Contains("بينية"))
                return PgProgramCategory.Interdisciplinary;
            if (normalized == "scientific" || normalized == "sciencesprogr" || normalized.Contains("علمية") || normalized.Contains("علميه"))
                return PgProgramCategory.Scientific;
            if (normalized == "health" || normalized == "healthprogr" || normalized == "healthprogr1" || normalized.Contains("صحية") || normalized.Contains("صحيه"))
                return PgProgramCategory.Health;
            if (normalized == "all" || normalized.Contains("الكل") || normalized.Contains("جميع"))
                return PgProgramCategory.All;

            PgProgramCategory result;
            if (Enum.TryParse(value, true, out result))
                return result;

            return PgProgramCategory.NouraStaff;
        }
    }

    public class PgCategoryOverviewModel
    {
        public int Id { get; set; }
        public string CategoryKey { get; set; }
        public string HeroTagAr { get; set; }
        public string HeroTagEn { get; set; }
        public string HeroSubTagAr { get; set; }
        public string HeroSubTagEn { get; set; }
        public string HeroTitleAr { get; set; }
        public string HeroTitleEn { get; set; }
        public string HeroDescriptionAr { get; set; }
        public string HeroDescriptionEn { get; set; }

        public string BannerTitleAr { get; set; }
        public string BannerTitleEn { get; set; }
        public string BannerDescriptionAr { get; set; }
        public string BannerDescriptionEn { get; set; }
        public string BannerButtonTextAr { get; set; }
        public string BannerButtonTextEn { get; set; }
        public string BannerUrl { get; set; }

        public string Card1TitleAr { get; set; }
        public string Card1TitleEn { get; set; }
        public string Card1DescAr { get; set; }
        public string Card1DescEn { get; set; }
        public string Card1Url { get; set; }

        public string Card2TitleAr { get; set; }
        public string Card2TitleEn { get; set; }
        public string Card2DescAr { get; set; }
        public string Card2DescEn { get; set; }
        public string Card2Url { get; set; }

        public bool IsActive { get; set; }

        public string GetHeroTag(bool isAr) { return isAr ? HeroTagAr : (string.IsNullOrEmpty(HeroTagEn) ? HeroTagAr : HeroTagEn); }
        public string GetHeroSubTag(bool isAr) { return isAr ? HeroSubTagAr : (string.IsNullOrEmpty(HeroSubTagEn) ? HeroSubTagAr : HeroSubTagEn); }
        public string GetHeroTitle(bool isAr) { return isAr ? HeroTitleAr : (string.IsNullOrEmpty(HeroTitleEn) ? HeroTitleAr : HeroTitleEn); }
        public string GetHeroDescription(bool isAr) { return isAr ? HeroDescriptionAr : (string.IsNullOrEmpty(HeroDescriptionEn) ? HeroDescriptionAr : HeroDescriptionEn); }

        public string GetBannerTitle(bool isAr) { return isAr ? BannerTitleAr : (string.IsNullOrEmpty(BannerTitleEn) ? BannerTitleAr : BannerTitleEn); }
        public string GetBannerDescription(bool isAr) { return isAr ? BannerDescriptionAr : (string.IsNullOrEmpty(BannerDescriptionEn) ? BannerDescriptionAr : BannerDescriptionEn); }
        public string GetBannerButtonText(bool isAr) { return isAr ? BannerButtonTextAr : (string.IsNullOrEmpty(BannerButtonTextEn) ? BannerButtonTextAr : BannerButtonTextEn); }

        public string GetCard1Title(bool isAr) { return isAr ? Card1TitleAr : (string.IsNullOrEmpty(Card1TitleEn) ? Card1TitleAr : Card1TitleEn); }
        public string GetCard1Desc(bool isAr) { return isAr ? Card1DescAr : (string.IsNullOrEmpty(Card1DescEn) ? Card1DescAr : Card1DescEn); }

        public string GetCard2Title(bool isAr) { return isAr ? Card2TitleAr : (string.IsNullOrEmpty(Card2TitleEn) ? Card2TitleAr : Card2TitleEn); }
        public string GetCard2Desc(bool isAr) { return isAr ? Card2DescAr : (string.IsNullOrEmpty(Card2DescEn) ? Card2DescAr : Card2DescEn); }
    }

    public class PgProgramItemModel
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string CategoryKey { get; set; }
        public string DegreeGroupKey { get; set; }
        public string DegreeGroupTitleAr { get; set; }
        public string DegreeGroupTitleEn { get; set; }
        public string CollegeAr { get; set; }
        public string CollegeEn { get; set; }
        public string DepartmentAr { get; set; }
        public string DepartmentEn { get; set; }
        public string ScopeAr { get; set; }
        public string ScopeEn { get; set; }
        public string DetailsUrl { get; set; }
        public string ApplyUrl { get; set; }
        public int ItemOrder { get; set; }
        public bool IsActive { get; set; }

        public string GetTitle(bool isAr) { return isAr ? TitleAr : (string.IsNullOrEmpty(TitleEn) ? TitleAr : TitleEn); }
        public string GetCollege(bool isAr) { return isAr ? CollegeAr : (string.IsNullOrEmpty(CollegeEn) ? CollegeAr : CollegeEn); }
        public string GetDepartment(bool isAr) { return isAr ? DepartmentAr : (string.IsNullOrEmpty(DepartmentEn) ? DepartmentAr : DepartmentEn); }
        public string GetScope(bool isAr) { return isAr ? ScopeAr : (string.IsNullOrEmpty(ScopeEn) ? ScopeAr : ScopeEn); }
        public string GetDegreeGroupTitle(bool isAr) { return isAr ? DegreeGroupTitleAr : (string.IsNullOrEmpty(DegreeGroupTitleEn) ? DegreeGroupTitleAr : DegreeGroupTitleEn); }

        public bool IsInternational
        {
            get
            {
                if (string.IsNullOrEmpty(ScopeAr) && string.IsNullOrEmpty(ScopeEn)) return false;
                string s = (ScopeAr + " " + ScopeEn).ToLowerInvariant();
                return s.Contains("عالمي") || s.Contains("international") || s.Contains("شراكة");
            }
        }
    }

    public class PgDegreeGroupViewModel
    {
        public string GroupKey { get; set; }
        public string GroupTitle { get; set; }
        public string IconClass { get; set; }
        public List<PgProgramItemModel> Programs { get; set; }

        public int Count
        {
            get { return Programs != null ? Programs.Count : 0; }
        }

        public string CountLabel(bool isAr)
        {
            int c = Count;
            if (isAr)
            {
                if (c == 1) return "برنامج 1";
                if (c == 2) return "برنامجان";
                if (c >= 3 && c <= 10) return c + " برامج";
                return c + " برنامج";
            }
            return c == 1 ? "1 Program" : c + " Programs";
        }
    }
}
