using HarmonyLib;
using System;

namespace ProbablyNuclear
{
    [HarmonyPatch(typeof(StoreClientList), nameof(StoreClientList.CreateThief))]
    public static class ThiefPatch
    {
        static void Postfix(ref StoreClient __result)
        {
            __result.mainDialogue.nextDialogue.endAction += () =>
            {
                if (StoreStation.Instance.dayCounter >= 21 && RNG.Roll(5 + Math.Clamp(StoreReputation.GetBMReputation() / 4, 0, 25)))
                {
                    PlayerStore.Instance.AddDirectSellingItemToTable(ModPreBuiltItems.LootCrateSyndicate(), isOwend: false, isStolen: true);
                }
            };
        }
    }
}
