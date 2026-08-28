using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public partial class ucPcGuide : PcSectionBase
    {
        protected override string ListName { get { return PcListNames.Certifications; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCertifications();
            }
        }

        private void BindCertifications()
        {
            try
            {
                LoadItems();

                rptCertifications.DataSource = Items;
                rptCertifications.DataBind();

                litCountText.Text = string.Format("{0} شهادة معروضة", Items.Count);

                var colleges = Items.Select(x => x.college)
                                    .Where(x => !string.IsNullOrEmpty(x))
                                    .Distinct()
                                    .OrderBy(x => x)
                                    .ToList();

                rptColleges.DataSource = colleges;
                rptColleges.DataBind();
            }
            catch (Exception ex)
            {
                PcLog.Write("ucPcGuide.BindCertifications", ex);
            }
        }
    }
}
