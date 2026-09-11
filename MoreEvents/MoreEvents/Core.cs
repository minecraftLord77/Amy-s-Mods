using HarmonyLib;
using Il2Cpp;
using Il2CppHarmonyLib;
using MelonLoader;
using static Il2CppSystem.Globalization.HebrewNumber;
using static UnityEngine.UIElements.UIR.Allocator2D;
[assembly: MelonInfo(typeof(MoreEvents.Core), "MoreEvents", "1.0.0", "amyalt125", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace MoreEvents;


public class Core : MelonMod
{
    public static string modIdentifier = "MoreEvents";
    public static bool hasPatched = false;
    public static MelonLogger.Instance logger;
    public override void OnInitializeMelon()
    {
        logger = LoggerInstance;
        logger.Msg("Initialized. Eventuality.");
        EventsPatch.patchAllEvents();
    }
    
    public static int isApocalypse()
    {
        if (StoreStation.instance.storeEventManager.IsEventActive("societalCollapse2"))
        {
            return 2;
        }
        if (StoreStation.instance.storeEventManager.IsEventActive("societalCollapse1"))
        {
            return 1;
        }
        
        return 0;
    }
}

public static class EventsPatch
{
    public static void patchAllEvents()
    {
        if (Core.hasPatched)
        {
            return;
        }
        

        foreach (var blueprint in NormalEventListModded.NewNormalEventBlueprints)
        {
            StoreEventManager.normalEventBlueprints.Add(blueprint);
        }
        foreach (var blueprint in CosmeticEventListModded.NewCosmeticEventBlueprints)
        {
            StoreEventManager.cosmeticEventBlueprints.Add(blueprint);
        }
        foreach (var kvp in ActionDictModded.newActions)
        {
            StoreEventActionDict.actions[kvp.Key] = kvp.Value;
        }
        Core.hasPatched = true;
    }
}
[HarmonyPatch(typeof(SecData),"CommitCrime")]
public static class InvestigationProgressPatch
{
    public static bool Prefix(string crimeID, int amount)
    {
        if (StoreStation.instance.storeEventManager.IsEventActive("contrabandCrackdown"))
        {
            amount = RNG.MultiplyAndRound(amount, 1.25);
        }
        return true;
    }
}

[HarmonyPatch(typeof(PhoneUIManager),"CloseUI")]
public class DebugPatch
{
    public static void Postfix()
    {
        //Core.logger.Msg("Debug events added");
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateTerroristAttack(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateFoodRecall(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateUpperLevelParty(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateContrabandCrackdown(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateGoldenTicketHunt(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateFactoryFarmRenovation(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateCommandLeak(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateRobbedTrain(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateSecurityBreach(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(CosmeticEventListModded.CreateCrazyManYellingAtTheSky(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateExpiredImmunivaxDumping(), 1, true);
        //StoreStation.instance.storeEventManager.QueueFuturEvent(NormalEventListModded.CreateSocietalCollapse1(), 0, true);

    }
}

public class ActionDictModded
{
    public static Dictionary<string, Action> newActions = new Dictionary<string, Action>()
    {
        {
            "terroristAttackModdedAction",
            delegate()
            {
                PlayerStore.instance.AddDirectSellingItemToTable(DirectoryMaster.Item("c4"),true,true,false,200);
            }
        },
        {
            "foodRecallModdedAction",
            delegate ()
            {
                StoreClient storeClient = ModdedEventClients.CreateShittyCustomer();
                storeClient.eventSourceId="foodRecall";
                PlayerStore.Instance.storeClientManager.AddClient(storeClient);
            }
        },
        {
            "upperLevelPartyModdedAction",
            delegate()
            {
                StoreClient storeClient = StoreClientListTierSubstance.CreateUpperLevelHedonist();
                storeClient.eventSourceId="upperLevelParty";
                PlayerStore.Instance.storeClientManager.AddClient(storeClient);
            }
        },
        {
            "contrabandCrackdownModdedAction",
            delegate ()
            {
                if (RNG.Roll(75))
                {
                    StoreClient storeClient = StoreClientList.CreateInspectionClient();
                    storeClient.eventSourceId="contrabandCrackdown";
                    PlayerStore.Instance.storeClientManager.AddClient(storeClient);
                }
            }
        },
        {
            "goldenTicketHuntModdedAction",
            delegate ()
            {
                if (RNG.Roll(50))
                {
                    StoreClient storeClient = ModdedEventClients.CreateUpperLevelTreatHunter();
                    storeClient.eventSourceId="goldenTicketHunt";
                    PlayerStore.Instance.storeClientManager.AddClient(storeClient);
                }
                
            }
        },
        {
            "factoryFarmRenovationModdedAction",
            delegate ()
            {
                StoreClient storeClient = StoreClientListEvent.CreateFoodBuyerClient();
                storeClient.eventSourceId="factoryFarmRenovation";
                PlayerStore.Instance.storeClientManager.AddClient(storeClient);

            }
        },
        {
            "hospitalRenovationModdedAction",
            delegate ()
            {
                StoreClient storeClient = StoreClientListEvent.CreatePharmacistClient();
                storeClient.eventSourceId="hospitalRenovation";
                PlayerStore.Instance.storeClientManager.AddClient(storeClient);

            }
        },
        {
            "commandLeakModdedAction",
            delegate ()
            {
                if (RNG.Roll(60))
                {
                    StoreClient storeClient = StoreClientListInformation.CreateShadyCustomer();
                    storeClient.eventSourceId="commandLeak";
                    PlayerStore.Instance.storeClientManager.AddClient(storeClient);
                }
            }
        },
        {
            "robbedTrainModdedAction",
            delegate ()
            {
                if (RNG.Roll(70))
                {
                    StoreClient storeClient = ModdedEventClients.CreateTrainThiefCrateSeller();
                    storeClient.eventSourceId="robbedTrain";
                    PlayerStore.Instance.storeClientManager.AddClient(storeClient);
                }
            }
        },
        {
            "securityBreachModdedAction",
            delegate ()
            {
                StoreClient storeClient = ModdedEventClients.CreateSecurityBreachCrateSeller();
                storeClient.eventSourceId="securityBreach";
                PlayerStore.Instance.storeClientManager.AddClient(storeClient);
            }
        },
        {
            "expiredImmunivaxDumpingModdedAction",
            delegate ()
            {
                StoreClient storeClient = ModdedEventClients.CreateExpiredImmunivaxSeller();
                storeClient.eventSourceId="expiredImmunivaxDumping";
                PlayerStore.Instance.storeClientManager.AddClient(storeClient);
            }
        },
    };
}

public class ModdedEventClients()
{
    public static StoreClient CreateShittyCustomer()
    {
        StoreClient storeClient = new StoreClient();
        storeClient.identifier = "spacerShit";
        storeClient.displayName = "Lower Level Spacer";
        storeClient.spriteName = "femaleLower1";
        storeClient.clientFaction = "FACTION_LOWER_LEVEL";
        storeClient.useClientBudget = true;
        int budget = RNG.GetRandomInt(35, 50);
        storeClient.SetBudget(budget);
        storeClient.clientIntent = StoreClient.ClientIntent.BUY;
        storeClient.canClientExposeFeature = new Func<ItemFeature, StoreClient, bool>(ClientCanExposeFunc.BasicClientExposeCapacity);
        storeClient.mainDialogue.SetText(storeClient.displayName, "Do you have any toilet paper? Or any bandages? Any kind of fabric?")
            .NextDialogue().SetText(storeClient.displayName, string.Format("Be quick, I brought {0} credits.", storeClient.GetBudget()))
            .SetEndAction(
            (Action) delegate
            {
            }
            );
        //This is stupid that i need to do this
        storeClient.clientBuyingIdList = new Il2CppSystem.Collections.Generic.List<string>();
        storeClient.clientBuyingTagList.Add("DOCUMENT");
        storeClient.clientBuyingIdList.Add("toilet_paper");
        storeClient.clientBuyingIdList.Add("kotton_fabric");
        storeClient.clientBuyingIdList.Add("bandage_item");
        storeClient.clientBuyingIdList.Add("hemostatic_bandage_item");
        storeClient.clientBuyingIdList.Add("topical_bandage_item");
        storeClient.clientBuyingIdList.Add("wanted_paper");
        
        storeClient.AddBasicDialog();
        storeClient.CompleteClientCreation();
        return storeClient;
    }
    public static StoreClient CreateUpperLevelTreatHunter()
    {
        StoreClient storeClient = new StoreClient();
        storeClient.identifier = "upperLevelTreatHunter";
        storeClient.displayName = "Upper Level Adventurer";
        storeClient.spriteName = SpriteDict.GetRandomUpperSpriteName();
        storeClient.clientFaction = "FACTION_UPPER_LEVEL";
        storeClient.useClientBudget = true;
        storeClient.SetBudget(RNG.GetRandomInt(250, 300) + StoreStation.GetDayCounter() * 2);
        storeClient.clientIntent = StoreClient.ClientIntent.BUY;
        storeClient.canClientExposeFeature = new Func<ItemFeature, StoreClient, bool>(ClientCanExposeFunc.BasicClientExposeCapacity);
        storeClient.mainDialogue.SetText(storeClient.displayName, "Hello, my child wants to see the candy factory.")
            .NextDialogue().SetText(storeClient.displayName, "Show me your best stock.")
            .SetEndAction(
            (Action)delegate{}
            );
        storeClient.clientBuyingTagList.Add("TREAT");
        storeClient.AddBasicDialog();
        storeClient.CompleteClientCreation();
        return storeClient;
    }
    public static StoreClient CreateTrainThiefCrateSeller() 
    {
        StoreClient storeClient = new StoreClient();
        storeClient.identifier = "trainThief";
        storeClient.displayName = "Thief";

        storeClient.spriteName = RNG.Roll(50) ? SpriteDict.GetRandomBMMaleSpriteName() : SpriteDict.GetRandomBMFemaleSpriteName();

        storeClient.clientFaction = "FACTION_BLACK_MARKET";
        storeClient.clientIntent = StoreClient.ClientIntent.SELL;
        storeClient.isOfferWholesale = true;
        storeClient.wholesaleDiscount = -10;
        storeClient.canClientExposeFeature = new Func<ItemFeature, StoreClient, bool>(ClientCanExposeFunc.BasicClientExposeCapacity);
        storeClient.mainDialogue.SetText(storeClient.displayName, RNG.Roll(50) ? "I have some crates for you, fresh off the train." : "Hey, I got some crates I need to sell.")
            .NextDialogue().SetText(storeClient.displayName, RNG.Roll(50) ? "Discount if you take the lot." : "I'm in a rush, take all of it if you can.")
            .NextDialogue().SetText(storeClient.displayName, RNG.Roll(50) ? "Take a quick peek." : "Don't take too long.")
            .SetEndAction(
            (Action)delegate 
            {
                List<GameItem> boxes = new List<GameItem>
                {
                    PreBuiltItemHelper.LootCrateEngineering(),
                    PreBuiltItemHelper.LootCrateEvidence(),
                    PreBuiltItemHelper.LootCrateMedical(),
                    PreBuiltItemHelper.LootCrateSecurity(),
                    PreBuiltItemHelper.LootCrateService(),
                };
                int count = RNG.GetRandomInt(1, 3);
                for (int i =0;i<count;i++)
                {
                    int value = RNG.GetRandomIntExculsive(0, boxes.Count);
                    PlayerStore.Instance.AddDirectSellingItemToTable(boxes[value], false, true, false, 100);
                    boxes.RemoveAt(value);
                }
                
            }
            );
        storeClient.AddBasicDialog();
        storeClient.CompleteClientCreation();
        return storeClient;
    }
    public static StoreClient CreateSecurityBreachCrateSeller()
    {
        StoreClient storeClient = new StoreClient();
        storeClient.identifier = "securityBreachCrateSeller";
        storeClient.displayName = "Thief";

        storeClient.spriteName = RNG.Roll(50) ? SpriteDict.GetRandomBMMaleSpriteName() : SpriteDict.GetRandomBMFemaleSpriteName();

        storeClient.clientFaction = "FACTION_BLACK_MARKET";
        storeClient.clientIntent = StoreClient.ClientIntent.SELL;
        storeClient.isOfferWholesale = true;
        storeClient.wholesaleDiscount = -10;
        storeClient.canClientExposeFeature = new Func<ItemFeature, StoreClient, bool>(ClientCanExposeFunc.BasicClientExposeCapacity);
        storeClient.mainDialogue.SetText(storeClient.displayName, RNG.Roll(50) ? "Got some goodies from Security." : "Gotta get rid of these quick.")
            .NextDialogue().SetText(storeClient.displayName,"I'll offer you a discount if you take the lot")
            .NextDialogue().SetText(storeClient.displayName, RNG.Roll(50) ? "Be quick." : "Don't take too long.")
            .SetEndAction(
            (Action)delegate
            {
                int heat = RNG.GetRandomInt(80, 100);
                PlayerStore.Instance.AddDirectSellingItemToTable(PreBuiltItemHelper.LootCrateEvidence(), false, true, false, heat);
                PlayerStore.Instance.AddDirectSellingItemToTable(PreBuiltItemHelper.LootCrateSecurity(), false, true, false, heat);
                GameItem keycard = DirectoryMaster.Item("sec_keycard");
                keycard.flavorText = "Ex-Officer Chen's personal keycard. Beer stains are visible.";
                PlayerStore.Instance.AddDirectSellingItemToTable(keycard, false, true, false, heat);

            }
            );
        storeClient.AddBasicDialog();
        storeClient.CompleteClientCreation();
        return storeClient;
    }
    public static StoreClient CreateExpiredImmunivaxSeller()
    {
        StoreClient storeClient = new StoreClient();
        storeClient.identifier = "expiredImmunivaxSeller";
        storeClient.displayName = "Scavenger";

        storeClient.spriteName = SpriteDict.GetRandomScavSpriteName();

        storeClient.clientFaction = "FACTION_LOWER_LEVEL";
        storeClient.clientIntent = StoreClient.ClientIntent.SELL;
        storeClient.isOfferWholesale = true;
        storeClient.wholesaleDiscount = -20;
        storeClient.canClientExposeFeature = new Func<ItemFeature, StoreClient, bool>(ClientCanExposeFunc.BasicClientExposeCapacity);
        storeClient.mainDialogue.SetText(storeClient.displayName, RNG.Roll(50) ? "Did some dumpster diving." : "I heard these were a bit more valuable.")
            .NextDialogue().SetText(storeClient.displayName, "I'll offer you a discount if you take 'em all'")
            .NextDialogue().SetText(storeClient.displayName, "Pretty sure they're good.")
            .SetEndAction(
            (Action)delegate
            {
                for (int i=0; i < 12; i++)
                {
                    PlayerStore.Instance.AddDirectSellingItemToTable(InsInjectorHelper.CreateExpiredInjector());
                }

            }
            );
        storeClient.AddBasicDialog();
        storeClient.CompleteClientCreation();
        return storeClient;
    }

}

public class NormalEventListModded
{
    public static List<StoreEventBlueprint> NewNormalEventBlueprints = new List<StoreEventBlueprint>
    {
        new StoreEventBlueprint(new Func<StoreEvent>(CreateTerroristAttack),2,"terroristAttack"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateFoodRecall),7,"foodRecall"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateUpperLevelParty),5,"upperLevelParty"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateContrabandCrackdown),7,"contrabandCrackdown"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateGoldenTicketHunt),1,"goldenTicketHunt"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateFactoryFarmRenovation),4,"factoryFarmRenovation"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateHospitalRenovation),4,"hospitalRenovation"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateCommandLeak),3,"commandLeak"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateRobbedTrain),5,"robbedTrain"),
        new StoreEventBlueprint(new Func<StoreEvent>(CreateSecurityBreach),7,"securityBreach"),
        //new StoreEventBlueprint(new Func<StoreEvent>(CreateExpiredImmunivaxDumping),10,"securityBreach"),
    };              
    public static StoreEvent CreateTerroristAttack()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "terroristAttack";
        storeEvent.newsName = "Attack On Our Soil!";
        storeEvent.newsDescription = "In a routine terrorist attack, many parts of the station were destroyed. Security has appealed for calm, while fires ravage the station. Expect a large price hike in many sectors.";

        storeEvent.displayName = "Terrorist Attack";

        storeEvent.duration = 7;
        storeEvent.importance = 9;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("POISON", 100, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("MATERIAL", 200, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("WEAPON", 100, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateFoodRecall()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "foodRecall";
        storeEvent.newsName = "New Virus Found In Nutrifruit!";
        storeEvent.newsDescription = "Many customers of Nutrifruit™ have reported symptoms of food poisoning. If you own any affected Nutrifruit™, a Nutrifruit™ Safety Officer will have confiscated it already. Expect food prices to rise.";

        storeEvent.displayName = "Food Contamination";

        storeEvent.duration = 1;
        storeEvent.importance = 3;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("FOOD", 25, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("MEDICAL", 10, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("HOUSEHOLD_GOOD", 10, storeEvent.newsName, storeEvent.displayName));
        
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateUpperLevelParty()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "upperLevelParty";
        storeEvent.newsName = "Upcoming Upper Level Party!";
        storeEvent.newsDescription = "A local hedonist has started sending invites for a big party at the local club. Experts suggest this party is gonna bring folks from all the corners of the station and illegal goods will potentially be exchanged. The security is keeping a watch over the situation, but for now remains under control.";

        storeEvent.displayName = "Upper Level Party";

        storeEvent.duration = 2;
        storeEvent.importance = 3;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.UPPER;
        storeEvent.negociationDatas.Add(new NegociationData("SUBSTANCE", 25, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("LUXURY_ITEM", 25, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("ALCOHOL", 25, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateContrabandCrackdown()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "contrabandCrackdown";
        storeEvent.newsName = "Security Starts War On Contraband";
        storeEvent.newsDescription = "Station Command has issued a new directive, ordering Security to seize as much contraband as possible. Masses of contraband are being seized and destroyed under the new orders. Expect contraband prices to rise.";

        storeEvent.displayName = "Contraband Crackdown";

        storeEvent.duration = 3;
        storeEvent.importance = 7;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("CONTRABAND", 300, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateGoldenTicketHunt()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "goldenTicketHunt";
        storeEvent.newsName = "Golden Ticket Hunt";
        storeEvent.newsDescription = "The infamous candy maker Pilly Ponka has announced the search for the Golden Ticket hidden in her treats. Winners get to tour around her mysterious candy factory. Expect high demand for treats.";

        storeEvent.displayName = "Golden Ticket Hunt";

        storeEvent.duration = 5;
        storeEvent.importance = 7;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("TREAT", 100, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateFactoryFarmRenovation()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "factoryFarmRenovation";
        storeEvent.newsName = "Nutrifruit Co. Factory Farm Renovation";
        storeEvent.newsDescription = "The famed hydroponics company Nutrifruit Co. is renovating their main factory farm! Local unrest grows as suppliers begin hoarding supplies for the long wait.";

        storeEvent.displayName = "Factory Farm Renovation";

        storeEvent.duration = RNG.GetRandomInt(3,4);
        storeEvent.importance = 99;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("MATERIAL", 40, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("FOOD", 30, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        storeEvent.onActiveActionId = storeEvent.identifier + "ModdedOnActiveAction";
        StoreStation.instance.storeEventManager.QueueFuturEvent(CreateFactoryFarmRenovationResupply(), 3, true);
        return storeEvent;
    }
    public static StoreEvent CreateFactoryFarmRenovationResupply()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "factoryFarmRenovationResupply";
        storeEvent.newsName = "Nutrifruit Renovation Resupply Arrives";
        storeEvent.newsDescription = "By request of Command from nearby stations, resupply ships arrive en masse, carrying large amounts of construction supplies and provisions. Expect material and food supplies to fall slightly.";

        storeEvent.displayName = "Factory Farm Renovation Resupply";

        storeEvent.duration = 2;
        storeEvent.importance = 98;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("MATERIAL", -20, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("FOOD", -15, storeEvent.newsName, storeEvent.displayName));
        return storeEvent;
    }
    public static StoreEvent CreateHospitalRenovation()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "hospitalRenovation";
        storeEvent.newsName = "Lower Level Hospital Renovation";
        storeEvent.newsDescription = "Maslock Hospital is undergoing a major renovation. Two clients have been redirected to nearby available hospitals. Expect medical prices to rise.";

        storeEvent.displayName = "Hospital Renovation";

        storeEvent.duration = RNG.GetRandomInt(3, 4);
        storeEvent.importance = 91;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("MEDICAL", 30, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";

        StoreStation.instance.storeEventManager.QueueFuturEvent(CreateHospitalRenovationResupply(), RNG.GetRandomInt(1,3), false);
        return storeEvent;
    }
    public static StoreEvent CreateHospitalRenovationResupply()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "hospitalRenovationResupply";
        storeEvent.newsName = "Hospital Renovation Resupply Arrives";
        storeEvent.newsDescription = "By request of Command from nearby stations, resupply ships arrive en masse, carrying large amounts of construction supplies and provisions. Expect material and medical supplies to fall slightly.";

        storeEvent.displayName = "Factory Farm Renovation Resupply";

        storeEvent.duration = 2;
        storeEvent.importance = 90;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("MEDICAL", -15, storeEvent.newsName, storeEvent.displayName));
        return storeEvent;
    }
    public static StoreEvent CreateCommandLeak()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "commandLeak";
        storeEvent.newsName = "Command Database Leaked!";
        storeEvent.newsDescription = "Security personnel are on the watch, as Command reports a massive data breach caused by unidentified hackers. Expect shady information to be available for the upcoming couple of days.";

        storeEvent.displayName = "Command Leak";

        storeEvent.duration = 5;
        storeEvent.importance = 2;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;

        storeEvent.negociationDatas.Add(new NegociationData("ACCESS_CARD", 30, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateRobbedTrain()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "robbedTrain";
        storeEvent.newsName = "High Stakes Robbery!";
        storeEvent.newsDescription = "A routine train shipment has been robbed! The train targetted was carrying supply crates and access cards. Expect a sharp fall in their prices.";

        storeEvent.displayName = "Train Robbery";

        storeEvent.duration = 3;
        storeEvent.importance = 9;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("SUPPLY_CRATE", -20, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("ACCESS_CARD", +20, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateSecurityBreach()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "securityBreach";
        storeEvent.newsName = string.Format("Security Breach In Sector 0{0}",RNG.GetRandomInt(0,9));
        storeEvent.newsDescription = "Security has reported a break-in into a secure storage area. Rumors report that thieves broke in using a Security officer's keycard left at a bar. Security has refused to comment on this matter.";

        storeEvent.displayName = "Security Breach";

        storeEvent.duration = 1;
        storeEvent.importance = 9;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    //public static StoreEvent CreateExpiredImmunivaxDumping()
    //{
    //    StoreEvent storeEvent = new StoreEvent();
    //    storeEvent.identifier = "expiredImmunivaxDumping";
    //    storeEvent.newsName = "Immunivax™ Dumping";
    //    storeEvent.newsDescription = "In a recent storage inspection, the Medical department has tossed out hundreds of Immunivax™s that expired yesterday. Many of these have found their way to scavengers' pockets. Expect medical prices to go down.";

    //    storeEvent.displayName = "Immunivax Recall";

    //    storeEvent.duration = 2;
    //    storeEvent.importance = 6;
    //    storeEvent.eventType = StoreEvent.EventType.NORMALE;
    //    storeEvent.eventArea = StoreEvent.EventArea.ALL;
    //    storeEvent.negociationDatas.Add(new NegociationData("MEDICAL", -20, storeEvent.newsName, storeEvent.displayName));
    //    storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
    //    return storeEvent;
    //}
    public static StoreEvent CreateSocietalCollapse1()
    {
        if (Core.isApocalypse() > 0 || StoreStation.GetDayCounter()<100)
        {
            Core.logger.Msg("Apocalypse active or day too low, fallback initiated");
            return CreateTerroristAttack();
        }
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "societalCollapse1";
        storeEvent.newsName = "Aug Invasion On The Lower Levels";
        storeEvent.newsDescription = "Security is reporting a small insurgent group of Augs on the Lower Levels. Citizens are to stay where they are and quarantine. Security has declined to comment on the severity of the situation.";

        storeEvent.displayName = "Aug Uprising";

        storeEvent.duration = RNG.GetRandomInt(3,5);
        storeEvent.importance = 15;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.LOWER;
        storeEvent.negociationDatas.Add(new NegociationData("WEAPON", 20, storeEvent.newsName, storeEvent.displayName));
        storeEvent.negociationDatas.Add(new NegociationData("AMMUNITION", 20, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
    public static StoreEvent CreateSocietalCollapse2()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "societalCollapse2";
        storeEvent.newsName = "Security Overwhelmed";
        storeEvent.newsDescription = "The Aug force is higher and more powerful than security expected. ";

        storeEvent.displayName = "The End Is Near";

        storeEvent.duration = 2;
        storeEvent.importance = 6;
        storeEvent.eventType = StoreEvent.EventType.NORMALE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        storeEvent.negociationDatas.Add(new NegociationData("MEDICAL", -20, storeEvent.newsName, storeEvent.displayName));
        storeEvent.addClientFromEventActionId = storeEvent.identifier + "ModdedAction";
        return storeEvent;
    }
}

public class CosmeticEventListModded
{
    public static List<StoreEventBlueprint> NewCosmeticEventBlueprints = new List<StoreEventBlueprint>
    {
        new StoreEventBlueprint(new Func<StoreEvent>(CreateCrazyManYellingAtTheSky),10,"crazyManYellingAtTheSky"),

    };
    public static StoreEvent CreateCrazyManYellingAtTheSky()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "crazyManYellingAtTheSky";
        storeEvent.newsName = "Crazy Man Yelling At Space";
        storeEvent.newsDescription = "As I must fill out the word and news quota for my ration, today's only exciting news is of my old man yelling at a shuttle through a window. He still has not figured out that they cannot hear him through space.";

        storeEvent.displayName = "Factory Farm Renovation Resupply";

        storeEvent.eventType = StoreEvent.EventType.COSMETIC;
        return storeEvent;
    }
}

public class pictures
{
  

    public static string nutrifruit = "<size=50%><cspace=0px><b></mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■<mark=#292234ff>■■■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■<mark=#A272ACff>■■■■■■■■■■■■■<mark=#292234ff>■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■<mark=#A272ACff>■■■■■■■■■■■■■<mark=#292234ff>■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■<mark=#A272ACff>■■■■■■■■■■■■■<mark=#292234ff>■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■<mark=#777081ff>■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■<mark=#777081ff>■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■<mark=#777081ff>■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■<mark=#6B5269ff>■■■■■■<mark=#777081ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■<mark=#6B5269ff>■■■■■■<mark=#777081ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■■■<mark=#777081ff>■■■<mark=#6B5269ff>■■■<mark=#777081ff>■■<mark=#A272ACff>■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■■■<mark=#777081ff>■■■<mark=#6B5269ff>■■■<mark=#777081ff>■■<mark=#A272ACff>■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■■■<mark=#777081ff>■■■<mark=#6B5269ff>■■■<mark=#777081ff>■■<mark=#A272ACff>■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■<mark=#88948Aff>■■■■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■<mark=#88948Aff>■■■■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■■■<mark=#292234ff>■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■■■<mark=#292234ff>■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■■■<mark=#292234ff>■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■<mark=#88948Aff>■■■■■■■■<mark=#A272ACff>■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#6B5269ff>■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#292234ff>■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■<br>■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■<mark=#88948Aff>■■■■■■■■<mark=#A272ACff>■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#6B5269ff>■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#292234ff>■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■<br>■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■<mark=#88948Aff>■■■■■■■■<mark=#A272ACff>■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#6B5269ff>■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#292234ff>■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■<br>■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#6B5269ff>■■■■■<mark=#BB9165ff>■■■■■■<mark=#C0A0A4ff>■■<mark=#BB9165ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■<br>■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#A272ACff>■■■■■■■■■■■<mark=#88948Aff>■■■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■<mark=#6B5269ff>■■■■■<mark=#BB9165ff>■■■■■■<mark=#C0A0A4ff>■■<mark=#BB9165ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■■■■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#6B5269ff>■■■■■■■■<mark=#BB9165ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#6B5269ff>■■■■■■■■<mark=#BB9165ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#6B5269ff>■■■■■■■■<mark=#BB9165ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■<mark=#4A2F38ff>■■<mark=#6B5269ff>■■■<mark=#A272ACff>■■■<mark=#6B5269ff>■■<mark=#BB9165ff>■■■■■■<mark=#C0A0A4ff>■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■<mark=#4A2F38ff>■■<mark=#6B5269ff>■■■<mark=#A272ACff>■■■<mark=#6B5269ff>■■<mark=#BB9165ff>■■■■■■<mark=#C0A0A4ff>■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■■■■■■■<mark=#4A2F38ff>■■<mark=#6B5269ff>■■■<mark=#A272ACff>■■■<mark=#6B5269ff>■■<mark=#BB9165ff>■■■■■■<mark=#C0A0A4ff>■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#6B5269ff>■■<mark=#BB9165ff>■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#BB9165ff>■■■■■<mark=#C0A0A4ff>■■■<mark=#BB9165ff>■■■■■<mark=#C0A0A4ff>■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#6B5269ff>■■<mark=#BB9165ff>■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#BB9165ff>■■■■■<mark=#C0A0A4ff>■■■<mark=#BB9165ff>■■■■■<mark=#C0A0A4ff>■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#BB9165ff>■■■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#BB9165ff>■■■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#BB9165ff>■■■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#777081ff>■■■■■■<mark=#A272ACff>■■<mark=#777081ff>■■■■■■<mark=#292234ff>■■<mark=#6B5269ff>■■■<mark=#A272ACff>■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■<mark=#6B5269ff>■■■<mark=#C0A0A4ff>■■■■■<mark=#BB9165ff>■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#777081ff>■■■■■■<mark=#A272ACff>■■<mark=#777081ff>■■■■■■<mark=#292234ff>■■<mark=#6B5269ff>■■■<mark=#A272ACff>■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■<mark=#6B5269ff>■■■<mark=#C0A0A4ff>■■■■■<mark=#BB9165ff>■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■<mark=#777081ff>■■■■■■<mark=#A272ACff>■■<mark=#777081ff>■■■■■■<mark=#292234ff>■■<mark=#6B5269ff>■■■<mark=#A272ACff>■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#A272ACff>■■■<mark=#88948Aff>■■■<mark=#A272ACff>■■<mark=#6B5269ff>■■■<mark=#C0A0A4ff>■■■■■<mark=#BB9165ff>■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■<mark=#A272ACff>■■■■■<mark=#777081ff>■■■■■■■■<mark=#A272ACff>■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#A272ACff>■■■■■■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■<mark=#C0A0A4ff>■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■<mark=#A272ACff>■■■■■<mark=#777081ff>■■■■■■■■<mark=#A272ACff>■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#A272ACff>■■■■■■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■<mark=#C0A0A4ff>■■■■■■<mark=#BB9165ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■■■■<mark=#777081ff>■■<mark=#6B5269ff>■■■<mark=#777081ff>■■■■■<mark=#A272ACff>■■■<mark=#6B5269ff>■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#777081ff>■■■<mark=#A272ACff>■■■<mark=#292234ff>■■■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■<mark=#C0A0A4ff>■■■<mark=#BB9165ff>■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■■■■<mark=#777081ff>■■<mark=#6B5269ff>■■■<mark=#777081ff>■■■■■<mark=#A272ACff>■■■<mark=#6B5269ff>■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#777081ff>■■■<mark=#A272ACff>■■■<mark=#292234ff>■■■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■<mark=#C0A0A4ff>■■■<mark=#BB9165ff>■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■■■■<mark=#777081ff>■■<mark=#6B5269ff>■■■<mark=#777081ff>■■■■■<mark=#A272ACff>■■■<mark=#6B5269ff>■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■<mark=#777081ff>■■■<mark=#A272ACff>■■■<mark=#292234ff>■■■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■<mark=#C0A0A4ff>■■■<mark=#BB9165ff>■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■■■■<mark=#A272ACff>■■■<mark=#292234ff>■■<mark=#6B5269ff>■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■■■■<mark=#A272ACff>■■■<mark=#292234ff>■■<mark=#6B5269ff>■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■<alpha=#FF><mark=#292234ff>■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#4A2F38ff>■■■<mark=#6B5269ff>■■■■■■■■<mark=#A272ACff>■■■<mark=#292234ff>■■<mark=#6B5269ff>■■■<mark=#777081ff>■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■<mark=#777081ff>■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■■<mark=#6B5269ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■<mark=#777081ff>■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■■<mark=#6B5269ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#777081ff>■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#BB9165ff>■■■■■■■■■■■<mark=#6B5269ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#777081ff>■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#BB9165ff>■■■■■■■■■■■<mark=#6B5269ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#777081ff>■■■■■■■■■■■<mark=#C0A0A4ff>■■■■■■■■<mark=#BB9165ff>■■■■■■■■■■■<mark=#6B5269ff>■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■<br>■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■<mark=#292234ff>■■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#777081ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#6B5269ff>■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■<mark=#292234ff>■■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#777081ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#6B5269ff>■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■<mark=#292234ff>■■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#777081ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■<mark=#BB9165ff>■■■■■■■■<mark=#6B5269ff>■■■<mark=#292234ff>■■</mark></mark><alpha=#00>X■■■■<br>■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■<br>■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■<alpha=#FF><mark=#292234ff>■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■</mark></mark><alpha=#00>X■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■<mark=#6B5269ff>■■■■■■■■■■■■■■■■■■<mark=#292234ff>■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■<br>■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■<alpha=#FF><mark=#292234ff>■■■■■■■■■■■■■■■■■■■</mark></mark><alpha=#00>X■■■■■■■■■■■■■■■■■■■■<br></size></cspace><alpha=#FF></mark></b>";
}