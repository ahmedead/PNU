using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Fields;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace PNU.Internet.WebParts
{
    public class ListSettings
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string SiteURL { get; set; }
        public string ListURL { get; set; }
        

    }
    public static class SPFactory
    {
        public static string GetDynamicURL(string URL)
        {
            try
            {
                return String.Format("{0}{1}", SPFactory.GetSiteURL(), URL);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "GetDynamicURL", ex.Message);
                return "";
            }


        }
        public static string GetPNUresResource(string key)
        {
            try
            {
                var obj = HttpContext.GetGlobalResourceObject("CommonGlobalResources", key);
                

                return obj != null ? obj.ToString() : string.Empty;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetPNUresResource", ex.Message);
            }
            return string.Empty;
            
        }

        public static bool IsArabic
        {
            get { return SPContext.Current.Web.Language == 1025; }
        }

        public static string GetSecondaryTypeBadges(object secondaryTypeObj)
        {
            if (secondaryTypeObj == null) return string.Empty;
            string secondaryType = secondaryTypeObj.ToString();
            if (string.IsNullOrWhiteSpace(secondaryType)) return string.Empty;

            string[] tracks = secondaryType.Split(new char[] { ';', ',', '#', '|' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (var track in tracks)
            {
                string trimmed = track.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.All(char.IsDigit)) continue;

                string badgeClass = "badge badge-info";
                if (trimmed.Contains("صحة") || trimmed.Contains("حياة") || trimmed.ToLower().Contains("health"))
                    badgeClass = "badge badge-success";
                else if (trimmed.Contains("حاسب") || trimmed.Contains("هندسة") || trimmed.ToLower().Contains("comp") || trimmed.ToLower().Contains("eng"))
                    badgeClass = "badge badge-warning";
                else if (trimmed.Contains("عام") || trimmed.ToLower().Contains("general"))
                    badgeClass = "badge badge-info";
                else if (trimmed.Contains("إدارة") || trimmed.Contains("أعمال") || trimmed.ToLower().Contains("business"))
                    badgeClass = "badge badge-primary";

                sb.AppendFormat("<span class=\"{0}\">{1}</span>", badgeClass, HttpUtility.HtmlEncode(trimmed));
            }
            return sb.ToString();
        }

        public static string GetPNUresResource(string key,string Language)
        {
            try
            {
                
                if(Language == "AR" || Language == "ar")
                    Language= "ar-SA";
                else
                    Language = "en-US";

                CultureInfo arabicCulture = new CultureInfo(Language);

                var obj = HttpContext.GetGlobalResourceObject("CommonGlobalResources", key, arabicCulture);
                return obj != null ? obj.ToString() : string.Empty;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "SPFactory - GetPNUresResource", ex.Message);
            }
            return string.Empty;

        }
        public static string GetEquivalantDayName(string englishDay)
        {
            try
            {
                if (PortalHelper.IsArabic)
                    return ConvertToArabicDay(englishDay);
                else
                    return englishDay;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetEquivalantDayName", ex.Message);
            }
            return string.Empty;

        }

        public static string ConvertToArabicDay(string englishDay)
        {
            try
            {// Create a CultureInfo object for Arabic culture
                CultureInfo arabicCulture = new CultureInfo("ar-SA");

                // Get the DateTimeFormatInfo for the specified culture
                DateTimeFormatInfo dtfi = arabicCulture.DateTimeFormat;

                // Find the corresponding day name in Arabic
                DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), englishDay);
                string arabicDay = dtfi.GetDayName(dayOfWeek);
                

                return arabicDay;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - ConvertToArabicDay", ex.Message);
            }
            return string.Empty;
        }

        public static List<ListSettings> GetListSettingAllItems()
        {
            try
            {
                string result = string.Empty;
                string siteURL = string.Format("{0}{1}", SPContext.Current.Site.Url, PortalHelper.ParentLangSite);
                SPListItemCollection items = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteURL))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.GetList($"{siteURL}/Lists/ListSettings");
                            if (list != null)
                            {
                                items = list.GetItems();

                            }
                        }
                    }
                });

                return MapListItemsToClass<ListSettings>(items);




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetListSettingAllItems", ex.Message);
            }
            return null;
        }
        public static List<T> GetAllItems<T>(string ListName )
        {
            try
            {
                List<ListSettings> _allLists = GetListSettingAllItems();
                ListSettings List = _allLists.Where(e => e.Title == ListName).FirstOrDefault();

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {

                        using (SPWeb web = site.OpenWeb(List.SiteURL))
                        {
                            SPList reqList = web.GetList(List.ListURL);

                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                 @"<OrderBy>
                                  <FieldRef Name='ItemOrder' Ascending='True' />
                               </OrderBy>");
                            objNew = reqList.GetItems(query);
                        }
                    }
                });
                if (!(objNew != null && objNew.Count > 0))
                    return null;
                return MapListItemsToClass<T>(objNew);




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetAllItems", ex.Message);
            }
            return null;
        }
        public static List<T> GetAllItems<T>(string ListURL,string SiteURL)
        {
            try
            {
                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {

                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.GetList(ListURL);

                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                 @"<OrderBy>
                          <FieldRef Name='ItemOrder' Ascending='False' />
                       </OrderBy>");
                            objNew = reqList.GetItems(query);
                        }
                    }
                });
                if (!(objNew != null && objNew.Count > 0))
                    return null;
                return MapListItemsToClass<T>(objNew);




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetAllItems", ex.Message);
            }
            return null;
            
        }

        public static List<T> GetAllItemsWithoutItemOrder<T>(string ListURL, string SiteURL)
        {
            try
            {
                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {

                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.GetList(ListURL);
                            objNew = reqList.GetItems();
                        }
                    }
                });
                if (!(objNew != null && objNew.Count > 0))
                    return null;
                return MapListItemsToClass<T>(objNew);




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetAllItemsWithoutItemOrder", ex.Message);
            }
            
            return null;
        }

        public static List<T> GetAllItemsByQuery<T>(string SiteURL,string ListURL, SPQuery query)
        {
            try
            {
                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.GetList(ListURL);
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;



                return MapListItemsToClass<T>(objNew);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetAllItemsByQuery", ex.Message);
            }
            
            return null;
        }

        public static List<T> GetAllDataByQuery<T>(string SiteURL, string ListName, SPQuery query)
        {
            try
            {
                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SiteURL))
                        {
                            SPList reqList = web.Lists[ListName];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;



                return MapListItemsToClass<T>(objNew);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetAllDataByQuery", ex.Message);
            }
            return null;
            
        }

        public static List<T> GetAllItemsByQueryListName<T>(string SiteURL, string ListName, SPQuery query)
        {
            try
            {
                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("admin"))
                        {
                            SPList reqList = web.Lists.TryGetList(ListName);
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;



                return MapListItemsToClass<T>(objNew);



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetAllItemsByQueryListName", ex.Message);
            }

            return null;
            
        }

        public static List<T> MapListItemsToClass<T>(SPListItemCollection objNew)
        {
            try
            {
                if (objNew == null) return null;
                List<T> list = new List<T>();
                T obj = default(T);
                obj = Activator.CreateInstance<T>();
                PropertyInfo[] objprops = obj.GetType().GetProperties();
                foreach (SPListItem _item in objNew)
                {
                    obj = Activator.CreateInstance<T>();
                    for (int i = 0; i < objprops.Length; i++)
                    {
                        PropertyInfo prop = objprops[i];
                        //if (prop.Name == "ID")
                        //    continue;
                        //Type PrpType;
                        if (prop != null)
                        {


                            try
                            {
                                if (prop.Name == "AttachmentURL")
                                {
                                    SPAttachmentCollection attachments = _item.Attachments;
                                    if (SPContext.Current.Web.Url.Contains("/en") && attachments.UrlPrefix.Contains("/ar/"))
                                    {
                                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                        {
                                            using (SPWeb web = site.OpenWeb("ar"))
                                            {
                                                SPFile file = web.GetFile(attachments.UrlPrefix + attachments[0]);
                                                prop.SetValue(obj, file.ServerRelativeUrl, null);
                                            }
                                        }


                                    }
                                    else
                                    {
                                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                        {
                                            using (SPWeb web = site.OpenWeb("ar"))
                                            {
                                                SPFile file = web.GetFile(attachments.UrlPrefix + attachments[0]);
                                                prop.SetValue(obj, file.ServerRelativeUrl, null);
                                            }
                                        }

                                        //SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                        //prop.SetValue(obj, file.ServerRelativeUrl, null);
                                    }

                                }
                                else
                                {
                                    if (_item[prop.Name] != null)
                                        if (prop.Name == "PublishingRollupImage")
                                        {
                                            ImageFieldValue PublishingRollupImage = (ImageFieldValue)_item[prop.Name];
                                            prop.SetValue(obj, PublishingRollupImage.ImageUrl, null);


                                        }
                                        else if (prop.Name == "PublishingPageImage")
                                        {
                                            ImageFieldValue PublishingPageImage = (ImageFieldValue)_item[prop.Name];
                                            prop.SetValue(obj, PublishingPageImage.ImageUrl, null);



                                        }
                                        else
                                        {
                                            if(prop.PropertyType == typeof(string))
                                                prop.SetValue(obj, _item[prop.Name].ToString(), null);
                                            else if (prop.PropertyType == typeof(Boolean))
                                                prop.SetValue(obj, Convert.ToBoolean(_item[prop.Name]), null);
                                        }
                                    //else
                                    //    prop.SetValue(obj, "", null);
                                }

                            }
                            catch (Exception ex)
                            { }


                        }


                    }
                    list.Add(obj);
                }


                return list;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - MapListItemsToClass", ex.Message);
            }

            return  null;
            
        }

        public static List<T> MapListItemsToClass<T>(List<SPListItem> objNew)
        {
            try
            {
                if (objNew == null) return null;
                List<T> list = new List<T>();
                T obj = default(T);
                obj = Activator.CreateInstance<T>();
                PropertyInfo[] objprops = obj.GetType().GetProperties();
                foreach (SPListItem _item in objNew)
                {
                    obj = Activator.CreateInstance<T>();
                    for (int i = 0; i < objprops.Length; i++)
                    {
                        PropertyInfo prop = objprops[i];
                        //if (prop.Name == "ID")
                        //    continue;
                        //Type PrpType;
                        if (prop != null)
                        {


                            try
                            {
                                if (prop.Name == "AttachmentURL")
                                {
                                    SPAttachmentCollection attachments = _item.Attachments;
                                    if (SPContext.Current.Web.Url.Contains("/en") && attachments.UrlPrefix.Contains("/ar/"))
                                    {
                                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                        {
                                            using (SPWeb web = site.OpenWeb("ar"))
                                            {
                                                SPFile file = web.GetFile(attachments.UrlPrefix + attachments[0]);
                                                prop.SetValue(obj, file.ServerRelativeUrl, null);
                                            }
                                        }


                                    }
                                    else
                                    {
                                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                        {
                                            using (SPWeb web = site.OpenWeb("ar"))
                                            {
                                                SPFile file = web.GetFile(attachments.UrlPrefix + attachments[0]);
                                                prop.SetValue(obj, file.ServerRelativeUrl, null);
                                            }
                                        }

                                        //SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                        //prop.SetValue(obj, file.ServerRelativeUrl, null);
                                    }

                                }
                                else
                                {
                                    if (_item[prop.Name] != null)
                                        if (prop.Name == "PublishingRollupImage")
                                        {
                                            ImageFieldValue PublishingRollupImage = (ImageFieldValue)_item[prop.Name];
                                            prop.SetValue(obj, PublishingRollupImage.ImageUrl, null);


                                        }
                                        else if (prop.Name == "PublishingPageImage")
                                        {
                                            ImageFieldValue PublishingPageImage = (ImageFieldValue)_item[prop.Name];
                                            prop.SetValue(obj, PublishingPageImage.ImageUrl, null);



                                        }
                                        else
                                        {
                                            if (prop.PropertyType == typeof(string))
                                                prop.SetValue(obj, _item[prop.Name].ToString(), null);
                                            else if (prop.PropertyType == typeof(Boolean))
                                                prop.SetValue(obj, Convert.ToBoolean(_item[prop.Name]), null);
                                        }
                                    //else
                                    //    prop.SetValue(obj, "", null);
                                }

                            }
                            catch (Exception ex)
                            { }


                        }


                    }
                    list.Add(obj);
                }


                return list;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "SPFactory - MapListItemsToClass", ex.Message);
            }

            return null;

        }

        public static T MapListItemsToClass<T>(SPListItem _item)
        {
            T obj = default(T);
            try
            {
                
                obj = Activator.CreateInstance<T>();
                PropertyInfo[] objprops = obj.GetType().GetProperties();
                obj = Activator.CreateInstance<T>();
                for (int i = 0; i < objprops.Length; i++)
                {
                    
                        PropertyInfo prop = objprops[i];
                        //if (prop.Name == "ID")
                        //    continue;
                        //Type PrpType;
                        if (prop != null)
                        {


                        try
                        {
                            if (prop.Name == "AttachmentURL")
                            {
                                SPAttachmentCollection attachments = _item.Attachments;
                                if (SPContext.Current.Web.Url.Contains("/en") && attachments.UrlPrefix.Contains("/ar/"))
                                {
                                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                    {
                                        using (SPWeb web = site.OpenWeb("ar"))
                                        {
                                            SPFile file = web.GetFile(attachments.UrlPrefix + attachments[0]);
                                            prop.SetValue(obj, file.ServerRelativeUrl, null);
                                        }
                                    }


                                }
                                else
                                {
                                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                                    {
                                        using (SPWeb web = site.OpenWeb("ar"))
                                        {
                                            SPFile file = web.GetFile(attachments.UrlPrefix + attachments[0]);
                                            prop.SetValue(obj, file.ServerRelativeUrl, null);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (_item[prop.Name] != null)
                                    if (prop.Name == "PublishingRollupImage")
                                    {
                                        ImageFieldValue PublishingRollupImage = (ImageFieldValue)_item[prop.Name];
                                        prop.SetValue(obj, PublishingRollupImage.ImageUrl, null);


                                    }

                                    else if (prop.Name == "PublishingPageImage")
                                    {
                                        ImageFieldValue PublishingPageImage = (ImageFieldValue)_item[prop.Name];
                                        prop.SetValue(obj, PublishingPageImage.ImageUrl, null);



                                    }
                                    else
                                    {
                                        if (prop.PropertyType == typeof(string))
                                            prop.SetValue(obj, _item[prop.Name].ToString(), null);
                                        else if (prop.PropertyType == typeof(Boolean))
                                            prop.SetValue(obj, Convert.ToBoolean(_item[prop.Name]), null);
                                    }
                                //else
                                //    prop.SetValue(obj, "", null);
                            }
                        }
                        catch (Exception ex)
                        { }

                    }






                }

                return obj;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - MapListItemsToClass", ex.Message);
            }
            return obj;
            
        }

        public static List<T> MapClassToClass<T>(List<T> items)
        {
            try
            {
                List<T> result = new List<T>();

                foreach (var item in items)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] objProps = obj.GetType().GetProperties();

                    for (int i = 0; i < objProps.Length; i++)
                    {
                        
                            PropertyInfo prop = objProps[i];

                            if (prop != null)
                            {
                                // Assuming the property names in your object match the field names in your SPListItem
                                // If there is a naming mismatch, you might need some attribute or mapping logic
                                prop.SetValue(obj, prop.GetValue(item), null);
                            }
                        
                    }

                    result.Add(obj);
                }

                return result;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - MapClassToClass", ex.Message);
            }

            return null;

        }


        public static T MapClassToClass<T>(object item)
        {
            try
            {
                if (item == null)
                {
                    return default(T);
                }

                T obj = Activator.CreateInstance<T>();
                PropertyInfo[] objProps = obj.GetType().GetProperties();

                foreach (PropertyInfo prop in objProps)
                {
                    
                        PropertyInfo itemProp = item.GetType().GetProperty(prop.Name);

                        if (itemProp != null && itemProp.CanRead)
                        {
                            object value = itemProp.GetValue(item);
                            prop.SetValue(obj, value, null);
                        }
                    
                }

                return obj;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - MapClassToClass", ex.Message);
            }

            return  default(T);
        }


        public static SPListItem MapClassToSPListItemForUpdating(SPListItem _Item, object objNew)
        {

            string TableName = objNew.GetType().Name;
            PropertyInfo[] objprops = objNew.GetType().GetProperties();
            for (int i = 0; i < objprops.Length; i++)
            {
                try
                {
                    PropertyInfo prop = objprops[i];
                    if (prop.Name == "ID")
                        continue;
                    Type PrpType;
                    if (prop != null)
                    {
                        PrpType = prop.PropertyType;
                        object Val = null;

                        Val = GetPropertyValue(objNew, prop.Name);
                        if (Val == null)
                            continue;
                        else if (Val.ToString().Contains("1/1/0001") || Val.ToString().Contains("01/01/0001"))
                            continue;
                        else if (Val.ToString() == "-1")
                            continue;

                        _Item[prop.Name] = Val;
                    }
                }
                catch (Exception ex)
                {

                }


            }

            return _Item;
        }


        /// <summary>
        /// Optional attribute to force a specific SPFieldType on a model property.
        /// Example: [SPField(SPFieldType.Note)] public string Description { get; set; }
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class SPFieldAttribute : Attribute
        {
            public SPFieldType FieldType { get; private set; }
            public bool RichText { get; set; }

            public SPFieldAttribute(SPFieldType fieldType)
            {
                FieldType = fieldType;
            }
        }


        public static SPList MapListFieldsFromClass<T>(SPList list)
        {
            try
            {
                PropertyInfo[] objprops = typeof(T).GetProperties();
                SPView view = list.DefaultView;
                bool viewChanged = false;

                for (int i = 0; i < objprops.Length; i++)
                {
                    PropertyInfo prop = objprops[i];
                    if (prop == null) continue;
                    if (prop.Name == "ID" || prop.Name == "Title") continue;

                    // Idempotent guard - never add twice
                    if (!list.Fields.ContainsField(prop.Name))
                    {
                        SPFieldType fieldType = ResolveFieldType(prop, out bool richText);

                        string internalName = list.Fields.Add(prop.Name, fieldType, false);
                        SPField field = list.Fields.GetFieldByInternalName(internalName);

                        if (fieldType == SPFieldType.Note && field is SPFieldMultiLineText noteField)
                        {
                            noteField.RichText = richText;
                            noteField.NumberOfLines = 8;
                            noteField.Update();
                        }
                    }

                    if (!view.ViewFields.Exists(prop.Name))
                    {
                        view.ViewFields.Add(prop.Name);
                        viewChanged = true;
                    }
                }

                if (viewChanged)
                    view.Update();

                return list;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "SPFactory - MapListFieldsFromClass", ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Resolves the SPFieldType for a property:
        /// 1) [SPField] attribute wins.
        /// 2) Otherwise inferred from the CLR type.
        /// 3) String properties whose name contains Description/Text/Body/Content/Notes/Welcome -> Note.
        /// </summary>
        private static SPFieldType ResolveFieldType(PropertyInfo prop, out bool richText)
        {
            richText = false;

            var attr = (SPFieldAttribute)Attribute.GetCustomAttribute(prop, typeof(SPFieldAttribute));
            if (attr != null)
            {
                richText = attr.RichText;
                return attr.FieldType;
            }

            Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (t == typeof(int) || t == typeof(double) || t == typeof(decimal))
                return SPFieldType.Number;
            if (t == typeof(bool))
                return SPFieldType.Boolean;
            if (t == typeof(DateTime))
                return SPFieldType.DateTime;

            if (t == typeof(string))
            {
                string n = prop.Name.ToLowerInvariant();
                if (n.Contains("description") || n.Contains("text") || n.Contains("body") ||
                    n.Contains("content") || n.Contains("notes") || n.Contains("welcome") ||
                    n.Contains("goals") || n.Contains("message") || n.Contains("s_coreq1"))
                    return SPFieldType.Note;
            }

            return SPFieldType.Text;
        }


        //public static SPList MapListFieldsFromClass<T>(SPList list)
        //{
        //    try
        //    {
        //        T obj = default(T);
        //        obj = Activator.CreateInstance<T>();
        //        PropertyInfo[] objprops = obj.GetType().GetProperties();
        //        obj = Activator.CreateInstance<T>();
        //        SPView view = list.DefaultView;
        //        for (int i = 0; i < objprops.Length; i++)
        //        {
                    

        //                PropertyInfo prop = objprops[i];
        //                //if (prop.Name == "ID")
        //                //    continue;
        //                //Type PrpType;
        //                if (prop != null)
        //                {
        //                    if (prop.Name == "ID" || prop.Name == "Title")
        //                        continue;
        //                    if (prop.Name == "S_COREQ1" || prop.Name == "Description")
        //                        list.Fields.Add(prop.Name, SPFieldType.Note, false);
        //                    else
        //                        list.Fields.Add(prop.Name, SPFieldType.Text, false);
        //                    view.ViewFields.Add(prop.Name);
        //                }


                    
        //        }

        //        view.Update();

        //        return list;



        //    }

        //    catch (Exception ex)
        //    {
        //        Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - MapListFieldsFromClass", ex.Message);
        //    }
        //    return list;
            
        //}

        public static string GetLocalizedTitle(object title, object titleEN)
        {
            try
            {
                if (PortalHelper.IsArabic) // Arabic language LCID
                {
                    
                    if (title != null && title.ToString().Trim() != "")
                        return title.ToString();
                    else if (titleEN != null && titleEN.ToString().Trim() != "")
                        return titleEN.ToString();
                    else
                        return "";
                }
                else
                {
                    if (titleEN != null && titleEN.ToString().Trim() != "")
                        return titleEN.ToString();
                    else if (title != null && title.ToString().Trim() != "")
                        return title.ToString();
                    else
                        return "";
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetLocalizedTitle", ex.Message);
            }


            

            return "";
        }


        public static string GetFacultyTitleLocalizedTitle(object title, object titleEN)
        {
            try
            {
                if (title != null)
                {
                    if (title.ToString().Contains("معهد"))
                    {
                        if (PortalHelper.IsArabic) // Arabic language LCID
                        {
                            if (title != null)
                                return title.ToString();
                            else if (titleEN != null)
                                return titleEN.ToString();
                            else
                                return "";
                        }
                        else
                        {
                            if (titleEN != null)
                                return titleEN.ToString();
                            else if (title != null)
                                return title.ToString();
                            else
                                return "";
                        }
                    }
                    else if (title.ToString().Contains("السنة التأسيسية") || title.ToString().Contains("Foundation Year"))
                    {
                        if (PortalHelper.IsArabic) // Arabic language LCID
                        {
                            if (title != null)
                                return title.ToString();
                            else if (titleEN != null)
                                return titleEN.ToString();
                            else
                                return "";
                        }
                        else
                        {
                            if (titleEN != null)
                                return titleEN.ToString();
                            else if (title != null)
                                return title.ToString();
                            else
                                return "";
                        }
                    }
                    else if (title.ToString().Contains("التطبيقية") )
                    {
                        if (PortalHelper.IsArabic) // Arabic language LCID
                        {
                            if (title != null)
                                return title.ToString();
                            else if (titleEN != null)
                                return titleEN.ToString();
                            else
                                return "";
                        }
                        else
                        {
                            if (titleEN != null)
                                return titleEN.ToString();
                            else if (title != null)
                                return title.ToString();
                            else
                                return "";
                        }
                    }
                    else
                        return GetPNUresResource("FacultyTitle") + " " + GetLocalizedTitle(title, titleEN);

                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetFacultyTitleLocalizedTitle", ex.Message);
            }
            
            return "";
            
        }

            
        

        public static string GetSiteURL()
        {
            try
            {
                if (PortalHelper.IsArabic) // Arabic language LCID
                {
                    return "/ar/";
                }
                else
                {
                    // English or default language
                    return "/en/";
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetSiteURL", ex.Message);
            }

            return "";
            
        }
        private static Type ResolveType(String typeName)
        {
            Type t = Type.GetType(typeName);
            try
            {
                if (t == null)
                    return null;

                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    t = u;
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - ResolveType", ex.Message);
            }
            
            
            return t;
        }

        static public object GetPropertyValue(object src, string PropertyName)
        {
            PropertyInfo PropInfo = null;
            try
            {
                PropInfo = src.GetType().GetProperty(PropertyName);
                return PropInfo.GetValue(src, null);

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - GetPropertyValue", ex.Message);
                return null;
            }
            
        }

        
        public static void AddCurrentUserToSharePointGroup( string siteURL, string userGroupName)
        {
            try
            {
                SPUser spUser = SPContext.Current.Web.CurrentUser;
                //Executes this method with Full Control rights even if the user does not otherwise have Full Control
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite spSite = new SPSite(siteURL))
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            
                                //Allow updating of some sharepoint lists, (here spUsers, spGroups etc...)
                                spWeb.AllowUnsafeUpdates = true;

                                //SPUser spUser = spWeb.EnsureUser(userLoginName);

                                if (spUser != null)
                                {

                                    SPGroup spGroup = spWeb.Groups[userGroupName];

                                    if (spGroup != null)
                                    {
                                        if (spUser.Groups != null)
                                            if (spUser.Groups.Count > 0)
                                            {
                                                bool Found = false;
                                                foreach (SPGroup _G in spUser.Groups)
                                                {
                                                    if (_G.Name == userGroupName)
                                                    {
                                                        Found = true;
                                                        break;
                                                    }
                                                }
                                                if (!Found)
                                                    spGroup.AddUser(spUser);
                                            }
                                    }
                                }
                            
                        }
                    }
                });




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - AddCurrentUserToSharePointGroup", ex.Message);
            }
            
            
        }

        public static SPListItem MapClassToSPListItem(SPListItem _Item, object objNew,bool IsNew=false)
        {
            try
            {//string TableName = objNew.GetType().Name;
                PropertyInfo[] objprops = objNew.GetType().GetProperties();
                for (int i = 0; i < objprops.Length; i++)
                {
                    
                        PropertyInfo prop = objprops[i];
                    if(IsNew)
                    {
                        if (prop.Name == "ID")
                            continue;
                    }
                    
                    Type PrpType;
                        if (prop != null)
                        {
                            PrpType = prop.PropertyType;
                            object Val = null;

                            Val = GetPropertyValue(objNew, prop.Name);
                            if (Val == null)
                                continue;
                            else if (Val.ToString().Contains("1/1/0001") || Val.ToString().Contains("01/01/0001"))
                                continue;
                            else if (Val.ToString() == "-1")
                                continue;

                            _Item[prop.Name] = Val;
                        }
                    


                }

                return _Item;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - MapClassToSPListItem", ex.Message);
            }
            return null;
            
        }

        public static List<T> DataTableMapToList<T>(DataTable dataTable)
        {
            try
            {
                List<T> list = new List<T>();
                T obj = default(T);

                foreach (DataRow row in dataTable.Rows)
                {
                    obj = Activator.CreateInstance<T>();

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        string MemberName = column.ColumnName;

                        PropertyInfo prop = obj.GetType().GetProperty(MemberName);
                        Type PrpType;

                        if (prop != null)
                        {
                            if (prop.Name == "TableName")
                                continue;

                            PrpType = prop.PropertyType;
                            object Val = null;

                            try
                            {
                                if (row[column] != DBNull.Value)
                                {
                                    PrpType = ResolveType(PrpType.ToString());
                                    Val = Convert.ChangeType(row[column], PrpType);
                                    prop.SetValue(obj, Val, null);
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }

                    list.Add(obj);
                }

                return list;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - DataTableMapToList", ex.Message);
            }
            
            return null;
        }

        public static void CreateCollegesList(List<CollegesDto> collList)
        {
            try
            {
                string CollegeListName = "Colleges";
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(CollegeListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<CollegesDto>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        SPList reqList = web.GetList(Settings.AllFaculties);

                        foreach (CollegesDto row in collList)
                        {

                            if (row.COLL_CODE != null && row.COLL_CODE == "BA")
                            {
                                continue;
                            }
                                
                            SPListItem listItem = list.Items.Add();

                            listItem = SPFactory.MapClassToSPListItem(listItem, row, true);
                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where><Eq>
                                                 <FieldRef Name='Code' />
                                                 <Value Type='Text'>" + row.COLL_CODE + @"</Value>
                                              </Eq></Where>");


                            SPListItemCollection _Data = reqList.GetItems(query);
                            SPListItem sPListItem = null;
                            if (_Data != null && _Data.Count > 0)
                            {
                                sPListItem = _Data[0];
                            }
                            else
                            {
                                sPListItem = reqList.Items.Add(); ;
                            }

                            sPListItem["Title"] = row.COLL_DESC;
                            sPListItem["Title_EN"] = row.COLL_DESC_EN;
                            sPListItem["Code"] = row.COLL_CODE;

                            if (row.COLL_CODE != null && row.COLL_CODE == "EI")
                            {
                                sPListItem["COLL_CLASS_AR"] = "المعاهد";
                                sPListItem["COLL_CLASS_EN"] = "Institutes";
                            }
                            else
                            {
                                sPListItem["COLL_CLASS_AR"] = row.COLL_CLASS_AR;
                                sPListItem["COLL_CLASS_EN"] = row.COLL_CLASS_EN;
                            }
                            

                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            sPListItem.Update();
                            web.AllowUnsafeUpdates = false;



                        }

                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - CreateCollegesList", ex.Message);
            }

            
        }
        public static void CreateCollegesCategoryList(List<CollegeCategoryDto> collList)
        {
            try
            {
                string CollegeListName = "CollegeCategory";
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(CollegeListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<CollegesDto>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (CollegeCategoryDto row in collList)
                        {
                            SPListItem listItem = list.Items.Add();
                            listItem = SPFactory.MapClassToSPListItem(listItem, row, true);
                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }




                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - CreateCollegesCategoryList", ex.Message);
            }

            
        }


        public static void CreatePLANS_ELEC_UList(List<PLANS_ELEC_U> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(CollegeListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<PLANS_ELEC_U>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (PLANS_ELEC_U row in collList)
                        {
                            SPListItem listItem = list.Items.Add();
                            listItem = SPFactory.MapClassToSPListItem(listItem, row, true);
                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }




                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - CreatePLANS_ELEC_UList", ex.Message);
            }



            
        }

        public static void CreatePLANS_ELEC_CList(List<PLANS_ELEC_C> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(CollegeListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<PLANS_ELEC_C>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (PLANS_ELEC_C row in collList)
                        {
                            SPListItem listItem = list.Items.Add();
                            listItem = SPFactory.MapClassToSPListItem(listItem, row, true);
                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }




                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - CreatePLANS_ELEC_CList", ex.Message);
            }


            
        }


        public static void CreatePLANS_ELEC_PList(List<PLANS_ELEC_P> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(CollegeListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<PLANS_ELEC_P>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (PLANS_ELEC_P row in collList)
                        {
                            SPListItem listItem = list.Items.Add();
                            listItem = SPFactory.MapClassToSPListItem(listItem, row, true);
                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }




                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - CreatePLANS_ELEC_PList", ex.Message);
            }

            
        }


        public static void CreateDeptsList(List<DeptsDto> collList)
        {
            try
            {
                string CollegeListName = "CollegeDepatments";
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(CollegeListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<DeptsDto>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        SPList reqList = web.GetList(Settings.AllFacultyDepartments);
                        foreach (DeptsDto row in collList)
                        {

                            SPListItem listItem = list.Items.Add();

                            listItem = SPFactory.MapClassToSPListItem(listItem, row, true);
                            SPQuery query = new SPQuery();
                            //query.Query = string.Concat(
                            //                 @"<Where><Eq>
                            //                     <FieldRef Name='Code' />
                            //                     <Value Type='Text'>" + row.DEPT_CODE + @"</Value>
                            //                  </Eq></Where>");

                            query.Query = $@"<Where>
                                          <And>
                                             <Eq>
                                                <FieldRef Name='Code' />
                                                <Value Type='Text'>{row.DEPT_CODE}</Value>
                                             </Eq>
                                             <Eq>
                                                <FieldRef Name='COLL_CODE' />
                                                <Value Type='Text'>{row.COLL_CODE}</Value>
                                             </Eq>
                                          </And>
                                       </Where>";
                            SPListItemCollection _Data = reqList.GetItems(query);
                            SPListItem sPListItem = null;
                            if (_Data != null && _Data.Count > 0)
                            {
                                sPListItem = _Data[0];
                            }
                            else
                            {
                                sPListItem = reqList.Items.Add(); ;
                            }

                            sPListItem["Title"] = row.DEPT_DESC;
                            sPListItem["Title_EN"] = row.DEPT_DESC_EN;
                            sPListItem["Code"] = row.DEPT_CODE;
                            sPListItem["COLL_DESC"] = row.COLL_DESC;
                            sPListItem["COLL_DESC_EN"] = row.COLL_DESC_EN;
                            sPListItem["COLL_CODE"] = row.COLL_CODE;
                            sPListItem["COLL_CLASS_AR"] = row.COLL_CLASS_AR;
                            sPListItem["COLL_CLASS_EN"] = row.COLL_CLASS_EN;

                            web.AllowUnsafeUpdates = true;
                            listItem.Update();
                            sPListItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }


                        


                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - CreateDeptsList", ex.Message);
            }

            
        }

        public static void CreateProgramsList(List<ProgramsDto> collList)
        {
            try
            {
                string CollegeListName = "CollegeDepatmentPrograms";
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("admin"))
                    {
                        SPList list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            web.AllowUnsafeUpdates = true;
                            list.Delete();
                            web.AllowUnsafeUpdates = false;
                        }

                        web.AllowUnsafeUpdates = true;
                        web.Lists.Add(CollegeListName, "", SPListTemplateType.GenericList);
                        list = web.Lists.TryGetList(CollegeListName);
                        if (list != null)
                        {
                            list = SPFactory.MapListFieldsFromClass<ProgramsDto>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        SPList reqList = web.GetList(Settings.AllDepartmentPrograms);
                        //SPList OldreqList = web.GetList(Settings.AllPrograms);

                        foreach (ProgramsDto row in collList)
                        {

                            SPListItem listItem = list.Items.Add();

                            listItem = SPFactory.MapClassToSPListItem(listItem, row, true);
                            SPQuery query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where><Eq>
                                                 <FieldRef Name='Code' />
                                                 <Value Type='Text'>" + row.PROG_CODE + @"</Value>
                                              </Eq></Where>");


                            SPListItemCollection _Data = reqList.GetItems(query);
                            SPListItem sPListItem = null;
                            if (_Data != null && _Data.Count > 0)
                            {
                                sPListItem = _Data[0];
                            }
                            else
                            {
                                sPListItem = reqList.Items.Add(); ;
                            }

                            sPListItem["Title"] = row.PROG_DESC;
                            sPListItem["Title_EN"] = row.PROG_DESC_EN;
                            sPListItem["Code"] = row.PROG_CODE;
                            sPListItem["COLL_DESC"] = row.COLL_DESC;
                            sPListItem["COLL_DESC_EN"] = row.COLL_DESC_EN;
                            sPListItem["COLL_CODE"] = row.COLL_CODE;
                            sPListItem["COLL_CLASS_AR"] = row.COLL_CLASS_AR;
                            sPListItem["COLL_CLASS_EN"] = row.COLL_CLASS_EN;


                            sPListItem["DEPT_CODE"] = row.DEPT_CODE;
                            sPListItem["DEPT_DESC"] = row.DEPT_DESC;
                            sPListItem["DEPT_DESC_EN"] = row.DEPT_DESC_EN;

                            //sPListItem["SOBCURR_DEGC_CODE"] = row.SOBCURR_DEGC_CODE;
                            //sPListItem["DEGC_DSC"] = row.DEGC_DSC;


                            //AllPrograms

                            SPList OldreqList = web.GetList(Settings.AllPrograms);
                            query = new SPQuery();
                            query.Query = string.Concat(
                                             @"<Where><Eq>
                                                 <FieldRef Name='Prog_Code' />
                                                 <Value Type='Text'>" + row.PROG_CODE + @"</Value>
                                              </Eq></Where>");


                            SPListItemCollection _OldData = OldreqList.GetItems(query);
                            SPListItem OldsPListItem = null;
                            if (_OldData != null && _OldData.Count > 0)
                            {
                                OldsPListItem = _OldData[0];
                            }
                            else
                            {
                                OldsPListItem = OldreqList.Items.Add(); ;
                            }

                            OldsPListItem["Title"] = row.PROG_DESC;
                            OldsPListItem["Title_EN"] = row.PROG_DESC_EN;
                            OldsPListItem["Code"] = row.PROG_CODE;
                            OldsPListItem["Prog_Title"] = row.PROG_DESC;
                            OldsPListItem["Prog_Code"] = row.PROG_CODE;

                            OldsPListItem["Coll_Code"] = row.COLL_CODE;
                            OldsPListItem["Coll_Title"] = row.COLL_DESC;
                            OldsPListItem["Coll_Title_EN"] = row.COLL_CLASS_EN;

                            OldsPListItem["Dept_Code"] = row.DEPT_CODE;
                            OldsPListItem["Sec_Code"] = row.DEPT_CODE;
                            OldsPListItem["Dept_Title"] = row.DEPT_DESC;
                            OldsPListItem["Dept_Title_EN"] = row.DEPT_DESC_EN;




                            web.AllowUnsafeUpdates = true;

                            listItem.Update();
                            sPListItem.Update();
                            OldsPListItem.Update();
                            web.AllowUnsafeUpdates = false;
                        }


                        


                    }
                }




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"SPFactory - CreateProgramsList", ex.Message);
            }

           
        }

    }


}
