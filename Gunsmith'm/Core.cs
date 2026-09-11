using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Common;
using MelonLoader;
using System;
using Il2CppInterop.Runtime;
using static Il2CppSystem.Xml.XmlWellFormedWriter.AttributeValueCache;
[assembly: MelonInfo(typeof(Gunsmith_m.Core), "Gunsmith'm", "1.0.0", "amy", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace Gunsmith_m;

public class Core : MelonMod
{
    public static MelonLogger.Instance logger;
    public static HashSet<string> canActivateIds = new HashSet<string> { };
    public override void OnInitializeMelon()
    {
        canActivateIds.Add("gunsmith_magnet");

        logger = LoggerInstance;
        LoggerInstance.Msg("Initialized. Gun magnet activated.");
        
    }
    
}





[HarmonyPatch(typeof(PhoneUIManager),"CloseUI")]
public class DebugPatch
{
    public static void Postfix()
    {
       EmporiumEntry.Instance.TryAddToPlayerInv(CustomerItemDirectory.GunsmithMagnet());
       //PlayerStore.Instance.AddDirectSellingItemToTable(CustomerItemDirectory.GunsmithMagnet(), true);
    }
}

public class CustomerItemDirectory
{
    public static bool IsGunsmithMagnet(GameItem item)
    {
        GameItem template = GunsmithMagnet();
        return template.name == item.name && template.unitValue == item.unitValue;
    }

    public static GameItem GunsmithMagnet()
    {
        GameItem gameItem = DirectoryMaster.Item("furnace");
        gameItem.identifier = "gunsmith_magnet";
        gameItem.RemoveAllGameItemType();
        gameItem.SetSpriteAndShape("Items/items_tool", "spectral_analyser");
        gameItem.SetGameItemType("TOOL");
        gameItem.name = "Gun Magnet";
        gameItem.shortDescription = "Attracts nearby guns";
        gameItem.flavorText = "You feel a slight pull in your pocket";
        gameItem.unitValue = 200L;
        gameItem.onActivateSlotItemFunc = (Action<GameItem, GameInventory, SlotMarker>)delegate
        {
            Core.logger.Msg("Activate");
        };
        return gameItem;
    }

}

[HarmonyPatch(typeof(GameItem), nameof(GameItem.MayActivateSlotItem))]
internal static class GameItem_MayActivateSlotItem_Patch
{
    private static bool Prefix(GameItem __instance, ref bool __result)
    {
        // Core.SpawnItemID is the item ID of your custom item. Replace it with the actual item ID you want to check.
        if (__instance == null || !Core.canActivateIds.Contains(__instance.identifier))
        {
            return true;
        }

        __result = true;
        return false;
    }
}

[HarmonyPatch(typeof(GameItem), nameof(GameItem.CanActivateSlotItem))]
internal static class GameItem_CanActivateSlotItem_Patch
{
    private static bool Prefix(GameItem __instance, ref bool __result)
    {
        if (__instance == null || !Core.canActivateIds.Contains(__instance.identifier))
        {
            return true;
        }

        __result = GeneralHelper.IsItemOwned(__instance);
        return false;
    }
}





[HarmonyPatch(typeof(StoreClientList),"CreateGunsmith")]
public class GunsmithPatch
{
    public static bool Prefix(ref StoreClient __result)
    {
        StoreClient storeClient = new StoreClient();
        storeClient.identifier = "lower_level_gunsmith";
        storeClient.displayName = LocHelper.GetLocalizedClientName("name_lower_level_gunsmith");
        storeClient.clientFaction = "FACTION_LOWER_LEVEL";
        storeClient.spriteName = SpriteDict.GetRandomLowerLevelSpriteName();
        storeClient.clientIntent = StoreClient.ClientIntent.SELL;
        storeClient.SetWholesale(-20);
        storeClient.mainDialogue.SetText(storeClient.displayName, LocHelper.GetLocalizedDialogue("dialog_gunsmith_1")).NextDialogue().SetText(storeClient.displayName, LocHelper.GetLocalizedDialogue("dialog_gunsmith_2")).SetEndAction((Action) delegate
        {
            if (!PlayerStore.Instance.FindAllItem().Exists((Il2CppSystem.Predicate<GameItem>)CustomerItemDirectory.IsGunsmithMagnet))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(CustomerItemDirectory.GunsmithMagnet());
            }
            if (RNG.Roll(33))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(PreBuiltItemHelper.CreateHeavyHandmadeGun(ItemFeatureList.EquipmentCondition.Good), false, false, false, 0);
            }
            if (RNG.Roll(33))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(PreBuiltItemHelper.CreateHeavyHandmadeGun(ItemFeatureList.EquipmentCondition.Poor), false, false, false, 0);
            }
            if (RNG.Roll(33))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(PreBuiltItemHelper.CreateHandmadeGun(ItemFeatureList.EquipmentCondition.Good), false, false, false, 0);
            }
            if (RNG.Roll(33))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(PreBuiltItemHelper.CreateHandmadeGun(ItemFeatureList.EquipmentCondition.Poor), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("small_pistol_ammo", true), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("small_pistol_ammo", true), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("small_pistol_ammo", true), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("small_pistol_ammo", true), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("heavy_pistol_ammo", true), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("heavy_pistol_ammo", true), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("heavy_pistol_ammo", true), false, false, false, 0);
            }
            if (RNG.Roll(50))
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item("heavy_pistol_ammo", true), false, false, false, 0);
            }
            PlayerStore.Instance.AddDirectSellingItemToTable(PreBuiltItemHelper.CreateHandmadeGun(ItemFeatureList.EquipmentCondition.Good), false, false, false, 0);
        });
        storeClient.AddBasicDialog();
        storeClient.CompleteClientCreation(false);
        __result = storeClient;
        return false;
    }
}