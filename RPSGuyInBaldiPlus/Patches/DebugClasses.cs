using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HarmonyLib;
using BepInEx.Configuration;
using System.Collections.Generic;
using TMPro;
using HarmonyLib.Tools;
using RPSGuyInBaldiPlus;
using System.Linq;

// stupid missing file man
#if DEBUG
namespace RPSGuyInBaldiPlus
{
    [HarmonyPatch(typeof(ItemManager))]
    [HarmonyPatch("Update")]
    class DebugSlotPatch
    {
        static bool Prefix(ItemManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {
                __instance.AddItem(RPSGuyInBaldiPlus.HammerObject);
            }
            return true;
        }
    }
}
#endif
