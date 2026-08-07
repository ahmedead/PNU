using System;
using Microsoft.SharePoint;
using Portal.Main.Helper;

namespace PNU.Internet.Search.Receivers
{
    /// <summary>
    /// Programmatically attach SearchListEventReceiver to a list/library.
    /// Call from a Feature receiver, or from your provisioning code, so
    /// that every NEW list automatically becomes indexable.
    /// </summary>
    public static class SearchEventReceiverInstaller
    {
        // Use the strong name of YOUR assembly here (the one that
        // contains SearchListEventReceiver). Easiest way is to read
        // the runtime value below.
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
                
            }
        }

        private static void EnsureReceiver(SPList list, SPEventReceiverType type)
        {
            // Skip if already registered
            foreach (SPEventReceiverDefinition def in list.EventReceivers)
            {
                if (def.Type == type
                    && string.Equals(def.Class, ClassName,
                        StringComparison.OrdinalIgnoreCase))
                    return;
            }

            SPEventReceiverDefinition newDef = list.EventReceivers.Add();
            newDef.Type         = type;
            newDef.Assembly     = AssemblyName;
            newDef.Class        = ClassName;
            newDef.Synchronization = SPEventReceiverSynchronization.Asynchronous;
            newDef.SequenceNumber = 10000;
            newDef.Update();
        }
    }
}
