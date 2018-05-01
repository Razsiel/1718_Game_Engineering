using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    public class BringToFront : MonoBehaviour
    {
        public void OnEnable()
        {
            transform.SetAsLastSibling();
        }
    }
}
