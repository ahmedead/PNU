using System;

namespace PNU.Internet.SearchIndex.DAL
{
    public class SearchIndexItem
    {
        public long ItemId { get; set; }

        public string SourceType { get; set; }
        public string SourceKey { get; set; }

        public Guid SiteId { get; set; }
        public Guid WebId { get; set; }
        public Guid? ListId { get; set; }
        public int? ListItemId { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }

        public string Url { get; set; }
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }
        public DateTime? DisplayDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string DisplayTitle(bool isArabic)
        {
            string t = isArabic ? TitleAr : TitleEn;
            if (string.IsNullOrWhiteSpace(t))
                t = isArabic ? TitleEn : TitleAr;
            return t ?? string.Empty;
        }

        public string DisplayContent(bool isArabic)
        {
            string c = isArabic ? ContentAr : ContentEn;
            if (string.IsNullOrWhiteSpace(c))
                c = isArabic ? ContentEn : ContentAr;
            return c ?? string.Empty;
        }

        public string DisplayCategory(bool isArabic)
        {
            string c = isArabic ? CategoryAr : CategoryEn;
            if (string.IsNullOrWhiteSpace(c))
                c = isArabic ? CategoryEn : CategoryAr;
            return c ?? string.Empty;
        }

        public string DisplayDateString
        {
            get
            {
                return DisplayDate.HasValue
                    ? DisplayDate.Value.ToString("dd-MM-yyyy")
                    : ModifiedDate.ToString("dd-MM-yyyy");
            }
        }
    }

    public class CategoryRow
    {
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }
        public int Count { get; set; }

        public string Display(bool isArabic)
        {
            string c = isArabic ? CategoryAr : CategoryEn;
            if (string.IsNullOrWhiteSpace(c))
                c = isArabic ? CategoryEn : CategoryAr;
            return c ?? "";
        }
    }
}
