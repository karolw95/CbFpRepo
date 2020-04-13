using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code
{
    public class Hoverable : MonoBehaviour
    {
        
        public TypesOfHover HoverType;
        private bool enableDrawing;
        bool childChecker = false;
        private Color[] colors = { Color.blue, Color.red, Color.yellow };
        private void Update()
        {
            GetMousePosition();
        }
        private void GetMousePosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 8;
            layerMask = ~layerMask;
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject == this.gameObject)
                    childChecker = true;
                foreach(Transform child in transform)
                {
                    if (child.gameObject == hit.collider.gameObject)
                        childChecker = true;
                }
            }
            if (childChecker)
            {
                enableDrawing = true;
                childChecker = false;
            }
            else
                enableDrawing = false;
            
        }
        private void OnGUI()
        {
            if (enableDrawing)
            {
                GUI.color = colors[(int)HoverType];
                GUI.DrawTexture(
                    new Rect(
                        Camera.main.WorldToScreenPoint(transform.position).x - 10,
                        Screen.height - Camera.main.WorldToScreenPoint(transform.position).y-44,
                        20,
                        34),
                    GameResources.HoverableIndicator);
            }
        }

    }
    public enum TypesOfHover { Collectable = 0, Attackable = 1, OtherInteraction = 2 };
}