namespace AGP_Pipe;

using AGP_Machine;

using System;
using UnityEngine;
using HarmonyLib;

public class AGP_Pipe
{
    public static AGP_Machine CreateAGPPipeStraight1()
    {
        AGP_Machine item = (AGP_Machine)ItemDirectory.CreateEmptyItem(null);
        
        return item;
    }
    

}