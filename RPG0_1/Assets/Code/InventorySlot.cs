using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    class InventorySlot : MonoBehaviour
    {
        public Image icon;
        public Button removeButton;

        Item item;

        public void AddItem(Item newItem)
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
            GameObject droppedItem = new GameObject(item.name);
            droppedItem.AddComponent<BoxCollider>();
            ItemPickup itemPickup = droppedItem.AddComponent<ItemPickup>();
            itemPickup.item = item;
            droppedItem.transform.position = PlayerManager.instance.player.transform.position + new Vector3(0, .5f) ;
            GameObject GFX = new GameObject();
            GFX.transform.SetParent(droppedItem.transform);
            MeshFilter meshFilter = GFX.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = item.ItemRenderer.GetComponent<MeshFilter>().sharedMesh;
            MeshRenderer meshRenderer =GFX.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterials = item.ItemRenderer.sharedMaterials;
            GFX.transform.localPosition = Vector3.zero;
            GFX.transform.localScale *= 100;
            droppedItem.layer =11;
            itemPickup.interactionTransform = itemPickup.transform;

            GameObject MiniMapIcon = new GameObject();
            MiniMapIcon.transform.SetParent(droppedItem.transform);
            MiniMapIcon.transform.localPosition = Vector3.zero;
            SpriteRenderer MiniMapSprite = MiniMapIcon.AddComponent<SpriteRenderer>();
            MiniMapSprite.sprite = Inventory.instance.ItemIcon;
            MiniMapIcon.layer = 13;
            MiniMapIcon.transform.localScale *= 2;
            MiniMapIcon.transform.eulerAngles = new Vector3(90, 0, 0);
            Inventory.instance.Remove(item);
        }
        public void UseItem()
        {
            if(item != null)
            {
                item.Use();
            }
        }
    }
}
