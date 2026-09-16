using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Certifications.Controls
{
    /// <summary>
    /// Serializes the certifications to the JSON payload the client script consumes.
    ///
    /// The keys are the ones the guide script already expects (title_ar, is_supported,
    /// course_1 ...), so the markup and the script are unchanged from the static page -
    /// only the source of the data moved from a hardcoded array to the SharePoint list.
    /// The mapping "field internal name -> JSON key" lives in CertListSchema.
    /// </summary>
    public static class CertJson
    {
        /// <summary>
        /// JSON for one payload, safe to emit inside a &lt;script&gt; block: "&lt;" is
        /// escaped so a value containing "&lt;/script&gt;" cannot close the element.
        /// </summary>
        public static string Encode(object payload)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            string json = serializer.Serialize(payload ?? new List<object>());

            return json
                .Replace("<", "\\u003c")
                .Replace(">", "\\u003e")
                .Replace("&", "\\u0026");
        }
    }

    /// <summary>Row DTO for the admin grid - all values already HTML-encoded.</summary>
    public class CertAdminRow
    {
        public int Id { get; set; }
        public List<string> Cells { get; set; }
        public string EditText { get; set; }
        public string DeleteText { get; set; }
        public string DeleteConfirm { get; set; }

        public CertAdminRow() { Cells = new List<string>(); }
    }
}
