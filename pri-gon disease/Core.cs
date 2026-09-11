using MelonLoader;
using HarmonyLib;
using Il2Cpp;
[assembly: MelonInfo(typeof(pri_gon_disease.Core), "pri-gon disease", "1.0.0", "wilso", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace pri_gon_disease;

public class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Prigonal prism.");
    }
}

[HarmonyPatch(typeof(HusbandryHelper),nameof(HusbandryHelper.OnGetPrionDisease))]
public class prionpatch
{
    public static bool Prefix()
    {
        return false;
    }
}