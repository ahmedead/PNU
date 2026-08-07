using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts
{
    public class clsFaculties
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string FacultyName { get; set; }
        public string Category { get; set; }
        public string MainFaculty { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int ItemOrder { get;  set; }
    }

    public static class busclsFaculties
    {
        public static List<clsFaculties> GetAllItems()
        {
            return SPFactory.GetAllItems<clsFaculties>(Settings.Faculties);


        }

        internal static List<clsFaculties> GetAllItemsByCategory(string Category)
        {
            SPQuery query = new SPQuery();
            query.Query = @"<Where>
                  <Eq>
                     <FieldRef Name='Category' />
                     <Value Type='Choice'>"+ Category + @"</Value>
                  </Eq>
               </Where>
               <OrderBy>
                  <FieldRef Name='ItemOrder' Ascending='True' />
               </OrderBy>";

            return SPFactory.GetAllItemsByQuery<clsFaculties>("ar", Settings.Faculties, query);
        }
    }
}
