
using HarmonyLib;
using HarmonyLib.Tools;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppQFSW.QC;
using MelonLoader;
using System.Diagnostics;
using static Il2CppSystem.Xml.XmlWellFormedWriter.AttributeValueCache;

[assembly: MelonInfo(typeof(Augging_Out.Core), "Augging Out", "1.0.0", "amy", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace Augging_Out;

public class Core : MelonMod
{
    public static MelonLogger.Instance logger;
    public override void OnInitializeMelon()
    {
        logger = LoggerInstance;
        logger.Msg("Initialized. Aug radar active");
        //ModHook.OnGameLoadedLate += (Action)AddItems;
    }
    //public static void AddItems()
    //{
    //    GameItem gameItem = DirectoryMaster.Item("energy_credit_ext");
    //    PowerHelper.InitPowerSourceItem(gameItem, 99999, 999);
    //    PowerHelper.InitRecharge(gameItem, 9999);
    //    gameItem.unitBaseValue = -999999999;
    //    EmporiumEntry.Instance.TryAddToPlayerInv(gameItem);
    //    EmporiumEntry.Instance.TryAddToPlayerInv(DirectoryMaster.Item("aug_scanner", true));
    //}
}

[HarmonyPatch(typeof(StoreClientList),"CreateAugMed")]
public class AugMedPatch
{
    public static void Postfix(StoreClient __result)
    {
        __result.SetBudget(1);
    }
}
[HarmonyPatch(typeof(StoreClientList), "CreateAugMaterial")]
public class AugMaterialPatch
{
    public static void Postfix(StoreClient __result)
    {
        __result.SetBudget(1);
    }
}

//[HarmonyPatch(typeof(PhoneUIManager), "CloseUI")]
//public class DebugPatch
//{
//    private static void Postfix()
//    {
//        Core.logger.Msg("rizzly");
//        PlayerStore.Instance.storeClientManager.AddNextClient(StoreClientList.CreateAugMed(1));
//        PlayerStore.Instance.storeClientManager.AddNextClient(StoreClientList.CreateAugMed(2));
//        PlayerStore.Instance.storeClientManager.AddNextClient(StoreClientList.CreateAugMed(3));
//        PlayerStore.Instance.storeClientManager.AddNextClient(StoreClientList.CreateAugMed(4));
//        //Core.AddItems();
//    }
//}

