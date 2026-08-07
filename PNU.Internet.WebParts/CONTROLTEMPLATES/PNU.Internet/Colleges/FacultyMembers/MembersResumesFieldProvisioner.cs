using Microsoft.SharePoint;
using System;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers
{
    /// <summary>
    /// Ensures the new DGA faculty-member columns exist on the MembersResumes list.
    /// Call MembersResumesFieldProvisioner.EnsureFields() from a Feature Receiver
    /// (recommended) or it will run lazily from ucMemberResume / ucMemberDegrees.
    /// </summary>
    public static class MembersResumesFieldProvisioner
    {
        // Cached flag so the check runs only once per app-domain, not per request
        private static bool _ensured = false;
        private static readonly object _lock = new object();

        public static void EnsureFields()
        {
            if (_ensured) return;

            lock (_lock)
            {
                if (_ensured) return;

                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                        {
                            using (SPWeb web = site.OpenWeb("admin"))
                            {
                                SPList list = web.Lists.TryGetList("MembersResumes");
                                if (list == null) return;

                                bool updated = false;
                                web.AllowUnsafeUpdates = true;

                                updated |= EnsureNoteField(list, "ResearchInterests", "Research Interests / الاهتمامات البحثية");
                                updated |= EnsureNoteField(list, "Bachelor", "Bachelor / البكالوريوس");
                                updated |= EnsureNoteField(list, "Master", "Master / الماجستير");
                                updated |= EnsureNoteField(list, "Doctorate", "Doctorate / الدكتوراه");

                                if (updated)
                                    list.Update();

                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    });

                    _ensured = true;
                }
                catch (Exception ex)
                {
                    Publics.WriteToLog(
                        HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "",
                        "MembersResumesFieldProvisioner - EnsureFields", ex.Message);
                }
            }
        }

        /// <summary>Adds a plain-text multi-line (Note) field if it doesn't exist. Returns true if added.</summary>
        private static bool EnsureNoteField(SPList list, string internalName, string displayName)
        {
            if (list.Fields.ContainsField(internalName))
                return false;

            list.Fields.Add(internalName, SPFieldType.Note, false);
            SPFieldMultiLineText field = list.Fields.GetFieldByInternalName(internalName) as SPFieldMultiLineText;
            if (field != null)
            {
                field.Title = displayName;
                field.RichText = false;
                field.Update();
            }
            return true;
        }
    }
}
