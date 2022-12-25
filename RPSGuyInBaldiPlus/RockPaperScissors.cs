using Rewired;
using Rewired.Integration.UnityUI;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000011 RID: 17
public class RockPaperScissors : MonoBehaviour
{
    // Token: 0x060000A2 RID: 162 RVA: 0x0000674C File Offset: 0x0000494C
    private void Start()
    {
        this.ec = Singleton<BaseGameManager>.Instance.Ec;
        this.publicActive = true;
        base.transform.position = this.player.transform.position;
        this.initPos = base.transform.position;
        this.player.plm.am.moveMods.Add(this.moveMod);
        Singleton<CoreGameManager>.Instance.GetCamera(this.player.playerNumber).UpdateTargets(base.transform, 24);
        this.textCanvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(this.player.playerNumber).canvasCam;
        this.textCanvas.transform.SetParent(null);
        this.textCanvas.transform.position = Vector3.zero;
        /*if (!Singleton<PlayerFileManager>.Instance.authenticMode)
        {
            //this feature was never used..
            this.textScaler.scaleFactor = (float)Mathf.RoundToInt((float)Singleton<PlayerFileManager>.Instance.resolutionY / 360f);
        }*/
        this.instructionsTmp.text = "Press " + Singleton<InputManager>.Instance.GetInputButtonName("Item1") + " For Rock \nPress " + Singleton<InputManager>.Instance.GetInputButtonName("Item2") + " for Paper \nPress " + Singleton<InputManager>.Instance.GetInputButtonName("Item3") + " for Scissors";
    }

    // Token: 0x060000A3 RID: 163 RVA: 0x000068EC File Offset: 0x00004AEC
    private void Update()
    {
        base.transform.position = this.player.transform.position;
        base.transform.rotation = this.player.transform.rotation;
        if (!this.pickedChoice)
        {
            this.opponentChoice = (int)Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f)); //Pick a random type between 0 and 2
        }
        if (Singleton<InputManager>.Instance.GetDigitalInput("Item1", true) && !this.pickedChoice)
        {
            this.playerChoice = 0;
            this.ResultRPS();
        }
        else if (Singleton<InputManager>.Instance.GetDigitalInput("Item2", true) && !this.pickedChoice)
        {
            this.playerChoice = 1;
            this.ResultRPS();
        }
        else if (Singleton<InputManager>.Instance.GetDigitalInput("Item3", true) && !this.pickedChoice)
        {
            this.playerChoice = 2;
            this.ResultRPS();
        }
        if (this.pickedChoice && this.checkDelay <= 0)
        {
            this.checkRPS();
        }
        if ((this.player.transform.position - this.initPos).magnitude > 10f || this.player.hidden)
        {
            this.End(false);
        }
    }

    private void ResultRPS()
    {
        this.pickedChoice = true;
        base.StartCoroutine(this.CheckDelay(2f));
        this.instructionsTmp.text = this.playerChoiceText[this.playerChoice] + " VS " + this.opponentChoiceText[this.opponentChoice];
    }

    private void checkRPS()
    {
        this.pickedChoice = false;
        this.publicActive = false;
        //if its a tie
        if ((this.playerChoice == 0 & this.opponentChoice == 0) || (this.playerChoice == 1 & this.opponentChoice == 1) || (this.playerChoice == 2 & this.opponentChoice == 2))
        {
            this.instructionsTmp.text = "Press " + Singleton<InputManager>.Instance.GetInputButtonName("Item1") + " For Rock \nPress " + Singleton<InputManager>.Instance.GetInputButtonName("Item2") + " for Paper \nPress " + Singleton<InputManager>.Instance.GetInputButtonName("Item3") + " for Scissors";
        }
        //if the player wins
        if ((this.playerChoice == 0 & this.opponentChoice == 2) || (this.playerChoice == 2 & this.opponentChoice == 1) || (this.playerChoice == 1 & this.opponentChoice == 0))
        {
            this.End(true);
        }
        //if the player loses
        if ((this.playerChoice == 0 & this.opponentChoice == 1) || (this.playerChoice == 1 & this.opponentChoice == 2) || (this.playerChoice == 2 & this.opponentChoice == 0))
        {
            if (this.ec.GetBaldi() != null)
            {
                this.ec.MakeNoise(this.player.transform.position, 100);
                this.ec.GetBaldi().GetExtraAnger(5f);
            }
            this.End(false);
        }
    }

    public void death()
    {
        this.End(false);
        this.rps.fuckingDies();
    }
    public void End(bool success)
    {
        this.player.plm.am.moveMods.Remove(this.moveMod);
        Singleton<CoreGameManager>.Instance.GetCamera(this.player.playerNumber).UpdateTargets(null, 24);
        this.rps.EndRPS(success);
        if (success)
        {
            Singleton<CoreGameManager>.Instance.AddPoints(35, this.player.playerNumber, true);
        }
        Destroy(this.textCanvas.gameObject);
        Destroy(base.gameObject);
    }

    private IEnumerator CheckDelay(float val)
    {
        this.checkDelay = val;
        while (this.checkDelay > 0f)
        {
            this.checkDelay -= 1f * Time.deltaTime;
            yield return null;
        }
        this.checkDelay = 0f;
        yield break;
    }

    public bool publicActive = false;
    private bool pickedChoice = false;
    private int playerChoice;
    private int opponentChoice;

    private string[] opponentChoiceText = new string[]
    {
        "Rock",
        "Paper",
        "Scissors"
    };
    private string[] playerChoiceText = new string[]
    {
        "Rock",
        "Paper",
        "Scissors"
    };

    private float checkDelay;

    // Token: 0x04000149 RID: 329
    [SerializeField]
    private MovementModifier moveMod = new MovementModifier(default(Vector3), 0f);

    // Token: 0x0400014B RID: 331
    public Canvas textCanvas;

    // Token: 0x0400014C RID: 332
    [SerializeField]
    private CanvasScaler textScaler;

    // Token: 0x0400014D RID: 333
    public TMP_Text instructionsTmp;

    // Token: 0x0400014F RID: 335
    public RPSGuy rps;
    public EnvironmentController ec;

    // Token: 0x04000150 RID: 336
    public PlayerManager player;

    // Token: 0x04000152 RID: 338
    private Vector3 initPos;
}
