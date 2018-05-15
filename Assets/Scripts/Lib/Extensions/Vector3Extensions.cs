using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Lib.Extensions {
    public static class Vector3Extensions {
        public static Vector3 SetXY(this Vector3 vector, Vector3 newVector) {
            vector.Set(newVector.x, newVector.y, vector.z);
            return vector;
        }

        public static Vector3 SetXZ(this Vector3 vector, Vector3 newVector) {
            vector.Set(newVector.x, vector.y, newVector.z);
            return vector;
        }

        public static Vector3 SetYZ(this Vector3 vector, Vector3 newVector) {
            vector.Set(vector.x, newVector.y, newVector.z);
            return vector;
        }

        public static void SetXZ(this Vector3 vector, Vector2Int vector2) {
            vector.Set(vector2.x, vector.y, vector2.y);
        }
    }
}