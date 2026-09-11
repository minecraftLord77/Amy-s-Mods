namespace AGP_Machines;

using System;
using UnityEngine;
using HarmonyLib;
using System.Runtime.CompilerServices;
using System.Transactions;
using System.Security.Cryptography;
using System.Collections.Generic;



public class CoreMod : IMod
{
    public static ModLog log;
    public static bool isEnabled = false;
    public void Init(ModManifest manifest)
    {
           
        log = new ModLog(manifest);
        log.Log("AGP_Pipes loaded");
            
    }
    public void RegisterItems(ModItemDirectory modItemDirectory)
    {
        modItemDirectory.Add("agp_pipe_straight1", ModdedItems.CreateAGPPipeStraight1());
    }

    public void OnEnable()
    {
        log.Log("Pipes enabled");
        isEnabled = true;

        ModHook.OnModItemDirectoryInit += RegisterItems; 
    }
        
    public void OnDisable()
    {
        log.Log("Pipes disabled");
        isEnabled = false;
        ModHook.OnModItemDirectoryInit -= RegisterItems;
    }

    

   
}

public class AGP_Machine : GameItem
{
    public AGP_Machine(string identifier, PixelWindow window = null) : base(identifier, window)
    {
        this.isEnabled = true;
    }

    public bool isEnabled;
    public Action<bool> OnEnable;
    public Action<GameItem, int> OnInput;
    public Action<GameItem, int> OnOutput;
    public Action OnNightEarly;
    public Action OnNightMiddle;
    public Action OnNightLate;

    public bool internalBool1;
    public bool internalBool2;
    public bool internalBool3;

    public int internalInt1;
    public int internalInt2;
    public int internalInt3;

    public float internalFloat1;
    public float internalFloat2;
    public float internalFloat3;

    public override void Validate() { }
    public override void Destroy() { }

}
