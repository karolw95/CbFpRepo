using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code
{
    public static class GameResources
    {
        public static Texture Target = (Texture)Resources.Load("PointerHQ");
        public static Texture HoverableIndicator = (Texture)Resources.Load("target");
    }
}
