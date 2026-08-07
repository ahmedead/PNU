using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Fields;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Integration
{
    public class StatisticsDto
    {
        public string CNT_STD { get; set; }
        public string CTN_COLL { get; set; }
        public string CNT_PROG { get; set; }
        public string CNT_ALL_DEPT { get; set; }

    }

    // this dto to read the count of students and count of academic members from PowerPi view
    public class PowerPiStatisticsDto
    {
        public string COUNT_STUDENTS { get; set; }
        public string COUNT_ACADEMIC { get; set; }

    }

    public class NewStatisticsDto
    {
        public string CNT_STD { get; set; }
        public string CNT_COLL { get; set; }
        public string CNT_PROG { get; set; }
        public string CNT_ALL_DEPT { get; set; }
        public string CNT_DEGREE_PROG { get; set; }
        public string SMRPRLE_DEGC_CODE { get; set; }
        public string STVDEGC_DESC { get; set; }
        public string DESC_EN { get; set; }
    }

    public class ListSettings
    {
        public string ID { get; set; }
        public string Title { get; set; }

        public string SiteURL { get; set; }
        public string ListURL { get; set; }


    }

    public static class Settings
    {


        #region HomePage
        

        public static string FacultyMembersList
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/CollMembersListName/";
            }
        }
        public static string RequestsList
        { //TimeLineList/";
            get
            {
                return "/ar/MediaCenter/Lists/RequestsList/";
            }
        }

        public static string Courses
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/Instcourses/";
            }
        }

        public static string NewStudyPlan
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/NewStudyPlan/";
            }
        }

        public static string NewStudyPlanListOnly
        { //TimeLineList/";
            get
            {
                return "NewStudyPlan";
            }
        }
        public static string AllPrograms
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllPrograms/";
            }
        }

        public static string AcademicCredits
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AcademicCredits/";
            }
        }

        public static string AllFaculties
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllFaculties/";
            }
        }

        public static string AllFacultyDepartments
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllFacultyDepartments/";
            }
        }
        public static string AllDepartmentPrograms
        { //TimeLineList/";
            get
            {
                return "/Admin/Lists/AllDepartmentPrograms/";
            }
        }

        public static string Faculties
        {
            get
            {
                return "Faculties";
            }
        }

        
        #endregion



        public static string DynamicPagesListURL
        {
            get
            {
                return "/lists/DynamicPages";
            }
        }

        public static string SubSiteTemplates
        {
            get
            {
                return "/Lists/SubSiteTemplates/";
            }
        }

        public static string RootSite
        {
            get
            {
                return SPContext.Current.Site.Url;
            }
        }

        public static string siteUrl
        {
            get
            {
                return SPContext.Current.Web.Url;
            }
        }

    }


    public class AllFaculties
    {
        public string Title { get; set; }
        public string Title_EN { get; set; }
        //public string FacultyName { get; set; }
        public string Category { get; set; }
        //public string Description { get; set; }

        public string DescriptionDisplay
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description.Length > 600 ? _description.Substring(0, 300) : _description;
            }

        }
        public string DescriptionDisplay_EN
        {
            get
            {

                if (_description_EN == null)
                {
                    return string.Empty;
                }


                return _description_EN.Length > 300 ? _description_EN.Substring(0, 300) : _description_EN;
            }
            set
            {

                _description_EN = value;
            }
        }

        private string _description;

        public string Description
        {
            get
            {

                if (_description == null)
                {
                    return string.Empty;
                }


                return _description;
            }
            set
            {

                _description = value;
            }
        }

        //public string Description_EN { get; set; }

        private string _description_EN;

        public string Description_EN
        {
            get
            {

                if (_description_EN == null)
                {
                    return string.Empty;
                }


                return _description_EN;
            }
            set
            {

                _description_EN = value;
            }
        }

        private string _ImageUrl;

        public string ImageUrl
        {
            get
            {

                if (_ImageUrl == null)
                {
                    return  "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" ;
                }
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
            }
        }
        public string LinkUrl { get; set; }
        public string ItemOrder { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }

        private string _Speech;

        public string Speech
        {
            get
            {

                if (_Speech == null)
                {
                    return string.Empty;
                }


                return _Speech;
            }
            set
            {

                _Speech = value;
            }
        }
        private string _Speech_EN;

        public string Speech_EN
        {
            get
            {

                if (_Speech_EN == null)
                {
                    return string.Empty;
                }


                return _Speech_EN;
            }
            set
            {

                _Speech_EN = value;
            }
        }

        private string _PublishingRollupImage;

        public string PublishingRollupImage
        {
            get
            {

                if (_PublishingRollupImage == null)
                {
                    return "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg";
                }
                return _PublishingRollupImage;
            }
            set
            {
                _PublishingRollupImage = value;
            }
        }

        public string ID { get; set; }

        public string COLL_CLASS_EN { get; set; }
        public string COLL_CLASS_AR { get; set; }

        public string DisplayImage
        {
            get
            {
                if (PublishingRollupImage != null && PublishingRollupImage != "")
                    return PublishingRollupImage;
                else
                {

                    return "/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg";
                }

            }


        }







    }

    public class clsCourses
    {
        public string course_specification { get; set; }

    }

    public class StatisticTypes
    {
        private StatisticTypes(string value) { Value = value; }

        public string Value { get; private set; }

        public static StatisticTypes CollegesInstitues { get { return new StatisticTypes("الكليات والمعاهد"); } }
        public static StatisticTypes Programs { get { return new StatisticTypes("البرامج الاكاديمية"); } }
        public static StatisticTypes Students { get { return new StatisticTypes("طالبة"); } }
        public static StatisticTypes Members { get { return new StatisticTypes("أعضاء هيئة التدريس"); } }

        public static StatisticTypes AllDepartments { get { return new StatisticTypes("قسم"); } }


        public override string ToString()
        {
            return Value;
        }

    }


    public static class SPFactory
    {
        public static List<ListSettings> GetListSettingAllItems()
        {
            try
            {
                string result = string.Empty;
                string siteURL = string.Format("{0}{1}", Config.RootSiteUrl, "/ar/");
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
                
            }
            return null;
        }
        public static List<T> GetAllItems<T>(string ListName)
        {
            try
            {
                List<ListSettings> _allLists = GetListSettingAllItems();
                ListSettings List = _allLists.Where(e => e.Title == ListName).FirstOrDefault();

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }
            return null;
        }
        public static List<T> GetAllItems<T>(string ListURL, string SiteURL)
        {
            try
            {
                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }

            return null;
        }

        public static List<T> GetAllItemsByQuery<T>(string SiteURL, string ListURL, SPQuery query)
        {
            try
            {
                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                                        using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                                        using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }

            return null;

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
                                        using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                                        using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                                    using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }

            return default(T);
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


        public static SPList MapListFieldsFromClass<T>(SPList list)
        {
            try
            {
                T obj = default(T);
                obj = Activator.CreateInstance<T>();
                PropertyInfo[] objprops = obj.GetType().GetProperties();
                obj = Activator.CreateInstance<T>();
                SPView view = list.DefaultView;
                for (int i = 0; i < objprops.Length; i++)
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

                view.Update();

                return list;



            }

            catch (Exception ex)
            {
                
            }
            return list;

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
                return null;
            }

        }


        public static void AddCurrentUserToSharePointGroup(string siteURL, string userGroupName)
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
                
            }


        }

        public static SPListItem MapClassToSPListItem(SPListItem _Item, object objNew, bool IsNew = false)
        {
            try
            {//string TableName = objNew.GetType().Name;
                PropertyInfo[] objprops = objNew.GetType().GetProperties();
                for (int i = 0; i < objprops.Length; i++)
                {

                    PropertyInfo prop = objprops[i];
                    if (IsNew)
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
                
            }

            return null;
        }

        public static void CreateCollegesList(List<CollegesDto> collList)
        {
            try
            {
                string CollegeListName = "Colleges";
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }


        }
        public static void CreateCollegesCategoryList(List<CollegeCategoryDto> collList)
        {
            try
            {
                string CollegeListName = "CollegeCategory";
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }


        }


        public static void CreatePLANS_ELEC_UList(List<PLANS_ELEC_U> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }




        }

        public static void CreatetblMemberCoursesList(List<tblMemberCourses> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                            list = SPFactory.MapListFieldsFromClass<tblMemberCourses>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (tblMemberCourses row in collList)
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

            }




        }

        public static void CreatetbtblMemberCourses_ENList(List<tblMemberCourses_EN> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                            list = SPFactory.MapListFieldsFromClass<tblMemberCourses_EN>(list);
                        }

                        web.AllowUnsafeUpdates = false;

                        if (list == null) return;

                        foreach (tblMemberCourses_EN row in collList)
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

            }




        }

        public static void CreatePLANS_ELEC_CList(List<PLANS_ELEC_C> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }



        }


        public static void CreatePLANS_ELEC_PList(List<PLANS_ELEC_P> collList, string CollegeListName)
        {
            try
            {
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }


        }


        public static void CreateDeptsList(List<DeptsDto> collList)
        {
            try
            {
                string CollegeListName = "CollegeDepatments";
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }


        }

        public static void CreateProgramsList(List<ProgramsDto> collList)
        {
            try
            {
                string CollegeListName = "CollegeDepatmentPrograms";
                using (SPSite site = new SPSite(Config.RootSiteUrl))
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
                
            }


        }

    }


}
