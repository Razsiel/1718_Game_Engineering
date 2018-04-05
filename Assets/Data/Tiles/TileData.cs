using System;
using System.Linq;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Data.Tiles {
    [Serializable]
    [CreateAssetMenu(menuName = "Data/Tiles/Tile")]
    public class TileData : ScriptableObject {
        [SerializeField] public Mesh Mesh;
        [SerializeField] public Material Material;
        [SerializeField] public TileDecorationData[] DecorationsData;

        //private event OnPlayerTileEnter;
        //private event OnPlayerTileLeave;

        public virtual bool IsWalkable(CardinalDirection direction) {
            return DecorationsData.Length == 0 || DecorationsData.All(d => d.IsWalkable(direction));
        }

        public virtual bool CanExit(CardinalDirection direction) {
            var oppositeDirection = direction.ToOppositeDirection();
            return IsWalkable(oppositeDirection);
        }

        public GameObject GenerateGameObject(GameObject parent, int x, int y, bool hidden = false) {
            return GenerateGameObject(parent.transform, x, y, hidden);
        }

        public GameObject GenerateGameObject(Transform parent, int x, int y, bool hidden = false) {
            var tile = new GameObject($"Tile ({x}, {y})",
                                      typeof(MeshFilter),
                                      typeof(MeshRenderer)) {
                hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };

            tile.transform.parent = parent;

            // Fill in components
            var meshFilter = tile.GetComponent<MeshFilter>();
            if (meshFilter != null) {
                meshFilter.mesh = this.Mesh;
            }
            var meshRenderer = tile.GetComponent<MeshRenderer>();
            if (meshRenderer != null) {
                meshRenderer.sharedMaterial = this.Material;
            }

            foreach (var decorationData in DecorationsData) {
                decorationData.GenerateGameObject(tile.transform, hidden);
            }

#if UNITY_EDITOR
            var textObject = new GameObject($"Text", typeof(TextMesh));
            textObject.transform.parent = tile.transform;
            textObject.transform.position = new Vector3(0, 20, 0);
            textObject.transform.Rotate(Vector3.right, 90);
            var text = textObject.GetComponent<TextMesh>();
            if (text != null) {
                text.alignment = TextAlignment.Center;
                text.anchor = TextAnchor.MiddleCenter;
                text.characterSize = 4;
                text.fontStyle = FontStyle.Bold;
                text.text = tile.name.Substring(5);
            }
#endif
            return tile;
        }
    }
}