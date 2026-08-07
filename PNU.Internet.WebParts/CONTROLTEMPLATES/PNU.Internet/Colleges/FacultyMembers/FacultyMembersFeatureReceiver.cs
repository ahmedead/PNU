using Microsoft.SharePoint;
using System;
using System.Runtime.InteropServices;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers
{
    /// <summary>
    /// OPTIONAL: attach to your existing (or a new) Feature in Visual Studio
    /// so the MembersResumes columns are created at deployment time instead
    /// of on first page load. If you use this, the lazy call inside
    /// ucMemberResume / ucMemberDegrees simply becomes a no-op check.
    ///
    /// Feature scope: Site (site collection).
    /// </summary>
    [Guid("d2f7a3c1-8b4e-4f6a-9c2d-1e5b7a9f0c3e")]
    public class FacultyMembersFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSite site = properties.Feature.Parent as SPSite;
                if (site == null) return;

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite elevatedSite = new SPSite(site.ID))
                    {
                        using (SPWeb web = elevatedSite.OpenWeb("admin"))
                        {
                            SPList list = web.Lists.TryGetList("MembersResumes");
                            if (list == null) return;

                            web.AllowUnsafeUpdates = true;

                            EnsureNoteField(list, "ResearchInterests", "Research Interests / الاهتمامات البحثية");
                            EnsureNoteField(list, "Bachelor", "Bachelor / البكالوريوس");
                            EnsureNoteField(list, "Master", "Master / الماجستير");
                            EnsureNoteField(list, "Doctorate", "Doctorate / الدكتوراه");

                            list.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                });
            
            }
            catch (Exception ex)
            {
                Publics.WriteToLog("FeatureActivated", "FacultyMembersFeatureReceiver", ex.Message);
            }
        }

        private static void EnsureNoteField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName))
                return;

            list.Fields.Add(internalName, SPFieldType.Note, false);
            SPFieldMultiLineText field = list.Fields.GetFieldByInternalName(internalName) as SPFieldMultiLineText;
            if (field != null)
            {
                field.Title = displayName;
                field.RichText = false;
                field.Update();
            }
        }
    }
}
