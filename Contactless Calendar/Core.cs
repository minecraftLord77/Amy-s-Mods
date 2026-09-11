using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using UnityEngine.UI;
[assembly: MelonInfo(typeof(ContactlessCalendar.Core), "ContactlessCalendar", "1.0.0", "amyalt125", null)]
[assembly: MelonGame("Questing Goose Studio", "Probably Stolen")]

namespace ContactlessCalendar;


public class Core : MelonMod
{
    public static MelonLogger.Instance logger;
    public override void OnInitializeMelon()
    {
        logger = LoggerInstance;
        logger.Msg("Initialized. Eventuality.");
    }
    public static void QueueFuturEventSmart(StoreEvent storeEvent, int day)
    {
        if (PlayerStore.instance.storeState == PlayerStore.StoreState.CLOSED)
        {
            StoreStation.instance.storeEventManager.QueueFuturEvent(storeEvent, day, true);
        }
        else if (PlayerStore.instance.storeState == PlayerStore.StoreState.OPEN)
        {
            StoreStation.instance.storeEventManager.QueueFuturEvent(storeEvent, day, true);
        }
        else if (PlayerStore.instance.storeState == PlayerStore.StoreState.MORNING)
        {
            StoreStation.instance.storeEventManager.QueueFuturEvent(storeEvent, day, true);
        }
    }
}

[HarmonyPatch(typeof(PhoneDialogList), "RedImpRepIntro")]
public class RedImpFreePatch
{
    public static bool Prefix(ref Dialogue __result)
    {
        Dialogue dialogue = new Dialogue();
        string localizedName = LocHelper.GetLocalizedName("name_red_imp_rep");
        dialogue.SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep_intro1")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep_intro2")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep_intro3")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep_intro4")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep_intro5")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep_intro6")).SetEndAction((Action)delegate
        {
            PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            StorePhoneClient.GetBrewerPhoneClient().phoneState = StorePhoneClient.PhoneState.Regular;
            StorePhoneClient.GetBrewerPhoneClient().UseService();
            PlayerStore.Instance.QueueFuturClient("courrierRedImpFree", 1);
            Core.QueueFuturEventSmart(ContactEventList.CreateRedImpArrival(), 1);
        });
        __result = dialogue;
        return false;
    }
}
[HarmonyPatch(typeof(PhoneDialogList),"RedImpRepRegular")]
public class RedImpPatch
{
    public static bool Prefix(ref Dialogue __result)
    {

        var array1= new Il2CppReferenceArray<Il2CppSystem.Object>(2);
        array1[0] = 8;
        array1[1] = 1;
        
        var array2 = new Il2CppReferenceArray<Il2CppSystem.Object>(2);
        array2[0] = 16;
        array2[1] = 2;
        Dialogue dialogue = new Dialogue();
        string displayName = LocHelper.GetLocalizedName("name_red_imp_rep");
        string title = "PLAYER";
        Dialogue order1 = new Dialogue();
        order1.SetText(title, LocHelper.GetLocalizedDialoguePhone("red_imp_order1", array1));
        Dialogue order2 = new Dialogue();
        order2.SetText(title, LocHelper.GetLocalizedDialoguePhone("red_imp_order2", array2));
        Dialogue dialogue2 = new Dialogue();
        dialogue2.SetText(displayName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep3"));
        Dialogue quit = new Dialogue();
        quit.SetText(title, LocHelper.GetLocalizedDialoguePhone("end_call"));
        dialogue.SetText(displayName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep2")).SetNextDialogue(dialogue2);
        dialogue2.AddChoice(DirectoryMaster.Item("$", true), "", "", order1, (Action)delegate
        {
            StorePhoneClient.GetBrewerPhoneClient().stockType = 0;
            StorePhoneClient.GetBrewerPhoneClient().UseService();
            PlayerStore.Instance.QueueFuturClient("courrierRedImp", 1);
            Core.QueueFuturEventSmart(ContactEventList.CreateRedImpArrival(), 1);
            order1.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep4")).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            });
        });
        if (StorePhoneClient.GetBrewerPhoneClient().amountSpent >= 400)
        {
            dialogue2.AddChoice(DirectoryMaster.Item("$", true), "", "", order2, (Action)delegate
            {
                StorePhoneClient.GetBrewerPhoneClient().stockType = 1;
                StorePhoneClient.GetBrewerPhoneClient().UseService();
                PlayerStore.Instance.QueueFuturClient("courrierRedImp", 2);
                Core.QueueFuturEventSmart(ContactEventList.CreateRedImpArrival(), 2);
                order2.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("red_imp_rep4")).SetEndAction((Action)delegate
                {
                    PhoneUIManager.Instance.OnPhoneDialogEnd(true);
                });
            });
        }
        dialogue2.AddChoice(DirectoryMaster.Item("$", true), "", "", quit, (Action)delegate
        {
            quit.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("end_call")).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            });
        });
        Core.logger.Msg(LocHelper.GetLocalizedDialoguePhone("end_call"));
        __result = dialogue;
        Core.logger.Msg(dialogue.text);
        return false;
    }
}

[HarmonyPatch(typeof(PhoneDialogList), "MiningIntro")]
public class CaulsonFreePatch
{
    public static bool Prefix(ref Dialogue __result)
    {
        Dialogue dialogue = new Dialogue();
        string localizedName = LocHelper.GetLocalizedName("name_caulson");
        string title = "PLAYER";
        dialogue.SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("caulson_intro1")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("caulson_intro2")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("caulson_intro3")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("caulson_intro4")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("caulson_intro5")).NextDialogue().SetText(title, LocHelper.GetLocalizedDialoguePhone("caulson_intro6")).NextDialogue().SetText(localizedName, LocHelper.GetLocalizedDialoguePhone("caulson_intro7")).SetEndAction((Action)delegate
        {
            PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            StorePhoneClient.GetMiningPhoneClient().phoneState = StorePhoneClient.PhoneState.Regular;
            StorePhoneClient.GetMiningPhoneClient().UseService();
            PlayerStore.Instance.QueueFuturClient("courrierMiningFree", 1);
            Core.QueueFuturEventSmart(ContactEventList.CreateCaulsonArrival(), 1);
        });
        __result = dialogue;
        return false;
    }
}
[HarmonyPatch(typeof(PhoneDialogList), "MiningRegular")]
public class CaulsonPatch
{
    public static bool Prefix(ref Dialogue __result)
    {
        var array1 = new Il2CppReferenceArray<Il2CppSystem.Object>(2);
        array1[0] = 6;
        array1[1] = 1;

        var array2 = new Il2CppReferenceArray<Il2CppSystem.Object>(2);
        array2[0] = 12;
        array2[1] = 2;
        Dialogue dialogue = new Dialogue();
        string displayName = LocHelper.GetLocalizedName("name_caulson");
        string title = "PLAYER";
        Dialogue order1 = new Dialogue();
        order1.SetText(title, LocHelper.GetLocalizedDialoguePhone("mining_order1", array1));
        Dialogue order2 = new Dialogue();
        order2.SetText(title, LocHelper.GetLocalizedDialoguePhone("mining_order2", array2));
        Dialogue quit = new Dialogue();
        quit.SetText(title, LocHelper.GetLocalizedDialoguePhone("end_call"));
        Dialogue dialogue2 = new Dialogue();
        dialogue2.SetText(displayName, LocHelper.GetLocalizedDialoguePhone("caulson_regular2"));
        dialogue.SetText(displayName, LocHelper.GetLocalizedDialoguePhone("caulson_regular1")).SetNextDialogue(dialogue2);
        dialogue2.AddChoice(DirectoryMaster.Item("$", true), "", "", order1, (Action)delegate
        {
            StorePhoneClient.GetMiningPhoneClient().stockType = 0;
            StorePhoneClient.GetMiningPhoneClient().UseService();
            PlayerStore.Instance.QueueFuturClient("courrierMining", 1);
            Core.QueueFuturEventSmart(ContactEventList.CreateCaulsonArrival(), 1);
            order1.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("caulson_ready")).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            });
        });
        if (StorePhoneClient.GetMiningPhoneClient().amountSpent >= 350)
        {
            dialogue2.AddChoice(DirectoryMaster.Item("$", true), "", "", order2, (Action)delegate
            {
                StorePhoneClient.GetMiningPhoneClient().stockType = 1;
                StorePhoneClient.GetMiningPhoneClient().UseService();
                PlayerStore.Instance.QueueFuturClient("courrierMining", 2);
                Core.QueueFuturEventSmart(ContactEventList.CreateCaulsonArrival(), 2);
                order2.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("caulson_ready")).SetEndAction((Action)delegate
                {
                    PhoneUIManager.Instance.OnPhoneDialogEnd(true);
                });
            });
        }
        dialogue2.AddChoice(DirectoryMaster.Item("$", true), "", "", quit, (Action)delegate
        {
            quit.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("goodbye")).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            });
        });
        __result = dialogue;
        return false;
    }
}

[HarmonyPatch(typeof(PhoneDialogList), "RevMerchantPhoneDialog")]
public class OdinPatch
{
    public static bool Prefix(ref Dialogue __result)
    {
        Dialogue dialogue = new Dialogue();
        string displayName = LocHelper.GetLocalizedName("name_odin");
        string title = "PLAYER";
        if (StoreReputation.IsPerkUnlocked("REV_BLACKLISTED"))
        {
            dialogue.SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_blacklisted1")).NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_blacklisted2")).NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_blacklisted3")).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
                StorePhoneClient.GetRevMerchantClient().phoneState = StorePhoneClient.PhoneState.NotResponding;
            });
            __result = dialogue;
            return false;
        }
        if (StoreStation.Instance.storeEventManager.IsEventActive("crackdown"))
        {
            dialogue.SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_crackdown1")).NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_crackdown2")).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            });
            __result = dialogue;
            return false;
        }
        Dialogue yes = new Dialogue();
        yes.SetText(title, LocHelper.GetLocalizedDialoguePhone("odin_yes"));
        Dialogue no = new Dialogue();
        no.SetText(title, LocHelper.GetLocalizedDialoguePhone("odin_no"));
        dialogue.SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_regular1")).AddChoice(DirectoryMaster.Item("$", true), "", "", yes, (Action)delegate
        {
            StorePhoneClient.GetRevMerchantClient().UseService();
            PlayerStore.Instance.QueueFuturClient("wanted4Normal", 2);
            Core.QueueFuturEventSmart(ContactEventList.CreateOdinArrival(), 2);
            var array1 = new Il2CppReferenceArray<Il2CppSystem.Object>(1);
            array1[0] = 2;
            yes.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_hold")).NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_dropby", array1)).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            });
        }).AddChoice(DirectoryMaster.Item("$", true), "", "", no, (Action)delegate
        {
            no.NextDialogue().SetText(displayName, LocHelper.GetLocalizedDialoguePhone("odin_goodbye")).SetEndAction((Action)delegate
            {
                PhoneUIManager.Instance.OnPhoneDialogEnd(true);
            });
        });
        __result = dialogue;
        return false;
    }
}

public class ContactEventList
{
    public static StoreEvent CreateRedImpArrival()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "redImpCourier";
        storeEvent.displayName = "Red Imp Courier";
        storeEvent.duration = 1;
        storeEvent.importance = -9999;
        storeEvent.eventType = StoreEvent.EventType.INNATE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        return storeEvent;
    }
    public static StoreEvent CreateCaulsonArrival()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "caulson";
        storeEvent.displayName = "Caulson Delivery";
        storeEvent.duration = 1;
        storeEvent.importance = -9999;
        storeEvent.eventType = StoreEvent.EventType.INNATE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        return storeEvent;
    }
    public static StoreEvent CreateOdinArrival()
    {
        StoreEvent storeEvent = new StoreEvent();
        storeEvent.identifier = "odin";
        storeEvent.displayName = "Odin Rendezvous";
        storeEvent.duration = 1;
        storeEvent.importance = -9999;
        storeEvent.eventType = StoreEvent.EventType.INNATE;
        storeEvent.eventArea = StoreEvent.EventArea.ALL;
        return storeEvent;
    }



}

[HarmonyPatch(typeof(CalendarEventElement),"HandleColor")]
public class colorPatch
{
    //Non-Func
    public static void Postfix(StoreEvent storeEvent, Image topImage)
    {
        Core.logger.Msg("1");
        if (contactColors.ContainsKey(storeEvent.identifier))
        {
            Core.logger.Msg("2");
            topImage.color = MetaHelper.HexToColor(contactColors[storeEvent.identifier],1f);
        }
    }

    public static Dictionary<string, string> contactColors = new Dictionary<string, string>
    {
        {"redImpCourier" ,"74313b"},
        {"caulson","373541" },
        {"odin", "5c6f46"}
    };
}


