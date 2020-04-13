using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class Inventory : MonoBehaviour
    {
        #region Singleton
        public static Inventory instance;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("Instance of Inventory");
                return;
            }
            instance = this;
        }
        #endregion
        public List<Item> items = new List<Item>();
        public int space = 20;
        public delegate void OnItemChanged();
        public OnItemChanged onItemChangedCallback;
        public Sprite ItemIcon;
        public Color ItemIconColor;

        public bool Add(Item item)
        {
            if(!item.isDefaultItem)
            {
                if(items.Count >= space)
                {
                    Debug.Log("Not enough space");
                    return false ;
                }
                items.Add(item);
                if(onItemChangedCallback!= null)
                    onItemChangedCallback.Invoke();
            }
            return true;
        }
        public void Remove(Item item)
        {
            items.Remove(item);
            if(onItemChangedCallback!= null)
                onItemChangedCallback.Invoke();

        }
    }
}