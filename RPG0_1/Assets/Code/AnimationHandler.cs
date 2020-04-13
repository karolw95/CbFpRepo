using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class AnimationHandler : MonoBehaviour
    {
        public bool HitHappens=false;
        void hit()
        {
            HitHappens=true;
        }
    }
}