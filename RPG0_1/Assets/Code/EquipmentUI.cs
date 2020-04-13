using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class EquipmentUI : MonoBehaviour
    {
        public Transform items;
        public GameObject equipmentUI;
        EquipmentManager equipment;

        EquipmentSlot[] slots;
        void Start()
        {
            equipment = EquipmentManager.instance;
            equipment.onEquipmentChanged += UpdateUI;

            slots = items.GetComponentsInChildren<EquipmentSlot>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Inventory"))
            {
                equipmentUI.SetActive(!equipmentUI.activeSelf);
            }
        }
        void UpdateUI(Equipment newItem, Equipment oldItem)
        {
            for (int i = 0; i < equipment.currentEquipment.Length; i++)
            {
                if(newItem==null)
                {
                    if(equipment.currentEquipment[i]==null )
                    {
                        slots[i].ClearSlot();
                    }
                }
                else
                {
                    if((int)newItem.EquipSlot == i && !newItem.isDefaultItem)
                    {
                        slots[i].Equip(newItem);
                    }
                }
            }
        }
    }
}