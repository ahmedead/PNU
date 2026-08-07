using Microsoft.SharePoint;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{
    public partial class ucPNUInNumbersCountVisits : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    SPSecurity.RunWithElevatedPrivileges(delegate ()
            //    {
            //        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            //        {
            //            using (SPWeb web = site.OpenWeb("/admin"))
            //            {

            //                SPList list = web.Lists["VisitorsCount"];
            //                if (list != null)
            //                {

            //                    SPListItemCollection items = list.GetItems();

            //                    if (items != null && items.Count > 0)
            //                    {
            //                        // 2. Group by Year-Month using LINQ
            //                        var monthlyData = items.Cast<ListItem>()
            //                            .Select(i => new {
            //                                ModifiedDate = (DateTime)i["Modified"]
            //                            })
            //                            .GroupBy(x => x.ModifiedDate.ToString("yyyy-MM"))
            //                            .Select(g => new {
            //                                Month = g.Key,
            //                                Count = g.Count()
            //                            })
            //                            .OrderBy(x => x.Month);

            //                        // 3. Export to Excel using ClosedXML
            //                        using (var workbook = new XLWorkbook())
            //                        {
            //                            var worksheet = workbook.Worksheets.Add("Monthly Report");

            //                            // Headers
            //                            worksheet.Cell(1, 1).Value = "Year-Month";
            //                            worksheet.Cell(1, 2).Value = "Item Count";
            //                            worksheet.Row(1).Style.Font.Bold = true;

            //                            // Data rows
            //                            int currentRow = 2;
            //                            foreach (var record in monthlyData)
            //                            {
            //                                worksheet.Cell(currentRow, 1).Value = record.Month;
            //                                worksheet.Cell(currentRow, 2).Value = record.Count;
            //                                currentRow++;
            //                            }

            //                            // Save File
            //                            string fileName = $"MonthlyReport_{DateTime.Now:yyyyMMdd}.xlsx";
            //                            workbook.SaveAs(fileName);
            //                            Console.WriteLine($"Exported successfully to {fileName}");
            //                        }

            //                    }


            //                }

                            
            //            }
            //        }
            //    });

            //}
        
        }
    }
}
