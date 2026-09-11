using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using System.Runtime.CompilerServices;


[assembly: MelonInfo(typeof(Expedited_Shipping.Core), "Expedited Shipping", "1.0.0", "amy", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

//Dyrclone, Murky
namespace Expedited_Shipping;

public class Core : MelonMod
{

    public static MelonLogger.Instance logger;
    public override void OnInitializeMelon()
    {

        logger = LoggerInstance;
        LoggerInstance.Msg("Initialized. Consider yourself expedited.");
    }

    public override void OnLateInitializeMelon()
    {
        MelonCoroutines.Start(WaitAndPatchTables());
    }
    private IEnumerator<NullabilityInfo> WaitAndPatchTables()
    {
        while (!LocalizationSettings.InitializationOperation.IsDone)
        {
            yield return null;
        }
        addLocKey("Mechanic", "type_scavenger", "Scavenger");
        addLocKey("Mechanic", "type_expedition_plan", "Expedition Plan");
        yield break;
    }
    private void addLocKey(string tableName, string key, string value)
    {
        foreach (UnityEngine.Localization.Locale locale in LocalizationSettings.AvailableLocales.Locales)
        {
            StringTable table = LocalizationSettings.StringDatabase.GetTable(tableName, locale);
            bool flag = table != null;
            if (flag)
            {
                table.RemoveEntry(key);
                table.AddEntry(key, value);
            }
        }
    }

    public static void ModdedShape(ref GameItem item, string path)
    {
        // Resource name format: Namespace.Folder.Filename
        Sprite sprite = LoadEmbeddedPng(path);
        //logger.Msg(sprite.texture);
        logger.Msg("png loaded");
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
            logger.Msg("Searched " + text2);
            if (text2.EndsWith(resourceSuffix, StringComparison.OrdinalIgnoreCase))
            {
                text = text2;
                break;
            }
        }
        if (text == null)
        {
            logger.Warning("Embedded icon '" + resourceSuffix + "' was not found.");
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
                    logger.Warning("Failed to decode icon '" + resourceSuffix + "'.");
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
}

[HarmonyPatch(typeof(PhoneUIManager), nameof(PhoneUIManager.CloseUI))]
public class DebugPatch
{
    public static void Postfix()
    {
        EmporiumEntry.Instance.TryAddToPlayerInv(ModdedItems.ExpeditionTerminal());
        EmporiumEntry.Instance.TryAddToPlayerInv(ModdedItems.RobberyPlan("lower_level_robbery");
    }
}

public class ModdedItems
{
    public static ValueTuple<PixelWindow, GameSlotInventory, GameInventory> CreateTerminalMachineWindow(int width, int height, bool isDraggable = true, bool isCentered = true, int minWidth = 5)
    {
        PixelWindow pixelWindow = new PixelWindow(isDraggable, null);
        GridPixelElement gridPixelElement = new GridPixelElement(5, 2, isCentered);
        GameSlotInventory gameSlotInventory = new GameSlotInventory();
        //GameGridInventory gameGridInventory = new GameGridInventory("10000010001000001110000010001000001", 7);
        GameGridInventory gameGridInventory2 = new GameGridInventory(9, 6);
        //GameGridInventory gameGridInventory3 = new GameGridInventory(9, 6);
        //GameSlotInventory gameSlotInventory2 = new GameSlotInventory();

        TagElement element = new TagElement(-1, true).SetText("Plan", 10000, RenderHandler.ColorPalette.VeryLightRed);
        //TagElement element2 = new TagElement(-1, true).SetText(LocHelper.GetLocalizedMechanic("mech_furnace_label_module"), 10000, RenderHandler.ColorPalette.VeryLightRed);
        TagElement element3 = new TagElement(-1, true).SetText(LocHelper.GetLocalizedMechanic("mech_furnace_label_input"), 10000, RenderHandler.ColorPalette.VeryLightRed);
        //TagElement element4 = new TagElement(-1, true).SetText(LocHelper.GetLocalizedMechanic("mech_furnace_label_output"), 10000, RenderHandler.ColorPalette.VeryLightRed);
        //TagElement element5 = new TagElement(-1, true).SetText(LocHelper.GetLocalizedMechanic("mech_furnace_label_note"), 10000, RenderHandler.ColorPalette.VeryLightRed);

        PixelElement newElement1 = new PixelElement(element.Pointer);
        //PixelElement newElement2 = new PixelElement(element2.Pointer);
        PixelElement newElement3 = new PixelElement(element3.Pointer);
        //PixelElement newElement4 = new PixelElement(element4.Pointer);
        //PixelElement newElement5 = new PixelElement(element5.Pointer);

        PixelElement newGameSlotInventory = new PixelElement(gameSlotInventory.Pointer);
        //PixelElement newGameGridInventory = new PixelElement(gameGridInventory.Pointer);
        PixelElement newGameGridInventory2 = new PixelElement(gameGridInventory2.Pointer);
        //PixelElement newGameGridInventory3 = new PixelElement(gameGridInventory3.Pointer);
        //PixelElement newGameSlotInventory2 = new PixelElement(gameSlotInventory2.Pointer);



        gridPixelElement.Attach(newElement1, 0, 0, 20, 20);
        //gridPixelElement.Attach(newElement2, 1, 0, 20, 20);
        gridPixelElement.Attach(newElement3, 1, 0, minWidth, 20);
        //gridPixelElement.Attach(newElement4, 3, 0, 5, 20);
        //gridPixelElement.Attach(newElement5, 4, 0, 5, 5);
        gridPixelElement.Attach(newGameSlotInventory, 0, 1, 20, 50);
        gridPixelElement.Attach(newGameGridInventory2, 1, 1, 20, 50);
        gridPixelElement.Attach(newGameGridInventory2, 2, 1, minWidth, 100);
        //gridPixelElement.Attach(newGameGridInventory3, 3, 1, 5, 100);
        //gridPixelElement.Attach(newGameSlotInventory2, 4, 1, 5, 5);

        PixelElement newGridPixelElement = new PixelElement(gridPixelElement.Pointer);
        pixelWindow.Attach(newGridPixelElement);
        return new ValueTuple<PixelWindow, GameSlotInventory, GameInventory>(pixelWindow, gameSlotInventory, gameGridInventory2);
    }

    public static void ModifyTag(ref GameItem item, string field, object newValue)
    {
        item.ModifyTag(field, (Action<TagState>)delegate (TagState state)
        {
            if (newValue is bool) state.SetBool(newValue);
            state.SetString("lowerlevelrobbery");
        }, false);
    }

    public static GameItem ExpeditionTerminal()
    {
        GameItem item = DirectoryMaster.Item("furnace");

        MachineryHelper.InitMachinery(item, 0, false);
        item.RemoveAllGameItemType();
        item.SetGameItemType("MACHINE");
        item.name = "Raider's Terminal";
        item.shortDescription = "Plan and supply raids from here.";
        Core.ModdedShape(ref item, "terminal_off.png");

        ValueTuple<PixelWindow, GameSlotInventory, GameInventory> inventoryWindow = CreateTerminalMachineWindow(8, 8, true, true, 5);
        PixelWindow contentWindow = inventoryWindow.Item1;
        GameSlotInventory planSlot = inventoryWindow.Item2;
        ContainerHelper.AllowOnlyTaggedItemsOr(planSlot, new[] { "generic_plan_blueprint" });
        GameInventory inputSlot = inventoryWindow.Item3;
        item.SetContentWindow(contentWindow);

        planSlot.onSlotAddItemEndFunc = (Action<GameItem,GameInventory,SlotMarker>) delegate{
            Core.ModdedShape(ref item, "terminal_on.png");
        };

        planSlot.onSlotRemoveItemEndFunc = (Action<GameItem, GameInventory, SlotMarker>)delegate {
            Core.ModdedShape(ref item, "terminal_off.png");
        };

        item.onCycleEndLateSlotItemFunc = (Action<GameItem, GameInventory, SlotMarker>)delegate
        {
            Expedition.ExpeditionHelper(ref item, planSlot, inputSlot);
        };

        return item;
    }


    public static GameItem RobberyPlan(string type)
    {
        GameItem item = DirectoryMaster.Item("business_permit");
        item.RemoveAllGameItemType();
        item.EnableTag("generic_plan_blueprint",false);
        item.SetGameItemType("expedition_plan");

        item.EnableTag("required_weapons");
        item.EnableTag("danger_level");

        if (type == "lower_level_robbery")
        {
            
        }

        item.EnableTag(type, false);

        //item.ModifyTag("generic_plan_blueprint", (Action<TagState>) delegate (TagState state)
        //{
        //    state.SetString("lowerlevelrobbery");
        //}, false);


        return item;
    }

}

public class Expedition
{

    public static void ExpeditionHelper(ref GameItem terminal, GameSlotInventory planSlot, GameInventory inputSlot)
    {
        GameItem plan = planSlot.childItem;
        if (plan == null || !plan.IsTag("plan"))
        {
            Core.logger.Msg("No plan inserted into raider terminal");
            return;
        }
        string planType = plan.GetTagReadonly("generic_plan_blueprint").GetString()
        int foodPerDay, danger, weapons;
        if (planType=="lower_level_robbery")
        {

        }

        plan.Destroy();
        planSlot.ExpelAll();
        Core.logger.Msg("Expedition activated");
    }

    
}


public class ModdedClients
{
    public static StoreClient CreateLowerLevelThief()
    {
        StoreClient storeClient = new StoreClient();
        storeClient.identifier = "revRaider";
        storeClient.displayName = LocHelper.GetLocalizedClientName("name_revolution_raider");
        storeClient.spriteName = SpriteDict.GetRandomRevSpriteName();
        StoreClientFactionSetup.InitRev(storeClient);
        storeClient.clientIntent = StoreClient.ClientIntent.DIALOGUE;
        storeClient.mainDialogue.SetText(storeClient.displayName, "Skibidi")
            .NextDialogue().SetText(storeClient.displayName, "Skibidi2")
            .NextDialogue().SetText(storeClient.displayName, "Skibidi3")
            .SetEndAction((Action)delegate
            {
                PlayerStore.Instance.AddDirectSellingItemToTable(DirectoryMaster.Item(""), true, true, false, 50);
            }
        );
        storeClient.AddBasicDialog();
        storeClient.CompleteClientCreation(false);
        return storeClient;
    }
}

