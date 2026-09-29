using MelonLoader;

[assembly: MelonInfo(typeof(ProbablyNuclear.Core), "porting real fr fr", "1.0.67", "lore67676", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace ProbablyNuclear;

public class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Probabl nuclaer initialized.");
    }
}