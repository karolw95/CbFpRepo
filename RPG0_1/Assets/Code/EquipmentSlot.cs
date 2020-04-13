using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    class EquipmentSlot : MonoBehaviour
    {
        [SerializeField]
        EquipmentSlotsNames id;
        public Image icon;
        public Button removeButton;

        Equipment item;

        public void Equip(Equipment newItem)
        {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
            removeButton.interactable = true;
        }
        public void ClearSlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            removeButton.interactable = false;

        }
        public void OnRemoveButton()
        {
            Debug.Log("Wyjebongo");
            bool isDefaultInSlot = false;
            foreach(Equipment defaultItem in EquipmentManager.instance.defaultItems)
            {
                if (defaultItem != null && defaultItem.EquipSlot == item.EquipSlot)
                {
                    EquipmentManager.instance.Equip(defaultItem);
                    isDefaultInSlot =  true;
                    break;
                }
            }
            if(!isDefaultInSlot)
                EquipmentManager.instance.Unequip((int)item.EquipSlot);

        }

    }
}
