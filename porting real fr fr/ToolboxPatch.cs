using HarmonyLib;

namespace ProbablyNuclear
{
    [HarmonyPatch(typeof(ToolboxHelper), "UpdateToolboxSprite")]
    public static class ToolboxPatch
    {
        public static bool Prefix(GameItem box)
        {
            if (ModLoader.Mods.Find((LoadedMod m) => m.Manifest.ID.Equals(box.spriteAtlasPath)) == null)
                return true; // Not modded, use default sprite
            else
            {
                bool flag = ToolboxHelper.GetUnlockedSlots(box) >= 9;
                box.SetSpriteAndShapeFromMod(box.spriteAtlasPath, flag ? $"{box.spritePath}_new" : $"{box.spritePath}_old");
                return false;
            }
        }
    }
}