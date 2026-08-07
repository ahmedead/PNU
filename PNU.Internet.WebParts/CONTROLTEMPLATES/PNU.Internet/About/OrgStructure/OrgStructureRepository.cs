using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About.OrgStructure
{
    /// <summary>
    /// Read/write access for the OrgStructureUnits list. Reads run under elevated
    /// privileges (site/web reopened by ID inside the delegate) so anonymous portal
    /// visitors can see the chart. Field mapping is manual via Safe* helpers.
    /// </summary>
    public class OrgStructureRepository
    {
        private readonly Guid _siteId;
        private readonly Guid _webId;

        public OrgStructureRepository(SPWeb web)
        {
            if (web == null) throw new ArgumentNullException("web");
            _siteId = web.Site.ID;
            _webId = web.ID;
        }

        /// <summary>Returns all active units ordered by OrgOrder, resolved for anonymous read.</summary>
        public List<OrgStructureUnit> GetActiveUnits()
        {
            var result = new List<OrgStructureUnit>();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (var site = new SPSite(_siteId))
                    using (var web = site.OpenWeb(_webId))
                    {
                        SPList list = web.Lists.TryGetList(OrgStructureProvisioner.ListName);
                        if (list == null) return;

                        var query = new SPQuery
                        {
                            Query =
                                "<Where><Eq><FieldRef Name='" + OrgStructureProvisioner.F_Active +
                                "'/><Value Type='Boolean'>1</Value></Eq></Where>" +
                                "<OrderBy><FieldRef Name='" + OrgStructureProvisioner.F_Order + "' Ascending='TRUE'/></OrderBy>",
                            ViewAttributes = "Scope='RecursiveAll'"
                        };

                        foreach (SPListItem item in list.GetItems(query))
                        {
                            result.Add(Map(item));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "OrgStructureRepository.GetActiveUnits", ex.Message);
            }

            return result;
        }

        /// <summary>Returns every unit (active + inactive) for the admin grid.</summary>
        public List<OrgStructureUnit> GetAllUnits()
        {
            var result = new List<OrgStructureUnit>();
            try
            {
                SPList list = SPContext.Current.Web.Lists.TryGetList(OrgStructureProvisioner.ListName);
                if (list == null) return result;

                var query = new SPQuery
                {
                    Query = "<OrderBy><FieldRef Name='" + OrgStructureProvisioner.F_Order + "' Ascending='TRUE'/></OrderBy>",
                    ViewAttributes = "Scope='RecursiveAll'"
                };

                foreach (SPListItem item in list.GetItems(query))
                {
                    result.Add(Map(item));
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "OrgStructureRepository.GetAllUnits", ex.Message);
            }
            return result;
        }

        public OrgStructureUnit GetById(int id)
        {
            try
            {
                SPList list = SPContext.Current.Web.Lists.TryGetList(OrgStructureProvisioner.ListName);
                if (list == null) return null;
                return Map(list.GetItemById(id));
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "OrgStructureRepository.GetById", ex.Message);
                return null;
            }
        }

        public void Save(OrgStructureUnit u)
        {
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPList list = web.Lists.TryGetList(OrgStructureProvisioner.ListName);
                if (list == null) return;

                bool allowUnsafe = web.AllowUnsafeUpdates;
                try
                {
                    web.AllowUnsafeUpdates = true;
                    SPListItem item = (u.Id > 0) ? list.GetItemById(u.Id) : list.AddItem();
                    item["Title"] = u.Title ?? string.Empty;
                    item[OrgStructureProvisioner.F_TitleEn] = u.TitleEn ?? string.Empty;
                    item[OrgStructureProvisioner.F_Order] = u.Order;
                    item[OrgStructureProvisioner.F_Selector] = u.Selector ?? string.Empty;
                    item[OrgStructureProvisioner.F_Desc] = u.Description ?? string.Empty;
                    item[OrgStructureProvisioner.F_DescEn] = u.DescriptionEn ?? string.Empty;
                    item[OrgStructureProvisioner.F_Badge] = u.Badge ?? string.Empty;
                    item[OrgStructureProvisioner.F_BadgeEn] = u.BadgeEn ?? string.Empty;
                    item[OrgStructureProvisioner.F_Meta] = u.Meta ?? string.Empty;
                    item[OrgStructureProvisioner.F_MetaEn] = u.MetaEn ?? string.Empty;
                    item[OrgStructureProvisioner.F_Icon] = u.Icon ?? string.Empty;
                    item[OrgStructureProvisioner.F_Theme] = u.Theme ?? string.Empty;
                    item[OrgStructureProvisioner.F_LinkUrl] = u.LinkUrl ?? string.Empty;
                    item[OrgStructureProvisioner.F_Active] = u.Active;
                    item.Update();
                }
                finally
                {
                    web.AllowUnsafeUpdates = allowUnsafe;
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "OrgStructureRepository.Save", ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPList list = web.Lists.TryGetList(OrgStructureProvisioner.ListName);
                if (list == null) return;

                bool allowUnsafe = web.AllowUnsafeUpdates;
                try
                {
                    web.AllowUnsafeUpdates = true;
                    list.GetItemById(id).Delete();
                }
                finally
                {
                    web.AllowUnsafeUpdates = allowUnsafe;
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "OrgStructureRepository.Delete", ex.Message);
            }
        }

        private static OrgStructureUnit Map(SPListItem item)
        {
            return new OrgStructureUnit
            {
                Id = item.ID,
                Title = SafeString(item, "Title"),
                TitleEn = SafeString(item, OrgStructureProvisioner.F_TitleEn),
                Order = SafeInt(item, OrgStructureProvisioner.F_Order),
                Selector = SafeString(item, OrgStructureProvisioner.F_Selector),
                Description = SafeString(item, OrgStructureProvisioner.F_Desc),
                DescriptionEn = SafeString(item, OrgStructureProvisioner.F_DescEn),
                Badge = SafeString(item, OrgStructureProvisioner.F_Badge),
                BadgeEn = SafeString(item, OrgStructureProvisioner.F_BadgeEn),
                Meta = SafeString(item, OrgStructureProvisioner.F_Meta),
                MetaEn = SafeString(item, OrgStructureProvisioner.F_MetaEn),
                Icon = SafeString(item, OrgStructureProvisioner.F_Icon),
                Theme = SafeString(item, OrgStructureProvisioner.F_Theme),
                LinkUrl = SafeString(item, OrgStructureProvisioner.F_LinkUrl),
                Active = SafeBool(item, OrgStructureProvisioner.F_Active)
            };
        }

        private static string SafeString(SPListItem item, string field)
        {
            try
            {
                if (!item.Fields.ContainsField(field)) return string.Empty;
                object v = item[field];
                return v == null ? string.Empty : v.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        private static int SafeInt(SPListItem item, string field)
        {
            try
            {
                if (!item.Fields.ContainsField(field)) return 0;
                object v = item[field];
                if (v == null) return 0;
                int i;
                return int.TryParse(Convert.ToDouble(v, CultureInfo.InvariantCulture).ToString("0", CultureInfo.InvariantCulture), out i) ? i : 0;
            }
            catch { return 0; }
        }

        private static bool SafeBool(SPListItem item, string field)
        {
            try
            {
                if (!item.Fields.ContainsField(field)) return false;
                object v = item[field];
                return v != null && Convert.ToBoolean(v);
            }
            catch { return false; }
        }

        private static string GetUrl()
        {
            try
            {
                return System.Web.HttpContext.Current != null
                    ? System.Web.HttpContext.Current.Request.Url.ToString()
                    : string.Empty;
            }
            catch { return string.Empty; }
        }
    }
}
