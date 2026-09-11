using MelonLoader;
using HarmonyLib;
using Il2Cpp;
[assembly: MelonInfo(typeof(testingchamber1.Core), "testingchamber1", "1.0.0", "amy", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace testingchamber1
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}

[HarmonyPatch(typeof(PhoneUIManager),nameof(PhoneUIManager.CloseUI))]
public static class debugpatch
{
    public static void Postfix()
    {
        PlayerStore.Instance.storeClientManager.AddNextClient(StoreClientListBarter.CreateAddictGeneral());
    }
}