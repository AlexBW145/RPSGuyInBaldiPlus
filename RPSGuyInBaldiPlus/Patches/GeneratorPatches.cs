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
        private static WeightedItemObject nosquee;
        private static bool gotSquee;
        static bool Prefix(LevelGenerator __instance)
        {
            if (gotSquee)
            {
                nosquee = __instance.ld.items[5];
                gotSquee = true;
            }
            __instance.ld.potentialNPCs.AddRange(RPSGuyInBaldiPlus.NpcList);
            //IS NOT POSSIBLE WITH ARRAYS, THEY NEED TO CHANGE IT INTO LISTS.
            if (ObjectFinders.GetFirstInstance((Character)13) != null)
            {
                __instance.ld.items[5] = RPSGuyInBaldiPlus.itemList.ToArray()[0];
                __instance.ld.shopItems[5] = RPSGuyInBaldiPlus.itemList.ToArray()[0];
            }
            else
            {
                __instance.ld.items[5] = nosquee;
                __instance.ld.shopItems[5] = nosquee;
            }
            return true;
        }

        private void Awake()
        {
            RPSGuyInBaldiPlus.rpsGuy.GetComponent<Navigator>().ec = Singleton<BaseGameManager>.Instance.Ec;
            RPSGuyInBaldiPlus.rpsGuy.GetComponent<RPSGuy>().ec = Singleton<BaseGameManager>.Instance.Ec;
            RPSGuyInBaldiPlus.rpsGuy.GetComponent<Navigator>()._startTile = Singleton<EnvironmentController>.Instance.npcSpawnTile[0];
            RPSGuyInBaldiPlus.rpsGuy.GetComponent<Navigator>()._targetTile = Singleton<EnvironmentController>.Instance.npcSpawnTile[0];
        }

        // FOR STUPIDS
        // idk how to make npcs spawn in out of floor 1..
    }
}