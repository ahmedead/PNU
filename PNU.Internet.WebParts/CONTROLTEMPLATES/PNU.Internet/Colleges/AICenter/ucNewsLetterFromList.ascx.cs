using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter
{
    public partial class ucNewsLetterFromList : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ListName { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ListName == null || ListName == "")
                    ListName = "Newsletters";
                

                

                if (!Page.IsPostBack)
                {
                    BindNewsletters();
                }
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }
        private void BindNewsletters()
        {
            bool isArabic = (SPContext.Current.Web.Language == 1025);

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                //using (SPWeb web = site.OpenWeb("/ar/Faculties/HD/AICentre/"))
                using (SPWeb web = site.OpenWeb("/ar/Faculties/IT/AICentre/"))
                {
                    SPList list = web.Lists.TryGetList("Newsletters");
                    if (list == null) return;

                    // CAML: IsActive = true; order by IssueOrder DESC then Created DESC as tiebreaker
                    SPQuery q = new SPQuery
                    {
                        Query = @"
                    <Where>
                      <Eq>
                        <FieldRef Name='Visibility' />
                        <Value Type='Boolean'>1</Value>
                      </Eq>
                    </Where>
                    <OrderBy>
                      <FieldRef Name='ItemOrder' Ascending='FALSE' />
                    </OrderBy>",
                        RowLimit = 200
                    };

                    SPListItemCollection items = list.GetItems(q);

                    // Build a light table for binding
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ModalId", typeof(string));
                    dt.Columns.Add("DisplayTitle", typeof(string));
                    dt.Columns.Add("FlipUrl", typeof(string));

                    foreach (SPListItem it in items)
                    {
                        string enTitle = Convert.ToString(it["Title"]) ?? "";
                        string arTitle = Convert.ToString(it["TitleAr"]) ?? "";

                        // Get URL fields properly
                        string urlEn = Convert.ToString(it["FlipUrlEn"]) ?? "";// GetUrlFromField(it, "FlipUrlEn");
                        string urlAr = Convert.ToString(it["FlipUrlAr"]) ?? ""; //GetUrlFromField(it, "FlipUrlAr");

                        string displayTitle = isArabic
                            ? (string.IsNullOrWhiteSpace(arTitle) ? enTitle : arTitle)
                            : (string.IsNullOrWhiteSpace(enTitle) ? arTitle : enTitle);

                        string flipUrl = isArabic
                            ? (string.IsNullOrWhiteSpace(urlAr) ? urlEn : urlAr)
                            : (string.IsNullOrWhiteSpace(urlEn) ? urlAr : urlEn);

                        // Fallback: if still empty, skip this item
                        if (string.IsNullOrWhiteSpace(flipUrl))
                            continue;

                        // Unique, stable modal id per item (use ListItemId)
                        string modalId = "N" + it.ID.ToString();

                        dt.Rows.Add(modalId, displayTitle, flipUrl);
                    }

                    rptNewsletters.DataSource = dt;
                    rptNewsletters.DataBind();

                    rptModals.DataSource = dt;
                    rptModals.DataBind();
                }
            }

           
        }

        private static string GetUrlFromField(SPListItem item, string fieldInternalName)
        {
            if (item == null || item[fieldInternalName] == null) return string.Empty;
            try
            {
                var u = new SPFieldUrlValue(item[fieldInternalName].ToString());
                return u != null ? (u.Url ?? string.Empty) : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }


    }


    
}
