using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Linq;
using MTM101BaldAPI;
using RPSGuyInBaldiPlus;

public class RPSGuy : NPC
{
    //two playtimes
    private void Start()
    {
        this.navigator.maxSpeed = this.normSpeed;
        this.navigator.SetSpeed(this.normSpeed);
    }

    // Token: 0x060000AB RID: 171 RVA: 0x00006BD6 File Offset: 0x00004DD6
    private void Update()
    {
        if (!this.controlOverride && !this.navigator.HasDestination && !this.playing && !this.isDead)
        {
            this.WanderRandom();
            this.CalloutChance();
        }
        if (this.cooldown <= 0f && this.isDead)
        {
            this.spriteRenderThing.sprite = this.alive;
            this.initialCooldown = 15f;
            this.navigator.maxSpeed = this.normSpeed;
            this.navigator.SetSpeed(this.normSpeed);
            this.isDead = false;
        }

        if (Resources.FindObjectsOfTypeAll<NPC>().ToList().Find(x => x.Character == Character.Principal) != null && !this.changedMat)
        {
            base.GetComponentInChildren<SpriteRenderer>().material = Resources.FindObjectsOfTypeAll<NPC>().ToList().Find(x => x.Character == Character.Principal).GetComponentInChildren<SpriteRenderer>().material;
            base.GetComponentInChildren<SpriteRenderer>().gameObject.AddComponent<Billboard>();
            this.changedMat = true;
        }
    }
    private bool changedMat = false;

    // Token: 0x060000AC RID: 172 RVA: 0x00006C01 File Offset: 0x00004E01
    public override void DestinationEmpty()
    {
        base.DestinationEmpty();
        if (!this.controlOverride && !this.returningFromDetour)
        {
            this.WanderRandom();
            this.CalloutChance();
        }
    }

    // Token: 0x060000AD RID: 173 RVA: 0x00006C28 File Offset: 0x00004E28
    public override void PlayerInSight(PlayerManager player)
    {
        if (this.cooldown <= 0f && !this.playing && !player.Tagged && !this.isDead)
        {
            if (!this.controlOverride)
            {
                this.TargetPlayer(player.transform.position);
                this.navigator.maxSpeed = this.runSpeed;
                this.navigator.SetSpeed(this.runSpeed);
            }
            this.aggroed = true;
            return;
        }
        if (this.aggroed && player.Tagged && !this.isDead)
        {
            Directions.ReverseList(this.navigator.currentDirs);
            this.WanderRandom();
            this.navigator.maxSpeed = this.normSpeed;
            this.navigator.SetSpeed(this.normSpeed);
            this.navigator.ClearDestination();
            this.aggroed = false;
        }
    }

    // Token: 0x060000AE RID: 174 RVA: 0x00006CEF File Offset: 0x00004EEF
    public override void PlayerSighted(PlayerManager player)
    {
        if (this.cooldown <= 0f && !this.audMan.audioDevice.isPlaying && !player.Tagged && !this.isDead && !this.playing)
        {
            this.audMan.PlaySingle(this.audLetsPlay);
        }
    }

    // Token: 0x060000AF RID: 175 RVA: 0x00006D2C File Offset: 0x00004F2C
    public override void PlayerLost(PlayerManager player)
    {
        if (!this.playing && this.cooldown <= 0f && !player.Tagged)
        {
            if (!this.controlOverride)
            {
                Directions.ReverseList(this.navigator.currentDirs);
                this.WanderRandom();
                this.navigator.maxSpeed = this.normSpeed;
                this.navigator.SetSpeed(this.normSpeed);
                this.navigator.ClearDestination();
            }
            this.aggroed = false;
        }
    }

    // Token: 0x060000B2 RID: 178 RVA: 0x00006DD4 File Offset: 0x00004FD4
    public void EndRPS(bool won)
    {
        base.StartCoroutine(this.Cooldown(this.initialCooldown));
        this.playing = false;
        this.navigator.maxSpeed = this.normSpeed;
        this.navigator.SetSpeed(this.normSpeed);
        ObjectFinders.GetFirstInstance((Items)1492).item.GetComponent<ITM_Hammer>().rps = null;
        if (won)
        {
            this.audMan.PlaySingle(this.audCongrats);
        }
        else
        {
            this.audMan.PlaySingle(this.audOops);
        }
        if (!this.controlOverride)
        {
            this.WanderRandom();
        }
    }

    // Token: 0x060000B3 RID: 179 RVA: 0x00006E74 File Offset: 0x00005074
    private void CalloutChance()
    {
        if (UnityEngine.Random.value <= this.calloutChance)
        {
            this.RandomCallout();
        }
    }

    // Token: 0x060000B4 RID: 180 RVA: 0x00006E8C File Offset: 0x0000508C
    private void RandomCallout()
    {
        if (!this.audMan.audioDevice.isPlaying && !this.playing)
        {
            this.audMan.PlaySingle(this.audCall);
        }
    }

    // Token: 0x060000B5 RID: 181 RVA: 0x00006ED0 File Offset: 0x000050D0
    private void OnTriggerEnter(Collider other)
    {
        if (!this.playing && this.cooldown <= 0f && other.tag == "Player" && !this.isDead)
        {
            PlayerManager component = other.GetComponent<PlayerManager>();
            if (!component.Tagged)
            {
                this.curRPS = GameObject.Instantiate<RockPaperScissors>(this.rpsPre);
                this.curRPS.player = component;
                this.curRPS.rps = this;
                ObjectFinders.GetFirstInstance((Items)1492).item.GetComponent<ITM_Hammer>().rps = this.curRPS;
                this.playing = true;
                this.navigator.maxSpeed = 0f;
                float radius = this.navigator.Cc.radius;
                this.navigator.Cc.radius = this.navigator.Radius;
                this.navigator.Cc.Move((base.transform.position - other.transform.position).normalized * 10f);
                this.navigator.Cc.radius = radius;
                this.audMan.PlaySingle(this.audGo);
                this.aggroed = false;
            }
        }
    }

    public void fuckingDies()
    {
        this.EndRPS(false);
        this.initialCooldown = 30f;
        this.audMan.FlushQueue(true);
        this.audMan.PlaySingle(this.audSad);
        this.spriteRenderThing.sprite = this.dead;
        this.navigator.maxSpeed = 0f;
        this.isDead = true;
    }

    // Token: 0x060000B6 RID: 182 RVA: 0x00006FF2 File Offset: 0x000051F2
    private IEnumerator Cooldown(float val)
    {
        this.cooldown = val;
        while (this.cooldown > 0f)
        {
            this.cooldown -= 1f * Time.deltaTime;
            yield return null;
        }
        this.cooldown = 0f;
        yield break;
    }

    // Token: 0x0400015F RID: 351

    public Sprite alive;

    public Sprite dead;

    public SpriteRenderer spriteRenderThing;

    private RockPaperScissors curRPS;
    public RockPaperScissors rpsPre;
    public Baldi baldimore;

    // Token: 0x04000160 RID: 352

    public AudioManager audMan;

    // Token: 0x04000162 RID: 354

    public SoundObject audCall = new SoundObject();

    // Token: 0x04000163 RID: 355

    public SoundObject audLetsPlay;

    // Token: 0x04000164 RID: 356

    public SoundObject audGo;

    // Token: 0x04000165 RID: 357

    public SoundObject audCongrats;

    // Token: 0x04000166 RID: 358

    public SoundObject audOops;

    // Token: 0x04000167 RID: 359

    public SoundObject audSad;

    // Token: 0x0400016A RID: 362
    [SerializeField]
    private float normSpeed = 12f;

    // Token: 0x0400016B RID: 363
    [SerializeField]
    private float runSpeed = 20f;

    // Token: 0x0400016C RID: 364
    [SerializeField]
    private float initialCooldown = 15f;

    // Token: 0x0400016D RID: 365
    [SerializeField]
    private float calloutChance = 0.05f;

    private bool isDead = false;

    // Token: 0x0400016E RID: 366
    private float cooldown;

    // Token: 0x0400016F RID: 367
    private float sadTime;

    // Token: 0x04000170 RID: 368
    private bool playing = false;
}