using Il2Cpp;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Common;
using Il2CppInterop.Runtime;
using MelonLoader;
using System;
using UnityEngine;
using Il2Cpp;
using MelonLoader;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using HarmonyLib;
using static Il2CppSystem.Xml.XmlWellFormedWriter.AttributeValueCache;

[assembly: MelonInfo(typeof(Gunsmith_m.Core), "DEMOBUGGER", "1.0.0", "amy", null)]
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





[HarmonyPatch(typeof(PhoneUIManager), "CloseUI")]
public class DebugPatch
{
    public static void Postfix()
    {
        //EmporiumEntry.Instance.TryAddToPlayerInv(CustomerItemDirectory.GunsmithMagnet());
        EmporiumEntry.Instance.TryAddToPlayerInv(CustomerItemDirectory.GunsmithMagnet());
        //PlayerStore.Instance.AddDirectSellingItemToTable(CustomerItemDirectory.GunsmithMagnet(), true);
    }
}

public class CustomerItemDirectory
{
    public static void ModdedShape(ref GameItem item, string path)
    {
        // Resource name format: Namespace.Folder.Filename
        Sprite sprite = LoadEmbeddedPng(path);
        //logger.Msg(sprite.texture);
        item.TryCast<GameItemElement>().background.image.sprite = sprite;
        item.TryCast<GameItemElement>().background.image.SetNativeSize();
        item.TryCast<GameItemElement>().fakeBackground.image.sprite = item.TryCast<GameItemElement>().background.image.sprite;
        item.TryCast<GameItemElement>().fakeBackground.image.SetNativeSize();

        item.spriteAtlasPath = "ModdedPath/";
        item.spritePath = path;
        Rect rect = sprite.rect;
        int gridSize = UISettings.current.gridSize;
        int mipLevel = (int)Math.Sqrt((double)gridSize);
        int num = (int)rect.width / gridSize;
        int num2 = (int)rect.height / gridSize;
        byte[] array = new byte[num * num2];
        for (int i = 0; i < num2; i++)
        {
            for (int j = 0; j < num; j++)
            {
                int num3 = i * num + j;

                int x = (int)rect.xMin / gridSize + j;
                int y = (int)rect.yMin / gridSize + (num2 - 1 - i);
                if (sprite.texture.GetPixel(x, y, mipLevel).a > 0f)
                {
                    array[num3] = 1;
                }
            }
        }
        GridShapeBuilder gridShapeBuilder = new GridShapeBuilder().SetData(array, num);
        GridShape finalShape = new GridShape(gridShapeBuilder.Pointer);
        item.SetShape(finalShape);
    }

    public static Sprite LoadEmbeddedPng(string resourceSuffix)
    {
        Assembly assembly = typeof(Core).Assembly;
        string text = null;
        foreach (string text2 in assembly.GetManifestResourceNames())
        {
            if (text2.EndsWith(resourceSuffix, StringComparison.OrdinalIgnoreCase))
            {
                text = text2;
                break;
            }
        }
        if (text == null)
        {
            return null;
        }
        Sprite result;
        using (Stream manifestResourceStream = assembly.GetManifestResourceStream(text))
        {
            if (manifestResourceStream == null)
            {
                return null;
            }


            byte[] array = new byte[manifestResourceStream.Length];
            if (manifestResourceStream.Read(array, 0, array.Length) <= 0)
            {
                result = null;
            }
            else
            {
                Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp,
                    hideFlags = HideFlags.HideAndDontSave
                };
                if (!ImageConversion.LoadImage(texture2D, array))
                {
                    UnityEngine.Object.Destroy(texture2D);
                    result = null;
                }
                else
                {
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                    sprite.hideFlags = HideFlags.HideAndDontSave;
                    result = sprite;
                }

            }
        }
        return result;
    }
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
    public static GameItem Emag()
    {
        GameItem gameItem = ItemDirectory.CreateEmptyItem("emag");
        ModdedShape(ref gameItem, gameItem.identifier);
        gameItem.SetName("EMAG");
        gameItem.SetValue(175L);
        gameItem.flavorText = "The Syndicate's iconic EMAG, created from a hacked ID card.";
        gameItem.shortDescription = "Opens all locks, no exceptions.";
        gameItem.SetGameItemType("ACCESS_CARD");
        gameItem.EnableManufacturer("self");
        AudioHelper.InitPlasticCard(gameItem);
        ContrabandHelper.InitContrabandItem(gameItem, 4);

        //gameItem.maySelectSlotItemFunc = (GameItem item, GameInventory _, SlotMarker _) => true;
        //gameItem.canSelectSlotItemFunc = (GameItem item, GameInventory _, SlotMarker _) => true;
        //gameItem.mayItemTargetItemFunc = (GameItem s, GameItem t) => GeneralHelper.IsItemOwned(s) && GeneralHelper.IsItemOwned(t) && LockHelper.IsLockedContainer(t);
        //gameItem.canItemTargetItemFunc = (GameItem s, GameItem t) => GeneralHelper.IsItemOwned(s) && GeneralHelper.IsItemOwned(t) && LockHelper.IsLockedContainer(t);

        gameItem.maySelectSlotItemFunc = (Func<GameItem, GameInventory, SlotMarker, bool>)delegate (GameItem _, GameInventory _, SlotMarker _) { return true; };
        gameItem.canSelectSlotItemFunc = (Func<GameItem, GameInventory, SlotMarker, bool>)delegate (GameItem _, GameInventory _, SlotMarker _) { return true; };
        gameItem.mayItemTargetItemFunc = (Func<GameItem, GameItem, bool>)delegate (GameItem s, GameItem t)
        {
            return GeneralHelper.IsItemOwned(s) && GeneralHelper.IsItemOwned(t) && LockHelper.IsLockedContainer(t);
        };
        gameItem.canItemTargetItemFunc = (Func<GameItem, GameItem, bool>)delegate (GameItem s, GameItem t)
        {
            return GeneralHelper.IsItemOwned(s) && GeneralHelper.IsItemOwned(t) && LockHelper.IsLockedContainer(t);
        };
        gameItem.onItemTargetItemFunc = (Action<GameItem, GameItem>)delegate (GameItem s, GameItem t)
        {
            LockHelper.TryOpenLock(s, t, "LOCK_ID_EMAG");
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




