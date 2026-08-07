using Microsoft.SharePoint;
using PNU.Internet.WebParts.Helpers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    public partial class PNUCouncil : UserControl
    {
        // ------------------------------------------------------------------
        //  State
        // ------------------------------------------------------------------

        private bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        private List<CouncilCard> _leadership = new List<CouncilCard>();
        private List<CouncilMember> _members = new List<CouncilMember>();
        private List<CouncilDoc> _documents = new List<CouncilDoc>();

        // ------------------------------------------------------------------
        //  Page lifecycle
        // ------------------------------------------------------------------

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    CouncilListProvisioner.EnsureCouncilLists();

                    LoadCouncilData();

                    rptLeadership.DataSource = _leadership;
                    rptLeadership.DataBind();

                    rptMembers.DataSource = _members;
                    rptMembers.DataBind();

                    rptDocuments.DataSource = _documents;
                    rptDocuments.DataBind();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        // ------------------------------------------------------------------
        //  ItemDataBound handlers (one per repeater)
        // ------------------------------------------------------------------

        protected void rptLeadership_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            CouncilCard card = e.Item.DataItem as CouncilCard;
            if (card == null) return;

            HtmlGenericControl icon = e.Item.FindControl("iconEl") as HtmlGenericControl;
            Literal litName = e.Item.FindControl("litName") as Literal;
            Literal litRole = e.Item.FindControl("litRole") as Literal;

            if (icon != null)
            {
                string iconClass = string.IsNullOrEmpty(card.IconClass) ? "hgi hgi-stroke hgi-user" : card.IconClass;
                icon.Attributes["class"] = iconClass + " fs-3";
            }
            if (litName != null)
            {
                litName.Text = Server.HtmlEncode(IsArabic ? card.NameAr : card.NameEn);
            }
            if (litRole != null)
            {
                litRole.Text = Server.HtmlEncode(IsArabic ? card.RoleAr : card.RoleEn);
            }
        }

        protected void rptMembers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            CouncilMember member = e.Item.DataItem as CouncilMember;
            if (member == null) return;

            Literal litNumber = e.Item.FindControl("litNumber") as Literal;
            Literal litName = e.Item.FindControl("litName") as Literal;
            Literal litRole = e.Item.FindControl("litRole") as Literal;
            Literal litType = e.Item.FindControl("litType") as Literal;

            if (litNumber != null)
            {
                litNumber.Text = Server.HtmlEncode(member.MemberNumber);
            }
            if (litName != null)
            {
                litName.Text = Server.HtmlEncode(IsArabic ? member.NameAr : member.NameEn);
            }
            if (litRole != null)
            {
                litRole.Text = Server.HtmlEncode(IsArabic ? member.RoleAr : member.RoleEn);
            }
            if (litType != null)
            {
                litType.Text = Server.HtmlEncode(IsArabic ? member.MemberTypeAr : member.MemberTypeEn);
            }
        }

        // Shared card-markup binder used by Leadership only (Members has its own handler above).
        private void BindCardMarkup(RepeaterItem item, CouncilCard card)
        {
            HtmlGenericControl icon = item.FindControl("iconEl") as HtmlGenericControl;
            Literal litName = item.FindControl("litName") as Literal;
            Literal litRole = item.FindControl("litRole") as Literal;

            if (icon != null)
            {
                string iconClass = string.IsNullOrEmpty(card.IconClass) ? "hgi hgi-stroke hgi-user" : card.IconClass;
                icon.Attributes["class"] = iconClass + " fs-3";
            }
            if (litName != null)
            {
                litName.Text = Server.HtmlEncode(IsArabic ? card.NameAr : card.NameEn);
            }
            if (litRole != null)
            {
                litRole.Text = Server.HtmlEncode(IsArabic ? card.RoleAr : card.RoleEn);
            }
        }

        protected void rptDocuments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            CouncilDoc doc = e.Item.DataItem as CouncilDoc;
            if (doc == null) return;

            HtmlGenericControl icon = e.Item.FindControl("iconEl") as HtmlGenericControl;
            Literal litTitle = e.Item.FindControl("litTitle") as Literal;
            HtmlAnchor lnk = e.Item.FindControl("lnkDoc") as HtmlAnchor;
            Literal litButton = e.Item.FindControl("litButton") as Literal;

            string title = IsArabic ? doc.TitleAr : doc.TitleEn;
            string button = IsArabic ? doc.ButtonLabelAr : doc.ButtonLabelEn;
            string ariaPrefix = IsArabic ? "عرض " : "View ";

            if (icon != null)
            {
                string iconClass = string.IsNullOrEmpty(doc.IconClass) ? "hgi hgi-stroke hgi-file-02" : doc.IconClass;
                icon.Attributes["class"] = iconClass + " fs-4";
            }
            if (litTitle != null)
            {
                litTitle.Text = Server.HtmlEncode(title);
            }
            if (lnk != null)
            {
                lnk.HRef = string.IsNullOrEmpty(doc.FileUrl) ? "#" : doc.FileUrl;
                lnk.Attributes["aria-label"] = ariaPrefix + title;
            }
            if (litButton != null)
            {
                litButton.Text = Server.HtmlEncode(string.IsNullOrEmpty(button)
                    ? (IsArabic ? "تحميل الملف" : "Download File")
                    : button);
            }
        }

        // ------------------------------------------------------------------
        //  Data loading
        // ------------------------------------------------------------------

        private void LoadCouncilData()
        {
            try
            {
                // Lists live under /ar/AboutUniversity regardless of which language the visitor is on.
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb targetWeb = site.OpenWeb(CouncilListProvisioner.SUBSITE_URL))
                    {
                        if (targetWeb == null || !targetWeb.Exists) return;

                        _leadership = LoadCardList(targetWeb, CouncilListProvisioner.LIST_LEADERSHIP);
                        _members = LoadMemberList(targetWeb, CouncilListProvisioner.LIST_MEMBERS);
                        _documents = LoadDocumentList(targetWeb, CouncilListProvisioner.LIST_DOCUMENTS);
                    }
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }
        }

        private List<CouncilCard> LoadCardList(SPWeb web, string listName)
        {
            List<CouncilCard> result = new List<CouncilCard>();

            SPList list = web.Lists.TryGetList(listName);
            if (list == null) return result;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(q))
            {
                result.Add(new CouncilCard
                {
                    NameAr = SafeString(item["Title"]),
                    NameEn = SafeString(item["TitleEn"]),
                    RoleAr = SafeString(item["RoleAr"]),
                    RoleEn = SafeString(item["RoleEn"]),
                    IconClass = SafeString(item["IconClass"])
                });
            }
            return result;
        }

        private List<CouncilMember> LoadMemberList(SPWeb web, string listName)
        {
            List<CouncilMember> result = new List<CouncilMember>();

            SPList list = web.Lists.TryGetList(listName);
            if (list == null) return result;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(q))
            {
                result.Add(new CouncilMember
                {
                    MemberNumber = SafeString(item["MemberNumber"]),
                    NameAr = SafeString(item["Title"]),
                    NameEn = SafeString(item["TitleEn"]),
                    RoleAr = SafeString(item["RoleAr"]),
                    RoleEn = SafeString(item["RoleEn"]),
                    MemberTypeAr = SafeString(item["MemberTypeAr"]),
                    MemberTypeEn = SafeString(item["MemberTypeEn"])
                });
            }
            return result;
        }

        private List<CouncilDoc> LoadDocumentList(SPWeb web, string listName)
        {
            List<CouncilDoc> result = new List<CouncilDoc>();

            SPList list = web.Lists.TryGetList(listName);
            if (list == null) return result;

            SPQuery q = new SPQuery
            {
                Query = @"<Where><Eq><FieldRef Name='Visibility'/><Value Type='Boolean'>1</Value></Eq></Where>
                         <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE'/></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(q))
            {
                SPFieldUrlValue url = new SPFieldUrlValue(SafeString(item["FileURL"]));
                result.Add(new CouncilDoc
                {
                    TitleAr = SafeString(item["Title"]),
                    TitleEn = SafeString(item["TitleEn"]),
                    FileUrl = string.IsNullOrEmpty(url.Url) ? "" : url.Url,
                    IconClass = SafeString(item["IconClass"]),
                    ButtonLabelAr = SafeString(item["ButtonLabelAr"]),
                    ButtonLabelEn = SafeString(item["ButtonLabelEn"])
                });
            }
            return result;
        }

        // ------------------------------------------------------------------
        //  Utilities
        // ------------------------------------------------------------------

        private static string SafeString(object v)
        {
            return v == null ? "" : v.ToString();
        }

        // ------------------------------------------------------------------
        //  DTOs
        // ------------------------------------------------------------------

        private class CouncilCard
        {
            public string NameAr { get; set; }
            public string NameEn { get; set; }
            public string RoleAr { get; set; }
            public string RoleEn { get; set; }
            public string IconClass { get; set; }
        }

        private class CouncilMember
        {
            public string MemberNumber { get; set; }
            public string NameAr { get; set; }
            public string NameEn { get; set; }
            public string RoleAr { get; set; }
            public string RoleEn { get; set; }
            public string MemberTypeAr { get; set; }
            public string MemberTypeEn { get; set; }
        }

        private class CouncilDoc
        {
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string FileUrl { get; set; }
            public string IconClass { get; set; }
            public string ButtonLabelAr { get; set; }
            public string ButtonLabelEn { get; set; }
        }
    }


}
