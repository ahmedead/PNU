using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using PNU.Internet.WebParts.ControlLoaderWebPart;
using CC = PNU.Internet.WebParts.ControlLoaderWebPart;
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.GeneralDest
{
    public partial class ucDestCertificates : UserControl
    {
        // TODO: Read from Web Part properties
        public string ListName { get; set; } = string.Empty;
        public string ColumnsCount { get; set; } = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public string CleanRichText(string html)
        {
            return ListHelper.CleanRichText(html);
        }

        private void LoadData()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        EnsureLists(web);

                        var certificates = LoadCertificates(web);

                        if (certificates.Count > 0)
                        {
                            rptCerts.DataSource = certificates;
                            rptCerts.DataBind();
                        }
                    }
                }
            });
        }

        protected void rptCerts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        private void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;

            EnsureAllLists(web);

            web.AllowUnsafeUpdates = false;
        }

        private void EnsureAllLists(SPWeb web)
        {
            EnsureCertificatesList(web);
        }

        private void EnsureCertificatesList(SPWeb web)
        {
            SPList list = web.Lists.TryGetList(ListName);

            if (list == null)
            {
                Guid listId = web.Lists.Add(
                    ListName,
                    "Stores Certificates",
                    SPListTemplateType.GenericList);

                list = web.Lists[listId];
            }

            ListHelper.EnsureField(list, "Title", SPFieldType.Text);
            ListHelper.EnsureField(list, "Date", SPFieldType.Text);
            ListHelper.EnsureChoiceField(list, "Type", new[] { "Award", "Certificate" });
            ListHelper.EnsureField(list, "URL", SPFieldType.Text);
            ListHelper.EnsureField(list, "ImageUrl", SPFieldType.Text);
            ListHelper.EnsureField(list, "Visible", SPFieldType.Boolean);
            ListHelper.EnsureField(list, "SortOrder", SPFieldType.Number);

            list.Update();
        }

        private List<CertificateItem> LoadCertificates(SPWeb web)
        {
            var result = new List<CertificateItem>();

            SPList list = web.Lists.TryGetList(ListName);
            if (list == null)
                return result;

            SPQuery query = new SPQuery
            {
                Query =
                    @"<Where>
                        <Eq>
                            <FieldRef Name='Visible' />
                            <Value Type='Boolean'>1</Value>
                        </Eq>
                      </Where>
                      <OrderBy>
                        <FieldRef Name='SortOrder' Ascending='TRUE' />
                      </OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                string type = Convert.ToString(item["Type"]);

                if (SPContext.Current.Web.Language == 1025)
                {
                    switch (type)
                    {
                        case "Certificate":
                            type = "شهادة";
                            break;

                        case "Award":
                            type = "جائزة";
                            break;
                    }
                }

                result.Add(new CertificateItem
                {
                    ID = item.ID,
                    Title = Convert.ToString(item["Title"]),
                    Date = Convert.ToString(item["Date"]),
                    Type = type,
                    URL = Convert.ToString(item["URL"]),
                    ImageUrl = Convert.ToString(item["ImageUrl"]),
                    SortOrder = item["SortOrder"] != null
                        ? Convert.ToInt32(item["SortOrder"])
                        : 0
                });
            }

            return result;
        }
    }

    [Serializable]
    public class CertificateItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }

        // Certificate | Award
        public string Type { get; set; }

        public string URL { get; set; }
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }
    }
}