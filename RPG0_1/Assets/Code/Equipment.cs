using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
    public class Equipment : Item    {

        public float armorModifier;
        public float damageModifier;
        [Header("Wearable equipment")]
        public EquipmentSlotsNames EquipSlot;
        public EquipmentMeshRegions[] coveredMeshRegions;
        public SkinnedMeshRenderer SkinnedMesh;
        [Header("Wpns/Shields equipment")]
        public bool InHand = false;
        public MeshRenderer NormalMesh;


        public override void Use()
        {
            base.Use();
            EquipmentManager.instance.Equip(this);
            RemoveFromInventory();
        }
    }

    public enum EquipmentSlotsNames { Head,Chest,Legs,Feet, LeftHand_Shield, RightHand_Weapon }
    public enum EquipmentMeshRegions { Body,  Arms, Legs, Feets }

}
