using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>University services and centres, grouped under their sub-headings.</summary>
    public partial class ucPgServices : PgSectionBase
    {
        protected override string ListName { get { return PgListNames.Services; } }

        protected override string DefaultSectionKey { get { return PgSectionKeys.Services; } }

        protected string GoToIconUrl { get { return PgListSchema.IconRoot + "goTo.png"; } }

        protected string OpenLabel
        {
            get { return PgHelper.Enc(PgHelper.GetRes("Pg_OpenService", "الانتقال إلى الخدمة", "Go to the service")); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSection();
        }

        protected override void OnItemsLoaded(List<SPListItem> raw)
        {
            var groups = new List<PgServiceGroup>();
            var index = new Dictionary<string, PgServiceGroup>(StringComparer.Ordinal);
            int position = 0;

            foreach (SPListItem item in raw)
            {
                string name = PgHelper.Enc(PgHelper.PickField(item, "GroupName"));

                PgServiceGroup group;
                if (!index.TryGetValue(name, out group))
                {
                    group = new PgServiceGroup { Name = name };
                    index[name] = group;
                    groups.Add(group);
                }

                group.Items.Add(new PgCard
                {
                    Id = item.ID,
                    Title = PgHelper.Enc(PgHelper.PickField(item, "Title")),
                    LinkUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "LinkUrl")),
                    IconUrl = PgHelper.Enc(PgHelper.SafeUrl(item, "IconUrl")),
                    GroupName = name,
                    ItemOrder = PgHelper.SafeInt(item, "ItemOrder"),
                    AccentClass = Accent(position++)
                });
            }

            if (rptGroups != null)
            {
                rptGroups.DataSource = groups;
                rptGroups.DataBind();
            }
        }
    }
}
