using MelonLoader;
using HarmonyLib;
using Il2Cpp;
[assembly: MelonInfo(typeof(BiggerBudgets.Core), "BiggerBudgets", "1.0.0", "amy", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace BiggerBudgets;

public class Core : MelonMod
{
    public static MelonLogger.Instance logger;
    public static int factor = 2;
    public override void OnInitializeMelon()
    {
        logger = LoggerInstance;
        logger.Msg("Initialized. Inflationary.");
        //ModHook.OnGameLoadedLate += (Action)AddItems;
    }
}

[HarmonyPatch(typeof(StoreClientListTierSubstance), nameof(StoreClientListTierSubstance.CreateUpperLevelHedonist))]
public class harmonyPatch
{
    
    public static void Postfix(ref StoreClient __result)
    {
        Core.logger.Msg("Activated");
        __result.SetBudget(__result.GetBudget() * Core.factor * 2);
    }
}

[HarmonyPatch(typeof(StoreClientList), "CreateUpperLowerVisitor")]
public class harmonyPatch2
{

    public static void Postfix(ref StoreClient __result)
    {
        Core.logger.Msg("Activated");
        __result.SetBudget(__result.GetBudget() * Core.factor);
    }
}

[HarmonyPatch(typeof(StoreClientList), nameof(StoreClientList.CreateUpperLowerVisitorLuxury))]
public class harmonyPatch3
{

    public static void Postfix(ref StoreClient __result)
    {
        Core.logger.Msg("Activated");
        __result.SetBudget(__result.GetBudget() * Core.factor);
    }
}


//[HarmonyPatch(typeof(PhoneUIManager), "CloseUI")]
//public class DebugPatch
//{
//    private static void Postfix()
//    {
//        Core.logger.Msg("rizzly");
//        PlayerStore.Instance.storeClientManager.AddNextClient(StoreClientListTierSubstance.CreateUpperLevelHedonist());
//        //Core.AddItems();
//    }
//}