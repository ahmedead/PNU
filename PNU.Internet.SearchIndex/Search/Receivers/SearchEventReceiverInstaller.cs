using System;
using Microsoft.SharePoint;
using PNU.Internet.SearchIndex.Logging;

namespace PNU.Internet.SearchIndex.Receivers
{
    /// <summary>
    /// Attach SearchListEventReceiver to a list at runtime.
    /// </summary>
    public static class SearchEventReceiverInstaller
    {
        private static readonly string AssemblyName =
            typeof(SearchListEventReceiver).Assembly.FullName;

        private static readonly string ClassName =
            typeof(SearchListEventReceiver).FullName;

        public static void Install(SPList list)
        {
            if (list == null) return;
            try
            {
                EnsureReceiver(list, SPEventReceiverType.ItemAdded);
                EnsureReceiver(list, SPEventReceiverType.ItemUpdated);
                EnsureReceiver(list, SPEventReceiverType.ItemDeleting);
                EnsureReceiver(list, SPEventReceiverType.ItemCheckedIn);
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("EventReceiver",
                    "Install " + (list != null ? list.Title : ""),
                    ex.Message);
            }
        }

        public static void Uninstall(SPList list)
        {
            if (list == null) return;
            try
            {
                for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
                {
                    var def = list.EventReceivers[i];
                    if (string.Equals(def.Class, ClassName,
                            StringComparison.OrdinalIgnoreCase))
                        def.Delete();
                }
            }
            catch (Exception ex)
            {
                SearchLogger.WriteToLog("EventReceiver",
                    "Uninstall " + (list != null ? list.Title : ""),
                    ex.Message);
            }
        }

        private static void EnsureReceiver(SPList list, SPEventReceiverType type)
        {
            foreach (SPEventReceiverDefinition def in list.EventReceivers)
            {
                if (def.Type == type
                    && string.Equals(def.Class, ClassName,
                        StringComparison.OrdinalIgnoreCase))
                    return;
            }

            SPEventReceiverDefinition newDef = list.EventReceivers.Add();
            newDef.Type = type;
            newDef.Assembly = AssemblyName;
            newDef.Class = ClassName;
            newDef.Synchronization = SPEventReceiverSynchronization.Asynchronous;
            newDef.SequenceNumber = 10000;
            newDef.Update();
        }
    }
}
