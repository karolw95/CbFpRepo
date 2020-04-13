using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [RequireComponent(typeof(CharacterStats))]
    public class EnemyInteraction : Interactable
    {
        GameObject player;
        CharacterStats myStats;
        private void Start()
        {
            player = PlayerManager.instance.player;
            myStats = GetComponent<CharacterStats>();
        }
        public override void Interact()
        {
            base.Interact();
            CharacterCombat playerCombat = player.GetComponent<CharacterCombat>();
            if(playerCombat != null)
            {
                playerCombat.Attack(myStats);
            }
        }
    }
}