using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    public class ListHelper
    {
        public static void EnsureField(SPList list, string internalName, SPFieldType type)
        {
            if (list.Fields.ContainsField(internalName))
                return;

            list.Fields.Add(internalName, type, false);

            SPField field = list.Fields[internalName];

            if (type == SPFieldType.Note)
            {
                SPFieldMultiLineText noteField = (SPFieldMultiLineText)field;
                noteField.RichText = true;
                noteField.RichTextMode = SPRichTextMode.FullHtml;
                noteField.Update();
            }

            SPView defaultView = list.DefaultView;
            if (!defaultView.ViewFields.Exists(internalName))
            {
                defaultView.ViewFields.Add(internalName);
                defaultView.Update();
            }
        }

        public static void EnsureLookupField(
            SPList list,
            string internalName,
            SPList lookupList,
            string lookupField = "Title",
            bool required = false)
        {
            if (list.Fields.ContainsField(internalName))
                return;

            string fieldName = list.Fields.AddLookup(
                internalName,
                lookupList.ID,
                required);

            SPFieldLookup lookup = (SPFieldLookup)list.Fields[fieldName];

            lookup.LookupField = lookupField;
            lookup.Update();

            SPView defaultView = list.DefaultView;

            if (!defaultView.ViewFields.Exists(internalName))
            {
                defaultView.ViewFields.Add(internalName);
                defaultView.Update();
            }

        }

        public static void EnsureChoiceField(SPList list, string internalName, string[] choices)
        {
            if (list.Fields.ContainsField(internalName))
                return;

            string fieldName = list.Fields.Add(
                internalName,
                SPFieldType.Choice,
                false);

            SPFieldChoice field = (SPFieldChoice)list.Fields[fieldName];

            field.Choices.Clear();

            foreach (string choice in choices)
            {
                field.Choices.Add(choice);
            }

            field.Update();

            list.Update();
        }

        public static string CleanRichText(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            // Remove SharePoint paste markers
            html = Regex.Replace(
                html,
                @"<span[^>]*id=""ms-rterangepaste-(start|end)""[^>]*>.*?</span>",
                "",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Decode encoded HTML
            html = HttpUtility.HtmlDecode(html);

            return html.Trim();
        }

        public static string GetUrlFieldValue(SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return string.Empty;

            SPFieldUrlValue urlValue = new SPFieldUrlValue(item[fieldName].ToString());
            return urlValue.Url;
        }

        public static int GetLookupFieldValue(SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return 0;

            SPFieldLookupValue urlValue = new SPFieldLookupValue(item[fieldName].ToString());
            return urlValue.LookupId;
        }

        public static int ParseInt(string value)
        {
            int result;
            return int.TryParse(value, out result) ? result : 0;
        }

  
    }
}
