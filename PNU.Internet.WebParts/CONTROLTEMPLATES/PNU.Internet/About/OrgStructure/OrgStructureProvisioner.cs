using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SharePoint;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.About.OrgStructure
{
    /// <summary>
    /// Idempotent provisioner for the OrgStructureUnits list. Creates the list and
    /// its fields if missing, grants anonymous read, and seeds the 84 default units
    /// ONLY when the list is first created (never re-seeds an existing list).
    /// Field provisioning is C# only — no PowerShell.
    /// </summary>
    public static class OrgStructureProvisioner
    {
        public const string ListName = "OrgStructureUnits";

        // Internal field names
        public const string F_Order = "OrgOrder";
        public const string F_Selector = "OrgSelector";
        public const string F_TitleEn = "TitleEn";
        public const string F_Desc = "OrgDesc";
        public const string F_DescEn = "OrgDescEn";
        public const string F_Badge = "OrgBadge";
        public const string F_BadgeEn = "OrgBadgeEn";
        public const string F_Meta = "OrgMeta";
        public const string F_MetaEn = "OrgMetaEn";
        public const string F_Icon = "OrgIcon";
        public const string F_Theme = "OrgTheme";
        public const string F_LinkUrl = "OrgLinkUrl";
        public const string F_Active = "OrgActive";

        /// <summary>
        /// Ensures the list exists with all fields and (on first creation) seed data.
        /// Call from OnInit for AUTHENTICATED users only.
        /// </summary>
        public static SPList EnsureList(SPWeb web)
        {
            if (web == null) return null;

            SPList list = web.Lists.TryGetList(ListName);
            bool created = false;

            bool allowUnsafe = web.AllowUnsafeUpdates;
            try
            {
                web.AllowUnsafeUpdates = true;

                if (list == null)
                {
                    Guid id = web.Lists.Add(ListName, "PNU organizational structure units", SPListTemplateType.GenericList);
                    list = web.Lists[id];
                    created = true;
                }

                EnsureField(list, F_Order, SPFieldType.Number, "Order");
                EnsureField(list, F_Selector, SPFieldType.Note, "Selector");
                EnsureField(list, F_TitleEn, SPFieldType.Text, "Title (EN)");
                EnsureField(list, F_Desc, SPFieldType.Note, "Description (AR)");
                EnsureField(list, F_DescEn, SPFieldType.Note, "Description (EN)");
                EnsureField(list, F_Badge, SPFieldType.Text, "Badge (AR)");
                EnsureField(list, F_BadgeEn, SPFieldType.Text, "Badge (EN)");
                EnsureField(list, F_Meta, SPFieldType.Text, "Meta (AR)");
                EnsureField(list, F_MetaEn, SPFieldType.Text, "Meta (EN)");
                EnsureField(list, F_Icon, SPFieldType.Text, "Icon");
                EnsureField(list, F_Theme, SPFieldType.Text, "Theme");
                // Link stored as plain text Note (URLs can exceed 255 chars / avoid Hyperlink field limits).
                EnsureField(list, F_LinkUrl, SPFieldType.Note, "Link URL");
                EnsureField(list, F_Active, SPFieldType.Boolean, "Active");

                list.Update();

                if (created)
                {
                    Seed(list);
                }

                GrantAnonymousRead(web, list);
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "OrgStructureProvisioner.EnsureList", ex.Message);
            }
            finally
            {
                web.AllowUnsafeUpdates = allowUnsafe;
            }

            return list;
        }

        private static void EnsureField(SPList list, string internalName, SPFieldType type, string displayName)
        {
            if (list.Fields.ContainsField(internalName)) return;

            string created = list.Fields.Add(internalName, type, false);
            SPField fld = list.Fields.GetFieldByInternalName(created);

            if (type == SPFieldType.Note)
            {
                SPFieldMultiLineText note = fld as SPFieldMultiLineText;
                if (note != null)
                {
                    note.RichText = false;   // Plain text mode
                    note.Update();
                }
            }

            if (!string.IsNullOrEmpty(displayName) && fld.Title != displayName)
            {
                fld.Title = displayName;
                fld.Update();
            }
        }

        private static void GrantAnonymousRead(SPWeb web, SPList list)
        {
            try
            {
                if (list == null) return;
                list.BreakRoleInheritance(true, false);
                list.AnonymousPermMask64 = SPBasePermissions.ViewListItems | SPBasePermissions.ViewVersions | SPBasePermissions.Open;
                list.Update();
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(GetUrl(), "OrgStructureProvisioner.GrantAnonymousRead", ex.Message);
            }
        }

        private static void Seed(SPList list)
        {
            foreach (OrgStructureUnit u in DefaultUnits())
            {
                SPListItem item = list.AddItem();
                item["Title"] = u.Title;
                item[F_TitleEn] = u.TitleEn;
                item[F_Order] = u.Order;
                item[F_Selector] = u.Selector;
                item[F_Desc] = u.Description;
                item[F_DescEn] = u.DescriptionEn;
                item[F_Badge] = u.Badge;
                item[F_BadgeEn] = u.BadgeEn;
                item[F_Meta] = u.Meta;
                item[F_MetaEn] = u.MetaEn;
                item[F_Icon] = u.Icon;
                item[F_Theme] = u.Theme;
                item[F_LinkUrl] = u.LinkUrl;
                item[F_Active] = u.Active;
                item.Update();
            }
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

        /// <summary>The 84 default PNU organizational units (Arabic seed content).</summary>
        public static List<OrgStructureUnit> DefaultUnits()
        {
            return new List<OrgStructureUnit>
            {
            new OrgStructureUnit { Order = 10, Selector = "rect[x=\"482.5\"][y=\"79.5\"][width=\"280\"][height=\"56\"]", Title = "مجلس الجامعة", TitleEn = "", Description = "مجلس الجامعة — جهة إشرافية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة إشرافية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "pnu-university-council.aspx", Active = true },
            new OrgStructureUnit { Order = 20, Selector = "rect[x=\"255.5\"][y=\"141.5\"][width=\"135\"][height=\"52\"]", Title = "المجالس الاستشارية", TitleEn = "", Description = "المجالس الاستشارية — جهة إشرافية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة إشرافية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 30, Selector = "rect[x=\"482.5\"][y=\"165.5\"][width=\"280\"][height=\"56\"]", Title = "رئيسة الجامعة", TitleEn = "", Description = "رئيسة الجامعة — قيادة تنفيذية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "قيادة تنفيذية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-hierarchy", Theme = "sa", LinkUrl = "university-president-message.aspx", Active = true },
            new OrgStructureUnit { Order = 40, Selector = "rect[x=\"852.5\"][y=\"172.5\"][width=\"135\"][height=\"52\"]", Title = "أمانة مجلس الجامعة", TitleEn = "", Description = "أمانة مجلس الجامعة — دعم إداري ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "دعم إداري", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "saSoft", LinkUrl = "pnu-university-council.aspx", Active = true },
            new OrgStructureUnit { Order = 50, Selector = "rect[x=\"255.5\"][y=\"203.5\"][width=\"135\"][height=\"52\"]", Title = "مجالس التوجهات الاستراتيجية", TitleEn = "", Description = "مجالس التوجهات الاستراتيجية — جهة إشرافية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة إشرافية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 60, Selector = "rect[x=\"632.5\"][y=\"234.5\"][width=\"135\"][height=\"40\"]", Title = "مكتب رئيسة الجامعة", TitleEn = "", Description = "مكتب رئيسة الجامعة — قيادة تنفيذية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "قيادة تنفيذية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-hierarchy", Theme = "saMuted", LinkUrl = "university-president-message.aspx", Active = true },
            new OrgStructureUnit { Order = 70, Selector = "rect[x=\"477.5\"][y=\"234.5\"][width=\"135\"][height=\"40\"]", Title = "مكتب رعاية المستفيدين", TitleEn = "", Description = "مكتب رعاية المستفيدين — مكتب ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مكتب", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-calendar-03", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 80, Selector = "rect[x=\"1073.5\"][y=\"493.5\"][width=\"163\"][height=\"80\"]", Title = "وكالة الجامعة", TitleEn = "", Description = "وكالة الجامعة — وكالة رئيسية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "وكالة رئيسية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-hierarchy", Theme = "sa", LinkUrl = "agency-details.aspx", Active = true },
            new OrgStructureUnit { Order = 90, Selector = "rect[x=\"864.5\"][y=\"493.5\"][width=\"163\"][height=\"80\"]", Title = "وكالة الجامعة للشؤون الأكاديمية", TitleEn = "", Description = "وكالة الجامعة للشؤون الأكاديمية — وكالة رئيسية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "وكالة رئيسية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-hierarchy", Theme = "sa", LinkUrl = "agency-details.aspx", Active = true },
            new OrgStructureUnit { Order = 100, Selector = "rect[x=\"216.5\"][y=\"493.5\"][width=\"163\"][height=\"80\"]", Title = "وكالة الجامعة للدراسات العليا والبحث العلمي", TitleEn = "", Description = "وكالة الجامعة للدراسات العليا والبحث العلمي — وكالة رئيسية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "وكالة رئيسية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-hierarchy", Theme = "sa", LinkUrl = "agency-details.aspx", Active = true },
            new OrgStructureUnit { Order = 110, Selector = "rect[x=\"1.5\"][y=\"493.5\"][width=\"163\"][height=\"80\"]", Title = "وكالة الجامعة للأصول والاستثمار", TitleEn = "", Description = "وكالة الجامعة للأصول والاستثمار — وكالة رئيسية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "وكالة رئيسية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-hierarchy", Theme = "sa", LinkUrl = "agency-details.aspx", Active = true },
            new OrgStructureUnit { Order = 120, Selector = "rect[x=\"1073.5\"][y=\"589.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الميزانية", TitleEn = "", Description = "إدارة الميزانية — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 130, Selector = "rect[x=\"864.5\"][y=\"589.5\"][width=\"163\"][height=\"52\"]", Title = "عمادة القبول والتسجيل", TitleEn = "", Description = "عمادة القبول والتسجيل — عمادة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "عمادة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "saSoft", LinkUrl = "deanship-details.aspx", Active = true },
            new OrgStructureUnit { Order = 140, Selector = "rect[x=\"655.5\"][y=\"589.5\"][width=\"163\"][height=\"52\"]", Title = "مركز سارة السديري لدراسات المرأة", TitleEn = "", Description = "مركز سارة السديري لدراسات المرأة — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 150, Selector = "rect[x=\"426.5\"][y=\"589.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للشؤون الصحية", TitleEn = "", Description = "الإدارة العامة للشؤون الصحية — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 160, Selector = "rect[x=\"216.5\"][y=\"589.5\"][width=\"163\"][height=\"52\"]", Title = "عمادة الدراسات العليا", TitleEn = "", Description = "عمادة الدراسات العليا — عمادة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "عمادة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "saSoft", LinkUrl = "deanship-details.aspx", Active = true },
            new OrgStructureUnit { Order = 170, Selector = "rect[x=\"1.5\"][y=\"589.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للمنشآت والتشغيل", TitleEn = "", Description = "الإدارة العامة للمنشآت والتشغيل — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 180, Selector = "rect[x=\"1073.5\"][y=\"653.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة المالية", TitleEn = "", Description = "الإدارة المالية — جهة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 190, Selector = "rect[x=\"864.5\"][y=\"653.5\"][width=\"163\"][height=\"52\"]", Title = "عمادة التطوير والجودة", TitleEn = "", Description = "عمادة التطوير والجودة — عمادة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "عمادة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "saSoft", LinkUrl = "deanship-details.aspx", Active = true },
            new OrgStructureUnit { Order = 200, Selector = "rect[x=\"655.5\"][y=\"653.5\"][width=\"163\"][height=\"52\"]", Title = "المرصد الوطني للمرأة", TitleEn = "", Description = "المرصد الوطني للمرأة — جهة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 210, Selector = "rect[x=\"426.5\"][y=\"653.5\"][width=\"135\"][height=\"52\"]", Title = "مستشفى الملك عبدالله بن عبدالعزيز الجامعي", TitleEn = "", Description = "مستشفى الملك عبدالله بن عبدالعزيز الجامعي — مستشفى جامعي ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مستشفى جامعي", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saSoft", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 220, Selector = "rect[x=\"216.5\"][y=\"653.5\"][width=\"163\"][height=\"52\"]", Title = "عمادة البحث العلمي والمكتبات", TitleEn = "", Description = "عمادة البحث العلمي والمكتبات — عمادة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "عمادة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "saSoft", LinkUrl = "deanship-details.aspx", Active = true },
            new OrgStructureUnit { Order = 230, Selector = "rect[x=\"1.5\"][y=\"653.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للاستثمار", TitleEn = "", Description = "الإدارة العامة للاستثمار — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 240, Selector = "rect[x=\"1073.5\"][y=\"717.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة المشتريات والمناقصات", TitleEn = "", Description = "إدارة المشتريات والمناقصات — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 250, Selector = "rect[x=\"864.5\"][y=\"717.5\"][width=\"163\"][height=\"52\"]", Title = "عمادة شؤون الطالبات", TitleEn = "", Description = "عمادة شؤون الطالبات — عمادة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "عمادة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "saSoft", LinkUrl = "deanship-details.aspx", Active = true },
            new OrgStructureUnit { Order = 260, Selector = "rect[x=\"655.5\"][y=\"717.5\"][width=\"163\"][height=\"52\"]", Title = "مركز أبحاث السلامة السلوكية والنفسية", TitleEn = "", Description = "مركز أبحاث السلامة السلوكية والنفسية — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 270, Selector = "rect[x=\"426.5\"][y=\"717.5\"][width=\"135\"][height=\"52\"]", Title = "مركز المهارات والمحاكاة الطبي", TitleEn = "", Description = "مركز المهارات والمحاكاة الطبي — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 280, Selector = "rect[x=\"216.5\"][y=\"717.5\"][width=\"163\"][height=\"52\"]", Title = "المجلس العلمي", TitleEn = "", Description = "المجلس العلمي — جهة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "primary", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 290, Selector = "rect[x=\"1.5\"][y=\"717.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الإيرادات البديلة", TitleEn = "", Description = "إدارة الإيرادات البديلة — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 300, Selector = "rect[x=\"1073.5\"][y=\"781.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة المستودعات", TitleEn = "", Description = "إدارة المستودعات — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 310, Selector = "rect[x=\"864.5\"][y=\"781.5\"][width=\"163\"][height=\"52\"]", Title = "مركز الدعم الطلابي والمهني", TitleEn = "", Description = "مركز الدعم الطلابي والمهني — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 320, Selector = "rect[x=\"655.5\"][y=\"781.5\"][width=\"163\"][height=\"52\"]", Title = "معهد التنمية والخدمات الاستشارية", TitleEn = "", Description = "معهد التنمية والخدمات الاستشارية — معهد ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "معهد", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-globe", Theme = "saMuted", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 330, Selector = "rect[x=\"426.5\"][y=\"781.5\"][width=\"135\"][height=\"52\"]", Title = "مركز أبحاث العلوم الطبيعية والصحية", TitleEn = "", Description = "مركز أبحاث العلوم الطبيعية والصحية — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 340, Selector = "rect[x=\"216.5\"][y=\"781.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة كراسي البحث", TitleEn = "", Description = "إدارة كراسي البحث — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 350, Selector = "rect[x=\"1.5\"][y=\"781.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الأصول", TitleEn = "", Description = "إدارة الأصول — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 360, Selector = "rect[x=\"1073.5\"][y=\"845.5\"][width=\"163\"][height=\"52\"]", Title = "مركز الوثائق والمحفوظات", TitleEn = "", Description = "مركز الوثائق والمحفوظات — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saSoft", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 370, Selector = "rect[x=\"864.5\"][y=\"845.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة التعلم الإلكتروني", TitleEn = "", Description = "إدارة التعلم الإلكتروني — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 380, Selector = "rect[x=\"655.5\"][y=\"845.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الأوقاف", TitleEn = "", Description = "إدارة الأوقاف — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 390, Selector = "rect[x=\"426.5\"][y=\"845.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للحوكمة والمخاطر والالتزام", TitleEn = "", Description = "الإدارة العامة للحوكمة والمخاطر والالتزام — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 400, Selector = "rect[x=\"216.5\"][y=\"845.5\"][width=\"163\"][height=\"52\"]", Title = "مركز الأبحاث العلمية", TitleEn = "", Description = "مركز الأبحاث العلمية — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 410, Selector = "rect[x=\"1.5\"][y=\"845.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة السلامة والمخاطر", TitleEn = "", Description = "إدارة السلامة والمخاطر — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 420, Selector = "rect[x=\"1073.5\"][y=\"909.5\"][width=\"163\"][height=\"52\"]", Title = "الاتصالات الإدارية المركزية", TitleEn = "", Description = "الاتصالات الإدارية المركزية — جهة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 430, Selector = "rect[x=\"864.5\"][y=\"909.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة البرامج الأكاديمية", TitleEn = "", Description = "إدارة البرامج الأكاديمية — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 440, Selector = "rect[x=\"655.5\"][y=\"909.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الإعلام والتأثير المعرفي", TitleEn = "", Description = "إدارة الإعلام والتأثير المعرفي — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 450, Selector = "rect[x=\"426.5\"][y=\"909.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للموارد البشرية", TitleEn = "", Description = "الإدارة العامة للموارد البشرية — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 460, Selector = "rect[x=\"216.5\"][y=\"909.5\"][width=\"163\"][height=\"52\"]", Title = "مركز الابتكار وريادة الأعمال", TitleEn = "", Description = "مركز الابتكار وريادة الأعمال — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 470, Selector = "rect[x=\"1073.5\"][y=\"973.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الحركة والنقل", TitleEn = "", Description = "إدارة الحركة والنقل — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 480, Selector = "rect[x=\"864.5\"][y=\"973.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة برنامج السنة التأسيسية للكليات الصحية", TitleEn = "", Description = "إدارة برنامج السنة التأسيسية للكليات الصحية — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 490, Selector = "rect[x=\"655.5\"][y=\"973.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة القانونية", TitleEn = "", Description = "الإدارة القانونية — جهة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 500, Selector = "rect[x=\"426.5\"][y=\"973.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للتحول الرقمي", TitleEn = "", Description = "الإدارة العامة للتحول الرقمي — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 510, Selector = "rect[x=\"216.5\"][y=\"973.5\"][width=\"163\"][height=\"52\"]", Title = "مركز الأبحاث الإنسانية", TitleEn = "", Description = "مركز الأبحاث الإنسانية — مركز ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مركز", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 520, Selector = "rect[x=\"1073.5\"][y=\"1037.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للأمن", TitleEn = "", Description = "الإدارة العامة للأمن — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 530, Selector = "rect[x=\"864.5\"][y=\"1037.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة مقررات المتطلبات العامة", TitleEn = "", Description = "إدارة مقررات المتطلبات العامة — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 540, Selector = "rect[x=\"655.5\"][y=\"1037.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة مراقبة المخزون", TitleEn = "", Description = "إدارة مراقبة المخزون — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 550, Selector = "rect[x=\"426.5\"][y=\"1037.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للمراجعة الداخلية", TitleEn = "", Description = "الإدارة العامة للمراجعة الداخلية — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 560, Selector = "rect[x=\"216.5\"][y=\"1037.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الابتعاث والتدريب والإشراف المشترك", TitleEn = "", Description = "إدارة الابتعاث والتدريب والإشراف المشترك — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 570, Selector = "rect[x=\"864.5\"][y=\"1101.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة التبادل الطلابي", TitleEn = "", Description = "إدارة التبادل الطلابي — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 580, Selector = "rect[x=\"655.5\"][y=\"1101.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الأمن السيبراني", TitleEn = "", Description = "إدارة الأمن السيبراني — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 590, Selector = "rect[x=\"426.5\"][y=\"1101.5\"][width=\"163\"][height=\"52\"]", Title = "الإدارة العامة للاتصال المؤسسي", TitleEn = "", Description = "الإدارة العامة للاتصال المؤسسي — إدارة عامة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة عامة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 600, Selector = "rect[x=\"216.5\"][y=\"1101.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة المؤتمرات والندوات", TitleEn = "", Description = "إدارة المؤتمرات والندوات — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 610, Selector = "rect[x=\"864.5\"][y=\"1165.5\"][width=\"163\"][height=\"52\"]", Title = "الكليات والمعاهد", TitleEn = "", Description = "الكليات والمعاهد — جهة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "جهة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "primary", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 620, Selector = "rect[x=\"655.5\"][y=\"1165.5\"][width=\"163\"][height=\"52\"]", Title = "وحدة التوعية الفكرية", TitleEn = "", Description = "وحدة التوعية الفكرية — وحدة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "وحدة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 630, Selector = "rect[x=\"426.5\"][y=\"1165.5\"][width=\"163\"][height=\"52\"]", Title = "بوابة نورة للتحول", TitleEn = "", Description = "بوابة نورة للتحول — منصة رقمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "منصة رقمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-globe", Theme = "saMuted", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 640, Selector = "rect[x=\"216.5\"][y=\"1165.5\"][width=\"163\"][height=\"52\"]", Title = "إدارة الجمعيات والمجلات العلمية", TitleEn = "", Description = "إدارة الجمعيات والمجلات العلمية — إدارة ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "إدارة", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-chart", Theme = "saSoft", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 650, Selector = "rect[x=\"655.5\"][y=\"1229.5\"][width=\"163\"][height=\"52\"]", Title = "مكتب إدارة البيانات", TitleEn = "", Description = "مكتب إدارة البيانات — مكتب ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "مكتب", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-calendar-03", Theme = "saMuted", LinkUrl = "all-departments.aspx", Active = true },
            new OrgStructureUnit { Order = 660, Selector = "rect[x=\"216.5\"][y=\"1229.5\"][width=\"163\"][height=\"52\"]", Title = "وحدة البحث والتطوير والابتكار", TitleEn = "", Description = "وحدة البحث والتطوير والابتكار — وحدة تنظيمية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "وحدة تنظيمية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-target-01", Theme = "saSoft", LinkUrl = "", Active = true },
            new OrgStructureUnit { Order = 670, Selector = "rect[x=\"949.5\"][y=\"1415.5\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية الطب البشري", TitleEn = "", Description = "كلية الطب البشري — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 680, Selector = "rect[x=\"789.5\"][y=\"1415.5\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية طب الأسنان", TitleEn = "", Description = "كلية طب الأسنان — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 690, Selector = "rect[x=\"629.5\"][y=\"1415.5\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية الصيدلية", TitleEn = "", Description = "كلية الصيدلية — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 700, Selector = "rect[x=\"469.5\"][y=\"1415.5\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية الصحة وعلم التأهيل", TitleEn = "", Description = "كلية الصحة وعلم التأهيل — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 710, Selector = "rect[x=\"309.5\"][y=\"1415.5\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية التمريض", TitleEn = "", Description = "كلية التمريض — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 720, Selector = "rect[x=\"149.5\"][y=\"1415.5\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية العلوم", TitleEn = "", Description = "كلية العلوم — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 730, Selector = "rect[x=\"949.5\"][y=\"1493.62\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية علوم الرياضة والنشاط البدني", TitleEn = "", Description = "كلية علوم الرياضة والنشاط البدني — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 740, Selector = "rect[x=\"789.5\"][y=\"1493.62\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية العلوم الأنسانية والإجتماعية", TitleEn = "", Description = "كلية العلوم الأنسانية والإجتماعية — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 750, Selector = "rect[x=\"629.5\"][y=\"1493.62\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية التربية والتنمية البشرية", TitleEn = "", Description = "كلية التربية والتنمية البشرية — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 760, Selector = "rect[x=\"469.5\"][y=\"1493.62\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية اللغات", TitleEn = "", Description = "كلية اللغات — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 770, Selector = "rect[x=\"309.5\"][y=\"1493.62\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية التصاميم والفنون", TitleEn = "", Description = "كلية التصاميم والفنون — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 780, Selector = "rect[x=\"149.5\"][y=\"1493.62\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية القانون", TitleEn = "", Description = "كلية القانون — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 790, Selector = "rect[x=\"949.5\"][y=\"1571.74\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية علوم الحاسب والمعلومات", TitleEn = "", Description = "كلية علوم الحاسب والمعلومات — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 800, Selector = "rect[x=\"789.5\"][y=\"1571.74\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية إدارة الأعمال", TitleEn = "", Description = "كلية إدارة الأعمال — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 810, Selector = "rect[x=\"629.5\"][y=\"1571.74\"][width=\"135\"][height=\"52.7573\"]", Title = "كلية الهندسة", TitleEn = "", Description = "كلية الهندسة — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 820, Selector = "rect[x=\"469.5\"][y=\"1571.74\"][width=\"135\"][height=\"52.7573\"]", Title = "الكلية التطبيقية", TitleEn = "", Description = "الكلية التطبيقية — كلية ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "كلية", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-user-group", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 830, Selector = "rect[x=\"309.5\"][y=\"1571.74\"][width=\"135\"][height=\"52.7573\"]", Title = "معهد تعلم اللغة العربية للناطقات بغيرها", TitleEn = "", Description = "معهد تعلم اللغة العربية للناطقات بغيرها — معهد ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "معهد", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-globe", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            new OrgStructureUnit { Order = 840, Selector = "rect[x=\"149.5\"][y=\"1571.74\"][width=\"135\"][height=\"52.7573\"]", Title = "معهد اللغة الإنجليزية", TitleEn = "", Description = "معهد اللغة الإنجليزية — معهد ضمن الهيكل التنظيمي لجامعة الأميرة نورة بنت عبدالرحمن.", DescriptionEn = "", Badge = "معهد", BadgeEn = "", Meta = "الهيكل التنظيمي", MetaEn = "", Icon = "hgi-globe", Theme = "primary", LinkUrl = "faculty-details.aspx", Active = true },
            };
        }
    }
}
