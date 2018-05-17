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
        public static float Mod(float a, float b)
        {
            return a - b * Mathf.Floor(a / b);
        }

        public static int Mod(int a, int b)
        {
            return a - b * Mathf.FloorToInt((float)a / b);
        }

        /// <summary>
        /// Rounds the given decimal down if fraction is midpoint (1.5 becomes 1)
        /// NOTE: This function only works on decimals with one fraction (2.55 won't work)
        /// </summary>
        /// <param name="a">Our decimal value to be rounded</param>
        /// <returns>Our rounded decimal value</returns>
        public static int RoundDown(decimal a)
        {
            int unrounded = (int)Math.Floor(a);
            decimal fraction = a - unrounded;
            if(fraction < 0.5m) return unrounded;
            else return unrounded + 1;
        }
    }
}
