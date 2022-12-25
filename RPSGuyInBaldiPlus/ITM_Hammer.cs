using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using RPSGuyInBaldiPlus;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.ComponentModel;

namespace RPSGuyInBaldiPlus
{
    public class ITM_Hammer : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (rps != null)
            {
                if (rps.publicActive)
                {
                    rps.death();
                    Destroy(base.gameObject);
                    return true;
                }
            }
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, pm.pc.ClickLayers))
            {
                if (this.hit.transform.tag == "Window")
                {
                    this.hit.transform.GetComponent<Window>().Break(true);
                    Destroy(base.gameObject);
                    return true;
                }
            }
            Destroy(base.gameObject);
            return false;
        }

        private RaycastHit hit;

        public RockPaperScissors rps;
    }
}
