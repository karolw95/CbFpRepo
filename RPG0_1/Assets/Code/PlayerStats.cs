using System;   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class PlayerStats : CharacterStats
    {
        // Start is called before the first frame update
        void Start()
        {
            EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        }

        private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if(newItem!=null)
            {
                armor.AddModifier(newItem.armorModifier);
                damage.AddModifier(newItem.damageModifier);
            }
            if(oldItem != null)
            {
                armor.RemoveModifier(oldItem.armorModifier);
                damage.RemoveModifier(oldItem.damageModifier);
            }
        }
        public override void Die()
        {
            base.Die();
            PlayerManager.instance.KillPlayer();
        }
        // Update is called once per frame

    }
}