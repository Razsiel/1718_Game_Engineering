using System;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Tiles {
    [Serializable]
    [CreateAssetMenu(menuName = "Data/Tiles/Decoration")]
    public class TileDecorationData : ScriptableObject {
        [SerializeField] public Mesh Mesh;
        [SerializeField] public Material Material;
        [SerializeField] public CardinalDirection Orientation;

        public virtual bool IsWalkable(CardinalDirection direction) {
            return true;
        }

        public GameObject GenerateGameObject(GameObject parent, bool hidden = false) {
            return GenerateGameObject(parent.transform, hidden);
        }

        public GameObject GenerateGameObject(Transform parent, bool hidden = false) {
            var decoration = new GameObject("Decoration",
                                            typeof(MeshFilter),
                                            typeof(MeshRenderer)) {
                hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };
            decoration.transform.parent = parent;

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