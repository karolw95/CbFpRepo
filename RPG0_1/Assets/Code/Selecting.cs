using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class Selecting : MonoBehaviour
    {
        Player player;
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log(Camera.main.ScreenPointToRay(Input.mousePosition));
            }
        }
        private Vector3 GetMousePosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 8;
            layerMask = ~layerMask;
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
                return hit.point;
            return -Vector3.one;
        }
    }
}