using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using static Il2CppSystem.Xml.XmlWellFormedWriter.AttributeValueCache;
[assembly: MelonInfo(typeof(SuperCharger.Core), "SuperCharger", "1.0.0", "wilso", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace SuperCharger;

public class Core : MelonMod
{
    public static MelonLogger.Instance logger;
    public override void OnInitializeMelon()
    {
        logger = LoggerInstance;
        logger.Msg("Initialized. asjdajsamddmasadsmadsm");
        //ModHook.OnGameLoadedLate += (Action)AddItems;
    }
}

[HarmonyPatch(typeof(PhoneUIManager),nameof(PhoneUIManager.CloseUI))]
public class debugpatch
{
    public static void Postfix()
    {
        EmporiumEntry.Instance.TryAddToPlayerInv(MachineRecharger.Recharger());
    }
}

[HarmonyPatch(typeof(MachineRecharger),nameof(MachineRecharger.Recharger))]
public class RechargerPatch()
{
    public static bool Prefix(ref GameItem __result)
    {
        ValueTuple<PixelWindow, GameInventory> valueTuple = MachineRecharger.CreateMachineInventoryWindow(true, true);
        PixelWindow item2 = valueTuple.Item1;
        GameInventory itemInventory = valueTuple.Item2;
        itemInventory.mayInventoryAddItemFunc = ((GameItem item, GameInventory _) => GeneralHelper.IsItemOwned(item) && item.IsTag("power_source_item"));
        GameItem machine = ItemDirectory.CreateEmptyItem(null).SetContentWindow(item2).SetSpriteAndShape("Items/items_tool", "recharger_base");
        machine.name = LocHelper.GetLocalizedItem("item_recharger_name", Array.Empty<object>());
        machine.shortDescription = LocHelper.GetLocalizedItem("item_recharger_desc", Array.Empty<object>());
        machine.EnableTag("IMPORTANT_TAG", false);
        machine.unitValue = 150L;
        machine.SetGameItemType("MACHINE");
        machine.EnableTag("CONTAINER_TAG", false);
        machine.EnableTag("manufacture", false);
        AudioHelper.InitSmallMachine(machine);
        machine.onCycleEndSlotItemFunc = delegate (GameItem item, GameInventory parentInventory, SlotMarker slot)
        {
            if (PlayerStore.Instance.isPowerOn)
            {
                foreach (GameItem gameItem in itemInventory.childItems)
                {
                    if (gameItem.IsTag("power_source_item"))
                    {
                        if (NetworkUpgrade.IsCommercialPowerUnlocked())
                        {
                            PowerHelper.ChargeBattery(gameItem, 6);
                        }
                        else
                        {
                            PowerHelper.ChargeBattery(gameItem, 3);
                        }
                    }
                }
            }
            machine.Validate();
        };
        Action<GameItem, GameInventory, SlotMarker> action = delegate (GameItem item, GameInventory parentInventory, SlotMarker slot)
        {
            machine.Validate();
        };
        itemInventory.onSlotAddItemEndFunc = action;
        Action<GameItem, GameInventory, SlotMarker> action1 = delegate (GameItem item, GameInventory parentInventory, SlotMarker slot)
        {
            machine.Validate();
        };
        itemInventory.onSlotRemoveItemEndFunc = action1;
        GameInventory itemInventory3 = itemInventory;
        itemInventory3.onSlotAddItemFunc = (Action<GameItem, GameInventory, SlotMarker>)Delegate.Combine(itemInventory3.onSlotAddItemFunc, new Action<GameItem, GameInventory, SlotMarker>(delegate (GameItem item, GameInventory _, SlotMarker _)
        {
            AudioManager.Instance.Play3("battery_insert");
        }));
        GameInventory itemInventory2 = itemInventory;
        itemInventory2.onSlotRemoveItemFunc = (Action<GameItem, GameInventory, SlotMarker>)Delegate.Combine(itemInventory2.onSlotRemoveItemFunc, new Action<GameItem, GameInventory, SlotMarker>(delegate (GameItem item, GameInventory _, SlotMarker _)
        {
            AudioManager.Instance.Play3("battery_removed");
        }));
        Func<GameItem, Il2CppSystem.Collections.Generic.List<ItemSpriteModifier>> func = delegate (GameItem machine)
        {
            List<ItemSpriteModifier> list = new List<ItemSpriteModifier>();
            int num = 0;
            foreach (GameItem gameItem in itemInventory.childItems)
            {
                list.Add(new ItemSpriteModifier(RenderHandler.LoadFromAtlas(gameItem.spriteAtlasPath, gameItem.spritePath), new Vector2((float)(num * 16), 0f), 0, false, 0, false, Color.white));
                num++;
            }
            list.Add(new ItemSpriteModifier(RenderHandler.LoadFromAtlas("Items/items_tool", "recharger_top"), new Vector2(0f, 0f), 0, false, 0, false, Color.white));
            if (!PlayerStore.Instance.isPowerOn)
            {
                return list;
            }
            num = 0;

            using (List<GameItem>.Enumerator enumerator = itemInventory.childItems.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (PowerHelper.IsPowerSourceFull(enumerator.Current))
                    {
                        list.Add(new ItemSpriteModifier(RenderHandler.LoadFromAtlas("Items/items_tool", "recharger_g"), new Vector2((float)(7 + num * 16), -8f), 0, false, 0, false, Color.white));
                    }
                    else
                    {
                        list.Add(new ItemSpriteModifier(RenderHandler.LoadFromAtlas("Items/items_tool", "recharger_o"), new Vector2((float)(7 + num * 16), -8f), 0, false, 0, false, Color.white));
                    }
                    num++;
                }
            }
            return list;
        };
        machine.getItemSpriteModifierForItemFunc = func;
        return machine;

        return false;
    }
    //public static void Postfix(ref GameItem __result, Dictionary<string, object> __locals)
    //{
    //    GameInventory itemInventory = DirectoryMaster.Item("furnace").parentInventory;
    //    GameItem machine = __result;
    //    Core.logger.Msg(__locals);
    //    if (__locals.TryGetValue("itemInventory", out object tempItemInventory))
    //    { 
    //        itemInventory = (GameInventory)tempItemInventory; 
    //    } else
    //    {
    //        Core.logger.Msg("uh oh.");
    //    }


    //    machine.onCycleEndSlotItemFunc = (Action<GameItem, GameInventory, SlotMarker>) delegate (GameItem item, GameInventory parentInventory, SlotMarker slot)
    //    {
    //        if (PlayerStore.Instance.isPowerOn)
    //        {
    //            foreach (GameItem gameItem in itemInventory.childItems)
    //            {
    //                if (gameItem.IsTag("power_source_item"))
    //                {
    //                    if (NetworkUpgrade.IsCommercialPowerUnlocked())
    //                    {
    //                        PowerHelper.ChargeBattery(gameItem, 6);
    //                    }
    //                    else
    //                    {
    //                        PowerHelper.ChargeBattery(gameItem, 3);
    //                    }
    //                }
    //            }
    //        }
    //        machine.Validate();
    //    };

    //    __result = machine;
    //}
}