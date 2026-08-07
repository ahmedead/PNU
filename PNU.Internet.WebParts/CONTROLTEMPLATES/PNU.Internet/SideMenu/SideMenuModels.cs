using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu
{
    /// <summary>
    /// Maps to the "SideMenuLevel1" list. Field structure mirrors TopMenuLevel1
    /// (Title, Title_EN, URL, ItemOrder, Visibility). Mapped manually in
    /// ucSideMenu.ascx.cs -- no reflection/attribute dependency.
    /// </summary>
    public class SideMenuLevel1
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string URL { get; set; }
        public int ItemOrder { get; set; }
        public bool Visibility { get; set; }

        /// <summary>Not a SharePoint field -- populated in code after the Level2 query.</summary>
        public List<SideMenuLevel2> LVL2 { get; set; }

        // --------------------------------------------------------------
        //  Presentation properties - computed in the code-behind BEFORE
        //  binding, so the .ascx only needs simple Eval() expressions.
        // --------------------------------------------------------------

        /// <summary>Title in the current UI language (Ar/En), already resolved.</summary>
        public string DisplayTitle { get; set; }

        /// <summary>Absolute/site-relative href, already resolved.</summary>
        public string ResolvedUrl { get; set; }

        /// <summary>True when this item (or one of its children) matches the current page.</summary>
        public bool IsActive { get; set; }

        /// <summary>Client-side id used by the Bootstrap collapse (groups only).</summary>
        public string GroupId { get; set; }

        /// <summary>Number of visible children - shown in the pill badge.</summary>
        public int ChildCount { get; set; }

        /// <summary>True when this item has children and should render as a collapsible group.</summary>
        public bool HasChildren { get; set; }
    }

    /// <summary>
    /// Maps to the "SideMenuLevel2" list. Field structure mirrors TopMenuLevel2,
    /// including the "Parent" lookup field pointing back to SideMenuLevel1.Title.
    /// </summary>
    public class SideMenuLevel2
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string URL { get; set; }
        public string Parent { get; set; }
        public int ItemOrder { get; set; }
        public bool Visibility { get; set; }

        // Presentation properties (see SideMenuLevel1)
        public string DisplayTitle { get; set; }
        public string ResolvedUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
