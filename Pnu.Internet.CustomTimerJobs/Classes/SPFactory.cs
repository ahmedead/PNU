using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Fields;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pnu.Internet.CustomTimerJobs
{
    public static class SPFactory
    {
        public static string GetPNUresResource(string key)
        {

            var obj = HttpContext.GetGlobalResourceObject("CommonGlobalResources", key);

            return obj != null ? obj.ToString() : string.Empty;
        }
        public static string GetEquivalantDayName(string englishDay)
        {
            if (PortalHelper.IsArabic)
                return ConvertToArabicDay(englishDay);
            else
                return englishDay;

        }

        public static string ConvertToArabicDay(string englishDay)
        {
            // Create a CultureInfo object for Arabic culture
            CultureInfo arabicCulture = new CultureInfo("ar-SA");

            // Get the DateTimeFormatInfo for the specified culture
            DateTimeFormatInfo dtfi = arabicCulture.DateTimeFormat;

            // Find the corresponding day name in Arabic
            DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), englishDay);
            string arabicDay = dtfi.GetDayName(dayOfWeek);

            return arabicDay;
        }

        
        public static List<T> GetAllItems<T>(string ListURL, string SiteURL)
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

        public static List<T> GetAllItemsWithoutItemOrder<T>(string ListURL, string SiteURL)
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

        public static List<T> GetAllItemsByQuery<T>(string SiteURL, string ListURL, SPQuery query)
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

        public static List<T> GetAllDataByQuery<T>(string SiteURL, string ListName, SPQuery query)
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

        public static List<T> GetAllItemsByQueryListName<T>(string SiteURL, string ListName, SPQuery query)
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

        public static List<T> MapListItemsToClass<T>(SPListItemCollection objNew)
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
                                    SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                    prop.SetValue(obj, file.ServerRelativeUrl, null);
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
                                        prop.SetValue(obj, _item[prop.Name].ToString(), null);
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

        public static T MapListItemsToClass<T>(SPListItem _item)
        {

            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            PropertyInfo[] objprops = obj.GetType().GetProperties();
            obj = Activator.CreateInstance<T>();
            for (int i = 0; i < objprops.Length; i++)
            {
                try
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
                                    SPFile file = SPContext.Current.Web.GetFile(attachments.UrlPrefix + attachments[0]);
                                    prop.SetValue(obj, file.ServerRelativeUrl, null);
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
                                        prop.SetValue(obj, _item[prop.Name].ToString(), null);
                            }
                        }
                        catch (Exception ex)
                        { }
                    }


                }
                catch (Exception ex)
                {

                }



            }

            return obj;
        }

        public static List<T> MapClassToClass<T>(List<T> items)
        {
            List<T> result = new List<T>();

            foreach (var item in items)
            {
                T obj = Activator.CreateInstance<T>();
                PropertyInfo[] objProps = obj.GetType().GetProperties();

                for (int i = 0; i < objProps.Length; i++)
                {
                    try
                    {
                        PropertyInfo prop = objProps[i];

                        if (prop != null)
                        {
                            // Assuming the property names in your object match the field names in your SPListItem
                            // If there is a naming mismatch, you might need some attribute or mapping logic
                            prop.SetValue(obj, prop.GetValue(item), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions if necessary
                    }
                }

                result.Add(obj);
            }

            return result;
        }


        public static T MapClassToClass<T>(object item)
        {
            if (item == null)
            {
                return default(T);
            }

            T obj = Activator.CreateInstance<T>();
            PropertyInfo[] objProps = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in objProps)
            {
                try
                {
                    PropertyInfo itemProp = item.GetType().GetProperty(prop.Name);

                    if (itemProp != null && itemProp.CanRead)
                    {
                        object value = itemProp.GetValue(item);
                        prop.SetValue(obj, value, null);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions if necessary
                }
            }

            return obj;
        }




        public static SPList MapListFieldsFromClass<T>(SPList list)
        {

            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            PropertyInfo[] objprops = obj.GetType().GetProperties();
            obj = Activator.CreateInstance<T>();
            SPView view = list.DefaultView;
            for (int i = 0; i < objprops.Length; i++)
            {
                try
                {

                    PropertyInfo prop = objprops[i];
                    //if (prop.Name == "ID")
                    //    continue;
                    //Type PrpType;
                    if (prop != null)
                    {
                        if (prop.Name == "ID" || prop.Name == "Title")
                            continue;
                        if (prop.Name == "S_COREQ1" || prop.Name == "Description")
                            list.Fields.Add(prop.Name, SPFieldType.Note, false);
                        else
                            list.Fields.Add(prop.Name, SPFieldType.Text, false);
                        view.ViewFields.Add(prop.Name);
                    }


                }
                catch (Exception ex)
                {

                }
            }

            view.Update();

            return list;
        }

        public static string GetLocalizedTitle(object title, object titleEN)
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


        public static string GetFacultyTitleLocalizedTitle(object title, object titleEN)
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
                else
                    return GetPNUresResource("FacultyTitle") + " " + GetLocalizedTitle(title, titleEN);

            }

            return "";

        }




        public static string GetSiteURL()
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
        private static Type ResolveType(String typeName)
        {
            Type t = Type.GetType(typeName);
            if (t == null)
                return null;

            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
            {
                t = u;
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
            catch
            {
                return null;
            }
        }


        public static void AddCurrentUserToSharePointGroup(string siteURL, string userGroupName)
        {
            SPUser spUser = SPContext.Current.Web.CurrentUser;
            //Executes this method with Full Control rights even if the user does not otherwise have Full Control
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite spSite = new SPSite(siteURL))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        try
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
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            spWeb.Update();
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });


        }

        public static SPListItem MapClassToSPListItem(SPListItem _Item, object objNew)
        {

            //string TableName = objNew.GetType().Name;
            PropertyInfo[] objprops = objNew.GetType().GetProperties();
            for (int i = 0; i < objprops.Length; i++)
            {
                try
                {
                    PropertyInfo prop = objprops[i];
                    //if (prop.Name == "ID")
                    //    continue;
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

        public static List<T> DataTableMapToList<T>(DataTable dataTable)
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

        public static void CreateCollegesList(List<CollegesDto> collList)
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

                        SPListItem listItem = list.Items.Add();

                        listItem = SPFactory.MapClassToSPListItem(listItem, row);
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

                        sPListItem["COLL_CLASS_AR"] = row.COLL_CLASS_AR;
                        sPListItem["COLL_CLASS_EN"] = row.COLL_CLASS_EN;

                        web.AllowUnsafeUpdates = true;
                        listItem.Update();
                        sPListItem.Update();
                        web.AllowUnsafeUpdates = false;



                    }



                    //SPListItemCollection _AllData = reqList.GetItems();

                    //List<AllFaculties> _AllListData = SPFactory.MapListItemsToClass<AllFaculties>(_AllData);
                    //if (_AllListData != null && _AllListData.Count > 0)
                    //{
                    //    List<AllFaculties> itemsNotInOtherList = _AllListData.Where(college => !collList.Any(faculty => faculty.COLL_CODE == college.Code)).ToList();

                    //    if (itemsNotInOtherList != null && itemsNotInOtherList.Count > 0)
                    //    {
                    //        foreach (AllFaculties item in itemsNotInOtherList)
                    //        {
                    //            web.AllowUnsafeUpdates = true;
                    //            _AllData.DeleteItemById(Convert.ToInt32(item.ID));
                    //            web.AllowUnsafeUpdates = false;

                    //        }
                    //    }

                    //}


                }
            }

        }
        public static void CreateCollegesCategoryList(List<CollegeCategoryDto> collList)
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
                        listItem = SPFactory.MapClassToSPListItem(listItem, row);
                        web.AllowUnsafeUpdates = true;
                        listItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }




                }
            }

        }


        public static void CreatePLANS_ELEC_UList(List<PLANS_ELEC_U> collList, string CollegeListName)
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
                        listItem = SPFactory.MapClassToSPListItem(listItem, row);
                        web.AllowUnsafeUpdates = true;
                        listItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }




                }
            }

        }

        public static void CreatePLANS_ELEC_CList(List<PLANS_ELEC_C> collList, string CollegeListName)
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
                        listItem = SPFactory.MapClassToSPListItem(listItem, row);
                        web.AllowUnsafeUpdates = true;
                        listItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }




                }
            }

        }


        public static void CreatePLANS_ELEC_PList(List<PLANS_ELEC_P> collList, string CollegeListName)
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
                        listItem = SPFactory.MapClassToSPListItem(listItem, row);
                        web.AllowUnsafeUpdates = true;
                        listItem.Update();
                        web.AllowUnsafeUpdates = false;
                    }




                }
            }

        }


        public static void CreateDeptsList(List<DeptsDto> collList)
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

                        listItem = SPFactory.MapClassToSPListItem(listItem, row);
                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         @"<Where><Eq>
                                                 <FieldRef Name='Code' />
                                                 <Value Type='Text'>" + row.DEPT_CODE + @"</Value>
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


                    //SPListItemCollection _AllData = reqList.GetItems();

                    //List<AllFacultyDepartments> _AllListData = SPFactory.MapListItemsToClass<AllFacultyDepartments>(_AllData);
                    //if (_AllListData != null && _AllListData.Count > 0)
                    //{
                    //    List<AllFacultyDepartments> itemsNotInOtherList = _AllListData.Where(college => !collList.Any(faculty => faculty.DEPT_CODE == college.Code)).ToList();

                    //    if (itemsNotInOtherList != null && itemsNotInOtherList.Count > 0)
                    //    {
                    //        foreach (AllFacultyDepartments item in itemsNotInOtherList)
                    //        {
                    //            web.AllowUnsafeUpdates = true;
                    //            _AllData.DeleteItemById(Convert.ToInt32(item.ID));
                    //            web.AllowUnsafeUpdates = false;

                    //        }
                    //    }

                    //}


                }
            }

        }

        public static void CreateProgramsList(List<ProgramsDto> collList)
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

                        listItem = SPFactory.MapClassToSPListItem(listItem, row);
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

    }

}
