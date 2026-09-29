//using HarmonyLib;

//namespace ProbablyNuclear
//{
//    [HarmonyPatch(typeof(DebugPanel), nameof(DebugPanel.AddProceduralDebugFunction))]
//    public static class DebugPatch
//    {
//        static void Postfix()
//        {
//            DebugPanel.AddNewDebugAction("Spawn ProbablyNuclear Crate Kit", delegate
//            {
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, ModPreBuiltItems.LootCrateSyndicate());
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("emag"));
//            }, DebugPanel.Instance.spawnGroup);

//            DebugPanel.AddNewDebugAction("Spawn ProbablyNuclear Items", delegate
//            {
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("cobra"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("cybersun_pen"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("emag"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("energy_sword"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("holoparasite_injector"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("hyperzine_injector"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("implanter"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("interdyne_herbals"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("minibomb"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("suspicious_toolbox"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("syndicate_business_card"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("syndicate_gas_mask"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("syndicate_soap"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("telecrystal"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("throwing_knife"));
//                GraphUtils.TryAcceptAll(EmporiumEntry.Instance.invElement, DirectoryMaster.Item("uplink"));
//            }, DebugPanel.Instance.spawnGroup);
//        }
//    }
//}
