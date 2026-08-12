using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    /// <summary>
    /// List name constants for the Page Creator module.
    /// </summary>
    public static class PageCreatorLists
    {
        public const string PageTemplates = "PageTemplates";
        public const string PageLayouts = "PageLayoutsCatalog";   // "PageLayouts" is reserved-feeling on publishing sites, so we use a distinct internal name
        public const string PageCreatorAdmins = "PageCreatorAdmins";
    }

    /// <summary>
    /// DTO for the PageTemplates list.
    /// Title            = TemplateName
    /// PageLayoutURL    = Note (plain text) – server-relative layout URL (long URLs safe)
    /// UserControlPath  = Note (plain text) – ~/_controltemplates/15/... path
    /// DefaultProperties= Note (plain text) – default Key=Value;Key2=Value2 for the control
    /// IsActive         = Boolean
    /// </summary>
    public class clsPageTemplate
    {
        public int ID { get; set; }
        public string TemplateName { get; set; }
        public string PageLayoutURL { get; set; }
        public string UserControlPath { get; set; }
        public string DefaultProperties { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for the PageLayoutsCatalog list.
    /// Title         = Name
    /// PageLayoutURL = Note (plain text) – server-relative layout URL
    /// IsActive      = Boolean
    /// </summary>
    public class clsPageLayout
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PageLayoutURL { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Request object passed from ucPageCreator to the business layer.
    /// </summary>
    public class clsPageCreateRequest
    {
        public string WebSiteUrl { get; set; }          // absolute or server-relative, e.g. /ar/Faculties/Science
        public string PageName { get; set; }            // with or without .aspx
        public string PageTitleAr { get; set; }
        public string PageTitleEn { get; set; }
        public string PageLayoutURL { get; set; }       // resolved from template or layout DDL
        public string UserControlPath { get; set; }     // may be empty (layout-only page)
        public string UserControlProperties { get; set; } // Key=Value;Key2=Value2
        public bool AddToBothSites { get; set; }
    }
}
