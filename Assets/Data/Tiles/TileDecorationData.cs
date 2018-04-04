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
        [SerializeField] public Vector3 RelativePosition;
        [SerializeField] public float Scale = 1f;

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

            var transform = decoration.transform;
            transform.position = RelativePosition;
            transform.localScale = Vector3.one * Scale;


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