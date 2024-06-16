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
using System.Collections;

namespace RPSGuyInBaldiPlus
{
    [BepInPlugin("alexbw145.bbplus.rpsguy", "RPS Guy in BB+", "1.2.0.0")]
    [BepInProcess("BALDI.exe")]
    public class RPSGuyInBaldiPlus : BaseUnityPlugin
    {
        public static BaseUnityPlugin plugin;
        void Awake()
        {
            Harmony harmony = new Harmony("alexbw145.bbplus.rpsguy");
            // Not needing that anymore, just updating garbage to somethning better...
            //rpsAssets = AssetBundle.LoadFromFile("BALDI_Data/StreamingAssets/Modded/alexbw145.bbplus.rpsguy/rpsguy.assets");
            new GameObject("RPSAssets", typeof(ResourceManager));
            plugin = this;

            harmony.PatchAll();
            /*ModExtraAssets.FindAllDBLoad<AudioClip>(ModExtraAssets.GetSubDirectory("RPSGuy/Audio"), "wav", this);
            ModExtraAssets.FindAllDBLoad<Sprite>(ModExtraAssets.GetSubDirectory("RPSGuy/Textures"), "png", this);
            ModExtraAssets.FindAllDBLoad<Texture2D>(ModExtraAssets.GetSubDirectory("RPSGuy/Textures"), "png", this);

            //This messy code upcomin...
            hammeritemEnum = EnumExtensions.CreateItemEnum("RPSHammer");
            HammerObject = ObjectCreatorHandlers.CreateObject("Itm_Hammer", "Desc_Hammer", ModExtraAssets.Get<Sprite>("Hammer_Small"), ModExtraAssets.Get<Sprite>("Hammer_Large"), hammeritemEnum, 100, 25);
            HammerObject.item = new GameObject().AddComponent<ITM_Hammer>();

            rpsGuy = new GameObject();
            rpsGuy.name = "RPSGuy";
            rpsGuy.transform.localPosition = new Vector3(0f, 5f, 0f);
            rpsGuy.transform.localScale = new Vector3(1f, 1f, 1f);
            rpsGuy.tag = "NPC";
            rpsGuy.layer = 17;
            rpsGuy.AddComponent<RPSGuy>();
            rpsGuy.AddComponent<Looker>();
            rpsGuy.AddComponent<Navigator>();
            rpsGuy.AddComponent<NavigatorDebugger>();
            rpsGuy.AddComponent<ActivityModifier>();
            rpsGuy.AddComponent<AudioManager>();
            //rpsGuy.AddComponent<AudioManager>();
            rpsGuy.AddComponent<CharacterController>();
            makeCCVal(rpsGuy.GetComponent<CharacterController>());
            rpsGuy.AddComponent<AudioSource>();
            rpsGuy.GetComponent<AudioSource>().spatialBlend = 1;
            rpsGuy.AddComponent<CapsuleCollider>();
            makeCCollideVal(rpsGuy.GetComponent<CapsuleCollider>());
            rpsGuy.AddComponent<Rigidbody>();
            makeRigidbodyVal(rpsGuy.GetComponent<Rigidbody>());

            GameObject spriteTransform = new GameObject();
            spriteTransform.name = "SpriteBase";
            spriteTransform.transform.parent = rpsGuy.transform;
            spriteTransform.transform.localPosition = new Vector3(0f, -2f, 0f);
            spriteTransform.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject spriteObj = new GameObject();
            spriteObj.name = "Sprite";
            spriteObj.transform.parent = spriteTransform.transform;
            spriteObj.transform.localPosition = new Vector3(0f, 0.8400002f, 0f);
            spriteObj.transform.localScale = new Vector3(1f, 1f, 1f);
            spriteObj.layer = 9;
            spriteObj.AddComponent<SpriteRenderer>();
            spriteObj.GetComponent<SpriteRenderer>().sprite = ModExtraAssets.Get<Sprite>("rockguy");
            spriteObj.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Assets/Material/SpriteStandard_Billboard.mat");

            //fixing stuff
            Material posterMat = new Material(Shader.Find("Shader Graphs/Standard"));
            posterMat.SetTexture("Texture", ModExtraAssets.Get<Texture2D>("pri_RPS"));
            posterMat.SetColor("Color", Color.white);
            posterMat.SetFloat("Offset", 0.015f);
            posterMat.SetTexture("LightMap", Resources.Load<Texture2D>("Assets/Texture2D/LightMap"));
            posterMat.SetFloat("LightingOffset", 0.01f);
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

            PosterObject poster = new PosterObject();
            poster = ObjectCreatorHandlers.CreatePosterObject(ModExtraAssets.Get<Texture2D>("pri_RPS-empty"), thatstupidMaterial.ToArray(), postTxtDataList.ToArray());

            // Doing the dumbass move ever
            //VAR SETTING
            rpsGuy.GetComponent<RPSGuy>().spriteBase = rpsGuy.GetComponentInChildren<Animator>().gameObject;
            rpsGuy.GetComponent<RPSGuy>().looker = rpsGuy.GetComponent<Looker>();
            rpsGuy.GetComponent<RPSGuy>().audMan = rpsGuy.GetComponent<AudioManager>();
            rpsGuy.GetComponent<RPSGuy>().spriteRenderThing = rpsGuy.GetComponentInChildren<SpriteRenderer>();
            rpsGuy.GetComponent<RPSGuy>().baseTrigger.AddToArray(rpsGuy.GetComponent<CapsuleCollider>());
            Traverse.Create(rpsGuy.GetComponent<RPSGuy>()).Field("poster").SetValue(poster);
            Traverse.Create(rpsGuy.GetComponent<RPSGuy>()).Field("character").SetValue(EnumExtensions.CreateCharacterEnum("RPSGuy"));
            Traverse.Create(rpsGuy.GetComponent<RPSGuy>()).Field("navigator").SetValue(rpsGuy.GetComponent<Navigator>());

            rpsGuy.GetComponent<AudioManager>().audioDevice = rpsGuy.GetComponent<AudioSource>();
            rpsGuy.GetComponent<AudioManager>().positional = true;

            rpsGuy.GetComponent<Navigator>().npc = rpsGuy.GetComponent<RPSGuy>();
            Traverse.Create(rpsGuy.GetComponent<Navigator>()).Field("cc").SetValue(rpsGuy.GetComponent<CharacterController>());
            Traverse.Create(rpsGuy.GetComponent<Navigator>()).Field("collider").SetValue(rpsGuy.GetComponent<Collider>());
            Traverse.Create(rpsGuy.GetComponent<Navigator>()).Field("avoidrooms").SetValue(true);
            Traverse.Create(rpsGuy.GetComponent<Navigator>()).Field("am").SetValue(rpsGuy.GetComponent<ActivityModifier>());

            Traverse.Create(rpsGuy.GetComponent<Looker>()).Field("npc").SetValue(rpsGuy.GetComponent<RPSGuy>());
            Traverse.Create(rpsGuy.GetComponent<Looker>()).Field("distance").SetValue(1000f);
            Traverse.Create(rpsGuy.GetComponent<Looker>()).Field("visibilityBuffer").SetValue(-0.5f);
            LayerMask layMaskVar = new LayerMask();
            layMaskVar |= (1 << 0);
            layMaskVar |= (1 << 15);
            layMaskVar |= (1 << 16);
            layMaskVar |= (1 << 21); //funny number
            Traverse.Create(rpsGuy.GetComponent<Looker>()).Field("layerMask").SetValue(layMaskVar);

            rpsGuy.GetComponent<RPSGuy>().alive = ModExtraAssets.Get<Sprite>("rockguy");
            rpsGuy.GetComponent<RPSGuy>().dead = ModExtraAssets.Get<Sprite>("rockguy_dead");

            //SOUNDS OUTSIDE OF PATCHING
            Color subtitleColor = new Color(0.7176471f, 0.6941177f, 0.6235294f);

            rpsGuy.GetComponent<RPSGuy>().audCall = ObjectCreatorHandlers.CreateSoundObject(ModExtraAssets.Get<AudioClip>("RPS_where"), "Vfx_RPS_where", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audLetsPlay = ObjectCreatorHandlers.CreateSoundObject(ModExtraAssets.Get<AudioClip>("RPS_play"), "Vfx_RPS_play", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audGo = ObjectCreatorHandlers.CreateSoundObject(ModExtraAssets.Get<AudioClip>("RPS_herewego"), "Vfx_RPS_herewego", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audCongrats = ObjectCreatorHandlers.CreateSoundObject(ModExtraAssets.Get<AudioClip>("RPS_lose"), "Vfx_RPS_lose", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audOops[0] = ObjectCreatorHandlers.CreateSoundObject(ModExtraAssets.Get<AudioClip>("RPS_ohteach1"), "Vfx_RPS_ohteach1", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audOops[1] = ObjectCreatorHandlers.CreateSoundObject(ModExtraAssets.Get<AudioClip>("RPS_ohteach2"), "Vfx_RPS_ohteach2", SoundType.Voice, subtitleColor);
            rpsGuy.GetComponent<RPSGuy>().audSad = ObjectCreatorHandlers.CreateSoundObject(ModExtraAssets.Get<AudioClip>("RPS_smashed"), "Nothing", SoundType.Effect, subtitleColor);

            //MINIGAME ITSELF
            rpsScreen = new GameObject();
            rpsScreen.name = "RPSScreen";
            rpsScreen.transform.localScale = new Vector3(1f, 1f, 1f);
            rpsScreen.AddComponent<RockPaperScissors>();

            GameObject canvasTextBase = new GameObject();
            canvasTextBase.name = "TextCanvas";
            canvasTextBase.transform.parent = rpsScreen.transform;
            canvasTextBase.AddComponent<RectTransform>();
            canvasTextBase.AddComponent<Canvas>();
            canvasTextBase.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            canvasTextBase.GetComponent<Canvas>().additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
            canvasTextBase.GetComponent<Canvas>().additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            canvasTextBase.GetComponent<Canvas>().additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            canvasTextBase.AddComponent<CanvasScaler>();
            canvasTextBase.GetComponent<CanvasScaler>().referencePixelsPerUnit = 16;
            canvasTextBase.AddComponent<GraphicRaycaster>();

            GameObject canvasText = new GameObject();
            canvasText.name = "Instructions";
            canvasText.transform.parent = canvasTextBase.transform;
            canvasText.AddComponent<RectTransform>();
            canvasText.GetComponent<RectTransform>().localPosition = Vector3.zero;
            canvasText.GetComponent<RectTransform>().localEulerAngles = new Vector3(460, 64);
            canvasText.GetComponent<RectTransform>().localScale = new Vector3(2.3f, 2.3f, 2.3f);
            canvasText.AddComponent<CanvasRenderer>();
            canvasText.GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            canvasText.AddComponent<TextMeshProUGUI>();
            canvasText.GetComponent<TextMeshProUGUI>().textStyle = TMP_Style.NormalStyle;
            canvasText.GetComponent<TextMeshProUGUI>().font = Resources.Load<TMP_FontAsset>("MonoBehaviour/COMIC_18_Pro.asset");
            canvasText.GetComponent<TextMeshProUGUI>().fontSize = 36;
            canvasText.GetComponent<TextMeshProUGUI>().enableAutoSizing = false;
            canvasText.GetComponent<TextMeshProUGUI>().color = Color.black;
            canvasText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            canvasText.GetComponent<TextMeshProUGUI>().enableWordWrapping = true;
            canvasText.GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Overflow;
            canvasText.GetComponent<TextMeshProUGUI>().horizontalMapping = TextureMappingOptions.Character;
            canvasText.GetComponent<TextMeshProUGUI>().verticalMapping = TextureMappingOptions.Character;

            rpsScreen.GetComponent<RockPaperScissors>().textCanvas = rpsScreen.GetComponentInChildren<Canvas>();
            rpsScreen.GetComponent<RockPaperScissors>().instructionsTmp = rpsScreen.GetComponentInChildren<TMP_Text>();

            rpsGuy.GetComponent<RPSGuy>().rpsPre = rpsScreen.GetComponent<RockPaperScissors>();

            //AND THEN WE GET IT IN THE GAME
            WeightedItemObject hammah = new WeightedItemObject();
            hammah.selection = HammerObject;
            hammah.weight = 75;
            itemList.Add(hammah);

            WeightedNPC theNPC = new WeightedNPC();
            theNPC.selection = rpsGuy.GetComponent<RPSGuy>();
            theNPC.weight = 100;
            NpcList.Add(theNPC);*/
        }

    }

    public class DependentRPS
    {
        public GameObject beanie;

        public PosterObject baldPoster;

        public GameObject jumpropeScreen;
    }

    [HarmonyPatch(typeof(NameManager),"Awake")]
    class PostLoadAssets
    {
        private static bool loaded = false;
        static void Prefix()
        {
            if (loaded) return;
            

            loaded = true;
        }
    }
}
