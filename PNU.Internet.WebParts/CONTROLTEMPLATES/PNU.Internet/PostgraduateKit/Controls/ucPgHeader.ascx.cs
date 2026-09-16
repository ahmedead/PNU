using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>Hero title and breadcrumb of one audience page.</summary>
    public partial class ucPgHeader : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Header; } }

        protected override string DefaultSectionKey { get { return string.Empty; } }

        public PgHeaderModel Model { get; private set; }

        public ucPgHeader()
        {
            Model = new PgHeaderModel();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();

            if (rptBreadcrumb != null)
            {
                rptBreadcrumb.DataSource = Model.BreadcrumbItems;
                rptBreadcrumb.DataBind();
            }
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            if (raw.Count == 0) return;

            SPListItem item = raw[0];
            Model.Title = PgHelper.Enc(PgHelper.PickField(item, "Title"));
            Model.Description = PgHelper.Enc(PgHelper.PickField(item, "Description"));
            Model.BreadcrumbItems = PgHelper.ParseBreadcrumb(PgHelper.PickField(item, "BreadcrumbText"));
        }
    }
}
