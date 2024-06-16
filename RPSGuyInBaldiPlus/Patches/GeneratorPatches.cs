using HarmonyLib;
using MTM101BaldAPI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace RPSGuyInBaldiPlus
{
    [HarmonyPatch(typeof(LevelGenerator))]
    [HarmonyPatch("StartGenerate")]
    class GeneratorPatch
    {
        static bool Prefix(LevelGenerator __instance)
        {
            __instance.ld.potentialNPCs.Add(RPSGuyInBaldiPlus.rpsGuy);
            __instance.ld.items = __instance.ld.items.AddToArray(RPSGuyInBaldiPlus.HammerObject);
            __instance.ld.shopItems = __instance.ld.shopItems.AddToArray(RPSGuyInBaldiPlus.HammerObject);
            return true;
        }
    }
}
