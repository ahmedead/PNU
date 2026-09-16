using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.DGA.Infrastructure
{
    public class DgaPgFieldDef
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public SPFieldType Type { get; set; }
        public bool Required { get; set; }
        public string[] Choices { get; set; }
        public string DefaultValue { get; set; }

        public DgaPgFieldDef(string name, string displayName, SPFieldType type, bool required = false, string[] choices = null, string defaultValue = null)
        {
            Name = name;
            DisplayName = displayName;
            Type = type;
            Required = required;
            Choices = choices;
            DefaultValue = defaultValue;
        }
    }

    public class DgaPgListDef
    {
        public string Name { get; set; }
        public string DisplayTitle { get; set; }
        public string Description { get; set; }
        public List<DgaPgFieldDef> Fields { get; set; }
        public Func<List<Dictionary<string, object>>> SeedDataFactory { get; set; }

        public DgaPgListDef(string name, string displayTitle, string description)
        {
            Name = name;
            DisplayTitle = displayTitle;
            Description = description;
            Fields = new List<DgaPgFieldDef>();
        }

        public DgaPgListDef AddField(string name, string displayName, SPFieldType type, bool required = false, string[] choices = null, string defaultValue = null)
        {
            Fields.Add(new DgaPgFieldDef(name, displayName, type, required, choices, defaultValue));
            return this;
        }
    }

    public static partial class DgaPgListSchema
    {
        private static readonly Dictionary<string, DgaPgListDef> _lists = new Dictionary<string, DgaPgListDef>(StringComparer.OrdinalIgnoreCase);

        static DgaPgListSchema()
        {
            RegisterCoreLists();
            RegisterContentLists();
            RegisterProcedureLists();
            RegisterFaqLists();
            RegisterHubLists();
        }

        public static DgaPgListDef Get(string listName)
        {
            if (string.IsNullOrEmpty(listName)) return null;
            _lists.TryGetValue(listName, out DgaPgListDef def);
            return def;
        }

        public static IEnumerable<DgaPgListDef> All => _lists.Values;

        private static void Register(DgaPgListDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.Name))
            {
                _lists[def.Name] = def;
            }
        }
    }
}
