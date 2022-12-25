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
using HarmonyLib;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;
using RPSGuyInBaldiPlus;
using AlmostEngine;
using MTM101BaldAPI;
using MTM101BaldAPI.LangExtender;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;

namespace RPSGuyInBaldiPlus
{


    [BepInPlugin("mtm101.rulerp.bbplus.rpsguy", "RPS Guy in BB+", "1.0.1")]
    [BepInProcess("BALDI.exe")]
    public class RPSGuyInBaldiPlus : BaseUnityPlugin
    {
        public const string HammerImage = "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pQ5Mf5Jea2nhI6BvG7i4bmnyHnC5Aww+6ZIhOZKfplAoAO9n9E05oP8W6F51e2vu4/QByFBXSzfAwSEwWqTsNY93d7X39u+ZZn8/Gedy6hAbNpMAAA0YaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJYTVAgQ29yZSA0LjQuMC1FeGl2MiI+CiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiCiAgICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iCiAgICB4bWxuczpHSU1QPSJodHRwOi8vd3d3LmdpbXAub3JnL3htcC8iCiAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgeG1wTU06RG9jdW1lbnRJRD0iZ2ltcDpkb2NpZDpnaW1wOmM0Njc0MGM5LTNiODctNGEyMS05MjhlLTExYjg2ZGZlY2M4OSIKICAgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo5OGZiMDkzYi1jMDAyLTQzNzktYTU2Mi03NGQ1ZDBhYzUzZmYiCiAgIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDowODkyODc4Mi04OWMzLTQ0NmUtODJkNi0yYWY5ZGE5MTUwMDAiCiAgIGRjOkZvcm1hdD0iaW1hZ2UvcG5nIgogICBHSU1QOkFQST0iMi4wIgogICBHSU1QOlBsYXRmb3JtPSJXaW5kb3dzIgogICBHSU1QOlRpbWVTdGFtcD0iMTY3MTA2MTg4OTAxMzY5MSIKICAgR0lNUDpWZXJzaW9uPSIyLjEwLjMwIgogICB0aWZmOk9yaWVudGF0aW9uPSIxIgogICB4bXA6Q3JlYXRvclRvb2w9IkdJTVAgMi4xMCI+CiAgIDx4bXBNTTpIaXN0b3J5PgogICAgPHJkZjpTZXE+CiAgICAgPHJkZjpsaQogICAgICBzdEV2dDphY3Rpb249InNhdmVkIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmZhMjZmNWNmLWNkY2QtNDI5Zi1iZGFiLTA1NTcyMmVmZjQ5MCIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iR2ltcCAyLjEwIChXaW5kb3dzKSIKICAgICAgc3RFdnQ6d2hlbj0iMjAyMi0xMi0xNFQxNzo1MToyOSIvPgogICAgPC9yZGY6U2VxPgogICA8L3htcE1NOkhpc3Rvcnk+CiAgPC9yZGY6RGVzY3JpcHRpb24+CiA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz5EbQjfAAAABmJLR0QA/wD/AP+gvaeTAAAACXBIWXMAAA7CAAAOwgEVKEqAAAAAB3RJTUUH5gwOFzMdp+J0EQAABKVJREFUeNrtm0Fy4zYQRR+nvMo5kg2wbF2FPgJ0DJWOQRxheBX1LsAqOUd24SzQgCjPJGMrEkXKQZVtlm3ZhY/u378/WvDJV3frPxgOYdIMAsQxdp8DACeTAApIBhyIF3ACWctXFPL6QOlusfH5Ei/EMYITBBAPSnkG0KRIHyDrH3GMv20WAHEyXXzDl01q0rJRO/GSDIo4QbPW1yJeUPhLx/jL5gAQJxO+nSnYCccUy6ZdPXEBr4gLF7+nUMBICp5JR/2yGQDq5gX9LrRDH9gf93cj2HusL9edvEI6Z79a7s9DfCvrQwCoZXQN7fN362cIh6FUgCfUAZM4EB9+CpAmRbNuPwWkDxNgH4IiaFJiujx1NdavBBd8AWzTAIiTiTHac9mmGHvLmw2L8XtMkQEFB6eDbDcFGtM7EzUzclOAWXiHEiUNoPbsAnjYva5bDnc/1vPDRBMyoAk0xyptf/SaaTgIwrwSFL7oXvfd5lIgHvcdyAW9yT9vvgCZTdzUqpAVRZm+hmmTHBCP+w5X5A6lxv/rSe7H888LZ4AkbUS52TIoTqb3lrTTIUzzKCivXzcX/FQIfaSe746xw2E1Qaw+xJlg3rgSfM/aHwtgaqWTLGiKTL8P0yZT4Np16mXSOYGaPF5bKtytDVXXWKCU06SrbJLuBsD+qMYHM0rMyukg06dIgbqGXiZFkQTSS3OOupWkwv2dGCcEpGoJWJk2WOQUhr6EfRNIxQ9cBSEu4sXVjWsGHbUYp4lVyORFANgdZzK5lwsH4dGk+LLUPxK7HClSWRCniPUZPJAVFs3B4SATuXhLVE3w4KqwqB8vCOKKdaLZFIJVhdOD+GBx1GsU2KVIMWCcgA8ounhlWPxGJlRX2XqDarOp9QxLN02LAzC3yC4sNKxE5ifmgNYnjNo12ySZfZYU8UUzLFkaH3YpWcURXtBUHuMY0QzSD4uJpIcB0LpFO/k4mq2eIudbhycGoOZ9IcM2L9Ak81Kp8FAA5hK5aAJ7TDUVwnNHQJPIzkYnvBHiaBSZ7x8FDwdgd4zlUqUmhRkn7d6xHzh9vZ82eDgAtSyWKACMEDXFRohPHQEXosjuFIIvvYLm0j2Ku593sBoAaiowO3NBSrOUARfu0jCtBoCaCk0gcR65kxTfjGQ9KQCX5ok0sySOCmNEuX0qrA6Aog30wkcsNpotF25aGlcZAbujdlpnjluXqAiWCj48dwq0VMg2eONtBmlUG76+nYO0WgCqNqj1oF2nGAhyo1RYdQTs2v2iNpWI0zaaWVTif4uEF1a+ylRy9Q5As0CO4ALin5wDmkCqK9n7DapcclgqXD+UuXoALlZ9T4J5iTrGUh3609V8sAkALglx9qaMrEUlZuODK0DYTgTkWRT41ikQ88w8u2JKvdtSBtRr9uYe+fO0svTBhjnjh67ZtsUBzHXBDAiH2Wj6Yam8KQCacTITSOJresym2P37Qei2eP7nVHiTDnbfqASkF/bvGNR+YcOrXqjUzUs/FJGEsn8nD2ySA+pgtnh7h2oq5qlaxzD0w59PToKXUSA9zTGKo3bd6+7XpyyD33HBQf7WLB1AcG9k8//rfesb+wJPn/kGkpkAAAAASUVORK5CYII=";
        public static Sprite HammerSprite32x;
        public static Sprite HammerSprite128x;

        public static List<WeightedItemObject> itemList = new List<WeightedItemObject>();

        public static ItemObject HammerObject;

        // EVERY NPCS ARE PREFABS
        public static AssetBundle rpsAssets;
        public static GameObject rpsGuy;
        public static List<WeightedNPC> NpcList = new List<WeightedNPC>();
        public static List<PosterTextData> postTxtDataList = new List<PosterTextData>();
        public static List<Material> thatstupidMaterial = new List<Material>();
        public static PosterObject poster;
        public static GameObject rpsScreen;

        private static Texture TextureFromBase64(string base64)
        {
            byte[] array = Convert.FromBase64String(base64);
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            ImageConversion.LoadImage(texture2D, array);
            texture2D.filterMode = 0;
            return texture2D;
        }

        private static void Save()
        {
            string pathtosave = Path.Combine(Application.persistentDataPath, "Mods", "RPS Guy");
            if (!Directory.Exists(pathtosave))
            {
                Directory.CreateDirectory(pathtosave);
            }
            DirectoryInfo fo = new DirectoryInfo(pathtosave);
            FileInfo[] filefo = fo.GetFiles("data.dat");
            if (filefo.Length != 0)
            {
                filefo[0].Delete();
            }
            FileStream stream = File.Create(Path.Combine(pathtosave, "data.dat"));
            BinaryWriter writer = new BinaryWriter(stream);

            //writer.Write((byte)SlotsPriv);

            writer.Close();
        }

        void Awake()
        {
            Harmony harmony = new Harmony("mtm101.rulerp.bbplus.rpsguy");

            //THE ITEM
            Texture tex = TextureFromBase64(HammerImage);
            HammerSprite32x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
            HammerSprite128x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            HammerObject = CreateObject("Itm_Hammer", "Desc_Hammer", HammerSprite32x, HammerSprite128x, (Items)1492, 100, 25);
            HammerObject.item = new GameObject().AddComponent<ITM_Hammer>();

            DontDestroyOnLoad(HammerObject.item);

            //THE NPC LOADIN STARTIN
            rpsAssets = AssetBundle.LoadFromFile("BALDI_Data/StreamingAssets/Modded/mtm101.rulerp.bbplus.rpsguy/rpsguy.assets");
            rpsGuy = rpsAssets.LoadAsset<GameObject>("Assets/rpsguy/RPS Guy.prefab");
            //Instantiate(rpsGuy); //i think this spawn him into the Basically Games Splash Screen..

            rpsGuy.AddComponent<RPSGuy>();
            rpsGuy.AddComponent<Looker>();
            rpsGuy.AddComponent<Navigator>();
            rpsGuy.AddComponent<NavigatorDebugger>();
            rpsGuy.AddComponent<ActivityModifier>();
            rpsGuy.AddComponent<AudioManager>();
            //rpsGuy.AddComponent<AudioManager>();

            //fixing stuff
            Material posterMat = new Material(Shader.Find("Shader Graphs/Standard"));
            posterMat = rpsAssets.LoadAsset<Material>("Assets/rpsguy/pri_RPS.mat");
            thatstupidMaterial.Add(posterMat);

            PosterTextData postTxt = new PosterTextData();
            postTxt.textKey = "PST_PRI_RPS1";
            postTxt.position = new IntVector2(48, 48);
            postTxt.size = new IntVector2(160, 32);
            postTxt.font = Resources.Load<TMP_FontAsset>("MonoBehaviour/COMIC_18_Pro.asset");
            postTxt.fontSize = 14;
            postTxt.color = Color.black;
            postTxt.style = FontStyles.Bold;
            postTxt.alignment = TextAlignmentOptions.Center;
            postTxtDataList.Add(postTxt);
            PosterTextData postinfoTxt = new PosterTextData();
            postinfoTxt.textKey = "PST_PRI_RPS2";
            postinfoTxt.position = new IntVector2(144, 96);
            postinfoTxt.size = new IntVector2(96, 128);
            postinfoTxt.font = Resources.Load<TMP_FontAsset>("MonoBehaviour/COMIC_12_Pro.asset");
            postinfoTxt.fontSize = 12;
            postinfoTxt.color = Color.black;
            postinfoTxt.style = FontStyles.Normal;
            postinfoTxt.alignment = TextAlignmentOptions.Center;
            postTxtDataList.Add(postinfoTxt);


            poster = CreatePosterObject(rpsAssets.LoadAsset<Texture2D>("Assets/rpsguy/pri_RPS-empty.png"), thatstupidMaterial.ToArray(), postTxtDataList.ToArray());

            //VAR SETTING
            rpsGuy.GetComponent<RPSGuy>().spriteBase = rpsGuy.GetComponentInChildren<Animator>().gameObject;
            rpsGuy.GetComponent<RPSGuy>().looker = rpsGuy.GetComponent<Looker>();
            rpsGuy.GetComponent<RPSGuy>().audMan = rpsGuy.GetComponent<AudioManager>();
            rpsGuy.GetComponent<RPSGuy>().spriteRenderThing = rpsGuy.GetComponentInChildren<SpriteRenderer>();
            rpsGuy.GetComponent<RPSGuy>().baseTrigger.AddToArray(rpsGuy.GetComponent<CapsuleCollider>());
            rpsGuy.GetComponent<AudioManager>().audioDevice = rpsGuy.GetComponent<AudioSource>();
            rpsGuy.GetComponent<AudioManager>().positional = true;
            rpsGuy.GetComponent<Navigator>().npc = rpsGuy.GetComponent<RPSGuy>();     

            rpsGuy.GetComponent<RPSGuy>().alive = rpsAssets.LoadAsset<Sprite>("Assets/rpsguy/rockguy.png");
            rpsGuy.GetComponent<RPSGuy>().dead = rpsAssets.LoadAsset<Sprite>("Assets/rpsguy/rockguy_dead.png");

            //SOUNDS OUTSIDE OF PATCHING
            Color subtitleColor = new Color(0.7176471f, 0.6941177f, 0.6235294f);

            rpsGuy.GetComponent<RPSGuy>().audCall = ObjectCreatorHandlers.CreateSoundObject(rpsAssets.LoadAsset<AudioClip>("Assets/rpsguy/RPS_where.wav"), "Vfx_RPS_where", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audLetsPlay = ObjectCreatorHandlers.CreateSoundObject(rpsAssets.LoadAsset<AudioClip>("Assets/rpsguy/RPS_play.wav"), "Vfx_RPS_play", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audGo = ObjectCreatorHandlers.CreateSoundObject(rpsAssets.LoadAsset<AudioClip>("Assets/rpsguy/RPS_herewego.wav"), "Vfx_RPS_herewego", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audCongrats = ObjectCreatorHandlers.CreateSoundObject(rpsAssets.LoadAsset<AudioClip>("Assets/rpsguy/RPS_lose.wav"), "Vfx_RPS_lose", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audOops = ObjectCreatorHandlers.CreateSoundObject(rpsAssets.LoadAsset<AudioClip>("Assets/rpsguy/RPS_ohteach1.wav"), "Vfx_RPS_ohteach1", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audSad = ObjectCreatorHandlers.CreateSoundObject(rpsAssets.LoadAsset<AudioClip>("Assets/rpsguy/RPS_smashed.wav"), "Nothing", SoundType.Effect, subtitleColor);

            //MINIGAME ITSELF
            rpsScreen = rpsAssets.LoadAsset<GameObject>("Assets/rpsguy/RPSScreen.prefab");
            rpsScreen.AddComponent<RockPaperScissors>();
            rpsScreen.GetComponent<RockPaperScissors>().textCanvas = rpsScreen.GetComponentInChildren<Canvas>();
            rpsScreen.GetComponent<RockPaperScissors>().instructionsTmp = rpsScreen.GetComponentInChildren<TMP_Text>();
            rpsScreen.GetComponent<RockPaperScissors>().instructionsTmp.font = Resources.Load<TMP_FontAsset>("MonoBehaviour/COMIC_18_Pro.asset");

            rpsGuy.GetComponent<RPSGuy>().rpsPre = rpsScreen.GetComponent<RockPaperScissors>();

            DontDestroyOnLoad(rpsAssets);

            //AND THEN WE GET IT IN THE GAME
            WeightedItemObject hammah = new WeightedItemObject();
            hammah.selection = HammerObject;
            hammah.weight = 150;
            itemList.Add(hammah);

            WeightedNPC theNPC = new WeightedNPC();
            theNPC.selection = rpsGuy.GetComponent<RPSGuy>();
            theNPC.weight = 100;
            NpcList.Add(theNPC);

            harmony.PatchAll();
        }

        public static ItemObject CreateObject(string localizedtext, string desckey, Sprite smallicon, Sprite largeicon, Items type, int price, int cost)
        {
            ItemObject obj = ScriptableObject.CreateInstance<ItemObject>();
            obj.nameKey = localizedtext;
            obj.itemSpriteSmall = smallicon;
            obj.itemSpriteLarge = largeicon;
            obj.itemType = type;
            obj.descKey = desckey;
            obj.cost = cost;
            obj.price = price;

            return obj;
        }

        //GRAHHH, CURRENT RELEASE VERSION OF THE API DOESN'T HAVE THIS!!
        public static PosterObject CreatePosterObject(Texture2D postertex, Material[] materials, PosterTextData[] text)
        {
            PosterObject obj = ScriptableObject.CreateInstance<PosterObject>();
            obj.baseTexture = postertex;
            obj.material = materials;
            obj.textData = text;

            return obj;
        }
    }
}
