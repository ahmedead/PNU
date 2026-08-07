using System;

namespace PNU.Internet.Search.DAL
{
    /// <summary>
    /// One row in dbo.SearchItems. Used both for upsert and read.
    /// </summary>
    public class SearchIndexItem
    {
        public long ItemId { get; set; }

        // Source identification
        public string SourceType { get; set; }   // "Page" | "ListItem"
        public string SourceKey  { get; set; }   // unique business key

        public Guid  SiteId { get; set; }
        public Guid  WebId  { get; set; }
        public Guid? ListId { get; set; }
        public int?  ListItemId { get; set; }

        // Searchable + displayable
        public string TitleAr   { get; set; }
        public string TitleEn   { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }

        public string    Url         { get; set; }
        public string    Category    { get; set; }
        public DateTime? DisplayDate { get; set; }
        public DateTime  ModifiedDate { get; set; }

        // ---- helpers used by the .ascx --------------------------------
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
}
