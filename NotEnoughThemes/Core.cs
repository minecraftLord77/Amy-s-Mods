using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


[assembly: MelonInfo(typeof(NotEnoughThemes.NotEnoughThemesCore), "NotEnoughThemes", "1.0.0", "NastyNick", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]



namespace NotEnoughThemes;

public class NotEnoughThemesCore : MelonMod
{
    public static MelonLogger.Instance logger;
    public override void OnInitializeMelon()
    {
        MelonPreferences.CreateCategory("NotEnoughThemes", display_name: "Not Enough Themes");
        MelonPreferences.CreateEntry<bool>("NotEnoughThemes", "includeVanilla", true, display_name: "Include Vanilla Themes (3 themes in the base game)");
        MelonPreferences.CreateEntry<bool>("NotEnoughThemes", "includeBasic", true, display_name: "Include basic themes added by this mod? (7 themes)");
        MelonPreferences.CreateEntry<bool>("NotEnoughThemes", "includePride", false, display_name: "Include Pride versions of all the themes above? (7 themes)");
        MelonPreferences.CreateEntry<bool>("NotEnoughThemes", "includeTrans", false, display_name: "Include Trans versions of all the themes above? (7 themes)");
        logger = LoggerInstance;
        LoggerInstance.Msg("Not Enough Themes Initialized");

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
        Assembly assembly = typeof(NotEnoughThemesCore).Assembly;
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


[HarmonyPatch(typeof(ToggleAppearanceHelper), nameof(ToggleAppearanceHelper.MakeSpriteCyclable), new Type[] { typeof(GameItem), typeof(string), typeof(string[]) })]
public class AppearancePatch
{
    public static void Postfix(ref GameItem item, string atlasPath, params string[] spriteVariants)
    {
        NotEnoughThemesCore.logger.Warning("Skibidi");
        if (item.identifier != "storage_bay" && item.identifier != "storage_bay_large")
        {
            NotEnoughThemesCore.logger.Msg("aborted");
            return; }

        // If the field is declared as Action<Gameltem, Gamelnventory, SlotMarker>
        Action<GameItem, GameInventory, SlotMarker> action = delegate (GameItem item, GameInventory _, SlotMarker _)
        {
            int num = (Array.IndexOf<string>(spriteVariants, item.spritePath) + 1) % spriteVariants.Length;
            item.SetSpriteQuick(atlasPath, spriteVariants[num]);
            NotEnoughThemesCore.logger.Msg("Skib");
        };
        item.onToggleSlotItemFunc = action;

    }
}

[HarmonyPatch(typeof(PhoneUIManager),nameof(PhoneUIManager.CloseUI))]
public class debugpatch
{
    public static void Postfix()
    {
        EmporiumEntry.Instance.TryAddToPlayerInv(DirectoryMaster.Item("storage_bay"));
        EmporiumEntry.Instance.TryAddToPlayerInv(DirectoryMaster.Item("penis"));
    }
}