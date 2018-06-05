using System;
using Assets.Scripts.Behaviours;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Tiles {
    [Serializable]
    [CreateAssetMenu(menuName = "Data/Tiles/Decoration")]
    public class DecorationData : ScriptableObject {
        [SerializeField] public Mesh Mesh;
        [SerializeField] public Material Material;

        public virtual bool IsWalkable(CardinalDirection direction, CardinalDirection orientation = CardinalDirection.None) {
            return true;
        }

        public GameObject GenerateGameObject(GameObject parent, bool hidden = false) {
            return GenerateGameObject(parent.transform, hidden);
        }

        public virtual GameObject GenerateGameObject(Transform parent, bool hidden = false) {
            var decoration = new GameObject("Decoration",
                                            typeof(MeshFilter),
                                            typeof(MeshRenderer),
                                            typeof(DecorationBehaviour)) {
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