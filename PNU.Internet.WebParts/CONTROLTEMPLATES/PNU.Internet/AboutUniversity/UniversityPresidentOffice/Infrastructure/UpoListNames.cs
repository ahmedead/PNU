using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice
{
    /// <summary>Central list-name constants for the University President Office (كلمة رئيسة الجامعة) page.</summary>
    public static class UpoListNames
    {
        public const string Sections  = "UpoSections";
        public const string Contacts  = "UpoContacts";
        public const string Signature = "UpoSignature";

        /// <summary>Content lists — these are what ucUpoAdmin offers in its picker.</summary>
        public static readonly string[] AllLists = new string[]
        {
            Sections, Contacts, Signature
        };
    }
}
