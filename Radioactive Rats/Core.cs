using MelonLoader;

[assembly: MelonInfo(typeof(Radioactive_Rats.Core), "Radioactive Rats", "1.0.0", "amy", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace Radioactive_Rats;

public class Core : MelonMod
{
    public static MelonLogger.Instance logger;
    public override void OnInitializeMelon()
    {
        logger = LoggerInstance;
        logger.Msg("Initialized. Aug radar active");
        //ModHook.OnGameLoadedLate += (Action)AddItems;
    }
}

public static class Sprite 