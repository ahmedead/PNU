using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA
{
    public class HomeNoraFutureItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string Description { get; set; }
        public string Description_EN { get; set; }
        public string LinkUrl { get; set; }
        public string LinkUrl_EN { get; set; }
        public string IconClass { get; set; }
        public double ItemOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class HomeNoraFutureTitleModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string AriaLabel { get; set; }
        public string AriaLabel_EN { get; set; }
        public bool ShowSection { get; set; }
    }

    /// <summary>
    /// Snapshot of the section as read from SharePoint. Loaded = false means the lists
    /// could not be read (the home page then shows its built-in fallback).
    /// </summary>
    public class HomeNoraFutureSectionData
    {
        public bool Loaded { get; set; }
        public HomeNoraFutureTitleModel Title { get; set; }
        public List<HomeNoraFutureItem> ActiveItems { get; set; }
    }

    /// <summary>
    /// Idempotent SharePoint list provisioner for the Nora Future section on the PNU Home Page.
    /// Provisions both 'HomeNoraFutureTitle' (Section title & heading) and 'HomeNoraFuture' (Cards / items)
    /// under the /ar/ContentAdmin web.
    /// </summary>
    public static class HomeNoraFutureProvisioner
    {
        public const string ListNameTitle = "HomeNoraFutureTitle";
        public const string ListNameItems = "HomeNoraFuture";
        public const string AdminWebPath = "/ar/ContentAdmin";

        private static volatile bool _alreadyProvisionedInProcess = false;

        /// <summary>
        /// Cheap guarded call (once per request / per process). Must stay parameterless:
        /// ContentAdminListProvisioner invokes it by reflection with no arguments.
        /// </summary>
        public static void EnsureAllListsExist()
        {
            if (DoneThisRequest()) return;
            if (_alreadyProvisionedInProcess) return;

            if (Provision())
                _alreadyProvisionedInProcess = true;
            MarkDoneThisRequest();
        }

        /// <summary>
        /// Always runs provisioning, ignoring the request/process guards (admin "Provision" button).
        /// Returns true when both lists exist afterwards.
        /// </summary>
        public static bool ForceProvision()
        {
            bool ok = Provision();
            if (ok) _alreadyProvisionedInProcess = true;
            MarkDoneThisRequest();
            return ok;
        }

        private static bool Provision()
        {
            bool ok = false;
            try
            {
                if (SPContext.Current == null) return false;
                Guid siteId = SPContext.Current.Site.ID;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = OpenAdminWeb(site))
                    {
                        if (web == null) return;
                        ok = EnsureListsOnWeb(web);
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "HomeNoraFutureProvisioner.Provision", ex.Message);
            }

            if (ok) HomeNoraFutureRepository.InvalidateCache();
            return ok;
        }

        /// <summary>
        /// Creates missing lists/fields on an already-open (elevated) admin web. Seeds a list
        /// only when it is created now, so deleting every row never brings the defaults back.
        /// </summary>
        public static bool EnsureListsOnWeb(SPWeb web)
        {
            bool unsafeUpdates = web.AllowUnsafeUpdates;
            web.AllowUnsafeUpdates = true;
            try
            {
                EnsureTitleList(web);
                EnsureItemsList(web);
            }
            finally
            {
                web.AllowUnsafeUpdates = unsafeUpdates;
            }

            return web.Lists.TryGetList(ListNameTitle) != null && web.Lists.TryGetList(ListNameItems) != null;
        }

        public static SPWeb OpenAdminWeb(SPSite site)
        {
            return ContentAdm.OpenAdminWeb(site);
        }

        #region Title List

        private static void EnsureTitleList(SPWeb web)
        {
            try
            {
                bool created = false;
                SPList list = web.Lists.TryGetList(ListNameTitle);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(ListNameTitle, "عنوان قسم برامج ومستقبل نورة في الصفحة الرئيسية (Home Nora Future Section Title)", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                    list.OnQuickLaunch = true;
                    list.Update();
                    created = true;
                }

                bool listChanged = false;
                listChanged |= EnsureField(list, "Title_EN", "Title (EN)", SPFieldType.Text);
                listChanged |= EnsureField(list, "AriaLabel", "Aria Label / Subtitle (AR)", SPFieldType.Text);
                listChanged |= EnsureField(list, "AriaLabel_EN", "Aria Label / Subtitle (EN)", SPFieldType.Text);
                listChanged |= EnsureField(list, "ShowSection", "Show Section on Home", SPFieldType.Boolean, "1");

                if (listChanged)
                {
                    list.Update();
                }

                if (created)
                {
                    SeedTitleData(list);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "HomeNoraFutureProvisioner.EnsureTitleList", ex.Message);
            }
        }

        private static void SeedTitleData(SPList list)
        {
            try
            {
                SPListItem item = list.AddItem();
                item["Title"] = "الدراسة في جامعة الأميرة نورة";
                item["Title_EN"] = "Study at Princess Nourah University";
                item["AriaLabel"] = "برامج ومستقبل نورة";
                item["AriaLabel_EN"] = "PNU Programs & Future";
                item["ShowSection"] = true;
                item.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "HomeNoraFutureProvisioner.SeedTitleData", ex.Message);
            }
        }

        #endregion

        #region Items List

        private static void EnsureItemsList(SPWeb web)
        {
            try
            {
                bool created = false;
                SPList list = web.Lists.TryGetList(ListNameItems);
                if (list == null)
                {
                    Guid listId = web.Lists.Add(ListNameItems, "بطاقات برامج ومستقبل نورة في الصفحة الرئيسية (Home Nora Future Cards)", SPListTemplateType.GenericList);
                    list = web.Lists[listId];
                    list.OnQuickLaunch = true;
                    list.Update();
                    created = true;
                }

                bool listChanged = false;
                listChanged |= EnsureField(list, "Title_EN", "Title (EN)", SPFieldType.Text);
                listChanged |= EnsureField(list, "Description", "Count / Description (AR)", SPFieldType.Text);
                listChanged |= EnsureField(list, "Description_EN", "Count / Description (EN)", SPFieldType.Text);
                listChanged |= EnsureField(list, "LinkUrl", "Link URL (AR)", SPFieldType.Text);
                listChanged |= EnsureField(list, "LinkUrl_EN", "Link URL (EN)", SPFieldType.Text);
                listChanged |= EnsureField(list, "IconClass", "Icon Class (HugeIcons)", SPFieldType.Text);
                listChanged |= EnsureField(list, "ItemOrder", "Item Order", SPFieldType.Number);
                listChanged |= EnsureField(list, "IsActive", "Is Active", SPFieldType.Boolean, "1");

                if (listChanged)
                {
                    list.Update();
                }

                if (created)
                {
                    SeedItemsData(list);
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "HomeNoraFutureProvisioner.EnsureItemsList", ex.Message);
            }
        }

        private static void SeedItemsData(SPList list)
        {
            try
            {
                foreach (HomeNoraFutureItem seed in HomeNoraFutureRepository.GetDefaultItems())
                {
                    SPListItem item = list.AddItem();
                    item["Title"] = seed.Title;
                    item["Title_EN"] = seed.Title_EN;
                    item["Description"] = seed.Description;
                    item["Description_EN"] = seed.Description_EN;
                    item["LinkUrl"] = seed.LinkUrl;
                    item["LinkUrl_EN"] = seed.LinkUrl_EN;
                    item["IconClass"] = seed.IconClass;
                    item["ItemOrder"] = seed.ItemOrder;
                    item["IsActive"] = true;
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(CurrentUrl(), "HomeNoraFutureProvisioner.SeedItemsData", ex.Message);
            }
        }

        #endregion

        #region Helpers

        private static bool EnsureField(SPList list, string internalName, string displayName, SPFieldType fieldType, string defaultValue = null)
        {
            if (list == null) return false;

            if (list.Fields.ContainsField(internalName))
            {
                // Existing Yes/No fields created without a default: give them one so new rows are active
                if (defaultValue != null)
                {
                    try
                    {
                        SPField existing = list.Fields.GetFieldByInternalName(internalName);
                        if (existing != null && string.IsNullOrEmpty(existing.DefaultValue))
                        {
                            existing.DefaultValue = defaultValue;
                            existing.Update();
                        }
                    }
                    catch { }
                }
                return false;
            }

            try
            {
                string fieldName = list.Fields.Add(internalName, fieldType, false);
                SPField field = list.Fields.GetFieldByInternalName(fieldName);
                if (field != null)
                {
                    field.Title = displayName;
                    if (defaultValue != null) field.DefaultValue = defaultValue;
                    field.Update();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool DoneThisRequest()
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                return ctx != null && ctx.Items.Contains("HomeNoraFutureProvDone");
            }
            catch { return false; }
        }

        private static void MarkDoneThisRequest()
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null) ctx.Items["HomeNoraFutureProvDone"] = true;
            }
            catch { }
        }

        internal static string CurrentUrl()
        {
            try
            {
                HttpContext ctx = HttpContext.Current;
                return ctx != null && ctx.Request != null ? ctx.Request.Url.ToString() : "";
            }
            catch { return ""; }
        }

        #endregion
    }

    /// <summary>
    /// Read side for the home page (cached) plus field helpers shared with the admin control.
    /// </summary>
    public static class HomeNoraFutureRepository
    {
        private const string CacheKeyPrefix = "PNU.HomeNoraFuture.Section.";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan FailureCacheDuration = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Returns the section data for the current site collection, cached for a few minutes.
        /// Admin saves call InvalidateCache() so changes show immediately on this server;
        /// other front-ends in the farm pick them up when their cache expires.
        /// </summary>
        public static HomeNoraFutureSectionData GetSectionData()
        {
            if (SPContext.Current == null) return new HomeNoraFutureSectionData { Loaded = false };

            Guid siteId = SPContext.Current.Site.ID;
            string key = CacheKeyPrefix + siteId;

            HomeNoraFutureSectionData cached = HttpRuntime.Cache[key] as HomeNoraFutureSectionData;
            if (cached != null) return cached;

            HomeNoraFutureSectionData data = LoadFromSharePoint(siteId);
            HttpRuntime.Cache.Insert(key, data, null,
                DateTime.UtcNow.Add(data.Loaded ? CacheDuration : FailureCacheDuration),
                Cache.NoSlidingExpiration);
            return data;
        }

        public static void InvalidateCache()
        {
            try
            {
                if (SPContext.Current == null) return;
                HttpRuntime.Cache.Remove(CacheKeyPrefix + SPContext.Current.Site.ID);
            }
            catch { }
        }

        private static HomeNoraFutureSectionData LoadFromSharePoint(Guid siteId)
        {
            HomeNoraFutureSectionData data = new HomeNoraFutureSectionData { Loaded = false, ActiveItems = new List<HomeNoraFutureItem>() };

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(siteId))
                    using (SPWeb web = HomeNoraFutureProvisioner.OpenAdminWeb(site))
                    {
                        if (web == null) return;

                        SPList itemsList = web.Lists.TryGetList(HomeNoraFutureProvisioner.ListNameItems);
                        if (itemsList == null) return;

                        SPListItem titleItem = GetTitleItem(web.Lists.TryGetList(HomeNoraFutureProvisioner.ListNameTitle));
                        if (titleItem != null)
                        {
                            data.Title = new HomeNoraFutureTitleModel
                            {
                                ID = titleItem.ID,
                                Title = SafeString(titleItem, "Title"),
                                Title_EN = SafeString(titleItem, "Title_EN"),
                                AriaLabel = SafeString(titleItem, "AriaLabel"),
                                AriaLabel_EN = SafeString(titleItem, "AriaLabel_EN"),
                                ShowSection = SafeBool(titleItem, "ShowSection", true)
                            };
                        }

                        // Active filter is applied in memory with the same "empty = active" rule the admin grid uses
                        foreach (SPListItem row in itemsList.GetItems(BuildOrderedQuery()))
                        {
                            HomeNoraFutureItem item = MapItem(row);
                            if (item.IsActive) data.ActiveItems.Add(item);
                        }

                        data.Loaded = true;
                    }
                });
            }
            catch (Exception ex)
            {
                data.Loaded = false;
                Publics.WriteToLog(HomeNoraFutureProvisioner.CurrentUrl(), "HomeNoraFutureRepository.LoadFromSharePoint", ex.Message);
            }

            return data;
        }

        /// <summary>The section title is a single-row list; always use the oldest row.</summary>
        public static SPListItem GetTitleItem(SPList titleList)
        {
            if (titleList == null) return null;
            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='ID' Ascending='TRUE' /></OrderBy>",
                RowLimit = 1
            };
            SPListItemCollection items = titleList.GetItems(query);
            return items.Count > 0 ? items[0] : null;
        }

        public static SPQuery BuildOrderedQuery()
        {
            return new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /><FieldRef Name='ID' Ascending='TRUE' /></OrderBy>"
            };
        }

        public static HomeNoraFutureItem MapItem(SPListItem row)
        {
            return new HomeNoraFutureItem
            {
                ID = row.ID,
                Title = SafeString(row, "Title"),
                Title_EN = SafeString(row, "Title_EN"),
                Description = SafeString(row, "Description"),
                Description_EN = SafeString(row, "Description_EN"),
                LinkUrl = SafeString(row, "LinkUrl"),
                LinkUrl_EN = SafeString(row, "LinkUrl_EN"),
                IconClass = SafeString(row, "IconClass"),
                ItemOrder = SafeDouble(row, "ItemOrder", 0),
                IsActive = SafeBool(row, "IsActive", true)
            };
        }

        public static List<HomeNoraFutureItem> GetDefaultItems()
        {
            return new List<HomeNoraFutureItem>
            {
                new HomeNoraFutureItem
                {
                    Title = "برامج البكالوريوس", Title_EN = "Undergraduate Programs",
                    Description = "أكثر من 40 برنامجاً أكاديمياً", Description_EN = "More than 40 academic programs",
                    LinkUrl = "/ar/RegAdm/RegBSC", LinkUrl_EN = "/en/RegAdm/RegBSC",
                    IconClass = "hgi-book-01", ItemOrder = 1, IsActive = true
                },
                new HomeNoraFutureItem
                {
                    Title = "برامج الدراسات العليا", Title_EN = "Postgraduate Programs",
                    Description = "برامج الماجستير والدكتوراه", Description_EN = "Master and Doctoral Programs",
                    LinkUrl = "/ar/RegAdm/PGD", LinkUrl_EN = "/en/RegAdm/PGD",
                    IconClass = "hgi-book-02", ItemOrder = 2, IsActive = true
                },
                new HomeNoraFutureItem
                {
                    Title = "برامج الدبلوم", Title_EN = "Diploma Programs",
                    Description = "برامج الدبلوم المهني والأكاديمي", Description_EN = "Diploma Programs",
                    LinkUrl = "/ar/RegAdm/HDIP", LinkUrl_EN = "/en/RegAdm/HDIP",
                    IconClass = "hgi-diploma", ItemOrder = 3, IsActive = true
                }
            };
        }

        /// <summary>
        /// Allows site-relative links ("/ar/..."), in-page anchors and absolute http(s) URLs only.
        /// Blocks javascript:, data:, protocol-relative "//host" and anything else.
        /// </summary>
        public static bool IsSafeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            string u = url.Trim();
            if (u.StartsWith("//", StringComparison.Ordinal)) return false;
            if (u.StartsWith("/", StringComparison.Ordinal) || u.StartsWith("#", StringComparison.Ordinal)) return true;
            return u.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || u.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        #region Field Helpers

        public static string SafeString(SPListItem item, string fieldName)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return string.Empty;
            if (!item.Fields.ContainsField(fieldName)) return string.Empty;
            object val = item[fieldName];
            return val != null ? val.ToString() : string.Empty;
        }

        public static bool SafeBool(SPListItem item, string fieldName, bool defaultValue)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return defaultValue;
            if (!item.Fields.ContainsField(fieldName)) return defaultValue;
            object val = item[fieldName];
            if (val == null) return defaultValue;
            if (val is bool) return (bool)val;

            string s = val.ToString().Trim();
            if (s == "1") return true;
            if (s == "0") return false;
            bool result;
            return bool.TryParse(s, out result) ? result : defaultValue;
        }

        public static double SafeDouble(SPListItem item, string fieldName, double defaultValue)
        {
            if (item == null || string.IsNullOrEmpty(fieldName)) return defaultValue;
            if (!item.Fields.ContainsField(fieldName)) return defaultValue;
            object val = item[fieldName];
            if (val == null) return defaultValue;
            if (val is double) return (double)val;
            double result;
            return double.TryParse(val.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result) ? result : defaultValue;
        }

        #endregion
    }
}
