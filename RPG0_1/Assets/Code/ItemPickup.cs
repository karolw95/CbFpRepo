using System;
using UnityEngine;

namespace Assets.Code
{
    class ItemPickup : Interactable
    {
        public Item item;
        public override void Interact()
        {
            base.Interact();
            PickUp();
        }

        private void PickUp()
        {
            Debug.Log("Pick up " + item.name);
            if(Inventory.instance.Add(item))
                Destroy(gameObject);
        }
    }
}
