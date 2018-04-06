using System;
using System.Linq;
using Assets.Data.Tiles;
using UnityEngine;

namespace Assets.Scripts.DataStructures {
    [Serializable]
    public class TileConfiguration {
        [SerializeField] public TileData Tile;
        [SerializeField] public TileDecorationData[] Decorations;
        
        public virtual bool IsWalkable(CardinalDirection direction)
        {
            return Decorations.Length == 0 || Decorations.All(d => d.IsWalkable(direction));
        }

        public virtual bool CanExit(CardinalDirection direction)
        {
            var oppositeDirection = direction.ToOppositeDirection();
            return IsWalkable(oppositeDirection);
        }

        public GameObject GenerateGameObject(GameObject root, int x, int y, bool hidden = false) {
            return GenerateGameObject(root.transform, x, y, hidden);
        }

        public GameObject GenerateGameObject(Transform root, int x, int y, bool hidden = false) {
            var tileConfigObject = new GameObject($"Tile Config ({x}, {y})",
                                      typeof(MeshFilter),
                                      typeof(MeshRenderer))
            {
                hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };

            tileConfigObject.transform.parent = root;

            var tile = Tile?.GenerateGameObject(tileConfigObject, hidden);
            foreach (var decoration in Decorations) {
                decoration?.GenerateGameObject(tile, hidden);
            }

#if UNITY_EDITOR
            var textObject = new GameObject($"Text", typeof(TextMesh));
            textObject.transform.parent = tileConfigObject.transform;
            textObject.transform.position = new Vector3(0, 20, 0);
            textObject.transform.Rotate(Vector3.right, 90);
            var text = textObject.GetComponent<TextMesh>();
            if (text != null)
            {
                text.alignment = TextAlignment.Center;
                text.anchor = TextAnchor.MiddleCenter;
                text.characterSize = 4;
                text.fontStyle = FontStyle.Bold;
                text.text = $"({x}, {y})";
            }
#endif

            return tileConfigObject;
        }
    }
}