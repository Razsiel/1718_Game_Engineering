using System;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Tiles {
    [Serializable]
    [CreateAssetMenu(menuName = "Data/Tiles/Decoration")]
    public class TileDecorationData : ScriptableObject {
        public Mesh Mesh;
        public Material Material;
        public CardinalDirection Orientation;
        public Vector3 RelativePosition;

        public virtual bool IsWalkable(CardinalDirection direction) {
            return true;
        }

        public GameObject GenerateGameObject(Transform parent, bool hidden = false) {
            var decoration = new GameObject("Decoration",
                                            typeof(MeshFilter),
                                            typeof(MeshRenderer)) {
                hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };
            decoration.transform.parent = parent;

            decoration.transform.position = RelativePosition;

            var meshFilter = decoration.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh = this.Mesh;
            }
            var meshRenderer = decoration.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.sharedMaterial = this.Material;
            }

            return decoration;
        }
    }
}