using MelonLoader;
using HarmonyLib;
using System;
using Il2Cpp;


[assembly: MelonInfo(typeof(Exchange_Er.Core), "Exchange'Er", "1.0.0", "wilso", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace Exchange_Er;

public class Core : MelonMod
{
    public static MelonLogger.Instance logger;

    public override void OnInitializeMelon()
    {
        logger = LoggerInstance;
        logger.Msg("Exchanger? I hardly know her!");
        
    }
}

[HarmonyPatch(typeof(ExchangeManager),nameof(ExchangeManager.HandleExchange))]
public class harmonyPatch
{
    public static bool Prefix(ExchangeManager __instance)
    {
        int num = 0;
        List<ExchangeBarter> list = new List<ExchangeBarter>();
        List<ExchangeBarter> buffer = new List<ExchangeBarter>();
        foreach (ExchangeBarter barter in __instance.activeBarter)
        {
            list.Add(barter);
        }
        foreach (ExchangeBarter exchangeBarter in __instance.promotionalBarter)
        {
            if (__instance.activePromotionalBarterIds.Contains(exchangeBarter.identifier))
            {
                list.Add(exchangeBarter);
            }
        }
        list.Sort((ExchangeBarter x, ExchangeBarter y) => y.priority.CompareTo(x.priority));
        foreach (ExchangeBarter exchangeBarter2 in list)
        {
            if (exchangeBarter2.CanAccept())
            {
                num++;
                exchangeBarter2.Accept();
                StoreReputation.GetBMFaction().ModProgressReputation(75);
                PlayerStore.instance.AddNightLog("Someone from Sawyer's crew visited.", "ab05ff");
                if (__instance.activePromotionalBarterIds.Contains(exchangeBarter2.identifier))
                {
                    __instance.activePromotionalBarterIds.Remove(exchangeBarter2.identifier);
                }
            }
        }
        return false;
    }
}