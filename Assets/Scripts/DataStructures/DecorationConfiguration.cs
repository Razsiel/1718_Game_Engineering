using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Tiles;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class DecorationConfiguration {
        [SerializeField] public TileDecorationData DecorationData;
        [SerializeField] public Vector3 RelativePosition;
        [SerializeField] public int Scale = 1;
        [SerializeField] public int Rotation;

        public GameObject GenerateGameObject(GameObject parent, bool hidden = false) {
            return GenerateGameObject(parent.transform, hidden);
        }

        private GameObject GenerateGameObject(Transform parent, bool hidden = false) {
            var decoration = DecorationData?.GenerateGameObject(parent, hidden);
            if (decoration != null) {
                var transform = decoration.transform;
                transform.position = RelativePosition;
                transform.localScale = Vector3.one * Scale;
                transform.eulerAngles = Vector3.up * Rotation;
            }
            return decoration;
        }
    }
}
