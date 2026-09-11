using MelonLoader;

[assembly: MelonInfo(typeof(ProbablySteaming.Core), "ProbablySteaming", "1.0.0", "wilso", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace ProbablySteaming
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}