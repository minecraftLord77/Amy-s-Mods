using HarmonyLib;

namespace ProbablyNuclear
{
    [HarmonyPatch(typeof(GameItem), nameof(GameItem.SetSpriteAndShape))]
    public static class SpriteShapePatch
    {
        static bool Prefix(string spriteAtlasPath, string spritePath, ref GameItem __result, ref GameItem __instance)
        {
            if (ModLoader.Mods.Find((LoadedMod m) => m.Manifest.ID.Equals(spriteAtlasPath)) == null)
                return true;

            __result = __instance.SetSpriteAndShapeFromMod(spriteAtlasPath, spritePath);
            return false;
        }
    }
}