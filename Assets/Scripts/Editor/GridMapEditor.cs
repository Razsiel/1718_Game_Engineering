using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ScriptableObjects.Grids;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor {
    [CustomEditor(typeof(GridMapData))]
    public class GridMapEditor : UnityEditor.Editor {
        GridMapData _instance;
        PropertyField[] _fields;

        private GameObject _previewObject;
        private UnityEditor.Editor _mapEditor;

        public void OnEnable() {
            _instance = target as GridMapData;
            _fields = ExposeProperties.GetProperties(_instance);
        }

        public void OnDisable() {
            DestroyImmediate(_previewObject);
            DestroyImmediate(_mapEditor);
        }

        public override void OnInspectorGUI() {
            if (_instance == null)
                return;

            this.DrawDefaultInspector();
            ExposeProperties.Expose(_fields);

            if (GUILayout.Button("Recalculate")) {
                _instance.RecalculateGrid();
                CreatePreview();
            }

            // draw custom grid editor here
            if (GUILayout.Button("Recreate preview")) {
                CreatePreview();
            }
        }

        private void CreatePreview() {
            _previewObject = CreatePreviewObjectFromGridMap(_instance);
            _mapEditor = UnityEditor.Editor.CreateEditor(_previewObject);
        }

        public override bool HasPreviewGUI() {
            return _instance?.Count > 0;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            if (_mapEditor != null && _previewObject != null) {
                _mapEditor.OnPreviewGUI(r, background);
            }
        }

        private GameObject CreatePreviewObjectFromGridMap(GridMapData mapData) {
            Debug.Log("Creating preview object");
            DestroyImmediate(GameObject.Find("Level Preview Object")); // clear previous preview
            var root = EditorUtility.CreateGameObjectWithHideFlags("Level Preview Object", HideFlags.HideAndDontSave);

            for (int y = 0; y < mapData.Height; y++) {
                for (int x = 0; x < mapData.Width; x++) {
                    var tile = mapData[x, y];
                    if (tile == null) {
                        continue;
                    }
                    var tileObject =
                        EditorUtility.CreateGameObjectWithHideFlags($"Tile ({x},{y})",
                                                                    HideFlags.HideAndDontSave,
                                                                    typeof(MeshFilter),
                                                                    typeof(MeshRenderer));
                    var meshFilter = tileObject.GetComponent<MeshFilter>();
                    if (meshFilter != null) {
                        meshFilter.mesh = tile.TileMesh;
                    }
                    var meshRenderer = tileObject.GetComponent<MeshRenderer>();
                    if (meshRenderer != null) {
                        meshRenderer.sharedMaterial = tile.TileMaterial;
                    }
                    var transform = tileObject.transform;
                    transform.parent = root.transform;
                    transform.position = new Vector3(x - (mapData.Width - 1) * 0.5f,
                                                     0,
                                                     y - (mapData.Height - 1) * 0.5f);
                    transform.localScale /= 32;
                }
            }

            return root;
        }
    }
}