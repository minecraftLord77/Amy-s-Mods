
namespace ProbablyNuclear
{
    public class ModPreBuiltItems
    {
        public static GameItem LootCrateSyndicate()
        {
            GameItem gameItem = DirectoryMaster.Item("syndicate_box");

            // Guarantee at least 1 high value item
            if (RNG.Roll(50))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("holoparasite_injector"));
            else
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("energy_sword"));

            // Pick from 1 large item as to not fill the whole box
            if (RNG.Roll(30))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("cobra"));
            else if (RNG.Roll(50))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("syndicate_gas_mask"));
            else
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("c4"));

            if (RNG.Roll(40))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("throwing_knife"));
            if (RNG.Roll(40))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("hyperzine_injector"));
            if (RNG.Roll(25))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("hyperzine_injector"));
            if (RNG.Roll(15))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("hyperzine_injector"));
            if (RNG.Roll(40))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("implanter"));
            if (RNG.Roll(40))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("cybersun_pen"));
            if (RNG.Roll(90))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("interdyne_herbals"));
            if (RNG.Roll(50))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("minibomb"));
            if (RNG.Roll(25))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("minibomb"));
            if (RNG.Roll(80))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("syndicate_soap"));

            GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("telecrystal"));
            if (RNG.Roll(85))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("telecrystal"));
            if (RNG.Roll(60))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("telecrystal"));
            if (RNG.Roll(35))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("telecrystal"));
            if (RNG.Roll(20))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("telecrystal"));
            if (RNG.Roll(5))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("telecrystal"));
            if (RNG.Roll(40))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("smoke_grenade"));
            if (RNG.Roll(50))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("heavy_pistol_ammo"));
            if (RNG.Roll(25))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("heavy_pistol_ammo"));
            if (RNG.Roll(40))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("heavy_pistol_ammo_p"));
            if (RNG.Roll(15))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("heavy_pistol_ammo_p"));
            if (RNG.Roll(60))
                GraphUtils.TryAcceptAll(gameItem, DirectoryMaster.Item("zyanide_pill"));

            LockHelper.LockUpContainer(gameItem);
            return gameItem;
        }

        public static GameItem SyndicateLootLow()
        {
            int rng = RNG.GetRandomInt(1, 100);

            return rng switch
            {
                <= 40 => DirectoryMaster.Item("syndicate_soap"),
                <= 60 => DirectoryMaster.Item("hyperzine_injector"),
                <= 75 => DirectoryMaster.Item("interdyne_herbals"),
                <= 90 => DirectoryMaster.Item("cybersun_pen"),
                _ => DirectoryMaster.Item("throwing_knife"),
            };
        }

        public static GameItem SyndicateLootMid()
        {
            int rng = RNG.GetRandomInt(1, 100);

            return rng switch
            {
                <= 30 => DirectoryMaster.Item("minibomb"),
                <= 45 => DirectoryMaster.Item("hyperzine_injector"),
                <= 53 => DirectoryMaster.Item("cybersun_pen"),
                <= 61 => DirectoryMaster.Item("throwing_knife"),
                <= 75 => DirectoryMaster.Item("syndicate_gas_mask"),
                <= 90 => DirectoryMaster.Item("implanter"),
                _ => DirectoryMaster.Item("suspicious_toolbox"),
            };
        }

        public static GameItem SyndicateLootHigh()
        {
            int rng = RNG.GetRandomInt(1, 100);

            return rng switch
            {
                <= 15 => DirectoryMaster.Item("minibomb"),
                <= 25 => DirectoryMaster.Item("suspicious_toolbox"),
                <= 45 => DirectoryMaster.Item("holoparasite_injector"),
                <= 65 => DirectoryMaster.Item("cobra"),
                <= 85 => DirectoryMaster.Item("energy_sword"),
                _ => DirectoryMaster.Item("emag"),
            };
        }
    }
}