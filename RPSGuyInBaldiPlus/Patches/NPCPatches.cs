using HarmonyLib;
using Mono.CSharp;
using MTM101BaldAPI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;

namespace RPSGuyInBaldiPlus.Patches
{
    [HarmonyPatch((typeof(NPC)))]
    [HarmonyPatchAll]
    class NPCPatch
    {
        // thanks to the real MissingTextureMan for giving me help on how to set private vars..
        //  I ain't kidding, he is a genius! 
        static bool Prefix(RPSGuy __instance, ref PosterObject ___poster, ref Character ___character, ref Navigator ___navigator)
        {
            if (__instance.name.StartsWith("RPS Guy") && __instance.tag == "NPC")
            {
                ___poster = RPSGuyInBaldiPlus.poster;
                ___character = (Character)13;
                ___navigator = __instance.GetComponent<Navigator>();
            }

            return __instance;
        }

    }
    //also mtm101 told me that patching was a bad idea because it affects ALL NPCS

    //this was a bad idea..
    [HarmonyPatch((typeof(Navigator)))]
    [HarmonyPatchAll]
    class NavigatorPatch
    {
        static bool Prefix(Navigator __instance, ref CharacterController ___cc, ref Collider ___collider, ref bool ___avoidRooms, ref ActivityModifier ___am)
        {
            if (__instance.name.StartsWith("RPS Guy") && __instance.tag == "NPC")
            {
                ___cc = __instance.GetComponent<CharacterController>();
                ___collider = __instance.GetComponent<Collider>();
                ___avoidRooms = true;
                ___am = __instance.GetComponent<ActivityModifier>();
            }

            return __instance;
        }
    }
    [HarmonyPatch((typeof(Looker)))]
    [HarmonyPatchAll]
    class LookerPatch
    {
        static bool Prefix(Looker __instance, ref RPSGuy ___npc, ref LayerMask ___layerMask, ref float ___distance, ref float ___visibilityBuffer)
        {
            if (__instance.name.StartsWith("RPS Guy") && __instance.tag == "NPC")
            {
                //[0, 12, 13, 18] Layermask Values, idk how to assign them..
                ___npc = __instance.GetComponent<RPSGuy>();
                ___distance = 1000f; //he doesn't have poor eyesight, unlike Playtime.
                ___visibilityBuffer = -0.5f;

                ___layerMask |= (1 << 0);
                ___layerMask |= (1 << 15);
                ___layerMask |= (1 << 16);
                ___layerMask |= (1 << 21); //funny number
            }

            return __instance;
        }
    }
    /*[HarmonyPatch((typeof(AudioManager)))]
    [HarmonyPatchAll]
    class AudManagerPatch
    {
        static bool Prefix(AudioManager __instance, ref SoundObject[] ___soundOnStart, ref bool ___loopOnStart)
        {
            if (__instance.transform.name.StartsWith("RPS Guy") && __instance.transform.tag == "NPC")
            {
                Color subtitleColor = new Color(183, 177, 159);
                ___soundOnStart.AddToArray(ObjectCreatorHandlers.CreateSoundObject(RPSGuyInBaldiPlus.rpsAssets.LoadAsset<AudioClip>("Assets/rpsguy/mus_rock.wav"), "Mfx_mus_rock", SoundType.Music, subtitleColor, 14.65179f));
                ___loopOnStart = true;
                return true;
            }
            return false;
        }
    }*/
}