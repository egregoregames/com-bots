using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Battles.BattleUI
{
    public class SoftwareHUDButtons : MonoBehaviour
    {
        public List<SoftwareButton> softwareButtons;

        private void Awake()
        {
            softwareButtons = GetComponentsInChildren<SoftwareButton>().ToList();
        }
        
        public Task<Software> AssignSoftwareToButtons(BotSo bot)
        {
            var tcs = new TaskCompletionSource<Software>();

            // Pass a callback that completes the Task when a selection is made
            void OnSoftwareSelected(Software software)
            {
                tcs.TrySetResult(software);
            }
            for (int i = 0; i < bot.Attacks.Count; i++)
            {
                softwareButtons[i].AssignSoftwareToButton(bot.Attacks[i], OnSoftwareSelected);
            }

            return tcs.Task; // Return a Task that completes when software is selected
        }

        
    }
}
