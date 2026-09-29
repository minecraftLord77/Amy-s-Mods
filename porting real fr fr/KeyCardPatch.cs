using HarmonyLib;
using Il2Cpp;
using MelonLoader;
namespace ProbablyNuclear
{
    [HarmonyPatch(typeof(LockHelper), nameof(LockHelper.CanOpenLock))]
    public static class KeyCardPatch
    {
        static void Postfix(ref GameItem box, ref string keyLockId, ref bool __result)
        {
            // if it's an emag, always pass. it's like command but even cooler
            if (keyLockId.Equals("LOCK_ID_EMAG"))
            {
                __result = true;
                return;
            }

            // if it doesn't match, isn't an emag but DID pass the original check, it's a command keycard.
            // Only fail if targeting a syndicate crate
            if (__result && LockHelper.GetLock(box).Equals("LOCK_ID_SYNDICATE"))
            {
                __result = false;
                return;
            }

            // otherwise just a normal card on a normal box
            return;
        }
    }
}
