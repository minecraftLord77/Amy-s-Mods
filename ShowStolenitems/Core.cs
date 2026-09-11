using MelonLoader;

[assembly: MelonInfo(typeof(ShowStolenitems.Core), "ShowStolenitems", "1.0.0", "wilso", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace ShowStolenitems
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized. I'm blue.");
        }
    }
}