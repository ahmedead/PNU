using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Agency.DGA
{
    /// <summary>Central list-name constants for the University Agency (وكالة الجامعة) page.</summary>
    public static class AgListNames
    {
        public const string Overview   = "AgOverview";
        public const string Cards      = "AgCards";
        public const string Objectives = "AgObjectives";
        public const string DeputyWord = "AgDeputyWord";
        public const string MainTasks  = "AgMainTasks";

        public static readonly string[] AllLists = new string[]
        {
            Overview, Cards, Objectives, DeputyWord, MainTasks
        };
    }
}
