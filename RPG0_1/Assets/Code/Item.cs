using System;
using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        new public string name = "New item";
        public Sprite icon = null;
        public bool isDefaultItem = false;
        public MeshRenderer ItemRenderer;

        public virtual void Use()
        {
            Debug.Log("Używando " + name);
        }
        public void RemoveFromInventory()
        {
            Inventory.instance.Remove(this);
        }
    }
}
