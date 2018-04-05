using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Lib.Extensions
{
    public static class MathHelper
    {
        public static float Mod(float a, float b) {
            return a - b * Mathf.Floor(a / b);
        }

        public static int Mod(int a, int b) {
            return a - b * Mathf.FloorToInt((float)a / b);
        }
    }
}
